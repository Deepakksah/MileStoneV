-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_query_matrix_access_entry]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_query_matrix_access_entry]
GO

CREATE PROCEDURE [security_query_matrix_access_entry]
	@SecurityAtribute NVARCHAR(100),
	@IDRole UNIQUEIDENTIFIER,
	@IDMatrix UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON

	SELECT [Value] FROM [SecurityPermissionsMatrix] 
	WHERE [SecurityAttributeID]  = @SecurityAtribute AND
			[IDRole] = @IDRole AND
			[IDMatrix] = @IDMatrix

END
GO
