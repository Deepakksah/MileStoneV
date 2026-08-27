-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_custom_properties_transaction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_custom_properties_transaction]
GO

CREATE PROCEDURE [update_custom_properties_transaction]
	@object_id VARCHAR(128),
	@custom_properties XML
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

	DECLARE @CurrentUtcTime DATETIME
	SET @CurrentUtcTime = GETUTCDATE()
	
	IF(@custom_properties IS NULL)
	BEGIN
		RETURN 0
	END
	
	-- Delete existing custom properties to update
	DELETE [CustomProperty]
	FROM @custom_properties.nodes('CustomProperties/CustomProperty') AS T(property)
	WHERE [object_id] = @object_id AND [name] = T.property.value('@Name', 'NameType')

	-- Insert new custom properties
	INSERT [CustomProperty] (
		[object_id],
		[name],
		[value]
	)
	SELECT @object_id,
		[name] = T.property.value('@Name', 'NameType'),
		[value] = T.property.value('@Value', 'NVARCHAR(MAX)')
	FROM @custom_properties.nodes('CustomProperties/CustomProperty') AS T(property)
END
GO
