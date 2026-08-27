-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Add [DriverName] computed columns to the [HardwareDefinition] table as VARCHAR(256)
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[HardwareDefinition]') AND name = N'DriverName')
BEGIN
	ALTER TABLE [HardwareDefinition]
	DROP COLUMN [DriverName]
END

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ExtractDriverName]') AND type in (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [ExtractDriverName]
GO

CREATE FUNCTION [dbo].[ExtractDriverName](@xml XML)
RETURNS VARCHAR(256)
WITH schemabinding
AS
BEGIN
	RETURN @xml.value('(/definition/driver/name)[1]', 'varchar(256)')
END
GO

ALTER TABLE HardwareDefinition 
ADD DriverName AS dbo.ExtractDriverName(Definition) PERSISTED
GO

-- Add [DriverVersion] computed columns to the [HardwareDefinition] table as VARCHAR(256)
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[HardwareDefinition]') AND name = N'DriverVersion')
BEGIN
	ALTER TABLE [HardwareDefinition]
	DROP COLUMN [DriverVersion]
END

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ExtractDriverVersion]') AND type in (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [ExtractDriverVersion]
GO

CREATE FUNCTION [dbo].[ExtractDriverVersion](@xml XML)
RETURNS VARCHAR(256)
WITH schemabinding
AS
BEGIN
	RETURN @xml.value('(/definition/driver/version)[1]', 'varchar(256)')
END
GO

ALTER TABLE HardwareDefinition 
ADD DriverVersion AS dbo.ExtractDriverVersion(Definition) PERSISTED
GO
