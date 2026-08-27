-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[security_update_namespace]') AND [type] IN (N'P', N'PC'))
DROP PROCEDURE [security_update_namespace]
GO

CREATE PROCEDURE [security_update_namespace]
	@namespace_id UNIQUEIDENTIFIER,
	@name NVARCHAR(260),
	@display_name NVARCHAR(260),
	@separator CHAR(1),
	@structure TINYINT,
	@action_values XML
AS
BEGIN
	SET NOCOUNT ON
 
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @userDefined TINYINT

		BEGIN TRAN

		SELECT	@userDefined = [user_defined]
		FROM	[SecurityNamespace]
		WHERE	[namespace_id] = @namespace_id

		IF(@userDefined IS NULL OR @userDefined = 0)
		BEGIN
			;THROW 60325, @procedureName, 1;
		END

		UPDATE	[SecurityNamespace]
		SET		[name] = @name,
				[display_name] = @display_name,
				[separator] = @separator,
				[structure] = @structure
		WHERE	[namespace_id] = @namespace_id

		-- Delete existing security actions
		DELETE [SecurityAction]
		WHERE [namespace_id] = @namespace_id

		-- Insert updated security actions
		INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible], [security_bit])
		SELECT ds.[namespace_id], ds.[name], ds.[display_name], ROW_NUMBER() OVER(ORDER BY ds.[temp]) AS [order_no], ds.[visible],
			CASE
				WHEN ROW_NUMBER() OVER(ORDER BY ds.[temp]) = 1 THEN 1
				ELSE POWER(CAST(2 AS DECIMAL(20)), CAST((ROW_NUMBER() OVER(ORDER BY ds.[temp]) - 1) AS DECIMAL(20)))
			END AS securityBit
		FROM
		(
			SELECT	@namespace_id AS [namespace_id],
					[name] = v.value('@name', 'NVARCHAR(120)'),
					[display_name] = v.value('@display_name', 'NVARCHAR(120)'),
					[visible] = v.value('@visible', 'INT'),
					1 AS [temp]
			FROM	@action_values.nodes('actions/action') AS T(v)
		) AS ds

		COMMIT TRAN
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