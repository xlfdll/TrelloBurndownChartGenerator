# Trello Burndown Chart Generator
A simple Trello burndown chart generator / viewer

<p align="center">
  <img src="https://github.com/xlfdll/xlfdll.github.io/raw/master/images/projects/TrelloBurndownChartGenerator/TrelloBurndownChartGenerator.png"
       alt="Trello Burndown Chart Generator">
</p>

## System Requirements
* .NET Framework 4.7.2

[Runtime configuration](https://docs.microsoft.com/en-us/dotnet/framework/migration-guide/how-to-configure-an-app-to-support-net-framework-4-or-4-5) is needed for running on other versions of .NET Framework.

## Usage
1. Open the Trello Board designated for burndown chart generation, choose **More** from the Menu

<p align="center">
  <img src="https://github.com/xlfdll/xlfdll.github.io/raw/master/images/projects/TrelloBurndownChartGenerator/Trello-ExportJSON-1.png"
       alt="Trello Board - More options in Menu">
</p>

2. Choose **Print and Export** -- **Export as JSON**. Save the JSON file

<p align="center">
  <img src="https://github.com/xlfdll/xlfdll.github.io/raw/master/images/projects/TrelloBurndownChartGenerator/Trello-ExportJSON-2.png"
       alt="Trello Board - Print and Export">
</p>

3. Open Trello Burndown Chart Generator, and select the saved JSON file in file dialog

The burndown chart will be shown.

## Development Prerequisites
* Visual Studio 2015+

Before the build, generate-build-number.sh needs to be executed in a Git / Bash shell to generate build information code file (BuildInfo.cs).

## External Sources
Trello icon is from [Modern UI Icons](http://modernuiicons.com/), which is licensed under [CC BY-ND 3.0](https://github.com/Templarian/WindowsIcons/blob/master/WindowsPhone/license.txt).
