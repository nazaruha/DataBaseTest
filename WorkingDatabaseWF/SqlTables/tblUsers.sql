IF NOT EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID(N'[dbo].[tblUsers]'))
EXEC dbo.sp_executesql @statement = N'
CREATE TABLE [dbo].[tblUsers](
  [Id] [int] IDENTITY PRIMARY KEY NOT NULL,
  [Name] [nvarchar](150) NOT NULL,
);'