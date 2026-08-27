-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
IF  EXISTS (
	SELECT * 
	FROM sys.columns 
	WHERE object_id = OBJECT_ID(N'[FailoverServers]') AND name = N'ServiceState')
BEGIN


	DECLARE @IDFailoverServer [GuidType]
	DECLARE @status INT

	BEGIN TRAN


	DECLARE failover_cursor CURSOR FOR
	SELECT fs.[IDFailoverServer]
	FROM [FailoverServers] fs
	WHERE fs.[ServiceState] = 2 -- Marked deleted

	OPEN failover_cursor

	FETCH NEXT FROM failover_cursor
	INTO @IDFailoverServer

	WHILE(@@FETCH_STATUS = 0)
	BEGIN
		EXEC @status = [delete_failover_server_transaction] @IDFailoverServer

		IF(@status <> 0)
		BEGIN
			ROLLBACK TRAN
			RETURN 
		END

		FETCH NEXT FROM failover_cursor
		INTO @IDFailoverServer
	END

	CLOSE failover_cursor
	DEALLOCATE failover_cursor

	COMMIT TRAN
END 
GO