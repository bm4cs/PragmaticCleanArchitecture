
select *
from "__EFMigrationsHistory";

SELECT *
FROM apartments;

select *
from users;

SELECT *
FROM bookings;

SELECT *
FROM role;

SELECT *
FROM role_user;

SELECT *
FROM permission;


DELETE FROM role_user;
DELETE FROM role;
DELETE FROM users;


ALTER TABLE role DROP CONSTRAINT pk_role CASCADE;