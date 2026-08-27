-- This header is used by the dbupdate7 tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

---------------------------------------------------------------------------
-- ConfigurationDataSet functions
---------------------------------------------------------------------------



IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[patrollingProfileAsDataSet]') AND [type] IN (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [patrollingProfileAsDataSet]
GO

CREATE FUNCTION [patrollingProfileAsDataSet]( @IDPatrollingProfile GuidType )
RETURNS XML
AS
BEGIN
	DECLARE @dataSet XML

	;WITH XMLNAMESPACES ( DEFAULT '{XMLNS}')
	SELECT @dataSet = (
		SELECT * FROM [PatrollingProfiles] 
		WITH(UPDLOCK)
		WHERE [IDPatrollingProfile] = @IDPatrollingProfile
		FOR XML PATH ('PatrollingProfiles'), ROOT('ConfigurationDataSet'), TYPE
	)

	RETURN @dataSet
END
GO

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[devicePrivacyMaskAsDataSet]') AND [type] IN (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [devicePrivacyMaskAsDataSet]
GO

CREATE FUNCTION [devicePrivacyMaskAsDataSet]( @IDDevice GuidType, @ModifiedTime DATETIME)
RETURNS XML
AS
BEGIN
	DECLARE @dataSet XML

	;WITH XMLNAMESPACES ( DEFAULT '{XMLNS}')
	SELECT @dataSet = (
		SELECT * FROM [DevicePrivacyMask] 
		WITH(UPDLOCK)
		WHERE [IDDevice] = @IDDevice AND [LastModified] = @ModifiedTime
		FOR XML PATH ('DevicePrivacyMask'), ROOT('ConfigurationDataSet'), TYPE
	)

	RETURN @dataSet
END
GO

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[deviceLinksAsDataSet]') AND [type] IN (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION deviceLinksAsDataSet
GO

CREATE FUNCTION deviceLinksAsDataSet( @IDDevice GuidType, @IDDeviceLinked GuidType )
RETURNS XML
AS
BEGIN
	DECLARE @dataSet XML

	;WITH XMLNAMESPACES ( DEFAULT '{XMLNS}')
	SELECT @dataSet = (
		SELECT * FROM [DeviceLinks] 
		WITH(UPDLOCK)
		WHERE [IDDevice] = @IDDevice AND [IDDeviceLinked] = @IDDeviceLinked
		FOR XML PATH ('DeviceLinks'), ROOT('ConfigurationDataSet'), TYPE
	)

	RETURN @dataSet
END
GO

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[devicePrivacyMaskAtOrBeforeTimeAsDataSet]') AND [type] IN (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [devicePrivacyMaskAtOrBeforeTimeAsDataSet]
GO

CREATE FUNCTION [devicePrivacyMaskAtOrBeforeTimeAsDataSet]( @IDDevice GuidType, @AtOrBeforeTime DATETIME)
RETURNS XML
AS
BEGIN
	DECLARE @dataSet XML

	;WITH XMLNAMESPACES ( DEFAULT '{XMLNS}')
	SELECT @dataSet = (
		SELECT * FROM [DevicePrivacyMask] 
		WITH(UPDLOCK)
		WHERE [IDDevice] = @IDDevice AND [LastModified] <= @AtOrBeforeTime
		FOR XML PATH ('DevicePrivacyMask'), ROOT('ConfigurationDataSet'), TYPE
	)

	RETURN @dataSet
END
GO

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[timeProfileAsDataSet]') AND [type] IN (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [timeProfileAsDataSet]
GO

CREATE FUNCTION [timeProfileAsDataSet]( @timeprofile_id GuidType )
RETURNS XML
AS
BEGIN
	DECLARE @dataSet XML

	;WITH XMLNAMESPACES ( DEFAULT '{XMLNS}')
	SELECT @dataSet = (
		SELECT * FROM [TimeProfile] 
		WITH(UPDLOCK)
		WHERE [pkid] = @timeprofile_id
		FOR XML PATH ('TimeProfile'), ROOT('ConfigurationDataSet'), TYPE
	)

	RETURN @dataSet
END
GO

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[recordingStorageAsDataSet]') AND [type] IN (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [recordingStorageAsDataSet]
GO

CREATE FUNCTION [recordingStorageAsDataSet]( @IDRecordingStorage GuidType )
RETURNS XML
AS
BEGIN
	DECLARE @dataSet XML

	;WITH XMLNAMESPACES ( DEFAULT '{XMLNS}')
	SELECT @dataSet = (
		SELECT * FROM [RecordingStorage] 
		WITH(UPDLOCK)
		WHERE [pkid] = @IDRecordingStorage 
		FOR XML PATH ('RecordingStorage'), ROOT('ConfigurationDataSet'), TYPE
	)

	RETURN @dataSet
END
GO

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[recordingStorageAsDataSet2]') AND [type] IN (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [recordingStorageAsDataSet2]
GO

CREATE FUNCTION [recordingStorageAsDataSet2]( @IDRecordingStorage1 GuidType, @IDRecordingStorage2 GuidType )
RETURNS XML
AS
BEGIN
	DECLARE @dataSet XML

	;WITH XMLNAMESPACES ( DEFAULT '{XMLNS}')
	SELECT @dataSet = (
		SELECT * FROM [RecordingStorage] 
		WITH(UPDLOCK)
		WHERE [pkid] IN (@IDRecordingStorage1, @IDRecordingStorage2)
		FOR XML PATH ('RecordingStorage'), ROOT('ConfigurationDataSet'), TYPE
	)

	RETURN @dataSet
END
GO

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[archiveStorageAsDataSet]') AND [type] IN (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [archiveStorageAsDataSet]
GO

CREATE FUNCTION [archiveStorageAsDataSet]( @IDArchiveStorage GuidType )
RETURNS XML
AS
BEGIN
	DECLARE @dataSet XML

	;WITH XMLNAMESPACES ( DEFAULT '{XMLNS}')
	SELECT @dataSet = (
		SELECT * FROM [ArchiveStorage] 
		WITH(UPDLOCK)
		WHERE [pkid] = @IDArchiveStorage 
		FOR XML PATH ('ArchiveStorage'), ROOT('ConfigurationDataSet'), TYPE
	)

	RETURN @dataSet
END
GO

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[recordingStorageDeviceAsDataSet]') AND [type] IN (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [recordingStorageDeviceAsDataSet]
GO

CREATE FUNCTION [recordingStorageDeviceAsDataSet]( @device_id GuidType )
RETURNS XML
AS
BEGIN
	DECLARE @dataSet XML

	;WITH XMLNAMESPACES ( DEFAULT '{XMLNS}')
	SELECT @dataSet = (
		SELECT * FROM [RecordingStorageDevice] 
		WITH(UPDLOCK)
		WHERE [device_id] = @device_id
		FOR XML PATH ('RecordingStorageDevice'), ROOT('ConfigurationDataSet'), TYPE
	)

	RETURN @dataSet
END
GO


IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[devicesAsDataSet]') AND [type] IN (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [devicesAsDataSet]
GO

CREATE FUNCTION [devicesAsDataSet]( @device_id GuidType )
RETURNS XML
AS
BEGIN
	DECLARE @dataSet XML

	;WITH XMLNAMESPACES ( DEFAULT '{XMLNS}')
	SELECT @dataSet = (
		SELECT [LastModified]
		      ,[IDDevice]
		      ,[IDHardware]
		      ,[Name]
		      ,[Description]
		      ,[Location]
		      ,[DeviceType]
		      ,[Enabled]
		      ,[Settings]
		      ,[Presets]
		      ,[Shortcut]
		      ,[MulticastEnabled]
		      ,[Streams]
		      ,[DeviceIndex]
		      ,[SubType]
		      ,[FailoverLiveOnly]
		      ,[DisableOnFailover]
		      ,[PtzManualSessionTimeout]
		      ,[PtzReservedSessionTimeout]
		      ,[PtzExtendedManualSessionTimeout]
		      ,GisPoint.AsTextZM() AS [GisPointString]
			  ,[ShortName]
			  ,[Icon]
			  ,[CoverageDirection]
			  ,[CoverageDepth]
			  ,[CoverageFieldOfView]
		FROM [Devices] 
		WITH(UPDLOCK)
		WHERE [IDDevice] = @device_id
		FOR XML PATH ('Devices'), ROOT('ConfigurationDataSet'), TYPE
	)

	RETURN @dataSet
END
GO

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[recorderPluginDeviceAsDataSet]') AND [type] IN (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [recorderPluginDeviceAsDataSet]
GO

CREATE FUNCTION [recorderPluginDeviceAsDataSet]( @device_id GuidType )
RETURNS XML
AS
BEGIN
	DECLARE @dataSet XML

	;WITH XMLNAMESPACES ( DEFAULT '{XMLNS}')
	SELECT @dataSet = (
		SELECT * FROM [RecorderPluginDevice] 
		WITH(UPDLOCK)
		WHERE [IDDevice] = @device_id
		FOR XML PATH ('RecorderPluginDevice'), ROOT('ConfigurationDataSet'), TYPE
	)

	RETURN @dataSet
END
GO

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[hardwareAsDataSet]') AND [type] IN (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [hardwareAsDataSet]
GO

CREATE FUNCTION [hardwareAsDataSet]( @hardware_id GuidType )
RETURNS XML
AS
BEGIN
	DECLARE @dataSet XML

	;WITH XMLNAMESPACES ( DEFAULT '{XMLNS}')
	SELECT @dataSet = (
		SELECT * FROM [Hardware] 
		WITH(UPDLOCK)
		WHERE [IDHardware] = @hardware_id
		FOR XML PATH ('Hardware'), ROOT('ConfigurationDataSet'), TYPE
	)

	RETURN @dataSet
END
GO

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[hardwareBlockAsDataSet]') AND [type] IN (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [hardwareBlockAsDataSet]
GO

CREATE FUNCTION [hardwareBlockAsDataSet]( @pkid INT )
RETURNS XML
AS
BEGIN
	DECLARE @dataSet XML

	;WITH XMLNAMESPACES ( DEFAULT '{XMLNS}')
	SELECT @dataSet = (
		SELECT * FROM [HardwareBlock] 
		WITH(UPDLOCK)
		WHERE [pkid] = @pkid
		FOR XML PATH ('HardwareBlock'), ROOT('ConfigurationDataSet'), TYPE
	)

	RETURN @dataSet
END
GO

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[recordersAsDataSet]') AND [type] IN (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [recordersAsDataSet]
GO

CREATE FUNCTION [recordersAsDataSet]( @recorder_id GuidType )
RETURNS XML
AS
BEGIN
	DECLARE @dataSet XML

	;WITH XMLNAMESPACES ( DEFAULT '{XMLNS}')
	SELECT @dataSet = (
		SELECT * FROM [Recorders] 
		WITH(UPDLOCK)
		WHERE [IDRecorder] = @recorder_id
		FOR XML PATH ('Recorders'), ROOT('ConfigurationDataSet'), TYPE
	)

	RETURN @dataSet
END
GO

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[recordingServerServiceRecorderAsDataSet]') AND [type] IN (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [recordingServerServiceRecorderAsDataSet]
GO

CREATE FUNCTION [recordingServerServiceRecorderAsDataSet]( @recording_server_service_id GuidType )
RETURNS XML
AS
BEGIN
	DECLARE @dataSet XML

	;WITH XMLNAMESPACES ( DEFAULT '{XMLNS}')
	SELECT @dataSet = (
		SELECT * FROM [RecordingServerServiceRecorder] 
		WITH(UPDLOCK)
		WHERE [recording_server_service_id] = @recording_server_service_id
		FOR XML PATH ('RecordingServerServiceRecorder'), ROOT('ConfigurationDataSet'), TYPE
	)

	RETURN @dataSet
END
GO

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[recordingServerServiceConfigurableAsDataSet]') AND [type] IN (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [recordingServerServiceConfigurableAsDataSet]
GO

CREATE FUNCTION [recordingServerServiceConfigurableAsDataSet]( @recording_server_service_id GuidType )
RETURNS XML
AS
BEGIN
	DECLARE @dataSet XML

	;WITH XMLNAMESPACES ( DEFAULT '{XMLNS}')
	SELECT @dataSet = (
		SELECT * FROM [RecordingServerServiceConfigurable] 
		WITH(UPDLOCK)
		WHERE [PKID] = @recording_server_service_id
		FOR XML PATH ('RecordingServerServiceConfigurable'), ROOT('ConfigurationDataSet'), TYPE
	)

	RETURN @dataSet
END
GO

---------------------------------------------------------------------------
-- HostingServer DataSet functions
---------------------------------------------------------------------------
IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[hostingServerAsDataSet]') AND [type] IN (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [hostingServerAsDataSet]
GO

CREATE FUNCTION [hostingServerAsDataSet]( @hosting_server_id INT )
RETURNS XML
AS
BEGIN
	DECLARE @dataSet XML

	;WITH XMLNAMESPACES ( DEFAULT '{XMLNS}')
	SELECT @dataSet = (
		SELECT * FROM [HostingServer] 
		WITH(UPDLOCK)
		WHERE [pkid] = @hosting_server_id
		FOR XML PATH ('HostingServer'), ROOT('ConfigurationDataSet'), TYPE
	)

	RETURN @dataSet
END
GO

---------------------------------------------------------------------------
-- HostedHardware DataSet functions
---------------------------------------------------------------------------
IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[hostedHardwareAsDataSet]') AND [type] IN (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [hostedHardwareAsDataSet]
GO

CREATE FUNCTION [hostedHardwareAsDataSet]( @hosted_hardware_id INT )
RETURNS XML
AS
BEGIN
	DECLARE @dataSet XML

	;WITH XMLNAMESPACES ( DEFAULT '{XMLNS}')
	SELECT @dataSet = (
		SELECT * FROM [HostedHardware] 
		WITH(UPDLOCK)
		WHERE [pkid] = @hosted_hardware_id
		FOR XML PATH ('HostedHardware'), ROOT('ConfigurationDataSet'), TYPE
	)

	RETURN @dataSet
END
GO

---------------------------------------------------------------------------
-- SecurityDataSet functions
---------------------------------------------------------------------------

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[securityPermissionsDeviceAsDataSet]') AND [type] IN (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [securityPermissionsDeviceAsDataSet]
GO

CREATE FUNCTION [securityPermissionsDeviceAsDataSet](@IDRole [GuidType], @IDRecordingServer [GuidType])
RETURNS XML
AS
BEGIN
	DECLARE @dataSet XML

	;WITH XMLNAMESPACES ( DEFAULT '{XMLNS}')
	SELECT @dataSet = (
		SELECT 
			spd.[IDRole],
			spd.[SecurityAttributeID],
			spd.[IDDevice],
			spd.[Value],
			spd.[IDTimeProfile],
			spd.[TimeBackward],
			spd.[IsDefaultTimeProfile],
			spd.[LastModified]
		FROM [SecurityPermissionsDevice] spd WITH(UPDLOCK)
		INNER JOIN [Devices] d ON d.[IDDevice] = spd.[IDDevice]
		INNER JOIN [Hardware] hw ON hw.[IDHardware] = d.[IDHardware]
		WHERE spd.[IDRole] = @IDRole AND hw.[IDRecorder] = @IDRecordingServer
		FOR XML PATH ('SecurityPermissionsDevice'), ROOT('SecurityDataSet'), TYPE
	)

	RETURN @dataSet
END
GO

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[securityRolesAsDataSet]') AND [type] IN (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [securityRolesAsDataSet]
GO

CREATE FUNCTION [securityRolesAsDataSet]( @role_id GuidType )
RETURNS XML
AS
BEGIN
	DECLARE @dataSet XML

	;WITH XMLNAMESPACES ( DEFAULT '{XMLNS}')
	SELECT @dataSet = (
		SELECT * 
		FROM [SecurityRoles] WITH(UPDLOCK)
		WHERE [IDRole] = @role_id
		FOR XML PATH ('SecurityRoles'), ROOT('SecurityDataSet'), TYPE
	)

	RETURN @dataSet
END
GO

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[securityPermissionsAsDataSet]') AND [type] IN (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [securityPermissionsAsDataSet]
GO

CREATE FUNCTION [securityPermissionsAsDataSet]( @role_id GuidType )
RETURNS XML
AS
BEGIN
	DECLARE @dataSet XML

	;WITH XMLNAMESPACES ( DEFAULT '{XMLNS}')
	SELECT @dataSet = (
		SELECT * 
		FROM [SecurityPermissions] WITH(UPDLOCK)
		WHERE [IDRole] = @role_id
		FOR XML PATH ('SecurityPermissions'), ROOT('SecurityDataSet'), TYPE
	)

	RETURN @dataSet
END
GO

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[securityPermissionsMatrixAsDataSet]') AND [type] IN (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [securityPermissionsMatrixAsDataSet]
GO

CREATE FUNCTION [securityPermissionsMatrixAsDataSet]( @role_id GuidType )
RETURNS XML
AS
BEGIN
	DECLARE @dataSet XML

	;WITH XMLNAMESPACES ( DEFAULT '{XMLNS}')
	SELECT @dataSet = (
		SELECT * 
		FROM [SecurityPermissionsMatrix] WITH(UPDLOCK)
		WHERE [IDRole] = @role_id
		FOR XML PATH ('SecurityPermissionsMatrix'), ROOT('SecurityDataSet'), TYPE
	)

	RETURN @dataSet
END
GO

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[timeProfileDefaultRoleRelationAsDataSet]') AND [type] IN (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [timeProfileDefaultRoleRelationAsDataSet]
GO

CREATE FUNCTION [timeProfileDefaultRoleRelationAsDataSet]( @role_id GuidType )
RETURNS XML
AS
BEGIN
	DECLARE @dataSet XML

	;WITH XMLNAMESPACES ( DEFAULT '{XMLNS}')
	SELECT @dataSet = (
		SELECT * 
		FROM [TimeProfileDefaultRoleRelation] WITH(UPDLOCK)
		WHERE [IDRole] = @role_id
		FOR XML PATH ('TimeProfileDefaultRoleRelation'), ROOT('SecurityDataSet'), TYPE
	)

	RETURN @dataSet
END
GO

---------------------------------------------------------------------------
-- UserDefinedEvent DataSet functions
---------------------------------------------------------------------------
IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[userDefinedEventAsDataSet]') AND [type] IN (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [userDefinedEventAsDataSet]
GO

CREATE FUNCTION [userDefinedEventAsDataSet]( @user_defined_dvent_id GuidType )
RETURNS XML
AS
BEGIN
	DECLARE @dataSet XML

	;WITH XMLNAMESPACES ( DEFAULT '{XMLNS}')
	SELECT @dataSet = (
		SELECT * FROM [EventTypes] 
		WITH(UPDLOCK)
		WHERE [IDEventType] = @user_defined_dvent_id
		FOR XML PATH ('EventTypes'), ROOT('ConfigurationDataSet'), TYPE
	)

	RETURN @dataSet
END
GO

---------------------------------------------------------------------------
-- DeviceGroup DataSet functions
---------------------------------------------------------------------------
IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[deviceGroupAsDataSet]') AND [type] IN (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [deviceGroupAsDataSet]
GO

CREATE FUNCTION [deviceGroupAsDataSet]( @device_group_id GuidType )
RETURNS XML
AS
BEGIN
	DECLARE @dataSet XML

	;WITH XMLNAMESPACES ( DEFAULT '{XMLNS}')
	SELECT @dataSet = (
		SELECT * FROM [DeviceGroups] 
		WITH(UPDLOCK)
		WHERE [IDDeviceGroup] = @device_group_id
		FOR XML PATH ('DeviceGroups'), ROOT('ConfigurationDataSet'), TYPE
	)

	RETURN @dataSet
END
GO

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[deviceGroupMembersTableAsDataSet]') AND [type] IN (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [deviceGroupMembersTableAsDataSet]
GO

CREATE FUNCTION [deviceGroupMembersTableAsDataSet]( @device_group_id GuidType, @device_ids tvp_StringTableGuidType READONLY )
RETURNS XML
AS
BEGIN
	DECLARE @dataSet XML

	;WITH XMLNAMESPACES ( DEFAULT '{XMLNS}')
	SELECT @dataSet = (
		SELECT dgm.* 
		FROM [DeviceGroupMembers] dgm WITH(UPDLOCK)
		INNER JOIN @device_ids ids ON ids.[ID] = dgm.[IDDevice]
		WHERE [IDDeviceGroup] = @device_group_id
		FOR XML PATH ('DeviceGroupMembers'), ROOT('ConfigurationDataSet'), TYPE
	)

	RETURN @dataSet
END
GO

---------------------------------------------------------------------------
-- MIP Object DataSet functions
---------------------------------------------------------------------------
IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[mipObjectAsDataSet]') AND [type] IN (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [mipObjectAsDataSet]
GO

CREATE FUNCTION [mipObjectAsDataSet]( @mipObject_id GuidType )
RETURNS XML
AS
BEGIN
	DECLARE @dataSet XML

	;WITH XMLNAMESPACES ( DEFAULT '{XMLNS}')
	SELECT @dataSet = (
		SELECT * FROM [MIPObject] 
		WITH(UPDLOCK)
		WHERE [IDMIPObject] = @mipObject_id
		FOR XML PATH ('MIPObject'), ROOT('ConfigurationDataSet'), TYPE
	)

	RETURN @dataSet
END
GO

---------------------------------------------------------------------------
-- ServerGroup DataSet functions
---------------------------------------------------------------------------
IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[serverGroupAsDataSet]') AND [type] IN (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [serverGroupAsDataSet]
GO

CREATE FUNCTION [serverGroupAsDataSet]( @server_group_id GuidType )
RETURNS XML
AS
BEGIN
	DECLARE @dataSet XML

	;WITH XMLNAMESPACES ( DEFAULT '{XMLNS}')
	SELECT @dataSet = (
		SELECT * FROM [ServerGroup] 
		WITH(UPDLOCK)
		WHERE [IDServerGroup] = @server_group_id
		FOR XML PATH ('ServerGroup'), ROOT('ConfigurationDataSet'), TYPE
	)

	RETURN @dataSet
END
GO

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[serverGroupMemberAsDataSet]') AND [type] IN (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [serverGroupMemberAsDataSet]
GO

CREATE FUNCTION [serverGroupMemberAsDataSet]( @server_group_id GuidType, @IDObject GuidType )
RETURNS XML
AS
BEGIN
	DECLARE @dataSet XML

	;WITH XMLNAMESPACES ( DEFAULT '{XMLNS}')
	SELECT @dataSet = (
		SELECT sgm.* 
		FROM [ServerGroupMembership] sgm WITH(UPDLOCK)
		WHERE [IDServerGroup] = @server_group_id
		AND [IDObject] = @IDObject
		FOR XML PATH ('ServerGroupMembership'), ROOT('ConfigurationDataSet'), TYPE
	)

	RETURN @dataSet
END
GO

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[recorderInfoAsDataSet]') AND [type] IN (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [recorderInfoAsDataSet]
GO

CREATE FUNCTION [recorderInfoAsDataSet]( @recorder_id GuidType )
RETURNS XML
AS
BEGIN
	DECLARE @dataSet XML

	;WITH XMLNAMESPACES ( DEFAULT '{XMLNS}')
	SELECT @dataSet = (
		SELECT * 
		FROM [RecorderInfo] WITH(UPDLOCK)
		WHERE [recorder_id] = @recorder_id
		FOR XML PATH ('RecorderInfo'), ROOT('ConfigurationDataSet'), TYPE
	)

	RETURN @dataSet
END
GO

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[failoverInfoAsDataSet]') AND [type] IN (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [failoverInfoAsDataSet]
GO

CREATE FUNCTION [failoverInfoAsDataSet]( @recording_server_service_id GuidType )
RETURNS XML
AS
BEGIN
	DECLARE @dataSet XML

	;WITH XMLNAMESPACES ( DEFAULT '{XMLNS}')
	SELECT @dataSet = (
		SELECT * 
		FROM [FailoverInfo] WITH(UPDLOCK)
		WHERE [recording_server_service_id] = @recording_server_service_id
		FOR XML PATH ('FailoverInfo'), ROOT('ConfigurationDataSet'), TYPE
	)

	RETURN @dataSet
END
GO

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[deviceDataDistributionAsDataSet]') AND [type] IN (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [deviceDataDistributionAsDataSet]
GO

CREATE FUNCTION [deviceDataDistributionAsDataSet]( @recorder_id GuidType, @track_id UniqueIdentifier )
RETURNS XML
AS
BEGIN
	DECLARE @dataSet XML

	;WITH XMLNAMESPACES ( DEFAULT '{XMLNS}')
	SELECT @dataSet = (
		SELECT * 
		FROM [DeviceDataDistribution] WITH(UPDLOCK)
		WHERE [recorder_id] = @recorder_id AND [track_id] = @track_id
		FOR XML PATH ('DeviceDataDistribution'), ROOT('ConfigurationDataSet'), TYPE
	)

	RETURN @dataSet
END
GO

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[systemSettingsAsDataSet]') AND [type] IN (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION systemSettingsAsDataSet
GO

CREATE FUNCTION [systemSettingsAsDataSet]( @setting_name NameType)
RETURNS XML
AS
BEGIN
	DECLARE @dataSet XML

	;WITH XMLNAMESPACES ( DEFAULT '{XMLNS}')
	SELECT @dataSet = (
		SELECT * 
		FROM [SystemSettings] WITH(UPDLOCK)
		WHERE [Name] = @setting_name 
		FOR XML PATH ('SystemSettings'), ROOT('ConfigurationDataSet'), TYPE
	)

	RETURN @dataSet
END
GO

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[audioMessageAsDataSet]') AND [type] IN (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION audioMessageAsDataSet
GO

CREATE FUNCTION [audioMessageAsDataSet](@Id [GuidType])
RETURNS XML
AS
BEGIN
	DECLARE @dataSet XML

	;WITH XMLNAMESPACES ( DEFAULT '{XMLNS}')
	SELECT @dataSet = (
		SELECT * 
		FROM [AudioMessage] WITH(UPDLOCK)
		WHERE [ID] = @Id
		FOR XML PATH ('AudioMessage'), ROOT('ConfigurationDataSet'), TYPE
	)

	RETURN @dataSet
END
GO

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[ruleAsDataSet]') AND [type] IN (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION ruleAsDataSet
GO

CREATE FUNCTION [ruleAsDataSet](@Id [GuidType])
RETURNS XML
AS
BEGIN
	DECLARE @dataSet XML

	;WITH XMLNAMESPACES ( DEFAULT '{XMLNS}')
	SELECT @dataSet = (
		SELECT * 
		FROM [Rules] WITH(UPDLOCK)
		WHERE [IDRule] = @Id
		FOR XML PATH ('Rules'), ROOT('ConfigurationDataSet'), TYPE
	)

	RETURN @dataSet
END
GO

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[deviceTrackEvidenceLockAsDataSet]') AND [type] IN (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [deviceTrackEvidenceLockAsDataSet]
GO

CREATE FUNCTION [deviceTrackEvidenceLockAsDataSet](@IDRecorder GuidType, @IDTrack UniqueIdentifier = null, @IDDevice GuidType = null, @IDMarkedData UniqueIdentifier = null)
RETURNS XML
AS
BEGIN
	DECLARE @dataSet XML

	;WITH XMLNAMESPACES ( DEFAULT '{XMLNS}')
	SELECT @dataSet = (
		SELECT * FROM [DeviceTrackEvidenceLock]
		-- WITH(UPDLOCK) -- Updatelock on views??
		WHERE ([IDRecorder] = @IDRecorder AND 
			(@IDTrack IS NULL OR [IDTrack] = @IDTrack) AND 
			(@IDDevice IS NULL OR [IDDevice] = @IDDevice) AND 
			(@IDMarkedData IS NULL OR [IDMarkedData] = @IDMarkedData))
		FOR XML PATH ('DeviceTrackEvidenceLock'), ROOT('ConfigurationDataSet'), TYPE
	)

	RETURN @dataSet
END
GO

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[deviceTrackInfoAsDataSet]') AND [type] IN (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [deviceTrackInfoAsDataSet]
GO

CREATE FUNCTION [deviceTrackInfoAsDataSet](@IDRecorder GuidType, @IDTrack UniqueIdentifier)
RETURNS XML
AS
BEGIN
	DECLARE @dataSet XML

	;WITH XMLNAMESPACES ( DEFAULT '{XMLNS}')
	SELECT @dataSet = (
		SELECT * FROM [DeviceTrackInfo]
		-- WITH(UPDLOCK) -- Updatelock on views??
		WHERE [IDRecorder] = @IDRecorder AND [IDTrack] = @IDTrack
		FOR XML PATH ('DeviceTrackInfo'), ROOT('ConfigurationDataSet'), TYPE
	)

	RETURN @dataSet
END
GO

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[deviceRestrictedMediaAsDataSet]') AND [type] IN (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [deviceRestrictedMediaAsDataSet]
GO

CREATE FUNCTION [deviceRestrictedMediaAsDataSet](@IDRestrictedMedia UNIQUEIDENTIFIER, @IDDevice GuidType)
RETURNS XML
AS
BEGIN
	DECLARE @dataSet XML

	;WITH XMLNAMESPACES ( DEFAULT '{XMLNS}')
	SELECT @dataSet = (
		SELECT * FROM [DeviceRestrictedMedia]
		-- WITH(UPDLOCK) -- Updatelock on views??
		WHERE [IDRestrictedMedia] = @IDRestrictedMedia AND [IDDevice] = @IDDevice
		FOR XML PATH ('DeviceRestrictedMedia'), ROOT('ConfigurationDataSet'), TYPE
	)

	RETURN @dataSet
END
GO

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[deviceTrackAsDataSet]') AND [type] IN (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [deviceTrackAsDataSet]
GO

CREATE FUNCTION [deviceTrackAsDataSet](@IDTrack UniqueIdentifier)
RETURNS XML
AS
BEGIN
	DECLARE @dataSet XML

	;WITH XMLNAMESPACES ( DEFAULT '{XMLNS}')
	SELECT @dataSet = (
		SELECT * FROM [DeviceTrack]
		WITH(UPDLOCK)
		WHERE [IDTrack] = @IDTrack
		FOR XML PATH ('DeviceTrack'), ROOT('ConfigurationDataSet'), TYPE
	)

	RETURN @dataSet
END
GO