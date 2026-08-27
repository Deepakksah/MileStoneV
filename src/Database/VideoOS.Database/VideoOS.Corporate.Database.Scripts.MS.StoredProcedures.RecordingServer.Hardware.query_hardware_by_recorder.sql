-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_hardware_by_recorder]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_hardware_by_recorder]
GO

CREATE PROCEDURE [query_hardware_by_recorder]
	@IDRecorder [GuidType]
 AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

	SELECT 
		[LastModified],
		[IDHardware],
		[IDHardwareDriver],
		[Name],
		[Description],
		[Uri] AS [Address],
		[LoginID],
		[EncryptedPassword],
		[IDRecorder],
		[Enabled],
		[VendorProductID],
		[DriverNumber],
		[HostedHardware_id],
		[DetectedModel],
		[CredentialsLastModified],
		[HardwareDataLastUpdated]
	FROM [Hardware]
	WHERE [IDRecorder] = @IDRecorder
END
GO
