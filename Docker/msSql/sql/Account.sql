USE rentdb;
GO
CREATE TABLE Account
(
    [Id] varchar(250) NOT NULL,
    [FirstName] varchar(200) NULL,
    [LastName] varchar(200) NULL,
    [Username] varchar(200) NULL,
    [Password] varchar(200) NULL,
    CONSTRAINT PK_Item PRIMARY KEY ([Id])
)
GO
INSERT INTO Account VALUES ('c9c51bd2-71a6-40f1-a98a-6b54481ceb47', 'George', 'Testerson', 'gtest', 'gtestpass123');
INSERT INTO Account VALUES ('802cb040-063a-493c-b3f4-fac3f1984748', 'Jon', 'Testerson', 'jtest', 'jtestpass123');
GO