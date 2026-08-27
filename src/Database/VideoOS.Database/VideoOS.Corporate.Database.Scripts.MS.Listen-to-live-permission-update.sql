-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================


DECLARE @CurrentTime DATETIME = GETUTCDATE()


-- Device legacy security permissions
UPDATE  [SecurityAttributes]
SET     [LastModified] = @CurrentTime,
        [Name] = 'Listen to restricted live',
        [Description] = 'Listen to restricted live'
WHERE   [SecurityAttributeID] IN ('DEVICEMICLIVEVIEWRESTRICTED', 'DEVICESPEAKERLIVEVIEWRESTRICTED')

GO
