-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
DELETE FROM [SecurityAccessEntryTimeProfile]
WHERE [is_default] = 0 AND [timeprofile_id] IS NULL 

DELETE FROM [SecurityAccessEntryTimeProfile]
WHERE [is_default] = 0 AND [timeprofile_id] NOT IN
(
	SELECT CONVERT(UNIQUEIDENTIFIER, pkid) FROM [TimeProfile]
)


ALTER TABLE [SecurityAccessEntryTimeProfile] 
ALTER COLUMN timeprofile_id GUIDTYPE NULL

-- Add foreign key to timeprofile
IF NOT EXISTS(SELECT * FROM sys.foreign_keys WHERE parent_object_id = OBJECT_ID(N'SecurityAccessEntryTimeProfile') AND [name] = 'FK_SecurityAccessEntryTimeProfile_TimeProfile')
ALTER TABLE [SecurityAccessEntryTimeProfile] WITH CHECK 
ADD CONSTRAINT [FK_SecurityAccessEntryTimeProfile_TimeProfile] FOREIGN KEY([timeprofile_id]) REFERENCES [TimeProfile] ([pkid])
GO

