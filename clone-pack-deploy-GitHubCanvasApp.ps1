
$pacTemp = "pack-temp"
$solutionName = "GitHubCanvasApp"
$unpackFolder = "src/$solutionName/unpacked-solution"
$gitHubCanvasAppFolder = "$unpackFolder/CanvasApps/dkdt_canvascallinggithub_3efc9_DocumentUri_msapp"
$devopsCanvasAppFolder ="src\TwoAppsTwoConnectorsSameInterface\unpacked-solution\CanvasApps\dkdt_canvascallingazuredevops_4df28_DocumentUri_msapp"

Remove-Item -Path "$gitHubCanvasAppFolder/*" -Recurse -Force
Copy-Item -Path "$devopsCanvasAppFolder/*" $gitHubCanvasAppFolder -Recurse -Force

$msappDirectories = Get-ChildItem -Path "$unpackFolder/CanvasApps" -Filter *_msapp
foreach ($msappDirectory in $msappDirectories) {
    $msappToCreate = $msappDirectory.FullName.Replace("_msapp",".msapp")
    pac canvas pack --sources $msappDirectory.FullName --msapp $msappToCreate
}
$zipfile = "$pacTemp/$solutionName.zip"
pac solution pack --zipfile $zipfile --folder $unpackFolder
pac solution import --path $zipfile
pac solution publish