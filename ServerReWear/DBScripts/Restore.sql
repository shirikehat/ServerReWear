Use master
Go

-- Declare the database name
DECLARE @DatabaseName NVARCHAR(255) = 'ReWear_DB';

-- Generate and execute the kill commands for all active connections
DECLARE @KillCommand NVARCHAR(MAX);

SET @KillCommand = (
    SELECT STRING_AGG('KILL ' + CAST(session_id AS NVARCHAR), '; ')
    FROM sys.dm_exec_sessions
    WHERE database_id = DB_ID(@DatabaseName)
);

IF @KillCommand IS NOT NULL
BEGIN
    EXEC sp_executesql @KillCommand;
    PRINT 'All connections to the database have been terminated.';
END
ELSE
BEGIN
    PRINT 'No active connections to the database.';
END
Go

IF EXISTS (SELECT * FROM sys.databases WHERE name = N'ReWear_DB')
BEGIN
    DROP DATABASE ReWear_DB;
END
Go

-- Create a login for the admin user
CREATE LOGIN [AdminLogin] WITH PASSWORD = 'admin123';
Go


--so user can restore the DB!
ALTER SERVER ROLE sysadmin ADD MEMBER [AdminLogin];
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
                WITH 
                REPLACE;
                ALTER DATABASE ReWear_DB SET MULTI_USER;

use ReWear_DB
Go


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