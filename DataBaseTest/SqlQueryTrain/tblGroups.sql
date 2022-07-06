IF NOT EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID(N'[dbo].[tblGroups]'))
EXEC dbo.sp_executesql @statement = N'
CREATE TABLE [dbo].[tblGroups](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
	[DateCreate] [date] NOT NULL,
);'