-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF OBJECT_ID ( '[dbo].[stategroup_query_by_id]') IS NOT NULL
	DROP PROCEDURE [stategroup_query_by_id]
GO

CREATE PROCEDURE [dbo].[stategroup_query_by_id]
	@Id UniqueIdentifier
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		[Id],
		[Name],
		[States],
		[last_modified]
	FROM [StateGroup] WHERE [Id] = @Id
END

GO