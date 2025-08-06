#!/bin/bash

# OptimalyTemplate Project Rename Script
# Usage: ./rename-project.sh "YourAppName"

set -e

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Function to print colored output
print_status() {
    echo -e "${BLUE}[INFO]${NC} $1"
}

print_success() {
    echo -e "${GREEN}[SUCCESS]${NC} $1"
}

print_warning() {
    echo -e "${YELLOW}[WARNING]${NC} $1"
}

print_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

# Validate input
if [ -z "$1" ]; then
    print_error "Usage: $0 <NewAppName>"
    print_error "Example: $0 \"MyAwesomeApp\""
    exit 1
fi

NEW_APP_NAME="$1"
NEW_APP_NAME_LOWER=$(echo "$NEW_APP_NAME" | tr '[:upper:]' '[:lower:]')

# Validate app name format
if ! [[ "$NEW_APP_NAME" =~ ^[A-Za-z][A-Za-z0-9]*$ ]]; then
    print_error "App name must start with a letter and contain only letters and numbers"
    print_error "Invalid: '$NEW_APP_NAME'"
    exit 1
fi

print_status "Starting project rename from 'OptimalyTemplate' to '$NEW_APP_NAME'"
print_status "Lowercase version: '$NEW_APP_NAME_LOWER'"

# Check if we're in the right directory
if [ ! -f "OptimalyTemplate.sln" ]; then
    print_error "OptimalyTemplate.sln not found. Please run this script from the project root directory."
    exit 1
fi

# Create backup
print_status "Creating backup..."
BACKUP_DIR="../OptimalyTemplate_backup_$(date +%Y%m%d_%H%M%S)"
cp -r . "$BACKUP_DIR"
print_success "Backup created at: $BACKUP_DIR"

# Function to rename files and directories
rename_files_and_dirs() {
    print_status "Renaming files and directories..."
    
    # Rename solution file
    if [ -f "OptimalyTemplate.sln" ]; then
        mv "OptimalyTemplate.sln" "${NEW_APP_NAME}.sln"
        print_success "Renamed solution file"
    fi
    
    # Rename project directories and files
    for dir in OT.DataLayer OT.ServiceLayer OT.PresentationLayer; do
        if [ -d "$dir" ]; then
            new_dir="${dir/OT./${NEW_APP_NAME}.}"
            mv "$dir" "$new_dir"
            print_success "Renamed directory: $dir -> $new_dir"
            
            # Rename .csproj files inside
            if [ -f "$new_dir/$dir.csproj" ]; then
                mv "$new_dir/$dir.csproj" "$new_dir/${new_dir}.csproj"
                print_success "Renamed project file: $dir.csproj -> ${new_dir}.csproj"
            fi
        fi
    done
}

# Function to update file contents
update_file_contents() {
    print_status "Updating file contents..."
    
    # Find all relevant files and update content
    find . -type f \( -name "*.cs" -o -name "*.csproj" -o -name "*.sln" -o -name "*.json" -o -name "*.yml" -o -name "*.yaml" -o -name "*.md" -o -name "*.cshtml" \) \
        -not -path "./bin/*" -not -path "./obj/*" -not -path "./.git/*" -not -path "./logs/*" | \
    while read -r file; do
        if [ -f "$file" ]; then
            # Create temporary file for replacements
            temp_file=$(mktemp)
            
            # Perform replacements
            sed "s/OptimalyTemplate/${NEW_APP_NAME}/g" "$file" | \
            sed "s/OT\./${NEW_APP_NAME}./g" | \
            sed "s/{{APP_NAME_LOWER}}/${NEW_APP_NAME_LOWER}/g" | \
            sed "s/optimalytemplate/${NEW_APP_NAME_LOWER}/g" > "$temp_file"
            
            # Only update if content changed
            if ! cmp -s "$file" "$temp_file"; then
                mv "$temp_file" "$file"
                print_success "Updated: $file"
            else
                rm "$temp_file"
            fi
        fi
    done
}

# Function to update database-specific configurations
update_database_config() {
    print_status "Updating database configurations..."
    
    # Update Directory.Build.props
    local props_file="Directory.Build.props"
    if [ -f "$props_file" ]; then
        sed -i.bak "s/<AppName>OptimalyTemplate<\/AppName>/<AppName>${NEW_APP_NAME}<\/AppName>/g" "$props_file"
        rm "${props_file}.bak" 2>/dev/null || true
        print_success "Updated Directory.Build.props"
    fi
    
    # Update connection strings in appsettings.json
    local config_file="${NEW_APP_NAME}.PresentationLayer/appsettings.json"
    if [ -f "$config_file" ]; then
        sed -i.bak "s/OptimalyTemplate_db/${NEW_APP_NAME_LOWER}_db/g" "$config_file"
        sed -i.bak "s/OptimalyTemplate_user/${NEW_APP_NAME_LOWER}_user/g" "$config_file"
        sed -i.bak "s/OptimalyTemplate2024!/${NEW_APP_NAME}2024!/g" "$config_file"
        rm "${config_file}.bak" 2>/dev/null || true
        print_success "Updated appsettings.json database configuration"
    fi
}

# Function to clean up template entities
cleanup_template_entities() {
    print_status "Do you want to remove template entities (TemplateProduct, TemplateCategory)? [y/N]"
    read -r response
    
    if [[ "$response" =~ ^[Yy]$ ]]; then
        print_status "Removing template entities..."
        
        # Remove template entity files
        find . -name "*Template*" -type f \( -name "*.cs" -o -name "*.cshtml" \) \
            -not -path "./.git/*" -not -path "./bin/*" -not -path "./obj/*" | \
        while read -r file; do
            rm "$file"
            print_success "Removed: $file"
        done
        
        # Remove template references from Program.cs and other configuration files
        find . -name "Program.cs" -o -name "*Extensions.cs" | \
        while read -r file; do
            if [ -f "$file" ]; then
                # Remove template service registrations
                sed -i.bak '/Template.*Service/d' "$file" 2>/dev/null || true
                sed -i.bak '/.*Template.*/d' "$file" 2>/dev/null || true
                rm "${file}.bak" 2>/dev/null || true
            fi
        done
        
        print_warning "Template entities removed. You'll need to:"
        print_warning "1. Remove template migrations"
        print_warning "2. Create new initial migration"
        print_warning "3. Update navigation in _Layout.cshtml"
    else
        print_status "Template entities kept for reference"
    fi
}

# Function to update git repository
update_git_config() {
    print_status "Do you want to reinitialize git repository? [y/N]"
    read -r response
    
    if [[ "$response" =~ ^[Yy]$ ]]; then
        rm -rf .git
        git init
        git add .
        git commit -m "Initial commit for $NEW_APP_NAME project"
        print_success "Git repository reinitialized"
    else
        print_status "Git repository kept as is"
    fi
}

# Function to generate docker-compose from template
generate_docker_compose() {
    print_status "Generating docker-compose.generated.yml..."
    
    if [ -f "docker-compose.yml" ]; then
        sed "s/{{APP_NAME_LOWER}}/${NEW_APP_NAME_LOWER}/g" docker-compose.yml | \
        sed "s/{{DOCKER_DB_USER}}/${NEW_APP_NAME_LOWER}_user/g" | \
        sed "s/{{DOCKER_DB_PASSWORD}}/${NEW_APP_NAME}2024!/g" | \
        sed "s/{{DOCKER_DB_NAME}}/${NEW_APP_NAME_LOWER}_db/g" | \
        sed "s/{{DOCKER_POSTGRES_PORT}}/5434/g" | \
        sed "s/{{DOCKER_PGADMIN_PORT}}/5051/g" > docker-compose.generated.yml
        
        print_success "Generated docker-compose.generated.yml"
    fi
}

# Main execution
main() {
    print_status "=================== PROJECT RENAME STARTED ==================="
    
    # Step 1: Rename files and directories
    rename_files_and_dirs
    
    # Step 2: Update file contents
    update_file_contents
    
    # Step 3: Update database configurations
    update_database_config
    
    # Step 4: Generate docker-compose
    generate_docker_compose
    
    # Step 5: Optional cleanup
    cleanup_template_entities
    
    # Step 6: Optional git reinitialization
    update_git_config
    
    print_success "=================== PROJECT RENAME COMPLETED ==================="
    print_success "Project successfully renamed to: $NEW_APP_NAME"
    print_status ""
    print_status "Next steps:"
    print_status "1. Review the generated files"
    print_status "2. Run: docker-compose -f docker-compose.generated.yml up -d"
    print_status "3. Run: dotnet ef database update -p ${NEW_APP_NAME}.DataLayer -s ${NEW_APP_NAME}.PresentationLayer"
    print_status "4. Run: dotnet run --project ${NEW_APP_NAME}.PresentationLayer"
    print_status ""
    print_status "Your app will be available at: http://localhost:5020"
    print_status "pgAdmin will be available at: http://localhost:5051"
    
    if [ -d "$BACKUP_DIR" ]; then
        print_status ""
        print_warning "Original backup saved at: $BACKUP_DIR"
    fi
}

# Run main function
main