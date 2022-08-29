`clozerwoods.service` is placed under `/etc/systemd/system`
Note `EnvironmentFile=/var/www/cnf/clozerwoods_env.var` which necessitates creating that file and adding any necessary environment variables to it.

Use unhex to add user:

    insert into users (Email, Password) values ('[EMAIL]', unhex(sha2('[PASSWORD]', 256)));