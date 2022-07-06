IF NOT EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID(N'[dbo].[tblUserRoles]'))
EXEC dbo.sp_executesql @statement = N'
CREATE TABLE [dbo].[tblUserRoles](
  [UserId] [int] NOT NULL,
  [RoleId] [int] NOT NULL,

 CONSTRAINT [PK_tblUserRoles] PRIMARY KEY CLUSTERED 
 (  [UserId] ASC, [RoleId] ASC ),

 CONSTRAINT [FK_tblUserRoles_tblUsers] FOREIGN KEY([UserId])
  REFERENCES [dbo].[tblUsers] ([Id]),

 CONSTRAINT [FK_tblUserRoles_tblRoles] FOREIGN KEY([RoleId])
  REFERENCES [dbo].[tblRoles] ([Id])
);'