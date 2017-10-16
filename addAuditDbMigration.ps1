Write-Host "Adding migrations for EF.Examples.Audit" -foregroundcolor "yellow"
Add-Migration -ProjectName TddBuddy.SpeedyLocalDb.EF.Examples.Audit -StartupProjectName TddBuddy.SpeedySqlLocalDb.Tests