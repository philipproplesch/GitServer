Properties {
  # Files
  $sln = "..\devplex.GitServer.sln"

  # Directories
  $bld = Split-Path $psake.build_script_file
  $out = "$bld\..\out"
  $src = "$bld\..\src"
  $tmp = "$bld\..\tmp"

  #Versioning
  $major = 0
  $minor = 1
  $version = "$major.$minor"
}

Task Default -depends Compile

Task Compile {
  Exec { msbuild $sln /t:Build /p:Configuration=Release }
}

Task Build -depends Clean, IncrementVersion {
  Exec { msbuild $sln /t:Rebuild /p:Configuration=Release /p:OutDir=$tmp /p:UseWPP_CopyWebApplication=True /p:PipelineDependsOnBuild=False }
  & "$bld\7zip\7za.exe" a -tzip $out\$script:version.zip $tmp\_PublishedWebsites\*
}

Task Clean {
  if ((Test-Path $out) -eq $false) {
    New-Item $out -Type Directory
  }

  if (Test-Path $tmp) {
    Remove-Item $tmp -Recurse -Force
  }  
}

Task IncrementVersion {
  $build = Generate-BuildNumber
  $revision = Generate-RevisionNumber $build

  $script:version = "$major.$minor.$build.$revision"
  
  $assemblyVersionRegex = 'AssemblyVersion\(\".*\"\)'
  $assemblyFileVersionRegex = 'AssemblyFileVersion\(\".*\"\)'
  $assemblyVersion = 'AssemblyVersion("' + $script:version + '")'
  $assemblyFileVersion = 'AssemblyFileVersion("' + $script:version + '")'

  Get-ChildItem -Path $src -Recurse -Filter AssemblyInfo.cs | ForEach-Object {
    $fileName = $_.FullName
    
    (Get-Content $fileName) | ForEach-Object {
      % { $_ -replace $assemblyVersionRegex, $assemblyVersion } |
      % { $_ -replace $assemblyFileVersionRegex, $assemblyFileVersion }
    } | Set-Content $fileName
  }
}

function Generate-BuildNumber {
  $now = Get-Date
  return (($now.Year - 2000) * 1000 + $now.DayOfYear)
}

function Generate-RevisionNumber($build) {
  $builds = Get-ChildItem -Path "$out\$major.$minor.$build.*.zip" | Measure-Object
  Write-Host $builds
  return $builds.Count + 1
}