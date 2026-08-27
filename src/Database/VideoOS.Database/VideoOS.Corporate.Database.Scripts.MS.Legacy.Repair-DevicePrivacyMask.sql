-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @OldDate DateTime
SET @OldDate = '1980-01-01T10:00:00'

-- For devices where the '1980' row has been deleted, make a '1980' copy of the oldest row
INSERT [DevicePrivacyMask] ([LastModified], [IDDevice], [PrivacyMask], [Enabled])
SELECT
	@OldDate, [IDDevice], [PrivacyMask], [Enabled]
FROM
	(
		SELECT *, ROW_NUMBER() OVER (PARTITION BY [IDDevice] ORDER BY [LastModified] ASC) [RowIndex]
		FROM [DevicePrivacyMask] [DP]
	) [DP2]
WHERE
	[DP2].[RowIndex] = 1
	AND NOT EXISTS (SELECT 1 FROM [DevicePrivacyMask] [DP3] WHERE [DP3].[IDDevice] = [DP2].[IDDevice] AND [DP3].[LastModified] = @OldDate)
GO