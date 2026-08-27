-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * from SystemSettings where IDSystemSetting = 'E785D231-F2F8-4721-935B-1CBA8A7AB9DF')
BEGIN
	INSERT INTO SystemSettings ([LastModified], [IDSystemSetting], [Name], [Setting])
	VALUES(getutcdate(), 'E785D231-F2F8-4721-935B-1CBA8A7AB9DF', 'ManagementClientUIProfilesXml',
	'<ManagementClientProfiles>
		<Profiles>
			<Profile Id=''EE8861E1-B6D5-4AFC-B30B-CA63B2D99F3C'' Name=''Default Management Client UI Profile'' Order=''0''>
				<UIElements></UIElements>
			</Profile>
		</Profiles>
		<ProfileRoleRelations />
	</ManagementClientProfiles>')
END
GO

