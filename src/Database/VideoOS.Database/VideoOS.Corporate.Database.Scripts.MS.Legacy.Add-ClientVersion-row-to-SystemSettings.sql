-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * from SystemSettings where Name = 'ClientVersion')
BEGIN
  INSERT INTO SystemSettings ([LastModified], [Name], [Setting])
	VALUES(getdate(), 'ClientVersion',
	'<versions>
	    <smartclient version="Apollo">
			<version major="[SubstMajorVersion]" minor="[SubstMinorVersion]" revision="[SubstRevisionLetterVersion]" beta="0" forceupgrade="false" displayname="[SubstVersion]"/>
				<upgradeinfo>
					<url>
						<hostname></hostname>
						<port></port>
						<path>/</path>
					</url>
					<message></message>
			    </upgradeinfo>
		</smartclient>
	</versions>')
End
GO
