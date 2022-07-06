IF NOT EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID(N'[dbo].[tblUserAdresses]'))
EXEC dbo.sp_executesql @statement = N'
CREATE TABLE [dbo].[tblUserAdresses](
  [UserId] [int] NOT NULL,
  [CityId] [int] NOT NULL,
  [Street] [nvarchar](150) NOT NULL,
  [HouseNumber] [nvarchar](50) NOT NULL,

 CONSTRAINT [PK_tblUserAdresses] PRIMARY KEY CLUSTERED 
 (  [UserId] ASC ),

 CONSTRAINT [FK_tblUserAdresses_tblUsers] FOREIGN KEY([UserId])
  REFERENCES [dbo].[tblUsers] ([Id]),

 CONSTRAINT [FK_tblUserAdresses_tblCities] FOREIGN KEY([CityId])
  REFERENCES [dbo].[tblCities] ([Id])
);'