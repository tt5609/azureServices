CREATE TABLE [dbo].[Attendee]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(200) NOT NULL, 
    [Email] NVARCHAR(150) NOT NULL, 
    [IsJoiningBanquet] BIT NOT NULL DEFAULT 0, 
    [Remark] NVARCHAR(MAX) NULL
)
