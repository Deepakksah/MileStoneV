-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
-- Add [MacAddress] computed columns to the [Hardware] table as VARCHAR(256)
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Hardware]') AND name = N'MacAddress')
BEGIN
	ALTER TABLE [Hardware]
	DROP COLUMN [MacAddress]
END

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ExtractMacAddress]') AND type in (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [ExtractMacAddress]
GO

CREATE FUNCTION [dbo].[ExtractMacAddress](@xml XML)
RETURNS VARCHAR(256)
WITH schemabinding
AS
BEGIN
	RETURN @xml.value('(/properties/settings[@hwid="hardware"]/setting[@typeid="F9D27CE8-8FA9-4B91-8C85-3E80DE7EF0AB"]/value)[1]', 'varchar(256)')
END
GO

ALTER TABLE Hardware
ADD MacAddress AS dbo.ExtractMacAddress(Settings) PERSISTED
GO

-- Add [SerialNumber] computed columns to the [Hardware] table as VARCHAR(256)
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Hardware]') AND name = N'SerialNumber')
BEGIN
	ALTER TABLE [Hardware]
	DROP COLUMN [SerialNumber]
END

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ExtractSerialNumber]') AND type in (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [ExtractSerialNumber]
GO

CREATE FUNCTION [dbo].[ExtractSerialNumber](@xml XML)
RETURNS VARCHAR(256)
WITH schemabinding
AS
BEGIN
	RETURN @xml.value('(/properties/settings[@hwid="hardware"]/setting[@typeid="F1DED00E-9614-475F-A6F7-BA29FCA8DE39"]/value)[1]', 'varchar(256)') 
END
GO

ALTER TABLE Hardware
ADD SerialNumber AS dbo.ExtractSerialNumber(Settings) PERSISTED
GO

-- Add [DetectedModel] computed columns to the [Hardware] table as VARCHAR(256)
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Hardware]') AND name = N'DetectedModel')
BEGIN
	ALTER TABLE [Hardware]
	DROP COLUMN [DetectedModel]
END

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ExtractDetectedModel]') AND type in (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [ExtractDetectedModel]
GO

CREATE FUNCTION [dbo].[ExtractDetectedModel](@xml XML)
RETURNS VARCHAR(256)
WITH schemabinding
AS
BEGIN
	RETURN @xml.value('(/properties/settings[@hwid="hardware"]/setting[@typeid="928B6039-4E4A-4B03-9A82-A1CF29748ACC"]/value)[1]', 'varchar(256)')
END
GO

ALTER TABLE Hardware
ADD DetectedModel AS dbo.ExtractDetectedModel(Settings) PERSISTED
GO