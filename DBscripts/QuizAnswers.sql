CREATE TABLE [dbo].[QuizAnswers] (
    [ID]     INT          IDENTITY (1, 1) NOT NULL,
    [UserID] VARCHAR (50) NOT NULL,
    [Q_word] VARCHAR (50) NOT NULL,
    [A_word] VARCHAR (50),
    PRIMARY KEY CLUSTERED ([ID] ASC)
);