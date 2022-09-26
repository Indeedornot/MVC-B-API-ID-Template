$context = "LogDbContext"

$dbName = "LogDatabase.sqlite"
$dbPath = Join-Path $PSScriptRoot $dbName
$dbExists = Test-Path -Path $dbPath -PathType Leaf

$migrationFName = "Migrations"
$migrationPath = Join-Path $PSScriptRoot $migrationFName
$migrationExists = Test-Path -Path $migrationPath -PathType Container

# dotnet ef database drop --context LogDbContext
# Write-Output Y

Set-Location (Get-Item $PSScriptRoot).Parent.Parent.FullName

if ($migrationExists)
{
    dotnet ef migrations remove --context $context
}

if ($dbExists)
{
    (Get-ChildItem $dbPath).Delete()
}

dotnet ef migrations add Initial -o $migrationPath --context $context
dotnet ef database update --connection "Data Source=$PSScriptRoot\$dbName" --context $context 
