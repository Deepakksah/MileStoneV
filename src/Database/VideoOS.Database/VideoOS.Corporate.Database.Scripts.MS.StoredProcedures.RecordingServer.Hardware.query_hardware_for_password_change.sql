-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_hardware_for_password_change]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [query_hardware_for_password_change]
GO

CREATE PROCEDURE [dbo].[query_hardware_for_password_change]
(
	@SelectAll BIT = NULL,
	@HardwareIds tvp_StringTableGuidType READONLY,
	@ActiveRecordingServers tvp_StringTableGuidType READONLY
)
 AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID);

	IF @SelectAll = 1
	BEGIN
		WITH temptable (hardware_id, mac_address, username, min_password_len, max_password_len, password_requirements, RS_active)
		AS
		(
			-- all hardware
			SELECT
				h.[IDHardware] AS [hardware_id],
				UPPER(h.[MacAddress]) AS [mac_address],
				h.[LoginID] AS [username],
				h.[Settings].value('(/properties/settings[@hwid="hardware"]/setting[@typeid="34766021-2AC9-4863-85FF-A09A805FC40D"]/value)[1]', 'int') as [min_password_len],
				h.[Settings].value('(/properties/settings[@hwid="hardware"]/setting[@typeid="C14FE929-EEB3-4E7E-9EF5-79F8233B5DE5"]/value)[1]', 'int') as [max_password_len],
				h.[Settings].value('(/properties/settings[@hwid="hardware"]/setting[@typeid="A67E3DD2-05F7-4CDE-993F-31BD64530280"]/value)[1]', 'varchar(256)') as [password_requirements],
				CASE WHEN ars.Id IS NULL THEN 0 ELSE 1 END AS RS_active
			FROM [Hardware] h
			LEFT JOIN @ActiveRecordingServers ars
			ON h.[IDRecorder] = ars.ID
			-- where hardware is enabled and password change is supported
			WHERE 
			(
				[Enabled] = 1 AND 
				UPPER([Settings].value('(/properties/settings[@hwid="hardware"]/setting[@typeid="FA67129C-F392-47DB-8173-06EA486B66E5"]/value)[1]', 'varchar(256)')) = 'YES'
			)
		)

		SELECT * FROM
		(
			SELECT hardware_id, mac_address, username, min_password_len, max_password_len, password_requirements,
					ROW_NUMBER() OVER (PARTITION BY mac_address, username ORDER BY RS_active DESC) AS cnt -- only select hardware which is distinct by MacAddress and LoginID, prioritize by active recording server
			FROM temptable
		) AS X
		WHERE X.cnt = 1 -- we don't use GROUP BY, because we prefer a single row rather than MAX() for all aggregated columns
	END
	ELSE
	BEGIN
		WITH temptable (hardware_id, mac_address, username, min_password_len, max_password_len, password_requirements, RS_active)
		AS
		(
			-- selected hardware by ID
			SELECT
				h.[IDHardware] AS [hardware_id],
				UPPER(h.[MacAddress]) AS [mac_address],
				h.[LoginID] AS [username],
				h.[Settings].value('(/properties/settings[@hwid="hardware"]/setting[@typeid="34766021-2AC9-4863-85FF-A09A805FC40D"]/value)[1]', 'int') as [min_password_len],
				h.[Settings].value('(/properties/settings[@hwid="hardware"]/setting[@typeid="C14FE929-EEB3-4E7E-9EF5-79F8233B5DE5"]/value)[1]', 'int') as [max_password_len],
				h.[Settings].value('(/properties/settings[@hwid="hardware"]/setting[@typeid="A67E3DD2-05F7-4CDE-993F-31BD64530280"]/value)[1]', 'varchar(256)') as [password_requirements],
				CASE WHEN ars.Id IS NULL THEN 0 ELSE 1 END AS RS_active
			FROM [Hardware] h
			INNER JOIN @HardwareIds h2 
			on h.IDHardware = h2.ID
			LEFT JOIN @ActiveRecordingServers ars
			on h.IDRecorder = ars.ID
			-- where hardware is enabled and password change is supported
			WHERE 
			(
				[Enabled] = 1 AND 
				UPPER([Settings].value('(/properties/settings[@hwid="hardware"]/setting[@typeid="FA67129C-F392-47DB-8173-06EA486B66E5"]/value)[1]', 'varchar(256)')) = 'YES'
			)
		)

		SELECT * FROM
		(
			SELECT hardware_id, mac_address, username, min_password_len, max_password_len, password_requirements,
					ROW_NUMBER() OVER (PARTITION BY mac_address, username ORDER BY RS_active DESC) AS cnt -- only select hardware which is distinct by MacAddress and LoginID, prioritize by active recording server
			FROM temptable
		) AS X
		WHERE X.cnt = 1 -- we don't use GROUP BY, because we prefer a single row rather than MAX() for all aggregated columns
	END
END
GO

