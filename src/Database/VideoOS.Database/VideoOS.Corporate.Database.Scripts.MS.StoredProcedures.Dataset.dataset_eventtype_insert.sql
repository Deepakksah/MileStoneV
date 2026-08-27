-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dataset_eventtype_insert]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dataset_eventtype_insert]
GO

CREATE PROCEDURE [dataset_eventtype_insert]
	@LastModified DATETIME,
	@IDEventType [GuidType],
	@Definition XML,
	@GeneratorType [InternalIdType],
	@IDGenerator [GuidType],
	@Builtin BIT,
	@Name [NameType],
	@GeneratorSubType VARCHAR(36),
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
	SET @namespace = 'UserDefinedEvent'

	-- Register the object in the security system
	BEGIN TRY
		EXEC security_register_object_transaction
			@namespace_name = @namespace,
			@object_id = @IDEventType,
			@parent_path = '/',
			@CurrentUtcTime = @LastModified
	END TRY
	BEGIN CATCH
		SET @ErrorArgs = dbo.[FormatErrorMessage](ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE()) + N'%%; %%Namespace=' + @namespace + N'%%; %%ID=' + CAST(@IDEventType AS NVARCHAR(36)) + N'%%'
		;THROW 65000, @ErrorArgs, 1;
	END CATCH

	INSERT [EventTypes] ([LastModified], [IDEventType], [Definition], [GeneratorType], [IDGenerator], [Builtin], [Name], [GeneratorSubType])
	VALUES (@LastModified, @IDEventType, @Definition, @GeneratorType, @IDGenerator, @Builtin, @Name, @GeneratorSubType);
END
GO