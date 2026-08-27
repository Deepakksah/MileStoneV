-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Ensure that DeviceId, Name is unique
UPDATE d
SET [Name] = [Name] + '(' + CAST(r AS NVARCHAR(100)) + ')'
FROM (
	SELECT [Name], ROW_NUMBER() OVER (PARTITION BY [IDDevice], [Name] ORDER BY name) AS r
  FROM [PatrollingProfiles]
) d
WHERE r > 1
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[PatrollingProfiles]') AND name = N'IX_UniqueNameOnDevice')
ALTER TABLE [PatrollingProfiles]
ADD CONSTRAINT [IX_UniqueNameOnDevice] UNIQUE NONCLUSTERED
(
	[IDDevice] ASC,
	[Name] ASC
) ON [PRIMARY]
GO
