IF NOT EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID(N'[dbo].[tblRoles]'))
EXEC dbo.sp_executesql @statement = N'
CREATE TABLE [dbo].[tblRoles](
  [Id] [int] IDENTITY PRIMARY KEY NOT NULL,
  [Name] [nvarchar](150) NOT NULL,
);'