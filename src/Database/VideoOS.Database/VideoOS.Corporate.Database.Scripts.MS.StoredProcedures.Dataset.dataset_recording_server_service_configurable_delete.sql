-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dataset_recording_server_service_configurable_delete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dataset_recording_server_service_configurable_delete]
GO

CREATE PROCEDURE [dataset_recording_server_service_configurable_delete]
	@pkid [GuidType]
AS
BEGIN
	-- This stored procedure (SP) is intentionally left empty.
	-- It is needed to have this SP for the purpose of the dataset updates (the
	-- data adapter must have a delete command so that it doesn't fail when
	-- processing a deleted data set row).
	--
	-- RecordingServerServiceConfigurable rows are really only deleted along with their
	-- associated Recorders row from the SP 'delete_recording_server_dataset'.

	-- When deleting a failover server the SP 'delete_failover_server' will delete.
	
	RETURN 0
END
GO