-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'ClientProfile') AND name = N'profile_type')
BEGIN
	ALTER TABLE [ClientProfile]
	ADD [profile_type] NVARCHAR(200) NULL
END
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'ClientProfile') AND name = N'SettingsLastModified')
BEGIN
	ALTER TABLE [ClientProfile]
	DROP COLUMN [SettingsLastModified]
END
GO

IF NOT EXISTS (SELECT 1 FROM ClientProfile WHERE [profile_type] = 'ManagementClientProfile')
BEGIN
	UPDATE [ClientProfile]
	SET [profile_type] = 'SmartClientProfile'
END
GO

DECLARE @ManagementClientXML XML
SELECT @ManagementClientXML = CAST(Setting AS XML) FROM SystemSettings WHERE Name = 'ManagementClientUIProfilesXml'

IF NOT EXISTS (SELECT 1 FROM ClientProfile WHERE [profile_type] = 'ManagementClientProfile')
BEGIN
	INSERT INTO ClientProfile([IDClientProfile], [Name], [Description], [LastModified], [Priority], [DefaultProfile], [Settings], [profile_type])
	SELECT
		profiles.value('@Id','GuidType') as Id
		, profiles.value('@Name', 'NVARCHAR(200)') as Name
		, CASE WHEN profiles.value('@Description','NVARCHAR(MAX)') IS NOT NULL THEN profiles.value('@Description','NVARCHAR(MAX)') ELSE '' END as Description
		, GETUTCDATE() as LastModified
		, profiles.value('@Order','INT') as Priority
		, CASE WHEN profiles.value('@Order','INT') = 0 THEN 1 ELSE 0 END as DefaultProfile
		, sett.query('<Settings>
					  {
						for $s in UIElement
						return
						<Setting key="{data($s/@Id)}">
						   <Value>Hide</Value>
						   <Locked>FALSE</Locked>
						</Setting>
					   }
					   </Settings>') as Settings
		, 'ManagementClientProfile' as profile_type
	FROM @ManagementClientXML.nodes(N'ManagementClientProfiles/Profiles/Profile') Profiles(profiles)
	CROSS APPLY profiles.nodes('UIElements') as Settings(sett)

	ALTER TABLE ClientProfileRoleRelation
	DROP CONSTRAINT [PK_ClientProfileRoleRelation]

	INSERT INTO ClientProfileRoleRelation(IDClientProfile, IDRole)
	SELECT relations.value('@ProfileId','GuidType') as ProfileId
		   , relations.value('@RoleId','GuidType') as RoleId
	FROM @ManagementClientXML.nodes(N'ManagementClientProfiles/ProfileRoleRelations/ProfileRoleRelation') Relations(relations)
	INNER JOIN SecurityRoles sr ON sr.IDRole = relations.value('@RoleId','GuidType')
	
END
GO

UPDATE ClientProfile
SET Name = dt.Name + '(' + CAST(r AS VARCHAR(10)) + ')'
FROM
(
	select Name, RANK() OVER(PARTITION BY Name, profile_type ORDER BY Priority) - 1 as r, IDClientProfile AS Id FROM ClientProfile
) as dt
WHERE r > 0 AND IDClientProfile = dt.Id

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[ClientProfile]') AND name = 'AK_UniqueName')
BEGIN
	ALTER TABLE ClientProfile
	ADD CONSTRAINT AK_UniqueName UNIQUE (Name, profile_type)
END
GO


