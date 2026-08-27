-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dataset_rule_insert]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dataset_rule_insert]
GO

CREATE PROCEDURE [dataset_rule_insert]
	@LastModified DATETIME,
	@IDRule [GuidType],
	@Name [NameType],
	@Description [DescriptionType],
	@Definition XML,
	@Enabled BIT,
	@Priority INT,
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

	DECLARE @namespace NVARCHAR(260) = 'Rule'

	-- Register the object in the security system
	BEGIN TRY
		EXEC security_register_object_transaction
			@namespace_name = @namespace,
			@object_id = @IDRule,
			@parent_path = '/',
			@CurrentUtcTime = @LastModified
	END TRY
	BEGIN CATCH
		SET @ErrorArgs = dbo.[FormatErrorMessage](ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE()) + N'%%; %%Namespace=' + @namespace + N'%%; %%ID=' + CAST(@IDRule AS NVARCHAR(36)) + N'%%'
		;THROW 65000, @ErrorArgs, 1;
	END CATCH

	INSERT INTO [Rules] ([LastModified], [IDRule], [Name], [Description], [Definition], [Enabled], [Priority]) 
	VALUES (@LastModified, @IDRule, @Name, @Description, @Definition, @Enabled, @Priority);
END
GO