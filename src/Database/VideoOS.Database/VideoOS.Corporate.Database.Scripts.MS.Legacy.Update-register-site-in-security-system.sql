-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

BEGIN TRAN

DECLARE security_cursor CURSOR
FOR SELECT [IDSite] FROM [Sites]
WHERE IDSite NOT IN 
(
	SELECT [object_Id] 
	FROM SecurityObject
)

DECLARE @UnregisteredSiteId GuidType
DECLARE @CurrentUtcTime DATETIME
SET @CurrentUtcTime = GETUTCDATE()

OPEN security_cursor

FETCH NEXT FROM security_cursor 
	INTO @UnregisteredSiteId
WHILE @@FETCH_STATUS = 0
BEGIN
	-- Register the security object
	EXEC security_register_object_transaction
					@namespace_name = 'Site',
					@object_id = @UnregisteredSiteId,
					@parent_path = '/',
					@CurrentUtcTime = @CurrentUtcTime
			
FETCH NEXT FROM security_cursor 
	INTO @UnregisteredSiteId
END

CLOSE security_cursor
DEALLOCATE security_cursor

COMMIT TRAN
