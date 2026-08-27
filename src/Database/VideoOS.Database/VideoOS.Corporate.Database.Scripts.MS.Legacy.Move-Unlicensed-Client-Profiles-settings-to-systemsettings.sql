-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
IF EXISTS 
(
	SELECT cp.Name
	FROM ClientProfile cp
	INNER JOIN [SecurityRoles] sr ON cp.Name = sr.IDRole
)
BEGIN
	IF NOT EXISTS ( SELECT [Name] FROM SystemSettings WHERE [Name] = 'SmartClientSettings')
	BEGIN
		INSERT INTO SystemSettings (LastModified, Name, Setting)
		SELECT GETUTCDATE(), 'SmartClientSettings', (SELECT *
		FROM
		(
			SELECT 
				Name AS IDRole,
				CASE  Settings.value('(/Settings/Setting[@key="PanesLiveTab"]/Value)[1]','varchar(max)')
					WHEN 'Available' THEN 1
					ELSE 0
				END AS Live,
				CASE  Settings.value('(/Settings/Setting[@key="PanesPlaybackTab"]/Value)[1]','varchar(max)')
					WHEN 'Available' THEN 1
					ELSE 0
				END AS Playback,
				CASE  Settings.value('(/Settings/Setting[@key="PanesSetupTab"]/Value)[1]','varchar(max)')
					WHEN 'Available' THEN 1
					ELSE 0
				END AS Setup
			FROM [ClientProfile]
			WHERE [Name] IN
			(
				SELECT [IDRole] 
				FROM [SecurityRoles]
			)
		) AS ClientSetting 
		FOR XML AUTO, ROOT('ClientSettings')
		)
	END
	
	DELETE FROM [ClientProfileRoleRelation]
	
	DELETE FROM [ClientProfile]
	WHERE DefaultProfile <> 1
END

IF NOT EXISTS ( SELECT [Name] FROM SystemSettings WHERE [Name] = 'SmartClientSettings')
BEGIN
	INSERT INTO [SystemSettings](LastModified, Name, Setting)
	VALUES(GETUTCDATE(), 'SmartClientSettings', N'<ClientSettings></ClientSettings>')
END

GO
