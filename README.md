Use unhex to add user:

    insert into users (Email, Password) values ('[EMAIL]', unhex(sha2('[PASSWORD]', 256)));