-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_pending_log]') AND type in (N'P', N'PC'))
DROP PROCEDURE [create_pending_log]
GO

CREATE PROCEDURE [create_pending_log]
	@session_id [GuidType],
	@log_id int,
	@param0 nvarchar(256) = NULL,
	@param1 nvarchar(256) = NULL,
	@param2 nvarchar(256) = NULL,
	@param3 nvarchar(256) = NULL,
	@param4 nvarchar(256) = NULL,
	@param5 nvarchar(256) = NULL,
	@param6 nvarchar(256) = NULL,
	@param7 nvarchar(256) = NULL
AS
BEGIN
	BEGIN TRY
		DECLARE @paramsXml XML
		IF(@param7 IS NOT NULL)
		BEGIN
			SELECT @paramsXml = CAST(N'<params>' +'<param>' + dbo.encodeXml(@param0) + '</param>' +'<param>' + dbo.encodeXml(@param1) + '</param>' +'<param>' + dbo.encodeXml(@param2) + '</param>' +'<param>' + dbo.encodeXml(@param3) + '</param>' +'<param>' + dbo.encodeXml(@param4) + '</param>' +'<param>' + dbo.encodeXml(@param5) + '</param>' + '<param>' + dbo.encodeXml(@param6) + '</param>' +  '<param>' + dbo.encodeXml(@param7) + '</param>' + '</params>' as XML)
		END
		ELSE IF(@param6 IS NOT NULL)
		BEGIN
			SELECT @paramsXml = CAST(N'<params>' +'<param>' + dbo.encodeXml(@param0) + '</param>' +'<param>' + dbo.encodeXml(@param1) + '</param>' +'<param>' + dbo.encodeXml(@param2) + '</param>' +'<param>' + dbo.encodeXml(@param3) + '</param>' +'<param>' + dbo.encodeXml(@param4) + '</param>' +'<param>' + dbo.encodeXml(@param5) + '</param>' + '<param>' + dbo.encodeXml(@param6) + '</param>' +  '</params>' as XML)
		END
		ELSE IF(@param5 IS NOT NULL)
		BEGIN
			SELECT @paramsXml = CAST(N'<params>' +'<param>' + dbo.encodeXml(@param0) + '</param>' +'<param>' + dbo.encodeXml(@param1) + '</param>' +'<param>' + dbo.encodeXml(@param2) + '</param>' +'<param>' + dbo.encodeXml(@param3) + '</param>' +'<param>' + dbo.encodeXml(@param4) + '</param>' +'<param>' + dbo.encodeXml(@param5) + '</param>' + '</params>' as XML)
		END
		ELSE IF(@param4 IS NOT NULL)
		BEGIN
			SELECT @paramsXml = CAST(N'<params>' +'<param>' + dbo.encodeXml(@param0) + '</param>' +'<param>' + dbo.encodeXml(@param1) + '</param>' +'<param>' + dbo.encodeXml(@param2) + '</param>' +'<param>' + dbo.encodeXml(@param3) + '</param>' +'<param>' + dbo.encodeXml(@param4) + '</param>' + '</params>' as XML)
		END
		ELSE IF(@param3 IS NOT NULL)
		BEGIN
			SELECT @paramsXml = CAST(N'<params>' +'<param>' + dbo.encodeXml(@param0) + '</param>' +'<param>' + dbo.encodeXml(@param1) + '</param>' +'<param>' + dbo.encodeXml(@param2) + '</param>' +'<param>' + dbo.encodeXml(@param3) + '</param>' + '</params>' as XML)
		END
		ELSE IF(@param2 IS NOT NULL)
		BEGIN
			SELECT @paramsXml = CAST(N'<params>' +'<param>' + dbo.encodeXml(@param0) + '</param>' +'<param>' + dbo.encodeXml(@param1) + '</param>' +'<param>' + dbo.encodeXml(@param2) + '</param>' + '</params>' as XML)
		END
		ELSE IF(@param1 IS NOT NULL)
		BEGIN
			SELECT @paramsXml = CAST(N'<params>' +'<param>' + dbo.encodeXml(@param0) + '</param>' +'<param>' + dbo.encodeXml(@param1) + '</param>' + '</params>' as XML)
		END
		ELSE IF(@param0 IS NOT NULL)
		BEGIN
			SELECT @paramsXml = CAST(N'<params>' +'<param>' + dbo.encodeXml(@param0) + '</param>' + '</params>' as XML)
		END

		INSERT INTO LogPending(session_id, log_id, params)
		VALUES(@session_id, @log_id, @paramsXml)
	END TRY
	BEGIN CATCH
		-- silently ignore errors
	END CATCH
END
GO
