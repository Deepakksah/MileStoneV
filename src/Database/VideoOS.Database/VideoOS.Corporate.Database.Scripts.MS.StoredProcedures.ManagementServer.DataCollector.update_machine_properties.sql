-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_machine_properties]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_machine_properties]
GO

CREATE PROCEDURE [update_machine_properties]
	@data_collector_id INT,
	@machine_properties XML
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()
	
		BEGIN TRANSACTION

		IF(@machine_properties IS NULL)
		BEGIN
			ROLLBACK TRANSACTION
			RETURN 0
		END
		
		-- Delete existing properties
		DELETE [MachineProperties]
		WHERE [data_collector_id] = @data_collector_id

		-- Insert new properties
		INSERT [MachineProperties] ([data_collector_id], [name], [value])
		SELECT 
			@data_collector_id, 
			T.property.value('@Name', 'NameType'), 
			T.property.value('@Value', 'NVARCHAR(MAX)')
		FROM @machine_properties.nodes('MachineProperties/MachineProperty') AS T(property)

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH 
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION
		END;
		DECLARE @shouldReThrowError BIT = dbo.[ShouldReThrowError](ERROR_NUMBER(), ERROR_SEVERITY())
		IF(@shouldReThrowError = 1)
			THROW;
		SET @ErrorArgs = dbo.[FormatErrorMessage](ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE())
		;THROW 60000, @ErrorArgs, 1;
	END CATCH;
END
GO