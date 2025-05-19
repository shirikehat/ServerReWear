use master
Go
IF EXISTS (SELECT * FROM sys.databases WHERE name = N'ReWear_DB')
BEGIN
    DROP DATABASE ReWear_DB;
END
Go

Create Database ReWear_DB
Go
use ReWear_DB
Go




-- Create a login for the admin user
CREATE LOGIN [AdminUser] WITH PASSWORD = 'admin123';
Go

-- Create a user in the TamiDB database for the login
CREATE USER [AdminUser] FOR LOGIN [AdminUser];
Go

-- Add the user to the db_owner role to grant admin privileges
ALTER ROLE db_owner ADD MEMBER [AdminUser];
Go


               USE master;
               DECLARE @latestBackupSet INT;
               SELECT TOP 1 @latestBackupSet = position
               FROM msdb.dbo.backupset
               WHERE database_name = 'ReWear_DB'
               AND backup_set_id IN (
                     SELECT backup_set_id
                     FROM msdb.dbo.backupmediafamily
                     WHERE physical_device_name = 'C:\Users\User\Source\Repos\shirikehat\ServerReWear\ServerReWear\wwwroot\..\DBScripts\backup.bak'
                 )
               ORDER BY backup_start_date DESC;
                ALTER DATABASE ReWear_DB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                RESTORE DATABASE ReWear_DB FROM DISK = 'C:\Users\User\Source\Repos\shirikehat\ServerReWear\ServerReWear\wwwroot\..\DBScripts\backup.bak' 
                WITH FILE=@latestBackupSet,
                REPLACE;
                ALTER DATABASE ReWear_DB SET MULTI_USER;

use ReWear_DB
Select * From Users
Select * From Status
Select * From Types
Select * From Products
select * from cart
select * from OrdersFrom
--EF Code
/*
scaffold-DbContext "Server = (localdb)\MSSQLLocalDB;Initial Catalog=ReWear_DB;User ID=AdminUser;Password=admin123;" Microsoft.EntityFrameworkCore.SqlServer -OutPutDir Models -Context ShiriDBContext -DataAnnotations -force
*/