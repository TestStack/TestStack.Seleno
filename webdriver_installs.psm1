function Install-ChromeDriver() {
	Install-Package Selenium.WebDriver.ChromeDriver
	Write-Host "Successfully added chrome web driver for use with Seleno."
}

function Install-PhantomJSDriver() {
	Write-Host "Installing phantomjs.exe package"
	Install-Package phantomjs.exe
	
	Write-Host "Marking phantomjs.exe as embedded resource."
	Set-RootProjectFileAsEmbeddedResource "phantomjs.exe"
	
	Write-Host "Successfully added PhantomJS web driver for use with Seleno."
}

function Install-IE64Driver() {
	Install-EmbeddedFileFromNugetPackageTools "WebDriver.IEDriver" "IEDriverServer.exe"
	Write-Host "Successfully added IE web driver for use with Seleno."
}

function Install-IE32Driver() {
	Install-Package Selenium.WebDriver.IEDriver
	Write-Host "Successfully added IE web driver for use with Seleno."
}

function Install-EmbeddedFileFromNugetPackageTools($packageName, $fileInToolsDirToEmbed) {
	Write-Host "Installing $packageName package"
	Install-Package $packageName

	Write-Host "Adding $fileInToolsDirToEmbed to project."
	$file = Join-Path (Get-PackageToolsDir $packageName) $fileInToolsDirToEmbed
	Update-RootProjectFile $file $fileInToolsDirToEmbed
	
	Write-Host "Marking $fileInToolsDirToEmbed as embedded resource."
	Set-RootProjectFileAsEmbeddedResource $fileInToolsDirToEmbed
}

function Get-PackageToolsDir($packageName) {
	$toolsPath = $PSScriptRoot
	$packagesDir = Join-Path (Join-Path $toolsPath "..") ".."
	$packageDir = Get-ChildItem $packagesDir | Where-Object { $_.Name -like "$packageName.*" } | Select-Object -First 1
	return Join-Path (Join-Path $packagesDir $packageDir) "tools"
}

function Update-RootProjectFile($source, $destination) {
	$project = Get-Project
	$item = $project.ProjectItems | Where-Object { $_.Name -eq $destination } | Select-Object -First 1
	if ($item -ne $null) {
		$item.Remove()
	}
	try {
		$project.ProjectItems.AddFromFile($source)
	}
	catch {}
	$item = $project.ProjectItems | Where-Object { $_.Name -eq $destination } | Select-Object -First 1
	if ($item -eq $null) {
		Write-Error "Unable to add $destination to project root."
		return
	}
}

function Set-RootProjectFileAsEmbeddedResource($file) {
	$project = Get-Project
	$item = $project.ProjectItems | Where-Object { $_.Name -eq $file } | Select-Object -First 1
	$item.Properties.Item("BuildAction").Value = [int]3 # Embed
	$item.Properties.Item("CopyToOutputDirectory").Value = [int]0 # Do not copy
}
