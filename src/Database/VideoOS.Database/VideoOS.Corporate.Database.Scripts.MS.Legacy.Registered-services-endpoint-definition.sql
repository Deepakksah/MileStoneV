-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.columns WHERE [object_id] = OBJECT_ID(N'RegisteredServers') AND name = N'EndpointDefinitions')
BEGIN
	ALTER TABLE RegisteredServers
	ADD [EndpointDefinitions] varchar(max) NOT NULL DEFAULT N'<endpoints></endpoints>'
END
GO

IF NOT EXISTS (SELECT * FROM [SystemSettings] WHERE [IDSystemSetting] = '7B114323-507B-453F-8FFE-C017CE36EFA3')
BEGIN
	INSERT INTO [SystemSettings](LastModified, IDSystemSetting, Name, Setting)
	VALUES 
	(
		GETDATE(), 
		'7B114323-507B-453F-8FFE-C017CE36EFA3', 
		'ServerEndpointDefinitions', 
		N'
		<endpoints>
			<endpoint>
				<name>ServerCommandService</name>
				<description>Endpoint for ServerCommandService using windows authentication</description>
				<authenticationmode>Windows</authenticationmode>
				<servicepath>ManagementServer/ServerCommandService.svc</servicepath>
				<scheme>http</scheme>
			</endpoint>
			<endpoint>
				<name>ServerCommandService</name>
				<description>Endpoint for ServerCommandService using basic user authentication</description>
				<authenticationmode>Basic</authenticationmode>
				<servicepath>ManagementServer/ServerCommandService.svc</servicepath>
				<scheme>https</scheme>
			</endpoint>
			<endpoint>
				<name>ServiceRegistrationService</name>
				<description>Endpoint for ServiceRegistrationService using windows authentication</description>
				<authenticationmode>Windows</authenticationmode>
				<servicepath>ManagementServer/ServiceRegistrationService.svc</servicepath>
				<scheme>http</scheme>
			</endpoint>
			<endpoint>
				<name>ServiceRegistrationService</name>
				<description>Endpoint for ServiceRegistrationService using basic user authentication</description>
				<authenticationmode>Basic</authenticationmode>
				<servicepath>ManagementServer/ServiceRegistrationService.svc</servicepath>
				<scheme>https</scheme>
			</endpoint>
		</endpoints>'
	)
END
GO
