#!/bin/bash

# Bash script to generate Docker configuration from Directory.Build.props

PROPS_FILE="Directory.Build.props"

# Check if Directory.Build.props exists
if [[ ! -f "$PROPS_FILE" ]]; then
    echo "❌ Directory.Build.props not found in current directory"
    exit 1
fi

# Extract values from Directory.Build.props using XML parsing
APP_NAME=$(xmllint --xpath "string(//AppName)" "$PROPS_FILE" 2>/dev/null)
DOCKER_POSTGRES_PORT=$(xmllint --xpath "string(//DockerPostgresPort)" "$PROPS_FILE" 2>/dev/null)
DOCKER_PGADMIN_PORT=$(xmllint --xpath "string(//DockerPgAdminPort)" "$PROPS_FILE" 2>/dev/null)
DOCKER_DB_NAME=$(xmllint --xpath "string(//DockerDbName)" "$PROPS_FILE" 2>/dev/null)
DOCKER_DB_USER=$(xmllint --xpath "string(//DockerDbUser)" "$PROPS_FILE" 2>/dev/null)
DOCKER_DB_PASSWORD=$(xmllint --xpath "string(//DockerDbPassword)" "$PROPS_FILE" 2>/dev/null)

# Fallback to grep/sed if xmllint is not available
if [[ -z "$APP_NAME" ]]; then
    APP_NAME=$(grep -oP '<AppName>\K[^<]+' "$PROPS_FILE")
    DOCKER_POSTGRES_PORT=$(grep -oP '<DockerPostgresPort>\K[^<]+' "$PROPS_FILE")
    DOCKER_PGADMIN_PORT=$(grep -oP '<DockerPgAdminPort>\K[^<]+' "$PROPS_FILE")
    DOCKER_DB_NAME=$(grep -oP '<DockerDbName>\K[^<]+' "$PROPS_FILE")
    DOCKER_DB_USER=$(grep -oP '<DockerDbUser>\K[^<]+' "$PROPS_FILE")
    DOCKER_DB_PASSWORD=$(grep -oP '<DockerDbPassword>\K[^<]+' "$PROPS_FILE")
fi

# Replace $(AppName) in DOCKER_DB_NAME, DOCKER_DB_USER and DOCKER_DB_PASSWORD if present
DOCKER_DB_NAME=${DOCKER_DB_NAME//\$(AppName)/$APP_NAME}
DOCKER_DB_USER=${DOCKER_DB_USER//\$(AppName)/$APP_NAME}
DOCKER_DB_PASSWORD=${DOCKER_DB_PASSWORD//\$(AppName)/$APP_NAME}

# Create lowercase version of AppName for container names
APP_NAME_LOWER=$(echo "$APP_NAME" | tr '[:upper:]' '[:lower:]')

echo "Generating Docker configuration for: $APP_NAME"
echo "PostgreSQL Port: $DOCKER_POSTGRES_PORT"
echo "pgAdmin Port: $DOCKER_PGADMIN_PORT"
echo "Database Name: $DOCKER_DB_NAME"
echo "Database User: $DOCKER_DB_USER"

# Generate docker-compose.yml
sed -e "s/{{APP_NAME}}/$APP_NAME/g" \
    -e "s/{{APP_NAME_LOWER}}/$APP_NAME_LOWER/g" \
    -e "s/{{DOCKER_POSTGRES_PORT}}/$DOCKER_POSTGRES_PORT/g" \
    -e "s/{{DOCKER_PGADMIN_PORT}}/$DOCKER_PGADMIN_PORT/g" \
    -e "s/{{DOCKER_DB_NAME}}/$DOCKER_DB_NAME/g" \
    -e "s/{{DOCKER_DB_USER}}/$DOCKER_DB_USER/g" \
    -e "s/{{DOCKER_DB_PASSWORD}}/$DOCKER_DB_PASSWORD/g" \
    docker-compose.yml > docker-compose.generated.yml

# Generate init SQL script
sed -e "s/{{APP_NAME}}/$APP_NAME/g" \
    -e "s/{{DOCKER_DB_NAME}}/$DOCKER_DB_NAME/g" \
    init-db/01-init.sql > init-db/01-init.generated.sql

# Generate connection string
CONNECTION_STRING="Host=localhost;Port=$DOCKER_POSTGRES_PORT;Database=$DOCKER_DB_NAME;Username=$DOCKER_DB_USER;Password=$DOCKER_DB_PASSWORD"

echo ""
echo "✅ Generated files:"
echo "   - docker-compose.generated.yml"
echo "   - init-db/01-init.generated.sql"
echo ""
echo "📋 Connection string for appsettings.json:"
echo "   \"DefaultConnection\": \"$CONNECTION_STRING\""
echo ""
echo "🚀 To start Docker services:"
echo "   docker-compose -f docker-compose.generated.yml up -d"