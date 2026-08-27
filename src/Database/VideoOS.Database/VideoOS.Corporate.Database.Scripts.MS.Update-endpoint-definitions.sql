-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================


DECLARE @updated nvarchar(max) = 	N'		
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
			<endpoint>
				<name>ServerCommandServiceOAuth</name>
				<description>Endpoint for ServerCommandService using OAuth authentication</description>
				<authenticationmode>OAuth</authenticationmode>
				<servicepath>ManagementServer/ServerCommandServiceOAuth.svc</servicepath>
				<scheme>https</scheme>
			</endpoint>
			<endpoint>
				<name>ServerCommandServiceOAuth</name>
				<description>Endpoint for ServerCommandService using OAuth authentication</description>
				<authenticationmode>OAuth</authenticationmode>
				<servicepath>ManagementServer/ServerCommandServiceOAuth.svc</servicepath>
				<scheme>http</scheme>
			</endpoint>
			<endpoint>
				<name>ServiceRegistrationServiceOAuth</name>
				<description>Endpoint for ServiceRegistrationService using OAuth authentication</description>
				<authenticationmode>OAuth</authenticationmode>
				<servicepath>ManagementServer/ServiceRegistrationServiceOAuth.svc</servicepath>
				<scheme>https</scheme>ServerEndpointDefinitions
			</endpoint>
			<endpoint>
				<name>ServiceRegistrationServiceOAuth</name>
				<description>Endpoint for ServiceRegistrationService using OAuth authentication</description>
				<authenticationmode>OAuth</authenticationmode>
				<servicepath>ManagementServer/ServiceRegistrationServiceOAuth.svc</servicepath>
				<scheme>http</scheme>ServerEndpointDefinitions
			</endpoint>
		</endpoints>';

UPDATE [dbo].[SystemSettings] set LastModified = GETDATE(), Setting = @updated WHERE Name = 'ServerEndpointDefinitions'
GO