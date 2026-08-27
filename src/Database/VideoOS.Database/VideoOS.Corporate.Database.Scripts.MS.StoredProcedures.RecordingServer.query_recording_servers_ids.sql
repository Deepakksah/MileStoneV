-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_recording_servers_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_recording_servers_ids]
GO

CREATE PROCEDURE [query_recording_servers_ids]
	@IDServerGroup [GuidType]
 AS
BEGIN
	SET NOCOUNT ON
	
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

	IF NOT EXISTS(SELECT * FROM [ServerGroup] WITH(UPDLOCK) WHERE [IDServerGroup] = @IDServerGroup)
	BEGIN
		;THROW 60089, @procedureName, 1;
	END

	BEGIN TRY
		INSERT [ServerGroupMembership] ([IDServerGroup], [IDObject], [ObjectType], [LastModified])
		SELECT 
			'909749AE-88C4-4780-A381-383D691E98FA',
			r.IDRecorder,
			'RECORDING_SERVER',
			GETUTCDATE()
		FROM [Recorders] r
		LEFT JOIN [ServerGroupMembership] sgm ON sgm.[IDObject] = r.[IDRecorder]
		WHERE sgm.[IDObject] IS NULL
	END TRY
	BEGIN CATCH
		;THROW 60000, @procedureName, 1;
	END CATCH

	SELECT r.[IDRecorder]
	FROM [Recorders] r 
	INNER JOIN [RecordingServerServiceRecorder] rssr ON r.[IDRecorder] = rssr.[recorder_id] AND rssr.[active]= 1
	-- Join for filtering on server group
	INNER JOIN [ServerGroupMembership] sgm ON sgm.[IDObject] = r.[IDRecorder] AND sgm.[IDServerGroup] = @IDServerGroup
END
GO