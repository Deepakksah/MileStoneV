-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS(SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Devices]') AND name = N'MulticastEnabled')
BEGIN
    EXEC('
        UPDATE [Devices]
        SET [MulticastSettings] =
            CASE
                WHEN [MulticastSettings].value(''(properties/settings/setting[name/text() = "Enabled"]/value)[1]'', ''VARCHAR(10)'') = ''True'' THEN ''1''
                ELSE ''0''
            END
    ')

    ALTER TABLE [Devices] ALTER COLUMN [MulticastSettings] NVARCHAR(1) NOT NULL

    ALTER TABLE [Devices] ALTER COLUMN [MulticastSettings] BIT NOT NULL

    EXEC sp_rename 'dbo.Devices.MulticastSettings', 'MulticastEnabled', 'COLUMN';
END

GO