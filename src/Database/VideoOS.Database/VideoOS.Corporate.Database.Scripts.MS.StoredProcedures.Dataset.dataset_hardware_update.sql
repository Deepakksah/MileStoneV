-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dataset_hardware_update]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dataset_hardware_update]
GO

CREATE PROCEDURE [dataset_hardware_update]
	@LastModified AS DATETIME,
	@IDHardware AS [GuidType],
	@IDHardwareDriver AS [GuidType],
	@Name AS [NameType],
	@Description AS [DescriptionType],
	@URI AS [UriTypeShort],
	@LoginID AS [UserIdType],
	@EncryptedPassword AS [PasswordType],
	@IDRecorder AS [GuidType],
	@Enabled AS BIT,
	@Settings AS XML,
	@EventSettings AS XML,
	@VendorProductID AS [NameType],
	@C18NDigest AS CHAR(44),
	@DriverNumber AS INT,
	@DriverProxyName  AS [NameType],
	@HostedHardware_id AS INT,
	@CameraData AS NVARCHAR(MAX),
	@EdgeSettings AS XML,
	@SubType AS INT,
	@CredentialsLastModified AS DATETIME,
	@HardwareDataLastUpdated AS DATETIME,
	@Original_LastModified AS DATETIME,
	@Original_IDHardware AS [GuidType],
	@Original_IDHardwareDriver AS [GuidType],
	@Original_Name AS [NameType],
	@Original_LoginID AS [UserIdType],
	@Original_EncryptedPassword AS [PasswordType],
	@Original_IDRecorder AS [GuidType],
	@Original_Enabled AS BIT,
	@IsNull_VendorProductID AS BIT,
	@Original_VendorProductID AS [NameType],
	@Original_C18NDigest AS CHAR(44),
	@Original_DriverNumber AS INT,
	@Original_DriverProxyName AS [NameType],
	@IsNull_HostedHardware_id AS BIT,
	@Original_HostedHardware_id AS INT,	
	@Original_SubType AS INT,
	@Original_CredentialsLastModified AS DATETIME,
	@Original_HardwareDataLastUpdated AS DATETIME
AS
BEGIN
	-- Initialize the ProcedureName for error messages
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	-- We need to make this check here because Sql Server 2005 doesn't support filtered index
	IF (@HostedHardware_id IS NOT NULL
		AND 
		@Original_HostedHardware_id IS NOT NULL
		AND
		@HostedHardware_id <> @Original_HostedHardware_id
		AND
		EXISTS (SELECT h.[IDHardware] 
				FROM [Hardware] h WITH(UPDLOCK)
				WHERE h.[HostedHardware_id] = @HostedHardware_id))
	BEGIN
		SET @ErrorArgs = @procedureName + N': %%Id=' + CAST(@HostedHardware_id AS NVARCHAR(36)) + N'%%'
		;THROW 60615, @ErrorArgs, 1;
	END

	-- Perform the update
	UPDATE
		[Hardware]
	SET
		[LastModified] = @LastModified,
		[IDHardware] = @IDHardware,
		[IDHardwareDriver] = @IDHardwareDriver,
		[Name] = @Name,
		[Description] = @Description,
		[URI] = @URI,
		[LoginID] = @LoginID,
		[EncryptedPassword] = @EncryptedPassword,
		[IDRecorder] = @IDRecorder,
		[Enabled] = @Enabled,
		[Settings] = @Settings,
		[EventSettings] = @EventSettings,
		[VendorProductID] = @VendorProductID,
		[C18NDigest] = @C18NDigest,
		[DriverNumber] = @DriverNumber,
		[DriverProxyName] = @DriverProxyName,
		[HostedHardware_id] = @HostedHardware_id,
		[CameraData] = @CameraData,
		[EdgeSettings] = @EdgeSettings,
		[SubType] = @SubType,
		[CredentialsLastModified] = @CredentialsLastModified,
		[HardwareDataLastUpdated] = @HardwareDataLastUpdated
	WHERE
		(
			([LastModified] = @Original_LastModified) AND
			([IDHardware] = @Original_IDHardware) AND
			([IDHardwareDriver] = @Original_IDHardwareDriver) AND
			([Name] = @Original_Name) AND
			([LoginID] = @Original_LoginID) AND
			([IDRecorder] = @Original_IDRecorder) AND
			([Enabled] = @Original_Enabled) AND
			((@IsNull_VendorProductID = 1 AND [VendorProductID] IS NULL) OR ([VendorProductID] = @Original_VendorProductID)) AND
			([C18NDigest] = @Original_C18NDigest) AND
			([DriverNumber] = @Original_DriverNumber) AND
			([DriverProxyName] = @Original_DriverProxyName) AND
			((@IsNull_HostedHardware_id = 1 AND [HostedHardware_id] IS NULL) OR ([HostedHardware_id] = @Original_HostedHardware_id)) AND
			([SubType] = @SubType) AND
			([CredentialsLastModified] = @Original_CredentialsLastModified OR [CredentialsLastModified] is NULL) AND
			([HardwareDataLastUpdated] = @Original_HardwareDataLastUpdated OR [HardwareDataLastUpdated] is NULL)
		)
	--SELECT LastModified, IDHardware, IDHardwareDriver, Name, Description, URI, LoginID, EncryptedPassword, IDRecorder, Enabled, Settings, EventSettings, VendorProductID, C18NDigest, DriverNumber, DriverProxyName, HostedHardware_id, CameraData FROM Hardware WHERE (IDHardware = @IDHardware)
END
GO
