Write-Host "Running migrations for EF.Examples.Attachment" -foregroundcolor "yellow"
Update-Database -ProjectName TddBuddy.SpeedyLocalDb.EF.Examples.Attachment -StartupProjectName TddBuddy.SpeedySqlLocalDb.Tests