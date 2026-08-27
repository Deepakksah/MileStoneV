-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
-- Add [OriginalIdDevice] computed columns to the [Devices] table as VARCHAR(36)

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Devices]') AND name = N'OriginalIdDevice')
BEGIN
	ALTER TABLE [Devices]
	DROP COLUMN [OriginalIdDevice]
END

IF EXISTS (SELECT * FROM sys.objects where object_id = OBJECT_ID(N'dbo.GetOriginalDeviceID') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))
DROP FUNCTION [dbo].[GetOriginalDeviceID]
GO

CREATE FUNCTION [dbo].GetOriginalDeviceID(@DeviceSettings XML, @DeviceID  UNIQUEIDENTIFIER)
RETURNS UNIQUEIDENTIFIER
with schemabinding
AS   
BEGIN   
	DECLARE @TempOriginalDeviceID UNIQUEIDENTIFIER;    
	SELECT @TempOriginalDeviceID = @DeviceSettings.value('(properties/settings/setting[@typeid=''C7A3ADD5-2EDC-4852-A72E-8BA695156C99'']/value)[1]', 'char(36)');   
	IF(@TempOriginalDeviceID IS NULL)
		RETURN Convert(UNIQUEIDENTIFIER ,@DeviceID)
	RETURN Convert(UNIQUEIDENTIFIER ,@TempOriginalDeviceID)
END; 

GO

ALTER TABLE [Devices] ADD [OriginalIdDevice] AS dbo.GetOriginalDeviceID([Settings], [IdDevice]) PERSISTED
GO