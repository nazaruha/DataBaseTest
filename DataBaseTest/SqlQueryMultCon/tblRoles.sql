IF NOT EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID(N'[dbo].[tblRoles]'))
EXEC dbo.sp_executesql @statement = N'
CREATE TABLE tblRoles
(
    Id int IDENTITY PRIMARY KEY NOT NULL,
    Role nvarchar(150) NOT NULL,
);'