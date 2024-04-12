# Console Commands

Here is a list of all the console commands that are currently available in the program:

## Basic Commands

### `configure`

Configures the application.

#### Available Parameters

N/A

#### Example Usage

`./ProjectTools.exe configure`

### `help`

Displays help information.

#### Available Parameters

N/A

#### Example Usage

`./ProjectTools.exe help`

### `report-issue`

Allows a user to report an issue with the application.

#### Available Parameters

| Short Name | Long Name    | Description                                                                                                                      | Required |
|------------|--------------|----------------------------------------------------------------------------------------------------------------------------------|----------|
| -s         | -silent      | If flag is provided, all non-necessary user interaction will be skipped and default values will be provided where not available. | False    |
| -t         | -title       | The title of the suggestion.                                                                                                     | False    |
| -d         | -description | The description of the suggestion.                                                                                               | False    |

#### Example Usage

- No Parameters: `./ProjectTools.exe report-issue`
- With Title and Description: `./ProjectTools.exe report-issue -t "My Title Here" -d "My Description Here"`
- With Title and Description, no check: `./ProjectTools.exe report-issue -s -t "My Title Here" -d "My Description Here"`

### `suggestion`

Allows a user to submit a suggestion for the application.

#### Available Parameters

| Short Name | Long Name    | Description                                                                                                                      | Required |
|------------|--------------|----------------------------------------------------------------------------------------------------------------------------------|----------|
| -s         | -silent      | If flag is provided, all non-necessary user interaction will be skipped and default values will be provided where not available. | False    |
| -t         | -title       | The title of the suggestion.                                                                                                     | False    |
| -d         | -description | The description of the suggestion.                                                                                               | False    |

#### Example Usage

- No Parameters: `./ProjectTools.exe suggestion`
- With Title and Description: `./ProjectTools.exe suggestion -t "My Title Here" -d "My Description Here"`
- With Title and Description, no check: `./ProjectTools.exe suggestion -s -t "My Title Here" -d "My Description Here"`

## Templating Commands

### `update-templates`

Updates the locally downloaded templates with the latest versions from the template repository(ies).

#### Available Parameters

| Short Name | Long Name     | Description                                                                                                                      | Required |
|------------|---------------|----------------------------------------------------------------------------------------------------------------------------------|----------|
| -s         | -silent       | If flag is provided, all non-necessary user interaction will be skipped and default values will be provided where not available. | False    |
| -i         | -ignore-cache | If flag is provided, the program will force-check for new templates and ignore the cached data.                                  | False    |
| -f         | -force        | If flag is provided, the program will force all templates to redownload.                                                         | False    |

#### Example Usage

- No Parameters: `./ProjectTools.exe update-templates`
- Force Redownload all Templates: `./ProjectTools.exe update-templates -f`
- Ignore Cache: `./ProjectTools.exe update-templates i`
- Force Redownload all Templates and ignore cache, with no user
  interaction: `./ProjectTools.exe update-templates -f -i -s`

### `list-templates`

Lists all locally available templates

#### Available Parameters

| Short Name | Long Name   | Description                                                                                          | Required |
|------------|-------------|------------------------------------------------------------------------------------------------------|----------|
| -q         | -quick-info | If flag is provided, the program will just list the template names and not details on the templates. | False    |

#### Example Usage

- No Parameters: `./ProjectTools.exe list-templates`
- Listing just template names: `./ProjectTools.exe list-templates -q`

### `prepare`

Prepares a directory by creating a new template from it.

#### Available Parameters

| Short Name | Long Name         | Description                                                                                                                  | Required |
|------------|-------------------|------------------------------------------------------------------------------------------------------------------------------|----------|
| -d         | -directory        | The directory to prepare as a template.                                                                                      | True     |
| -o         | -output-directory | The output directory to place the template into.                                                                             | False    |
| -c         | -skip-cleaning    | If flag is provided, the working directory won't be deleted at the end of the prepare process.                               | False    |
| -t         | -type             | The type of the solution to prepare. Defaults to auto.                                                                       | False    |
| -i         | -what-if          | If flag is provided, the program will not actually generate the project, but will instead just show what would be generated. | False    |

##### Available Options for `-t`/`-type`

| Value  | Description                                               |
|--------|-----------------------------------------------------------|
| auto   | Automatically determines the type of solution to prepare. |
| DotSln | A Visual Studio (.sln) solution.                          |

#### Example Usage

- Minimal Parameters: `./ProjectTools.exe prepare -d "Path To My Solution" -o "Template Output Path"`

### `generate`

Generates a new project from a template.

#### Available Parameters

| Short Name | Long Name                         | Description                                                                                                                  | Required |
|------------|-----------------------------------|------------------------------------------------------------------------------------------------------------------------------|----------|
| -n         | -name                             | The name of the project to generate.                                                                                         | True     |
| -o         | -output-directory                 | The output directory to place the template into.                                                                             | True     |
| -t         | -template                         | The template to use to generate the project.                                                                                 | True     |
| -f         | -force                            | If flag is provided, the program will force the generation of the project, even if the output directory is not empty.        | False    |
| -c         | -solution-config                  | The configuration to use when generating the solution. If not provided, will use a default-named file.                       | False    |
| -p         | -preserve-default-solution-config | If flag is provided, the program will not cleanup the default solution configuration file.                                   | False    |
| -i         | -what-if                          | If flag is provided, the program will not actually generate the project, but will instead just show what would be generated. | False    |

##### Available Options for `-t`/`-template`

`-t`/`-template` supports any template that is available locally. To see a list of available templates,
run `./ProjectTools.exe list-templates`.

#### Example Usage

- Minimal
  Parameters: `./ProjectTools.exe generate -n "My Solution Name" -t "My Template To Use" -o "Template Output Path"`

## Tooling Commands

Coming Soon!
