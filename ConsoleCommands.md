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

| Short Name | Long Name | Description | Required |
| --- | --- | --- | --- |
| -s  | -silent | If flag is provided, all non-necessary user interaction will be skipped and default values will be provided where not available. | False |
| -t  | -title | The title of the suggestion. | False |
| -d  | -description | The description of the suggestion. | False |

#### Example Usage

- No Parameters: `./ProjectTools.exe report-issue`
- With Title and Description: `./ProjectTools.exe report-issue -t "My Title Here" -d "My Description Here"`
- With Title and Description, no check: `./ProjectTools.exe report-issue -s -t "My Title Here" -d "My Description Here"`

### `suggestion`

Allows a user to submit a suggestion for the application.

#### Available Parameters

| Short Name | Long Name | Description | Required |
| --- | --- | --- | --- |
| -s  | -silent | If flag is provided, all non-necessary user interaction will be skipped and default values will be provided where not available. | False |
| -t  | -title | The title of the suggestion. | False |
| -d  | -description | The description of the suggestion. | False |

#### Example Usage

- No Parameters: `./ProjectTools.exe suggestion`
- With Title and Description: `./ProjectTools.exe suggestion -t "My Title Here" -d "My Description Here"`
- With Title and Description, no check: `./ProjectTools.exe suggestion -s -t "My Title Here" -d "My Description Here"`

## Templating Commands

### `update-templates`

Updates the locally downloaded templates with the latest versions from the template repository(ies).

#### Available Parameters

| Short Name | Long Name | Description | Required |
| --- | --- | --- | --- |
| -s  | -silent | If flag is provided, all non-necessary user interaction will be skipped and default values will be provided where not available. | False |
| -i  | -ignore-cache | If flag is provided, the program will force-check for new templates and ignore the cached data. | False |
| -f  | -force | If flag is provided, the program will force all templates to redownload. | False |

#### Example Usage

- No Parameters: `./ProjectTools.exe update-templates`
- Force Redownload all Templates: `./ProjectTools.exe update-templates -f`
- Ignore Cache: `./ProjectTools.exe update-templates i`
- Force Redownload all Templates and ignore cache, with no user interaction: `./ProjectTools.exe update-templates -f -i -s`

### `list-templates`

Lists all locally available templates

#### Available Parameters

| Short Name | Long Name | Description | Required |
| --- | --- | --- | --- |
| -q  | -quick-info | If flag is provided, the program will just list the template names and not details on the templates. | False |

#### Example Usage

- No Parameters: `./ProjectTools.exe list-templates`
- Listing just template names: `./ProjectTools.exe list-templates -q`

### `prepare`

Prepares a directory by creating a new template from it.

#### Available Parameters

| Short Name | Long Name | Description | Required |
| --- | --- | --- | --- |
| -d  | -directory | The directory to prepare as a template. | True |
| -o  | -output-directory | The output directory to place the template into. | False |
| -c  | -skip-cleaning | If flag is provided, the working directory won't be deleted at the end of the prepare process. | False |
| -t  | -type | The type of the solution to prepare. Defaults to auto. | False |

##### Available Options for `-t`/`-type`

| Value | Description |
| --- | --- |
| auto | Automatically determines the type of solution to prepare. |
| DotSln | A Visual Studio (.sln) solution. |

#### Example Usage

- Minimal Parameters: `./ProjectTools.exe prepare -d "Path To My Solution" -o "Template Output Path"`

### `generate`

Coming Soon!

## Tooling Commands

Coming Soon!
