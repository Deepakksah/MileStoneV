-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[security_create_namespace]') AND [type] IN (N'P', N'PC'))
DROP PROCEDURE [security_create_namespace]
GO

CREATE PROCEDURE [security_create_namespace]
	@namespace_id UNIQUEIDENTIFIER,
	@name NVARCHAR(260),
	@display_name NVARCHAR(260),
	@separator CHAR(1),
	@structure TINYINT,
	@order_after_namespace_id UNIQUEIDENTIFIER,
	@action_values XML
AS
BEGIN
	SET NOCOUNT ON
 
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRAN

		DECLARE @order_number INT
		SET @order_number = (SELECT [order_no] FROM [SecurityNamespace] WHERE [namespace_id] = @order_after_namespace_id)
		IF(@order_number IS NULL)
		BEGIN
			SET @order_number = (SELECT COALESCE(MAX([order_no]), 0) FROM [SecurityNamespace])
		END
	
		SET @order_number = @order_number + 1

		UPDATE	[SecurityNamespace]
		SET		[order_no] = [order_no] + 1
		WHERE	[order_no] >= @order_number

		BEGIN TRY
			INSERT [SecurityNamespace] ([namespace_id], [name], [display_name], [separator], [structure], [order_no], [user_defined])
			VALUES (@namespace_id, @name, @display_name, @separator, @structure, @order_number, 1)
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
	
		-- Insert actions for new security namespace
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

		-- Create root group object for new security namespace
		INSERT [SecurityObject] ([namespace_id], [parent_path], [object_id], [object_type], [object_name], [security_object_id], [inherits_from_id])
		VALUES (@namespace_id, '', @separator, 1, @name, NEWID(), NULL )

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