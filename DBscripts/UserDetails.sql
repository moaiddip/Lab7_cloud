CREATE TABLE [dbo].[UserDetails] (
    [UserID]          INT   IDENTITY(1,1)       NOT NULL,
[Username] VARCHAR (20) NOT NULL,
    [FirstName]            VARCHAR (50) NOT NULL,
    [Surname]         VARCHAR (50) NOT NULL,
    [NativeLanguage] VARCHAR (30) NOT NULL,
    [LearningLanguage] VARCHAR (30) NOT NULL,
    [Grade]           VARCHAR (2)  NULL,
    PRIMARY KEY CLUSTERED ([UserID] ASC)
);