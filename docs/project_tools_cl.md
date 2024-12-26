## Available Commands

| Command                | Description                                                 |
|------------------------|-------------------------------------------------------------|
| generate               | Generate a new project from a template.                     |
| prepare                | Prepare a template.                                         |
| list-templates         | Lists all templates available to use.                       |
| list-template-builders | Get information about what template builders are available. |
| update-templates       | Lists all templates available to use.                       |
| configure              | Configure application settings.                             |
| update                 | Checks for updates to the program.                          |
| suggestion             | Make a suggestion for the program.                          |
| report-issue           | Report an issue with the program.                           |
| about                  | Get information about the program.                          |

## Commands

### generate

#### Parameters

| Name                    | Short Name | Is Required | Default Value | Type   | Description                                                                                                |
|-------------------------|------------|-------------|---------------|--------|------------------------------------------------------------------------------------------------------------|
| force                   | f          | false       | false         | bool   | Overrides the existing directory if it already exists.                                                     |
| parent-output-directory | o          | true        |               | string | The parent output directory for the new solution.                                                          |
| template                | t          | true        |               | string | The template to use.                                                                                       |
| name                    | n          | true        |               | string | The name of the project.                                                                                   |
| generate-project-config | c          | false       |               | string | The specific generate project configuration file to use.                                                   |
| what-if                 | i          | false       | false         | bool   | If flag is provided, the solution will not be generated, but the user will be guided through all settings. |
| remove-existing-config  | r          | false       | false         | bool   | If flag is provided, we will delete the existing project generation config file before execute anything.   |

### prepare

#### Parameters

| Name                    | Short Name | Is Required | Default Value | Type   | Description                                                                                                |
|-------------------------|------------|-------------|---------------|--------|------------------------------------------------------------------------------------------------------------|
| force                   | f          | false       | false         | bool   | If flag is provided, any existing template will be overriden.                                              |
| parent-output-directory | o          | true        |               | string | The parent output directory for the new solution.                                                          |
| template                | t          | true        |               | string | The template to use.                                                                                       |
| name                    | n          | true        |               | string | The name of the project.                                                                                   |
| generate-project-config | c          | false       |               | string | The specific generate project configuration file to use.                                                   |
| what-if                 | i          | false       | false         | bool   | If flag is provided, the solution will not be generated, but the user will be guided through all settings. |
| remove-existing-config  | r          | false       | false         | bool   | If flag is provided, we will delete the existing project generation config file before execute anything.   |


WIP
