-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dataset_hardware_insert]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dataset_hardware_insert]
GO

CREATE PROCEDURE [dataset_hardware_insert]
	@LastModified DATETIME,
	@IDHardware [GuidType],
	@IDHardwareDriver [GuidType],
	@Name [NameType],
	@Description [DescriptionType],
	@URI [UriTypeShort],
	@LoginID [UserIdType],
	@EncryptedPassword [PasswordType],
	@IDRecorder [GuidType],
	@Enabled BIT,
	@Settings XML,
	@EventSettings XML,
	@VendorProductID [NameType],
	@C18NDigest CHAR(44),
	@DriverNumber INT,
	@DriverProxyName  [NameType],
	@HostedHardware_id INT,
	@CameraData NVARCHAR(MAX),
	@EdgeSettings XML,
	@SubType INT,
	@HardwareDataLastUpdated DATETIME,
	@user_sid VARCHAR(256) = NULL
AS
BEGIN
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	-- We need to make this check here because Sql Server 2005 doesn't support filtered index
	IF (@HostedHardware_id IS NOT NULL AND
		EXISTS (SELECT h.[IDHardware] 
				FROM [Hardware] h WITH(UPDLOCK)
				WHERE h.[HostedHardware_id] = @HostedHardware_id))
	BEGIN
		SET @ErrorArgs = @procedureName + N': %%Id=' + CAST(@HostedHardware_id AS NVARCHAR(36)) + N'%%'
		;THROW 60615, @ErrorArgs, 1;
	END

	DECLARE @namespace NVARCHAR(260) = 'Hardware'

	-- Register the object in the security system
	BEGIN TRY
		EXEC security_register_object_transaction
			@namespace_name = @namespace,
			@object_id = @IDHardware,
			@parent_path = '/',
			@CurrentUtcTime = @LastModified
	END TRY
	BEGIN CATCH
		SET @ErrorArgs = dbo.[FormatErrorMessage](ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE()) + N'%%; %%Namespace=' + @namespace + N'%%; %%ID=' + CAST(@IDHardware AS NVARCHAR(36)) + N'%%'
		;THROW 65000, @ErrorArgs, 1;
	END CATCH

	-- Perform the insert
	INSERT INTO 
		[Hardware] ([LastModified], [IDHardware], [IDHardwareDriver], [Name], [Description], [URI], [LoginID], [EncryptedPassword], [IDRecorder], [Enabled], [Settings], [EventSettings], [VendorProductID], [C18NDigest], [DriverNumber], [DriverProxyName], [HostedHardware_id], [CameraData], [EdgeSettings], [SubType], [HardwareDataLastUpdated])
	VALUES
		(@LastModified, @IDHardware, @IDHardwareDriver, @Name, @Description, @URI, @LoginID, @EncryptedPassword, @IDRecorder, @Enabled, @Settings, @EventSettings, @VendorProductID, @C18NDigest, @DriverNumber, @DriverProxyName, @HostedHardware_id, @CameraData, @EdgeSettings, @SubType, @HardwareDataLastUpdated)
END
GO