IF NOT EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID(N'[dbo].[tblCities]'))
EXEC dbo.sp_executesql @statement = N'
CREATE TABLE [dbo].[tblCities](
  [Id] [int] IDENTITY PRIMARY KEY NOT NULL,
  [RegionId] [int] NOT NULL,
  [Name] [nvarchar](150) NOT NULL,
  CONSTRAINT [FK_tblCities_tblRegions] FOREIGN KEY([RegionId])
    REFERENCES [dbo].[tblRegions] ([Id]),
);'