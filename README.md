# ProjectTools
Wanting to make a template from a project but frustrated at Visual Studio for making creating templates from complex solutions complicated (especially once you start trying to add custom logic to things...)? So was I. This isn't a perfect solution, but it at least makes things easier!

This is my ~~[first](https://github.com/vonderborch/SolutionCreator)~~ ~~[second](https://github.com/vonderborch/Templater)~~ third attempt at this and has been rewritten from the ground up to have much less spaghetti code everywhere.

# Default Templates

The default location templates are stored is: https://github.com/vonderborch/ProjectTools/tree/main/Templates/TEMPLATES

Other repos can be configured to be used as well by updating the config to point to the repo(s). Templates must be fairly close to the root of the repo (< 3 subfolders deep) for the app to find them, and the repo must be public (or you must have access to it).

# Download
The latest release is available on the releases page: https://github.com/vonderborch/ProjectTools/releases/latest

# Execution

## App/GUI Version

Coming eventually!

## Command Prompt/Terminal Version

See the [Console Commands](https://github.com/vonderborch/ProjectTools/blob/main/ConsoleCommands.md) file for available commands.

# Existing Templates
The application will automatically download templates from the [Template Repository](https://github.com/vonderborch/ProjectTools/tree/main/Templates/TEMPLATES) by default. Currently available templates are:
- Velentr.BASE: A simple library that isn't tied to anything XNA related
- Velentr.BASE_DUAL_SUPPORT: A library that has two different implementations: one for FNA and one for Monogame
- Velentr.GENERIC_DUAL_SUPPORT: A library that has one generic implementation (not tied to FNA or Monogame) and then either extensions or custom implementations for FNA and Monogame

# Creating a New Template
Create a solution, then run `./ProjectTools.exe prepare` against the solution!

# Known Issues
- Brittle, not a lot of validation checking means it can crash easily and won't tell you what went wrong too well...

Have an issue? https://github.com/vonderborch/ProjectTools/issues

# Future Plans
See list of issues under the Milestones: https://github.com/vonderborch/ProjectTools/milestones
