USE rentdb;
GO
CREATE TABLE Favorite
(
    [Id] varchar(250) NOT NULL,
    [AccountId] varchar(250) NOT NULL,
    CONSTRAINT PK_Favorite PRIMARY KEY ([Id]),
)
GO
INSERT INTO Favorite VALUES ('c63e0813-d64b-4897-8174-f34881b3df7e', 'c9c51bd2-71a6-40f1-a98a-6b54481ceb47');
INSERT INTO Favorite VALUES ('2dc9ef15-ed56-4bbb-8883-51c5793446f9', 'c9c51bd2-71a6-40f1-a98a-6b54481ceb47');
INSERT INTO Favorite VALUES ('d003d8e5-1725-4e19-ad82-619cc5c1bb12', '802cb040-063a-493c-b3f4-fac3f1984748');
GO