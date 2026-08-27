-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_verify_objectIds]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_verify_objectIds]
GO

CREATE PROCEDURE [dbo].[security_verify_objectIds]
	@namespace_id UNIQUEIDENTIFIER,
	@object_ids tvp_StringTableGuidType READONLY
AS
BEGIN
	SET NOCOUNT ON

	SELECT oi.ID
	FROM @object_ids oi 
	WHERE NOT EXISTS (
		SELECT null 
		FROM SecurityObject so 
		WHERE so.object_id = oi.ID AND so.namespace_id = @namespace_id)
		
END
GO