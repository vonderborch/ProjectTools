Welcome to the ProjectTools wiki!

## What Is "ProjectTools"?

ProjectTools is a program, available in a command-line interface (ProjectTools.CL) or a graphical user interface
(ProjectTools.App) (NOTE: Coming soon), that is designed to help with creating templates for projects and using those
templates to create new projects or extend existing projects, similar to cookie-cutter. This was built out mostly due to
my annoyance with how templates are handled in Visual Studio and because I wanted to build out my own solution to this
problem for myself.

## ProjectTools.CL

ProjectTools.CL is a command-line interface that is designed to be used in a terminal or command prompt.

## ProjectTools.App

Documentation coming soon!

## Program File Location

All program files (templates, settings, etc.) are located in the ProjectTools documents directory for the user running
the program.

On Windows, this is typically located at `C:\Users\<username>\Documents\ProjectTools`.

On Linux, this is typically located at `/home/<username>/Documents/ProjectTools`.

On MacOS, this is typically located at `/Users/<username>/Documents/ProjectTools`.

### Settings File

The settings file is `settings.json`. It is a JSON file that contains all of the settings for the program. A definition
of the available settings is below (last updated for settings version 1.6, 2024-12-30):

Example Settings File:
```json
{
  "GitSourcesAndAccessTokens": {
    "https://github.com/": "my_token_here"
  },
  "LastAppUpdateCheck": {
    "ProjectTools.CL": "0001-01-01T00:00:00",
    "ProjectTools.App": "0001-01-01T00:00:00"
  },
  "LastTemplatesUpdateCheck": "0001-01-01T00:00:00",
  "PythonVersion": "3.12.8",
  "RepositoriesAndGitSources": {
    "https://github.com/vonderborch/ProjectTools": "https://github.com/"
  },
  "SecondsBetweenAppUpdateChecks": 86400,
  "SecondsBetweenTemplateUpdateChecks": 86400,
  "SettingsVersion": "1.6"
}
```

#### GitSourcesAndAccessTokens

A dictionary of Git sources and their access tokens. This is used to authenticate with Git sources when downloading new
templates or checking for updates to the application. A source and token for `https://github.com` is required due to the
application checks.

#### LastAppUpdateCheck

The last time an update check was performed for the application. This is used to determine when the next update check
should be performed, in conjunction with the `SecondsBetweenAppUpdateChecks` setting.

#### LastTemplatesUpdateCheck

The last time an update check was performed for the templates. This is used to determine when the next update check
should be performed, in conjunction with the `SecondsBetweenTemplateUpdateChecks` setting.

#### PythonVersion

The version of Python that the program should use when running Python scripts during project generation. This is used to
determine if the locally cached Python installation used by the program needs to be updated. NOTE: a version of Python
is provided with the program.

#### RepositoriesAndGitSources

A dictionary of repositories and their associated Git sources. This is used to determine where to look for templates
when downloading new templates or checking for updates to the templates. Each github source should match a source in the
`GitSourcesAndAccessTokens` setting.

#### SecondsBetweenAppUpdateChecks

The number of seconds between update checks for the application. This is used in conjunction with the
`LastAppUpdateCheck` setting to determine when the next update check should be performed. The default is 86400 seconds
(24 hours).

#### SecondsBetweenTemplateUpdateChecks

The number of seconds between update checks for the templates. This is used in conjunction with the
`LastTemplatesUpdateCheck` setting to determine when the next update check should be performed. The default is 86400
seconds (24 hours).

#### SettingsVersion

The version of the settings file. This is used to determine if the settings file needs to be updated when the program is
updated. The current version is `1.6`. Settings files will be updated automatically when a new settings version is used
after a program update, if possible.