-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SequenceCounter]') AND type in (N'U'))
CREATE TABLE [SequenceCounter]
(
	[Name] VARCHAR(10), 
	[Value] INT, 
	[MaxValue] INT
) ON [PRIMARY]
GO


IF NOT EXISTS(SELECT * FROM [SequenceCounter] WHERE [Name] = 'BookmarkId')
INSERT INTO [SequenceCounter]([Name], [Value], [MaxValue])
VALUES('BookmarkId',0, 999999)
GO



