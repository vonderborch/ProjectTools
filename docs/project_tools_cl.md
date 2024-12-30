## Available Commands

| Command                | Description                                                                                 |
|------------------------|---------------------------------------------------------------------------------------------|
| generate               | Generate a new project from a template.                                                     |
| prepare                | Prepare a new template.                                                                     |
| list-templates         | Lists all templates available to use.                                                       |
| list-template-builders | Get information about what template builders are available to use when preparing templates. |
| update-templates       | Checks for any new or updated templates at the configured template repositories.            |
| configure              | Configure application settings.                                                             |
| update                 | Checks for updates to the program.                                                          |
| suggestion             | Make a suggestion for the program.                                                          |
| report-issue           | Report an issue with the program.                                                           |
| about                  | Get information about the program.                                                          |

## Commands

### generate

A command to generate a new project from a template.

#### Parameters

| Name                    | Short Name | Is Required | Default Value | Type   | Description                                                                                                |
|-------------------------|------------|-------------|---------------|--------|------------------------------------------------------------------------------------------------------------|
| force                   | f          | false       | false         | bool   | Overrides the existing directory if it already exists.                                                     |
| generate-project-config | c          | false       |               | string | The specific generate project configuration file to use.                                                   |
| name                    | n          | true        |               | string | The name of the project.                                                                                   |
| parent-output-directory | o          | true        |               | string | The parent output directory for the new solution.                                                          |
| remove-existing-config  | r          | false       | false         | bool   | If flag is provided, we will delete the existing project generation config file before execute anything.   |
| template                | t          | true        |               | string | The template to use.                                                                                       |
| what-if                 | i          | false       | false         | bool   | If flag is provided, the solution will not be generated, but the user will be guided through all settings. |

### prepare

A command to prepare a new template.

#### Parameters

| Name             | Short Name | Is Required | Default Value | Type   | Description                                                                                               |
|------------------|------------|-------------|---------------|--------|-----------------------------------------------------------------------------------------------------------|
| directory        | d          | true        |               | string | The directory to prepare as a template.                                                                   |
| force            | f          | false       | false         | bool   | If flag is provided, any existing template will be overriden.                                             |
| output-directory | o          | true        |               | string | The output directory to place the template into.                                                          |
| skip-cleaning    | c          | false       | false         | bool   | If flag is provided, the working directory won't be deleted at the end of the prepare process.            |
| template-builder | t          | false       | auto          | string | The type of template builder to use to prepare the template. Defaults to auto.                            |
| what-if          | i          | false       | false         | bool   | If flag is provided, the template will not be prepared, but the user will be guided through all settings. |

NOTE: For a list of available template builders, you can execute the `list-template-builders` command with the program.

### list-templates

A command that lists all templates available to use.

#### Parameters

| Name | Short Name | Is Required | Default Value | Type | Description                                                          |
|------|------------|-------------|---------------|------|----------------------------------------------------------------------|
| full | f          | false       | false         | bool | If flag is set, full information on the templates will be displayed. |

### list-templates-builders

A command that gets information about what template builders are available to use when preparing templates.

#### Parameters

| Name | Short Name | Is Required | Default Value | Type | Description                                                                  |
|------|------------|-------------|---------------|------|------------------------------------------------------------------------------|
| full | f          | false       | false         | bool | If flag is set, full information on the template builders will be displayed. |

### update-templates

A command that checks for any new or updated templates at the configured template repositories. This command will
automatically run at a configured time interval based on the settings for the program, although it can be manually
forced to check for updates using the `-f` flag.

#### Parameters

| Name             | Short Name | Is Required | Default Value | Type | Description                                                                                                                      |
|------------------|------------|-------------|---------------|------|----------------------------------------------------------------------------------------------------------------------------------|
| force-check      | f          | false       | false         | bool | If flag is provided, the program will force-check for new templates and ignore the cached data.                                  |
| force-redownload | r          | false       | false         | bool | If flag is provided, the program will force all templates to re-download.                                                        |
| silent           | s          | false       | false         | bool | If flag is provided, all non-necessary user interaction will be skipped and default values will be provided where not available. |

### configure

A command to configure application settings.

NOTE: Not all settings are configurable through this command. Some settings may require manual editing of the settings
file located in the ProjectTools documents directory.

#### Parameters

N/A

### update

A command that checks for updates to the program. This command will
automatically run at a configured time interval based on the settings for the program, although it can be manually
forced to check for updates using the `-f` flag.

#### Parameters

| Name        | Short Name | Is Required | Default Value | Type | Description                                                                |
|-------------|------------|-------------|---------------|------|----------------------------------------------------------------------------|
| force-check | f          | false       | false         | bool | If flag is provided, the program will force-check for application updates. |

### suggestion

A command for making a suggestion for the program.

#### Parameters

| Name        | Short Name | Is Required | Default Value | Type   | Description                                                                                                                      |
|-------------|------------|-------------|---------------|--------|----------------------------------------------------------------------------------------------------------------------------------|
| description | d          | false       |               | string | The description of the new feature or functionality.                                                                             |
| silent      | s          | false       | false         | bool   | If flag is provided, all non-necessary user interaction will be skipped and default values will be provided where not available. |
| title       | t          | false       |               | string | The title of the new feature or functionality.                                                                                   |

### report-issue

A command for reporting an issue with the program.

#### Parameters

| Name        | Short Name | Is Required | Default Value | Type   | Description                                                                                                                      |
|-------------|------------|-------------|---------------|--------|----------------------------------------------------------------------------------------------------------------------------------|
| description | d          | false       |               | string | The description of the issue.                                                                                                    |
| silent      | s          | false       | false         | bool   | If flag is provided, all non-necessary user interaction will be skipped and default values will be provided where not available. |
| title       | t          | false       |               | string | The title of the issue.                                                                                                          |

### about

A command for getting information about the program.

#### Parameters

N/A
