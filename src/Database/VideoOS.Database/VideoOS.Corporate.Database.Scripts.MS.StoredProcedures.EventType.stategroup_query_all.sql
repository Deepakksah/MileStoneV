-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF OBJECT_ID ( N'[dbo].[stategroup_query_all]', 'P') IS NOT NULL
DROP PROCEDURE [stategroup_query_all]
GO

CREATE PROCEDURE [dbo].[stategroup_query_all]
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		[Id],
		[Name],
		[States],
		[last_modified]
	FROM [StateGroup]
END
GO