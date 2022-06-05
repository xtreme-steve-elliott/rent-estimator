CREATE TABLE [dbo].[Account]
(
    [Id] varchar(250) NOT NULL,
    [FirstName] varchar(200) NULL,
    [LastName] varchar(200) NULL,
    [Username] varchar(200) NULL,
    [Password] varchar(200) NULL,
    [CreatedAt] datetimeoffset DEFAULT SYSDATETIMEOFFSET(),
    [LastUpdatedAt] datetimeoffset  DEFAULT SYSDATETIMEOFFSET(),
    CONSTRAINT PK_Item PRIMARY KEY ([Id])
)
GO