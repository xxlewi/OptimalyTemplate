# OptimalyTemplate Project Rename Script (PowerShell)
# Usage: .\rename-project.ps1 "YourAppName"

param(
    [Parameter(Mandatory=$true)]
    [string]$NewAppName
)

# Colors for output
$ErrorColor = "Red"
$SuccessColor = "Green"
$WarningColor = "Yellow"
$InfoColor = "Cyan"

function Write-Status {
    param([string]$Message)
    Write-Host "[INFO] $Message" -ForegroundColor $InfoColor
}

function Write-Success {
    param([string]$Message)
    Write-Host "[SUCCESS] $Message" -ForegroundColor $SuccessColor
}

function Write-Warning {
    param([string]$Message)
    Write-Host "[WARNING] $Message" -ForegroundColor $WarningColor
}

function Write-Error {
    param([string]$Message)
    Write-Host "[ERROR] $Message" -ForegroundColor $ErrorColor
}

# Validate input
if ([string]::IsNullOrWhiteSpace($NewAppName)) {
    Write-Error "Usage: .\rename-project.ps1 `"NewAppName`""
    Write-Error "Example: .\rename-project.ps1 `"MyAwesomeApp`""
    exit 1
}

$NewAppNameLower = $NewAppName.ToLower()

# Validate app name format
if ($NewAppName -notmatch '^[A-Za-z][A-Za-z0-9]*$') {
    Write-Error "App name must start with a letter and contain only letters and numbers"
    Write-Error "Invalid: '$NewAppName'"
    exit 1
}

Write-Status "Starting project rename from 'OptimalyTemplate' to '$NewAppName'"
Write-Status "Lowercase version: '$NewAppNameLower'"

# Check if we're in the right directory
if (!(Test-Path "OptimalyTemplate.sln")) {
    Write-Error "OptimalyTemplate.sln not found. Please run this script from the project root directory."
    exit 1
}

# Create backup
Write-Status "Creating backup..."
$BackupDir = "..\OptimalyTemplate_backup_$(Get-Date -Format 'yyyyMMdd_HHmmss')"
Copy-Item -Path "." -Destination $BackupDir -Recurse -Force
Write-Success "Backup created at: $BackupDir"

# Function to rename files and directories
function Rename-FilesAndDirs {
    Write-Status "Renaming files and directories..."
    
    # Rename solution file
    if (Test-Path "OptimalyTemplate.sln") {
        Rename-Item "OptimalyTemplate.sln" "$NewAppName.sln"
        Write-Success "Renamed solution file"
    }
    
    # Rename project directories and files
    $directories = @("OT.DataLayer", "OT.ServiceLayer", "OT.PresentationLayer")
    foreach ($dir in $directories) {
        if (Test-Path $dir) {
            $newDir = $dir -replace "OT\.", "$NewAppName."
            Rename-Item $dir $newDir
            Write-Success "Renamed directory: $dir -> $newDir"
            
            # Rename .csproj files inside
            $oldCsprojPath = "$newDir\$dir.csproj"
            $newCsprojPath = "$newDir\$newDir.csproj"
            if (Test-Path $oldCsprojPath) {
                Rename-Item $oldCsprojPath $newCsprojPath
                Write-Success "Renamed project file: $dir.csproj -> $newDir.csproj"
            }
        }
    }
}

# Function to update file contents
function Update-FileContents {
    Write-Status "Updating file contents..."
    
    # Find all relevant files
    $files = Get-ChildItem -Recurse -Include @("*.cs", "*.csproj", "*.sln", "*.json", "*.yml", "*.yaml", "*.md", "*.cshtml") |
        Where-Object { 
            $_.FullName -notlike "*\bin\*" -and 
            $_.FullName -notlike "*\obj\*" -and 
            $_.FullName -notlike "*\.git\*" -and 
            $_.FullName -notlike "*\logs\*" 
        }
    
    foreach ($file in $files) {
        try {
            $content = Get-Content $file.FullName -Raw -ErrorAction Stop
            $originalContent = $content
            
            # Perform replacements
            $content = $content -replace "OptimalyTemplate", $NewAppName
            $content = $content -replace "OT\.", "$NewAppName."
            $content = $content -replace "\{\{APP_NAME_LOWER\}\}", $NewAppNameLower
            $content = $content -replace "optimalytemplate", $NewAppNameLower
            
            # Only update if content changed
            if ($content -ne $originalContent) {
                Set-Content $file.FullName $content -NoNewline
                Write-Success "Updated: $($file.FullName)"
            }
        }
        catch {
            Write-Warning "Could not update file: $($file.FullName) - $($_.Exception.Message)"
        }
    }
}

# Function to update database-specific configurations
function Update-DatabaseConfig {
    Write-Status "Updating database configurations..."
    
    $configFile = "$NewAppName.PresentationLayer\appsettings.json"
    if (Test-Path $configFile) {
        try {
            $content = Get-Content $configFile -Raw
            $content = $content -replace "OptimalyTemplate_db", "${NewAppNameLower}_db"
            $content = $content -replace "OptimalyTemplate_user", "${NewAppNameLower}_user"
            $content = $content -replace "OptimalyTemplate2024!", "${NewAppName}2024!"
            Set-Content $configFile $content -NoNewline
            Write-Success "Updated database configuration"
        }
        catch {
            Write-Warning "Could not update database configuration: $($_.Exception.Message)"
        }
    }
}

# Function to clean up template entities
function Cleanup-TemplateEntities {
    $response = Read-Host "Do you want to remove template entities (TemplateProduct, TemplateCategory)? [y/N]"
    
    if ($response -eq "y" -or $response -eq "Y") {
        Write-Status "Removing template entities..."
        
        # Remove template entity files
        $templateFiles = Get-ChildItem -Recurse -Include @("*Template*.cs", "*Template*.cshtml") |
            Where-Object { 
                $_.FullName -notlike "*\.git\*" -and 
                $_.FullName -notlike "*\bin\*" -and 
                $_.FullName -notlike "*\obj\*" 
            }
        
        foreach ($file in $templateFiles) {
            Remove-Item $file.FullName -Force
            Write-Success "Removed: $($file.FullName)"
        }
        
        # Remove template references from configuration files
        $configFiles = Get-ChildItem -Recurse -Include @("Program.cs", "*Extensions.cs") |
            Where-Object { 
                $_.FullName -notlike "*\.git\*" -and 
                $_.FullName -notlike "*\bin\*" -and 
                $_.FullName -notlike "*\obj\*" 
            }
        
        foreach ($file in $configFiles) {
            try {
                $content = Get-Content $file.FullName
                $filteredContent = $content | Where-Object { $_ -notlike "*Template*Service*" -and $_ -notlike "*Template*" }
                Set-Content $file.FullName $filteredContent
            }
            catch {
                Write-Warning "Could not clean template references from: $($file.FullName)"
            }
        }
        
        Write-Warning "Template entities removed. You'll need to:"
        Write-Warning "1. Remove template migrations"
        Write-Warning "2. Create new initial migration"
        Write-Warning "3. Update navigation in _Layout.cshtml"
    }
    else {
        Write-Status "Template entities kept for reference"
    }
}

# Function to update git repository
function Update-GitConfig {
    $response = Read-Host "Do you want to reinitialize git repository? [y/N]"
    
    if ($response -eq "y" -or $response -eq "Y") {
        if (Test-Path ".git") {
            Remove-Item ".git" -Recurse -Force
        }
        
        & git init
        & git add .
        & git commit -m "Initial commit for $NewAppName project"
        Write-Success "Git repository reinitialized"
    }
    else {
        Write-Status "Git repository kept as is"
    }
}

# Function to generate docker-compose from template
function Generate-DockerCompose {
    Write-Status "Generating docker-compose.generated.yml..."
    
    if (Test-Path "docker-compose.yml") {
        try {
            $content = Get-Content "docker-compose.yml" -Raw
            $content = $content -replace "\{\{APP_NAME_LOWER\}\}", $NewAppNameLower
            $content = $content -replace "\{\{DOCKER_DB_USER\}\}", "${NewAppNameLower}_user"
            $content = $content -replace "\{\{DOCKER_DB_PASSWORD\}\}", "${NewAppName}2024!"
            $content = $content -replace "\{\{DOCKER_DB_NAME\}\}", "${NewAppNameLower}_db"
            $content = $content -replace "\{\{DOCKER_POSTGRES_PORT\}\}", "5434"
            $content = $content -replace "\{\{DOCKER_PGADMIN_PORT\}\}", "5051"
            
            Set-Content "docker-compose.generated.yml" $content -NoNewline
            Write-Success "Generated docker-compose.generated.yml"
        }
        catch {
            Write-Warning "Could not generate docker-compose.generated.yml: $($_.Exception.Message)"
        }
    }
}

# Main execution
function Main {
    Write-Status "=================== PROJECT RENAME STARTED ==================="
    
    try {
        # Step 1: Rename files and directories
        Rename-FilesAndDirs
        
        # Step 2: Update file contents
        Update-FileContents
        
        # Step 3: Update database configurations
        Update-DatabaseConfig
        
        # Step 4: Generate docker-compose
        Generate-DockerCompose
        
        # Step 5: Optional cleanup
        Cleanup-TemplateEntities
        
        # Step 6: Optional git reinitialization
        Update-GitConfig
        
        Write-Success "=================== PROJECT RENAME COMPLETED ==================="
        Write-Success "Project successfully renamed to: $NewAppName"
        Write-Status ""
        Write-Status "Next steps:"
        Write-Status "1. Review the generated files"
        Write-Status "2. Run: docker-compose -f docker-compose.generated.yml up -d"
        Write-Status "3. Run: dotnet ef database update -p $NewAppName.DataLayer -s $NewAppName.PresentationLayer"
        Write-Status "4. Run: dotnet run --project $NewAppName.PresentationLayer"
        Write-Status ""
        Write-Status "Your app will be available at: http://localhost:5020"
        Write-Status "pgAdmin will be available at: http://localhost:5051"
        
        if (Test-Path $BackupDir) {
            Write-Status ""
            Write-Warning "Original backup saved at: $BackupDir"
        }
    }
    catch {
        Write-Error "An error occurred during the rename process: $($_.Exception.Message)"
        Write-Error "You can restore from backup at: $BackupDir"
        exit 1
    }
}

# Run main function
Main