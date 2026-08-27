-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

BEGIN
  DELETE FROM [SystemSettings] WHERE [name] = 'ClientVersion'
  INSERT INTO [SystemSettings] ([LastModified], [Name], [Setting])
	VALUES(getdate(), 'ClientVersion',
	'<versions>
		<smartclient version="Apollo">
			<version major="[SubstMajorVersion]" minor="[SubstMinorVersion]" revision="[SubstRevisionLetterVersion]" beta="[SubstBetaVersion]" forceupgrade="false" displayname="[SubstVersion]"/>
			<upgradeinfo>
				<url>
					<hostname></hostname>
					<port></port>
					<path>/</path>	
				</url>
				<message></message>
			</upgradeinfo>
		</smartclient>
		<upgrade>
			<forcebeloworat major="20" minor="3" revision="a"/>
		</upgrade>
	</versions>')
End
GO