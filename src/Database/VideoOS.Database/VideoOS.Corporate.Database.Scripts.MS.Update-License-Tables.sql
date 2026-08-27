-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================



IF (EXISTS(SELECT 1 FROM information_schema.table_constraints WHERE CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME = 'LicenseDevice' AND CONSTRAINT_NAME = 'PK_LicenseDevice'))
BEGIN
	ALTER TABLE [dbo].[LicenseDevice] DROP CONSTRAINT [PK_LicenseDevice];
END

GO

IF (EXISTS(SELECT 1 FROM information_schema.table_constraints WHERE CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME = 'LicenseHardware' AND CONSTRAINT_NAME = 'PK_LicenseHardware'))
BEGIN
	ALTER TABLE [dbo].[LicenseHardware] DROP CONSTRAINT [PK_LicenseHardware];
END

GO

CREATE FUNCTION [dbo].[fn_bin_TO_BASE64]
(
    @STRING VARBINARY(MAX)
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
    RETURN (
        SELECT
            CAST(N'' AS XML).value(
                  'xs:base64Binary(xs:hexBinary(sql:column("bin")))'
                , 'NVARCHAR(MAX)'
            )   Base64Encoding
        FROM (
            SELECT @STRING AS bin
        ) AS bin_sql_server_temp
    )
END
GO


IF (NOT EXISTS(SELECT 1
    FROM   INFORMATION_SCHEMA.COLUMNS
    WHERE  TABLE_NAME = 'LicenseDevice'
            AND COLUMN_NAME = 'UniqueId'))
BEGIN
	ALTER TABLE [LicenseDevice]
	ADD [UniqueId] VARCHAR(700);
END

GO

IF (NOT EXISTS(SELECT 1
    FROM   INFORMATION_SCHEMA.COLUMNS
    WHERE  TABLE_NAME = 'LicenseHardware'
            AND COLUMN_NAME = 'UniqueId'))
BEGIN
	ALTER TABLE [LicenseHardware]
	ADD [UniqueId] VARCHAR(700);
END

GO

UPDATE lh
SET lh.[UniqueId] = h.MacAddress
FROM LicenseHardware lh
	JOIN Hardware h ON (lh.IDHardware = h.IDHardware);

GO

UPDATE ld
SET ld.UniqueId = [dbo].[fn_bin_TO_BASE64](HASHBYTES('SHA2_256', h.MacAddress + ':' + d.DeviceType + ':' + CAST(d.DeviceIndex AS VARCHAR(20))))
FROM [Devices] d
	JOIN [Hardware] h ON (h.IDHardware = d.IDHardware)
	JOIN LicenseDevice ld ON (ld.IDDevice = d.IDDevice);

GO



DROP FUNCTION [dbo].[fn_bin_TO_BASE64];
GO


ALTER TABLE [dbo].[LicenseDevice]
ALTER COLUMN [UniqueId] VARCHAR(700) NOT NULL;
GO


ALTER TABLE [dbo].[LicenseHardware]
ALTER COLUMN [UniqueId] VARCHAR(700) NOT NULL;
GO



IF (NOT EXISTS(SELECT 1 FROM information_schema.table_constraints WHERE CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME = 'LicenseDevice' AND CONSTRAINT_NAME = 'PK_LicenseDevice_UniqueId_Id'))
BEGIN
	ALTER TABLE [dbo].[LicenseDevice] ADD  CONSTRAINT [PK_LicenseDevice_UniqueId_Id] PRIMARY KEY NONCLUSTERED 
	(
		[IDDevice] ASC, [UniqueId]
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF,ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
END
GO



IF (NOT EXISTS(SELECT 1 FROM information_schema.table_constraints WHERE CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME = 'LicenseHardware' AND CONSTRAINT_NAME = 'PK_LicenseHardware_UniqueId_Id'))
BEGIN
	ALTER TABLE [dbo].[LicenseHardware] ADD  CONSTRAINT [PK_LicenseHardware_UniqueId_Id] PRIMARY KEY NONCLUSTERED 
	(
		[IDHardware] ASC, [UniqueId]
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
END
GO
