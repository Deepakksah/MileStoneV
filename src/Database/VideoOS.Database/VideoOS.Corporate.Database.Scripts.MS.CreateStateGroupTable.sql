-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF OBJECT_ID ( '[StateGroup]', 'U') IS NULL
CREATE TABLE [StateGroup]
(
	[Id] UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[States] [nvarchar](max) NOT NULL,
	[last_modified] DATETIME NOT NULL,
) ON [PRIMARY]
GO

IF OBJECT_ID ( '[dbo].[stategroup_create_initial]', 'P') IS NOT NULL
	DROP PROCEDURE stategroup_create_initial
GO

CREATE PROCEDURE [dbo].stategroup_create_initial
	@Id [UniqueIdentifier],
	@Name [NameType],
	@States NVarchar(1024)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @CurrentUtcTime DATETIME
	SET @CurrentUtcTime = GETUTCDATE()

	INSERT INTO [StateGroup]
	(
		[Id],
		[Name],
		[States],
		[last_modified]
	) VALUES (
		@Id,
		@Name,
		@States,
		@CurrentUtcTime
	)

	RETURN 0
END
GO


BEGIN TRAN

-- Insert predefined State Groups

	EXEC [stategroup_create_initial] 'C0279551-7CD7-40C5-AA62-C6350A9C4430','Feed Overflow','No Overflow,Overflow'
	EXEC [stategroup_create_initial] '5EEDDBBA-7066-4AAA-92E5-6CB9FAE1FC85','Manual PTZ session started','Started,Idle'
	EXEC [stategroup_create_initial] '2F965115-6F57-4004-AC0B-4246AEE5200A','Output','Not Active,Active'
	EXEC [stategroup_create_initial] '15F76BAD-F660-448F-9E1C-0539D3F54F71','Recording','Not Recording,Recording'
	EXEC [stategroup_create_initial] '137E6804-F29D-418F-B9BF-1B7733B2DB95','Live feed requested','No,Yes'
	EXEC [stategroup_create_initial] '2A91B8A3-0BEE-4C4E-BC6D-49EAB6776738','Manual recording','Not Recording,Recording'
	EXEC [stategroup_create_initial] 'FA20664C-C35D-4346-A87D-87E9D7511194','Input','Not Active,Active'
	EXEC [stategroup_create_initial] '435FAFBE-3A5B-4A90-A02D-69008740B0C7','Communication H/W','Stopped,Started,Error'
	EXEC [stategroup_create_initial] '54FD5E27-FA54-4D5D-89D3-E4C288B0C7F2','Failover active','No,Yes'
	EXEC [stategroup_create_initial] '53B40C77-CF48-42A2-A432-DB65AE3AFC7A','Communication','Stopped,Started,Error'
	EXEC [stategroup_create_initial] 'C1EB0D2B-1903-45F6-B0C8-3F4CD1879B2A','Service','Normal,Critical'
	EXEC [stategroup_create_initial] 'FEB52F64-AD52-41F4-AC13-16576460DB7D','Memory usage','Normal,Warning,Critical'
	EXEC [stategroup_create_initial] '8710ED5D-1FB8-4551-81FB-49B269267D5D','Nvidia memory','Normal,Warning,Critical'
	EXEC [stategroup_create_initial] '60217956-BEA0-4C7D-ADD5-6485C2176FFD','Recording FPS','Normal,Warning,Critical'
	EXEC [stategroup_create_initial] '8C958201-AA02-40CF-A643-FA7F91D0BED2','CPU usage','Normal,Warning,Critical'
	EXEC [stategroup_create_initial] '720EF62A-78BE-418A-8E4C-EE04FFCA6429','Motion detected','Motion,No Motion'

COMMIT TRAN
GO