
# Run this file with powershell to publish package to NuGet

$nuget_url = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"

function WriteMessage ([string] $message){
    Write-Host $message -ForegroundColor Yellow -BackgroundColor Black
}

function Pause ($message = "Press any key to continue . . . ") {
    if ((Test-Path variable:psISE) -and $psISE) {
        $Shell = New-Object -ComObject "WScript.Shell"
        $Button = $Shell.Popup("Click OK to continue.", 0, "Script Paused", 0)
    }
    else {     
        WriteMessage $message
        [void][System.Console]::ReadKey($true)
        Write-Host
    }
}

WriteMessage "Downloading nuget.exe"
Invoke-WebRequest -Uri $nuget_url -OutFile "nuget.exe"

WriteMessage "Remember to update release notes in UriBuilderExtended/UriBuilderExtended.nuspec"
Pause

WriteMessage "Creating package"
./nuget pack UriBuilderExtended\UriBuilderExtended.csproj -Prop Configuration=Release


$file = Get-ChildItem *.nupkg
$package = ""
if($file -is [System.IO.FileSystemInfo])
{
    $package = $file.Name
}
if($package -ne "")
{
    WriteMessage "Package created: $package"

    WriteMessage "Pushing package"
    $api_key = Read-Host "Please enter API key"
    ./nuget push $package -Source nuget.org -ApiKey $api_key -NonInteractive
}
else
{
    WriteMessage "Unable to detect package after creation. Please clean the directory for any existing packages."
}

WriteMessage "Cleaning up"
Remove-Item "nuget.exe"
Remove-Item $package
