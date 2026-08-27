-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dataset_device_delete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dataset_device_delete]
GO

CREATE PROCEDURE [dataset_device_delete]
	@Original_IDDevice [GuidType]
AS
BEGIN
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	-- Make sure that a current transaction is active
	IF (@@TRANCOUNT = 0)
	BEGIN
		;THROW 60008, @procedureName, 1;
	END

	DECLARE @CurrentUtcTime DATETIME
	SET @CurrentUtcTime = GETUTCDATE()

	DECLARE @namespace NVARCHAR(260)

	SELECT	@namespace = r.[SecurityNamespaceName]
	FROM	[RegisteredDeviceTypes] r
			INNER JOIN [Devices] d ON d.[DeviceType] = r.[DeviceType]
	WHERE	d.[IDDevice] = @Original_IDDevice

	-- Unregister user object
	BEGIN TRY
		EXEC [security_unregister_object_transaction]
			@namespace_name = @namespace,
			@object_id = @Original_IDDevice,
			@CurrentUtcTime = @CurrentUtcTime
	END TRY
	BEGIN CATCH
		SET @ErrorArgs = dbo.[FormatErrorMessage](ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE())  + N'%%; %%Namespace=' + @namespace + N'%%; %%ID=' + CAST(@Original_IDDevice AS NVARCHAR(36)) + N'%%'
		;THROW 65001, @ErrorArgs, 1;
	END CATCH

	-- The calling code must ensure that no evidence locks exists for the device in order for
	-- the following delete not to fail.
	-- Foreign key constraints ensure that is it not possible to delete a device if a MarkedDataDevice entry
	-- exists for the device being deleted.

	BEGIN TRY
		-- Delete bookmarks (only - not evidence locks)
		DECLARE @bookmark_type AS INT = 8
		DELETE	mdd
		FROM	[MarkedDataDevices] mdd
				INNER JOIN [MarkedData] md ON md.[IDMarkedData] = mdd.[IDMarkedData]
		WHERE	mdd.[IDDevice] = @Original_IDDevice
				AND md.[MarkDataType] = @bookmark_type

		-- Delete bookmark entry if it has no more devices
		DELETE	md
		FROM	[MarkedData] AS md
		WHERE	NOT EXISTS (
				SELECT 1 FROM [MarkedDataDevices] AS mdd
				WHERE md.[IDMarkedData] = mdd.[IDMarkedData]
				)

		 -- Delete RestrictedMediaLive
		DELETE rml
		FROM   RestrictedMediaLive AS rml
		WHERE  rml.IDDevice = @Original_IDDevice

		 -- Delete RestrictedMediaDevices
		DELETE rmd
		FROM       RestrictedMediaDevice AS rmd
		WHERE      rmd.IDDevice = @Original_IDDevice

		-- Delete RestrictedMedia
		DELETE rm
		FROM  [RestrictedMedia] AS rm
		WHERE NOT EXISTS(
				SELECT 1
				FROM   RestrictedMediaDevice AS rmd
				WHERE  rm.IDRestrictedMedia = rmd.IDRestrictedMedia )

		-- Delete the device
		DELETE FROM	[Devices]
		WHERE		[IDDevice] = @Original_IDDevice
	END TRY
	BEGIN CATCH
		;THROW 60000, @procedureName, 1;
	END CATCH
END
GO