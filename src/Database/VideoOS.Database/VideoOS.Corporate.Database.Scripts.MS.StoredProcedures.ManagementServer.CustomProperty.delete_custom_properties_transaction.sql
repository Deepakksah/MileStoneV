-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_custom_properties_transaction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [delete_custom_properties_transaction]
GO

CREATE PROCEDURE [delete_custom_properties_transaction]
	@object_id [GuidType] = NULL
AS
BEGIN
	SET NOCOUNT ON;

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

	-- Make sure that a current transaction is active
	IF(@@TRANCOUNT = 0)
	BEGIN
		;THROW 60008, @procedureName, 1;
	END

	IF(@object_id IS NOT NULL)
	BEGIN
		-- Delete custom properties based on object id
		DELETE [CustomProperty]
		WHERE [object_id] = @object_id
	END
	ELSE
	BEGIN
		-- Delete custom properties based on delete table
		DELETE [CustomProperty]
		WHERE [object_id] IN (
			SELECT [object_id]
			FROM [TempCustomPropertyDelete]
			WHERE [SPID] = @@SPID
		)
	END

	-- Now cleanup the temp table. Note this is also done when deleting based on object id
	DELETE [TempCustomPropertyDelete]
	WHERE [SPID] = @@SPID
END
GO
