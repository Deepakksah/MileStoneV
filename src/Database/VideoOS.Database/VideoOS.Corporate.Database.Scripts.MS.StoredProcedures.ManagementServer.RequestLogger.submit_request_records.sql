-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[submit_request_records]') AND type in (N'P', N'PC'))
DROP PROCEDURE [submit_request_records]
GO

CREATE PROCEDURE [submit_request_records]
	@request_records tvp_RequestLogTable READONLY,
	@request_parameters tvp_RequestParameterTable READONLY
AS
BEGIN
	SET NOCOUNT ON
 
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @addedRequestLog TABLE
		(
			[TempId] BIGINT PRIMARY KEY NOT NULL,
			[RequestId] BIGINT NOT NULL
		)

		BEGIN TRANSACTION
  
		INSERT [RequestLog] ([TempId], [WebMethodName], [StartTime], [ExecutionTime], [IdentityName], [IPAddress], [ExecutionCount])
		OUTPUT INSERTED.[TempId], INSERTED.[RequestId] INTO @addedRequestLog
		SELECT [TempId], [WebMethodName], [StartTime], [ExecutionTime], [IdentityName], [IPAddress], [ExecutionCount]
		FROM @request_records

		INSERT [RequestLogParameter] ([RequestId], [ParameterName], [ParameterIndex], [ParameterValue])
		SELECT arl.[RequestId], rp.[ParameterName], rp.[ParameterIndex], rp.[ParameterValue]
		FROM @request_parameters rp
		INNER JOIN @addedRequestLog arl ON arl.[TempId] = rp.[TempId]

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
