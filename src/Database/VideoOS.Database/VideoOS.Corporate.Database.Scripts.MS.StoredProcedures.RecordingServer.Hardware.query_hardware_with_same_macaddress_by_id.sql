-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_hardware_with_same_macaddress_by_id]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_hardware_with_same_macaddress_by_id]
GO


CREATE PROCEDURE [query_hardware_with_same_macaddress_by_id]
	@IDHardware [GuidType]
 AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	
	DECLARE @macAddress VARCHAR(256)
	SELECT  @macAddress = [MacAddress] FROM [Hardware] WHERE [IDHardware] = @IDHardware

	SELECT
		[IDRecorder] AS [recorder_id],
		[IDHardware] AS [hardware_id],
		[Name] AS [name],
		UPPER([MacAddress]) AS [mac_address],
		[LoginID] AS [username]
	FROM [Hardware] h
	WHERE [MacAddress] = @macAddress
END
GO
