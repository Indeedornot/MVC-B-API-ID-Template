#dotnet ef database update 0 --connection "Data Source=$(Get-Location)\LogDatabase.sqlite" --context LogDbContext
Set-Location (get-item $PSScriptRoot).Parent.Parent.FullName
dotnet ef database drop --context LogDbContext
dotnet ef migrations remove --context LogDbContext 
$dbPath = Join-Path $PSScriptRoot \LogDatabase.sqlite
$dbExists = Test-Path -Path $dbPath -PathType Leaf
if ($($dbExists)) {
    (Get-ChildItem $($dbPath)).Delete()
}
dotnet ef migrations add Initial -o ./Logging/Database/Migrations --context LogDbContext
dotnet ef database update --connection "Data Source=$($PSScriptRoot)\LogDatabase.sqlite" --context LogDbContext
