-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @namespace_id UNIQUEIDENTIFIER
 
DECLARE namespace_cursor CURSOR FOR
SELECT [namespace_id]
FROM [SecurityNamespace]
 
OPEN namespace_cursor
FETCH NEXT FROM namespace_cursor INTO @namespace_id
 
WHILE(@@FETCH_STATUS = 0)
BEGIN

	IF NOT EXISTS(SELECT * FROM [SecurityAction] WHERE [namespace_id] = @namespace_id AND [name] = 'ADMIN_SECURITY')
	BEGIN
		INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible], [security_bit], [legacy_security_name])
		SELECT @namespace_id, 'ADMIN_SECURITY', 'Administrate security', MAX([order_no]) + 1, 0, MAX(security_bit) * 2, NULL
		FROM [SecurityAction] sa
		WHERE [namespace_id] = @namespace_id
		GROUP BY [namespace_id]
	END

	FETCH NEXT FROM namespace_cursor INTO @namespace_id
END
CLOSE namespace_cursor
DEALLOCATE namespace_cursor
GO
