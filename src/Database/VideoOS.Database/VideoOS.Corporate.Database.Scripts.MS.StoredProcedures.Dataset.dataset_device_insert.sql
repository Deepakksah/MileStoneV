-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dataset_device_insert]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dataset_device_insert]
GO

CREATE PROCEDURE [dataset_device_insert]
	@LastModified DATETIME,
	@IDDevice [GuidType],
	@IDHardware [GuidType],
	@Name [NameType],
	@Description NVARCHAR(MAX),
	@Location NVARCHAR(MAX),
	@DeviceType [InternalIdType],
	@Enabled BIT,
	@Settings XML,
	@Presets XML,
	@Shortcut [ShortNameType],
	@MulticastEnabled BIT,
	@Streams XML,
	@DeviceIndex INT,
	@SubType INT,
	@FailoverLiveOnly BIT,
	@DisableOnFailover BIT,
	@PtzManualSessionTimeout INT,
	@PtzReservedSessionTimeout INT,
	@PtzExtendedManualSessionTimeout INT,
	@GisPointString NVARCHAR(MAX),
	@ShortName NVARCHAR(128),
	@Icon INT,
	@CoverageDirection FLOAT,
	@CoverageDepth FLOAT,
	@CoverageFieldOfView FLOAT,
	@user_sid VARCHAR(256) = NULL
AS
BEGIN
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	-- Make sure that a current transaction is active
	IF(@@TRANCOUNT = 0)
	BEGIN
		;THROW 60008, @procedureName, 1;
	END

	DECLARE @namespace NVARCHAR(260)

	SELECT @namespace = r.[SecurityNamespaceName] FROM [RegisteredDeviceTypes] r
	WHERE r.[DeviceType] = @DeviceType

	-- Register the object in the security system
	BEGIN TRY
		EXEC security_register_object_transaction
			@namespace_name = @namespace,
			@object_id = @IDDevice,
			@parent_path = '/',
			@CurrentUtcTime = @LastModified
	END TRY
	BEGIN CATCH
		SET @ErrorArgs = dbo.[FormatErrorMessage](ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE()) + N'%%; %%Namespace=' + @namespace + N'%%; %%ID=' + CAST(@IDDevice AS NVARCHAR(36)) + N'%%'
		;THROW 65000, @ErrorArgs, 1;
	END CATCH

	INSERT [Devices] ([LastModified], [IDDevice], [IDHardware], [Name], [Description], [Location], [DeviceType], [Enabled], [Settings], [Presets], [Shortcut], [MulticastEnabled], [Streams], [DeviceIndex], [SubType], [FailoverLiveOnly], [DisableOnFailover], [PtzManualSessionTimeout], [PtzReservedSessionTimeout], [PtzExtendedManualSessionTimeout], [GisPoint], [ShortName], [Icon], [CoverageDirection], [CoverageDepth], [CoverageFieldOfView])
	VALUES (@LastModified, @IDDevice, @IDHardware, @Name, @Description, @Location, @DeviceType, @Enabled, @Settings, @Presets, @Shortcut, @MulticastEnabled, @Streams, @DeviceIndex, @SubType, @FailoverLiveOnly, @DisableOnFailover, @PtzManualSessionTimeout, @PtzReservedSessionTimeout, @PtzExtendedManualSessionTimeout, geography::STPointFromText(@GisPointString, 4326), @ShortName, @Icon, @CoverageDirection, @CoverageDepth, @CoverageFieldOfView);
END
GO