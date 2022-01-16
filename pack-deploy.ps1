$pacTemp = "pack-temp"
$solutionName = "TwoAppsTwoConnectorsSameInterface"
$unpackFolder = "src/$solutionName/unpacked-solution"

$msappDirectories = Get-ChildItem -Path "$unpackFolder/CanvasApps" -Filter *_msapp
foreach ($msappDirectory in $msappDirectories) {
    $msappToCreate = $msappDirectory.FullName.Replace("_msapp",".msapp")
    pac canvas pack --sources $msappDirectory.FullName --msapp $msappToCreate
}
$zipfile = "$pacTemp/$solutionName.zip"
pac solution pack --zipfile $zipfile --folder $unpackFolder
pac solution import --path $zipfile
pac solution publish