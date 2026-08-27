-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_create_role_transaction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_create_role_transaction]
GO

CREATE PROCEDURE [security_create_role_transaction]
	@IDRole [GuidType],
	@RoleType TINYINT,
	@Name [NameType],
	@Description [DescriptionType],
	@DualAuthorizationRequired BIT,
	@PtzReservedAsAnonymous BIT,
	@AllowMobileClientLogOn BIT,
	@AllowSmartClientLogOn BIT,
	@AllowWebClientLogOn BIT,
	@LastModified DATETIME,
	@RetentionOptionId GuidType = NULL,
	@ManagementClientProfile [GuidType] = NULL,
	@SmartClientProfile [GuidType] = NULL
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	DECLARE @CurrentUtcTime DATETIME
	SET @CurrentUtcTime = GETUTCDATE()

	-- Make sure that a current transaction is active
	IF(@@TRANCOUNT = 0)
	BEGIN
		;THROW 60008, @procedureName, 1;
	END

	BEGIN TRY
		INSERT [SecurityRoles] ([IDRole], [Type], [Name], [Description], [DualRequired], [PtzReservedAsAnonymous], [AllowMobileClientLogOn], [AllowSmartClientLogOn], [AllowWebClientLogOn], [LastModified])
		VALUES (@IDRole, @RoleType, @Name, @Description, @DualAuthorizationRequired, @PtzReservedAsAnonymous, @AllowMobileClientLogOn, @AllowSmartClientLogOn, @AllowWebClientLogOn, @LastModified)
	END TRY
	BEGIN CATCH
		IF(ERROR_NUMBER() = 2601 OR ERROR_NUMBER() = 2627) -- CONSTRAINT VIOLATION
		BEGIN
			-- The id or name already exists
			SET @ErrorArgs = @procedureName + N': %%Name=' + @Name + N'%%'
			;THROW 60001, @ErrorArgs, 1;
		END
		;THROW
	END CATCH
	
	-- add the new group to the identity cache
	INSERT INTO [SecurityIdentity](
		[SID],
		[Type],
		[DisplayName],
		[Description],
		[Domain],
		[AccountName],
		[DistinguishedName],
		[Deleted],
		[LastModified]
	)
	VALUES(
		@IDRole,
		4,
		@Name,
		@Description,
		'',
		'',
		'',
		0,
		@LastModified
	)

	IF(@RetentionOptionId IS NULL)
	BEGIN
		-- Use Default retention option
		SELECT @RetentionOptionId = CAST(SETTING AS XML).value('declare namespace x="urn:milestone-systems"; (/x:RetentionOptionsXmlFormatter/x:RetentionOptions/x:RetentionTimeOption[x:Default="true"]/x:Id)[1]', 'GuidType') 
		FROM [SystemSettings]
		WHERE [Name] = 'RetentionOptions'
	END

	INSERT INTO [RetentionOptionRoleRelation] 
	(
		[IdRole],
		[IdRetentionOption]
	)
	VALUES 
	(
		@IDRole,
		@RetentionOptionId
	)

	IF(@ManagementClientProfile IS NULL)
	BEGIN
		-- Use Default management client profile
		SELECT @ManagementClientProfile = IDClientProfile 
		FROM [ClientProfile]
		WHERE DefaultProfile = 1 AND profile_type = 'ManagementClientProfile'
	END
	
	IF(@SmartClientProfile IS NULL)
	BEGIN
		-- Use Default smart client profile
		SELECT @SmartClientProfile = IDClientProfile 
		FROM [ClientProfile]
		WHERE DefaultProfile = 1 AND profile_type = 'SmartClientProfile'
	END

	INSERT INTO [ClientProfileRoleRelation]
	(
		[IDRole],
		[IDClientProfile]
	)
	VALUES 
	(
		@IDRole,
		@ManagementClientProfile
	),
	(
		@IDRole,
		@SmartClientProfile
	)

	DECLARE @ManagementServerNamespaceId GuidType
	SET @ManagementServerNamespaceId = 'E9914E39-A67A-496C-A3C1-5B4AAAB28F15'

	-- Allow Connect on new role
	INSERT INTO [SecurityAccessEntry]([security_object_id], [user_sid], [action_name], [access_deny])
	SELECT so.[security_object_id], @IDRole, 'CONNECT', 0
	FROM [SecurityObject] so
	WHERE so.[namespace_id] = @ManagementServerNamespaceId 
	AND so.[object_id] = '/'
	AND NOT EXISTS
	(
		SELECT *
		FROM [SecurityAccessEntry] sae
		WHERE sae.[action_name] = 'CONNECT'
		AND sae.[security_object_id] = so.[security_object_id]
		AND sae.[user_sid] =  @IDRole
	)
	
	-- Get DataSet after changes
	DECLARE @dataSetAfter XML
	SET @dataSetAfter = dbo.securityRolesAsDataSet(@IDRole)

	-- Create diffgram
	EXEC [create_task_transaction]
		@type = 'diffgram_security_insert',
		@target = '909749AE-88C4-4780-A381-383D691E98FA',
		@data_a = @dataSetAfter,
		@current_time = @CurrentUtcTime
END
GO