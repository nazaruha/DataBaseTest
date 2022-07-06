IF NOT EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID(N'[dbo].[tblRegions]'))
EXEC dbo.sp_executesql @statement = N'
CREATE TABLE [dbo].[tblRegions](
  [Id] [int] IDENTITY PRIMARY KEY NOT NULL,
  [Name] [nvarchar](150) NULL,
);'