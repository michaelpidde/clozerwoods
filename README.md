# Configuration
`clozerwoods.service` is placed under `/etc/systemd/system`

Note `EnvironmentFile=/var/www/cnf/clozerwoods_env.var` which necessitates creating that file and adding any necessary environment variables to it.

# Deploying Migrations
Generate a script with `dotnet ef migrations script --idempotent`

Run SQL on production instance.

# Random Stuff
Use unhex to add user:

    insert into users (Email, Password) values ('[EMAIL]', unhex(sha2('[PASSWORD]', 256)));