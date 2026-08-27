-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_query_objectids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_query_objectids]
GO

CREATE PROCEDURE [dbo].[security_query_objectids]
	@namespace_id UNIQUEIDENTIFIER,
	@userDefined int
AS
BEGIN
	SET NOCOUNT ON

	IF @userDefined = '0'
	BEGIN
		SELECT 
			[object_id] 
		FROM [SecurityObject]
		WHERE [namespace_id] = @namespace_id AND [parent_path] = '/'
	END
	ELSE
    IF @userDefined = '1'
	BEGIN
		SELECT 
			[full_path] as object_id
		FROM [SecurityObject]
		WHERE [namespace_id] = @namespace_id AND NOT ([parent_path] = '')
	END
END
GO