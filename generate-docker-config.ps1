# PowerShell script to generate Docker configuration from Directory.Build.props
param(
    [string]$PropsFile = "Directory.Build.props"
)

# Check if Directory.Build.props exists
if (-not (Test-Path $PropsFile)) {
    Write-Error "Directory.Build.props not found in current directory"
    exit 1
}

# Read and parse Directory.Build.props
[xml]$props = Get-Content $PropsFile

# Extract values
$AppName = $props.Project.PropertyGroup.AppName
$DockerSqlServerPort = $props.Project.PropertyGroup.DockerSqlServerPort
$DockerDbName = $props.Project.PropertyGroup.DockerDbName
$DockerDbPassword = $props.Project.PropertyGroup.DockerDbPassword

# Create lowercase version of AppName for container names
$AppNameLower = $AppName.ToLower()

Write-Host "Generating Docker configuration for: $AppName"
Write-Host "SQL Server Port: $DockerSqlServerPort"
Write-Host "Database Name: $DockerDbName"

# Generate docker-compose.yml
$dockerComposeTemplate = Get-Content "docker-compose.yml" -Raw
$dockerComposeContent = $dockerComposeTemplate `
    -replace "{{APP_NAME}}", $AppName `
    -replace "{{APP_NAME_LOWER}}", $AppNameLower `
    -replace "{{DOCKER_SQLSERVER_PORT}}", $DockerSqlServerPort `
    -replace "{{DOCKER_DB_NAME}}", $DockerDbName `
    -replace "{{DOCKER_DB_PASSWORD}}", $DockerDbPassword

$dockerComposeContent | Out-File "docker-compose.generated.yml" -Encoding UTF8

# Generate init SQL script
$sqlTemplate = Get-Content "init-db/01-init.sql" -Raw
$sqlContent = $sqlTemplate `
    -replace "{{APP_NAME}}", $AppName `
    -replace "{{DOCKER_DB_NAME}}", $DockerDbName

$sqlContent | Out-File "init-db/01-init.generated.sql" -Encoding UTF8

# Generate appsettings connection string
$connectionString = "Server=localhost,$DockerSqlServerPort;Database=$DockerDbName;User Id=sa;Password=$DockerDbPassword;TrustServerCertificate=true;MultipleActiveResultSets=true"

Write-Host ""
Write-Host "✅ Generated files:"
Write-Host "   - docker-compose.generated.yml"
Write-Host "   - init-db/01-init.generated.sql"
Write-Host ""
Write-Host "📋 Connection string for appsettings.json:"
Write-Host "   `"DefaultConnection`": `"$connectionString`""
Write-Host ""
Write-Host "🚀 To start Docker services:"
Write-Host "   docker-compose -f docker-compose.generated.yml up -d"