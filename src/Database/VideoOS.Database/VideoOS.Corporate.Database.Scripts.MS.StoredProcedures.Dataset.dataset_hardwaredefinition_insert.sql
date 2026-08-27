-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dataset_hardwaredefinition_insert]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dataset_hardwaredefinition_insert]
GO

CREATE PROCEDURE [dataset_hardwaredefinition_insert]
	@digest CHAR(44),
	@definition XML
AS
BEGIN
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

	-- Make sure that a current transaction is active
	IF(@@TRANCOUNT = 0)
	BEGIN
		;THROW 60008, @procedureName, 1;
	END

	IF NOT EXISTS (SELECT [C18NDigest] FROM [HardwareDefinition] WITH(UPDLOCK) WHERE ([C18NDigest] = @digest))
	BEGIN
		INSERT INTO [HardwareDefinition] ([C18NDigest], [Definition]) VALUES (@digest, @definition)
	END
	
	SELECT [C18NDigest], [Definition] FROM [HardwareDefinition] WHERE [C18NDigest] = @digest
END
GO