-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF OBJECT_ID ( '[dbo].[stategroup_update_initial]', 'P') IS NOT NULL
	DROP PROCEDURE stategroup_update_initial
GO

CREATE PROCEDURE [dbo].stategroup_update_initial
	@Id [UniqueIdentifier],
	@Name [NameType],
	@States NVarchar(1024)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @CurrentUtcTime DATETIME
	SET @CurrentUtcTime = GETUTCDATE()

	UPDATE [StateGroup] SET 
		[Name] = @Name,
		[States] = @States,
		[last_modified] = @CurrentUtcTime
	WHERE [Id] = @Id

	RETURN 0
END
GO

BEGIN TRAN

-- Update predefined State Groups - System monitor's one

	EXEC [stategroup_update_initial] 'C1EB0D2B-1903-45F6-B0C8-3F4CD1879B2A','Service','Normal,Critical,Undefined'
	EXEC [stategroup_update_initial] 'FEB52F64-AD52-41F4-AC13-16576460DB7D','Memory usage','Normal,Warning,Critical,Undefined'
	EXEC [stategroup_update_initial] '8710ED5D-1FB8-4551-81FB-49B269267D5D','GPU memory','Normal,Warning,Critical,Undefined'
	EXEC [stategroup_update_initial] '60217956-BEA0-4C7D-ADD5-6485C2176FFD','Recording FPS','Normal,Warning,Critical,Undefined'
	EXEC [stategroup_update_initial] '8C958201-AA02-40CF-A643-FA7F91D0BED2','CPU usage','Normal,Warning,Critical,Undefined'

	EXEC [stategroup_create_initial] 'F4261175-44D3-4705-B653-C33C8F0CA70E','Free space','Normal,Warning,Critical,Undefined'
	EXEC [stategroup_create_initial] '8B9B8FA4-3510-4725-8BD0-01AB4E8A71C1','Live FPS','Normal,Warning,Critical,Undefined'
	EXEC [stategroup_create_initial] 'A4F206A2-EAF1-4BC3-9C35-21E9DF0F0FC8','GPU decoding','Normal,Warning,Critical,Undefined'
	EXEC [stategroup_create_initial] '120B550D-3DBD-46A1-94B9-420CAB8E39E5','GPU rendering','Normal,Warning,Critical,Undefined'
	EXEC [stategroup_create_initial] '9D48B11A-0690-409F-8C7A-8FBC8812F657','Retention time','Normal,Warning,Critical,Undefined'
	EXEC [stategroup_create_initial] '72614B0B-62BF-46C2-B74F-9CDE0EEA87EA','Used space','Normal,Warning,Critical,Undefined'

COMMIT TRAN
GO