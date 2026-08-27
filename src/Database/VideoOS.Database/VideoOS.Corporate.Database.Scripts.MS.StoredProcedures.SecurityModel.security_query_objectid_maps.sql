-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_query_objectid_maps]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_query_objectid_maps]
GO

CREATE PROCEDURE [dbo].[security_query_objectid_maps]
	@namespace_id UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON

	SELECT 
	     object_id, full_path
	FROM [SecurityObject]
	WHERE [namespace_id] = @namespace_id AND NOT ([parent_path] = '')
END
GO