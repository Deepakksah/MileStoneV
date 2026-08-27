-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

ALTER TABLE [SequenceCounter] ALTER COLUMN [Name] NVARCHAR(30) NOT NULL

IF NOT EXISTS(SELECT * FROM [SequenceCounter] WHERE [Name] = 'MarkedDataId')
BEGIN
	INSERT INTO [SequenceCounter] ([Name], [Value], [MaxValue])	VALUES('MarkedDataId',0, 999999)
END
GO



