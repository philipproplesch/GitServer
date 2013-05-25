Properties {
	# Files
	$sln = "..\devplex.GitServer.sln"

	# Directories
	$bld = Split-Path $psake.build_script_file
	$out = "$bld\..\out"
}

Task Default -depends Compile

Task Compile {
	Exec { msbuild $sln /t:Build /p:Configuration=Release }
}

# TODO: Increment assembly version on build.
Task Build -depends Clean {
	 Exec { msbuild $sln /t:Rebuild /p:Configuration=Release /p:OutDir=$out }
}

Task Clean {
	if (Test-Path $out) {
    Remove-Item $out -Recurse -Force
  }
}