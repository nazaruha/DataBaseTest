IF NOT EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID(N'[dbo].[tblUsersAndRoles]'))
EXEC dbo.sp_executesql @statement = N'
CREATE TABLE tblUsersAndRoles
(
    UserId int NOT NULL,
    RoleId int NOT NULL,
    CONSTRAINT [FK_tblUsersAndRoles_tblUsers] FOREIGN KEY([UserId])
    REFERENCES [dbo].[tblUsers]([Id]),
    CONSTRAINT [FK_tblUsersAndRoles_tblRoles] FOREIGN KEY([RoleId])
    REFERENCES [dbo].[tblRoles]([Id]),
    CONSTRAINT [PK_tblUsersAndRoles_1] PRIMARY KEY CLUSTERED 
    (
	    [UserId] ASC,
	    [RoleId] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY];'