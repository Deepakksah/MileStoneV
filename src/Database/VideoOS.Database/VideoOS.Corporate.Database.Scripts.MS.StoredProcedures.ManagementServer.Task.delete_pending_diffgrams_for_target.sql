-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_pending_diffgrams_for_target]') AND type = 'P')
DROP PROCEDURE [delete_pending_diffgrams_for_target]
GO

CREATE PROCEDURE [delete_pending_diffgrams_for_target]
	@target_id [GuidType]
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

	-- Make sure that a current transaction is active
	IF(@@TRANCOUNT = 0)
	BEGIN
		;THROW 60008, @procedureName, 1;
	END

	DELETE FROM [Task] WITH(READCOMMITTED)
	WHERE
		[target] = @target_id AND
		[pkid] in (	SELECT [pkid] AS task_id
					FROM [Task]
					WHERE [target] = @target_id AND
					[type] IN (
								'diffgram_insert',
								'diffgram_update',
								'diffgram_delete',
								'diffgram_security_insert',
								'diffgram_security_update',
								'diffgram_security_delete'
							))
END
GO