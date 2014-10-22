CREATE TABLE [dbo].[Attendee]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(200) NOT NULL, 
    [Email] NVARCHAR(150) NOT NULL, 
	[NumberOfAttendee] int,
    [AttendBanquet] BIT NOT NULL DEFAULT 0, 
    [Comment] NVARCHAR(MAX) NULL,
	[LastUpdated] DateTime DEFAULT getdate()
)
