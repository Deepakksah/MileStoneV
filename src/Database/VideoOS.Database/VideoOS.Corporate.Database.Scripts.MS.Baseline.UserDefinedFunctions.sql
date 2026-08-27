SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION ConvertIntegerToIP(@IPAddress AS BIGINT)
RETURNS VARCHAR(15)
AS
BEGIN
	DECLARE @Octet1 TINYINT
	DECLARE @Octet2 TINYINT
	DECLARE @Octet3 TINYINT
	DECLARE @Octet4 TINYINT
	DECLARE @RestOfIP BIGINT
	SET @Octet1 = @IPAddress / CAST(16777216 AS BIGINT)
	SET @RestOfIP = @IPAddress - (@Octet1 * CAST(16777216 AS BIGINT))
	SET @Octet2 = @RestOfIP / 65536
	SET @RestOfIP = @RestOfIP - (@Octet2 * 65536)
	SET @Octet3 = @RestOfIP / 256
	SET @Octet4 = @RestOfIP - (@Octet3 * 256)
	RETURN(
		CONVERT(VARCHAR, @Octet1) + '.' +
		CONVERT(VARCHAR, @Octet2) + '.' +
		CONVERT(VARCHAR, @Octet3) + '.' +
		CONVERT(VARCHAR, @Octet4))
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION ConvertIPToInteger(@IPAddress AS VARCHAR(15))
RETURNS BIGINT
AS
BEGIN
	RETURN (
		CONVERT(BIGINT, PARSENAME(@IPAddress, 1)) +
		CONVERT(BIGINT, PARSENAME(@IPAddress, 2)) * 256 +
		CONVERT(BIGINT, PARSENAME(@IPAddress, 3)) * 65536 +
		CONVERT(BIGINT, PARSENAME(@IPAddress, 4)) * CAST(16777216 AS BIGINT))
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [encodeXml]( @txt NVARCHAR(MAX) )
RETURNS NVARCHAR(MAX)
AS
BEGIN
	DECLARE @xmlText NVARCHAR(MAX)
	SET @xmlText = REPLACE(@txt, '&', '&amp;')
	SET @xmlText = REPLACE(@xmlText, '<', '&lt;')
	SET @xmlText = REPLACE(@xmlText, '>', '&gt;')
	SET @xmlText = REPLACE(@xmlText, '"', '&quot;')
	SET @xmlText = REPLACE(@xmlText, '''', '&apos;')
	RETURN @xmlText
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [EscapeWildcards]
(
	@input AS NVARCHAR(MAX)
)
RETURNS NVARCHAR(MAX)
AS BEGIN
	DECLARE @result NVARCHAR(MAX)
	SET @result =  REPLACE(@input, '\','\\' )
	SET @result =  REPLACE(@result, '[', '\[')
	SET @result =  REPLACE(@result, '%', '\%')
	SET @result =  REPLACE(@result, '_', '\_')
	
	RETURN @result
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION fn_SIDToString
(
  @BinSID AS VARBINARY(100)
)
RETURNS VARCHAR(100)
AS BEGIN
  IF LEN(@BinSID) % 4 <> 0 RETURN(NULL)
  DECLARE @StringSID VARCHAR(100)
  DECLARE @i AS INT
  DECLARE @j AS INT
  SELECT @StringSID = 'S-'
     + CONVERT(VARCHAR, CONVERT(INT, CONVERT(VARBINARY, SUBSTRING(@BinSID, 1, 1)))) 
  SELECT @StringSID = @StringSID + '-'
     + CONVERT(VARCHAR, CONVERT(INT, CONVERT(VARBINARY, SUBSTRING(@BinSID, 3, 6))))
  SET @j = 9
  SET @i = LEN(@BinSID)
  WHILE @j < @i
  BEGIN
    DECLARE @val BINARY(4)
    SELECT @val = SUBSTRING(@BinSID, @j, 4)
    SELECT @StringSID = @StringSID + '-'
      + CONVERT(VARCHAR, CONVERT(BIGINT, CONVERT(VARBINARY, REVERSE(CONVERT(VARBINARY, @val))))) 
    SET @j = @j + 4
  END
  RETURN ( @StringSID ) 
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION dbo.fn_TRIM(@s NVARCHAR(MAX))
RETURNS NVARCHAR(MAX)
AS
BEGIN
	DECLARE @length INT = DATALENGTH(@s) / 2
	DECLARE @firstCharPosition INT = PATINDEX('%[^' + CHAR(32) + CHAR(09) + CHAR(10) + CHAR(13) + ']%', @s)
	DECLARE @lastCharPosition INT = @length - (PATINDEX('%[^' + CHAR(32) + CHAR(09) + CHAR(10) + CHAR(13) + ']%', REVERSE(@s)) - 1)
	RETURN SUBSTRING(@s, @firstCharPosition, @lastCharPosition - @firstCharPosition + 1)
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [FormatErrorMessage](@ERROR_NUMBER INT, @ERROR_SEVERITY INT, @ERROR_STATE INT, @ERROR_PROCEDURE NVARCHAR(MAX), @ERROR_LINE INT, @ERROR_MESSAGE NVARCHAR(MAX))
RETURNS NVARCHAR(MAX)
AS
BEGIN
	DECLARE @errorMessage NVARCHAR(MAX)
	SELECT @errorMessage = N': %%Status=' + CAST(@ERROR_NUMBER AS NVARCHAR(10)) 
		+ N': %%Severity=' + CAST(@ERROR_SEVERITY AS NVARCHAR(10)) 
		+ N': %%State=' + CAST(@ERROR_STATE AS NVARCHAR(10)) 
		+ N': %%Procedure=' + @@SERVERNAME + '.' + db_name() + '..' + @ERROR_PROCEDURE
		+ N': %%Line=' + CAST(@ERROR_LINE AS NVARCHAR(10)) 
		+ N': %%Message=' + @ERROR_MESSAGE;
	RETURN @errorMessage
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [GetDeviceGroupRelations] (@IDDeviceGroup GuidType)
RETURNS NVARCHAR(MAX)
AS
BEGIN
	IF(@IDDeviceGroup IS NULL OR @IDDeviceGroup = '')
	BEGIN
		RETURN ''
	END
	DECLARE @Ids NVARCHAR(MAX)
	;WITH CTERelations AS
	(
		SELECT @IDDeviceGroup AS ID
		UNION ALL
				 
		SELECT dg.IDParentDeviceGroup AS ID
		FROM [DeviceGroups] dg
		INNER JOIN CTERelations cte ON cte.ID = dg.IDDeviceGroup	 
	)
	SELECT @Ids = 
	STUFF(( 
		SELECT '; ' + ID 
		FROM CTERelations
		FOR XML PATH('')), 1,1, '')
	RETURN @Ids
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [GetServerGroupRelations] (@IDServerGroup GuidType)
RETURNS NVARCHAR(MAX)
AS
BEGIN
	DECLARE @Ids NVARCHAR(MAX)
	IF(@IDServerGroup IS NULL OR @IDServerGroup = '')
	BEGIN
		RETURN ''
	END
	;WITH CTERelations AS
	(
		SELECT @IDServerGroup AS ID
		UNION ALL
						 
		SELECT sg.IDParentGroup AS ID
		FROM [ServerGroup] sg
		INNER JOIN CTERelations cte ON cte.ID = sg.IDServerGroup
	)
	SELECT @Ids = 
	STUFF(( 
		SELECT '; ' + ID 
		FROM CTERelations
		FOR XML PATH('')), 1,1, '')
	RETURN @Ids
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [GetSitesRelations] (@IDSite GuidType)
RETURNS NVARCHAR(MAX)
AS
BEGIN
	 DECLARE @Ids NVARCHAR(MAX)
	;WITH CTERelations AS
	(
		SELECT sr.IDSiteMaster AS ID
		FROM SitesRelation sr
		INNER JOIN Sites s ON s.IDSite = sr.IDSiteMaster
		WHERE sr.IDSiteSlave = @IDSite
		AND s.IDSiteType = 3
		UNION ALL
		
		SELECT sr.IDSiteMaster AS ID
		FROM SitesRelation sr
		INNER JOIN CTERelations cte ON cte.ID = sr.IDSiteSlave
		INNER JOIN Sites s ON s.IDSite = sr.IDSiteMaster
		WHERE s.IDSiteType = 3
	)
	SELECT @Ids = 
	STUFF(( 
		SELECT '; ' + ID 
		FROM CTERelations
		FOR XML PATH('')), 1,1, '')
	RETURN @Ids
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [SecurityAvailableDevices] (@RoleIds XML, @SecurityGroupActionName VARCHAR(100), @EnabledDevicesOnly BIT, @IDRecorder GuidType)
RETURNS @Devices TABLE (IDDevice GuidType)
AS
BEGIN
	DECLARE @RoleIdTable TABLE (ID GuidType)
	INSERT INTO @RoleIdTable
	SELECT ID FROM XmlToIdTable(@RoleIds)
	
	DECLARE @IsAdmin BIT
	DECLARE @CameraOverallAllow BIT
	DECLARE @MicrophoneOverallAllow BIT
	DECLARE @SpeakerOverallAllow BIT
	DECLARE @OutputOverallAllow BIT
	DECLARE @InputOverallAllow BIT
	DECLARE @MetadataOverallAllow BIT
	DECLARE @CameraOverallDeny BIT
	DECLARE @MicrophoneOverallDeny BIT
	DECLARE @SpeakerOverallDeny BIT
	DECLARE @OutputOverallDeny BIT
	DECLARE @InputOverallDeny BIT
	DECLARE @MetadataOverallDeny BIT
	
	IF(EXISTS(SELECT ID FROM @RoleIdTable roles2 WHERE ID = '1f30d14b-ba91-4e8a-8838-eda17931f8fc'))
	BEGIN
		SET @IsAdmin = 1
	END
	ELSE
	BEGIN
		SET @IsAdmin = 0
	END
	IF(@SecurityGroupActionName = 'GENERIC_READ')
	BEGIN
		IF(@IsAdmin = 1)
		BEGIN
			INSERT @Devices 
			SELECT IDDevice
			FROM Devices
			INNER JOIN [Hardware] ON [Devices].[IDHardware] = [Hardware].[IDHardware]
			WHERE
				([Hardware].[IDRecorder] = @IDRecorder OR @IDRecorder IS NULL) AND
				([Hardware].[Enabled] = 1 AND [Devices].[Enabled] = 1 OR @EnabledDevicesOnly = 0)
		END
		ELSE
		BEGIN
			SELECT @CameraOverallAllow = COALESCE(MAX(CASE 
					WHEN [access_deny] = 0 AND namespace_id = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85' THEN 1
					ELSE 0
					END), 0),
				@CameraOverallDeny = COALESCE(MAX(CASE 
					WHEN [access_deny] = 1 AND namespace_id = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85' THEN 1
					ELSE 0
					END), 0),
				@MicrophoneOverallAllow = COALESCE(MAX(CASE 
					WHEN [access_deny] = 0 AND namespace_id = '15F48F88-CA89-4926-9A84-2B02864EC77A' THEN 1
					ELSE 0
					END),0),
				@MicrophoneOverallDeny =COALESCE(MAX(CASE 
					WHEN [access_deny] = 1 AND namespace_id = '15F48F88-CA89-4926-9A84-2B02864EC77A' THEN 1
					ELSE 0
					END),0),
				@SpeakerOverallAllow = COALESCE(MAX(CASE 
					WHEN [access_deny] = 0 AND namespace_id = '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD' THEN 1
					ELSE 0
					END),0),
				@SpeakerOverallDeny =COALESCE(MAX(CASE 
					WHEN [access_deny] = 1 AND namespace_id = '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD' THEN 1
					ELSE 0
					END),0),
				@OutputOverallAllow = COALESCE(MAX(CASE 
					WHEN [access_deny] = 0 AND namespace_id = 'A95FCE90-753D-4CB7-86FB-F8CAD9F93B67' THEN 1
					ELSE 0
					END),0),
				@OutputOverallDeny =COALESCE(MAX(CASE 
					WHEN [access_deny] = 1 AND namespace_id = 'A95FCE90-753D-4CB7-86FB-F8CAD9F93B67' THEN 1
					ELSE 0
					END),0),
				@InputOverallAllow = COALESCE(MAX(CASE 
					WHEN [access_deny] = 0 AND namespace_id = '3ED79EF0-EE2F-40B8-A49A-17E9380C5932' THEN 1
					ELSE 0
					END),0),
				@InputOverallDeny =COALESCE(MAX(CASE 
					WHEN [access_deny] = 1 AND namespace_id = '3ED79EF0-EE2F-40B8-A49A-17E9380C5932' THEN 1
					ELSE 0
					END),0),
				@MetadataOverallAllow = COALESCE(MAX(CASE 
					WHEN [access_deny] = 0 AND namespace_id = 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D' THEN 1
					ELSE 0
					END),0),
				 @MetadataOverallDeny =COALESCE(MAX(CASE 
					WHEN [access_deny] = 1 AND namespace_id = 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D' THEN 1
					ELSE 0
					END), 0)
			FROM [SecurityObject] so
					INNER JOIN [SecurityAccessEntry] sae ON so.[security_object_id] = sae.[security_object_id]
					INNER JOIN @RoleIdTable roles ON sae.[user_sid] = roles.ID
					WHERE so.[object_id] = '/'
					AND sae.[action_name] = 'GENERIC_READ'
					AND	namespace_id IN 
					('623D03F8-C5D5-46BC-A2F4-4C03562D4F85'
					,'15F48F88-CA89-4926-9A84-2B02864EC77A'
					,'48B602BC-E752-4BBF-8E2A-7DE01F53A6DD'
					,'3ED79EF0-EE2F-40B8-A49A-17E9380C5932'
					,'A95FCE90-753D-4CB7-86FB-F8CAD9F93B67'
					,'EDE4D51C-F691-4894-9C0B-C3AE096DC04D')
			INSERT @Devices 
			SELECT DISTINCT (d.IDDevice)
			FROM [Devices] d
			INNER JOIN [Hardware] h ON d.[IDHardware] = h.[IDHardware]
			LEFT JOIN
			(
				SELECT * FROM [SecurityPermissionsDevice]
				WHERE [Value] = 'True' 
				AND SecurityAttributeID IN 
				('DEVICEVISIBLE',
				 'DEVICEMICVISIBLE',
				 'DEVICESPEAKERVISIBLE',
				 'DEVICEINPUTVISIBLE',
				 'DEVICEOUTPUTVISIBLE',
				 'DEVICEMETADATAVISIBLE')
				AND IDRole IN (SELECT ID FROM @RoleIdTable)
			) spd ON spd.IDDevice = d.IDDevice
			WHERE 
			(h.[IDRecorder] = @IDRecorder OR @IDRecorder IS NULL) AND
			(@EnabledDevicesOnly = 0 OR (h.[Enabled] = 1 AND d.[Enabled] = 1)) AND
			(
				(@CameraOverallAllow = 1 AND @CameraOverallDeny = 0 AND d.DeviceType = 'Camera') OR
				(@MicrophoneOverallAllow = 1 AND @MicrophoneOverallDeny = 0 AND d.DeviceType = 'AudioInput') OR
				(@SpeakerOverallAllow = 1 AND @SpeakerOverallDeny = 0 AND d.DeviceType = 'AudioOutput') OR
				(@InputOverallAllow = 1 AND @InputOverallDeny = 0 AND d.DeviceType = 'Input') OR
				(@OutputOverallAllow = 1 AND @OutputOverallDeny = 0 AND d.DeviceType = 'Output') OR
				(@MetadataOverallAllow = 1 AND @MetadataOverallDeny = 0 AND d.DeviceType = 'Metadata')
				OR
				(
					((spd.SecurityAttributeID = 'DEVICEVISIBLE' AND  @CameraOverallDeny = 0) OR
					(spd.SecurityAttributeID = 'DEVICEMICVISIBLE'  AND @MicrophoneOverallDeny = 0) OR
					(spd.SecurityAttributeID = 'DEVICESPEAKERVISIBLE' AND @SpeakerOverallDeny = 0) OR
					(spd.SecurityAttributeID = 'DEVICEINPUTVISIBLE' AND @InputOverallDeny = 0) OR
					(spd.SecurityAttributeID = 'DEVICEOUTPUTVISIBLE' AND @OutputOverallDeny = 0) OR
					(spd.SecurityAttributeID = 'DEVICEMETADATAVISIBLE' AND @MetadataOverallDeny = 0))
				)
			)
		END
	END
	ELSE IF(@SecurityGroupActionName = 'READ_BOOKMARKS')
	BEGIN
		IF(@IsAdmin = 1)
		BEGIN
			INSERT @Devices 
			SELECT IDDevice
			FROM Devices
			INNER JOIN [Hardware] ON [Devices].[IDHardware] = [Hardware].[IDHardware]
			WHERE
				([Hardware].[IDRecorder] = @IDRecorder OR @IDRecorder IS NULL) AND
				([Hardware].[Enabled] = 1 AND [Devices].[Enabled] = 1 OR @EnabledDevicesOnly = 0) AND
				([Devices].[DeviceType] = 'Camera' OR [Devices].[DeviceType] = 'AudioInput' OR [Devices].[DeviceType] = 'AudioOutput')
		END
		ELSE
		BEGIN
			SELECT @CameraOverallAllow = COALESCE(MAX(CASE 
					WHEN [access_deny] = 0 AND namespace_id = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85' THEN 1
					ELSE 0
					END),0),
				@CameraOverallDeny = COALESCE(MAX(CASE 
					WHEN [access_deny] = 1 AND namespace_id = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85' THEN 1
					ELSE 0
					END),0),
				@MicrophoneOverallAllow = COALESCE(MAX(CASE 
					WHEN [access_deny] = 0 AND namespace_id = '15F48F88-CA89-4926-9A84-2B02864EC77A' THEN 1
					ELSE 0
					END),0),
				@MicrophoneOverallDeny =COALESCE(MAX(CASE 
					WHEN [access_deny] = 1 AND namespace_id = '15F48F88-CA89-4926-9A84-2B02864EC77A' THEN 1
					ELSE 0
					END),0),
				@SpeakerOverallAllow = COALESCE(MAX(CASE 
					WHEN [access_deny] = 0 AND namespace_id = '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD' THEN 1
					ELSE 0
					END),0),
				@SpeakerOverallDeny =COALESCE(MAX(CASE 
					WHEN [access_deny] = 1 AND namespace_id = '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD' THEN 1
					ELSE 0
					END), 0)
			FROM [SecurityObject] so
					INNER JOIN [SecurityAccessEntry] sae ON so.[security_object_id] = sae.[security_object_id]
					INNER JOIN @RoleIdTable roles ON sae.[user_sid] = roles.ID
					WHERE so.[object_id] = '/'
					AND sae.[action_name] = 'READ_BOOKMARKS'
					AND	namespace_id IN 
					('623D03F8-C5D5-46BC-A2F4-4C03562D4F85'
					,'15F48F88-CA89-4926-9A84-2B02864EC77A'
					,'48B602BC-E752-4BBF-8E2A-7DE01F53A6DD'
					)
			INSERT @Devices 
			SELECT DISTINCT (d.IDDevice)
			FROM [Devices] d
			INNER JOIN [Hardware] h ON d.[IDHardware] = h.[IDHardware]
			LEFT JOIN
			(
				SELECT * FROM [SecurityPermissionsDevice]
				WHERE [Value] = 'True' 
				AND (SecurityAttributeID = 'DEVICEBOOKMARKVIEW' OR
					 SecurityAttributeID = 'DEVICEMICBOOKMARKVIEW' OR
					 SecurityAttributeID = 'DEVICESPEAKERBOOKMARKVIEW')
				AND IDRole IN (SELECT ID FROM @RoleIdTable)
			
			) spd ON spd.IDDevice = d.IDDevice
			WHERE 
			(h.[IDRecorder] = @IDRecorder OR @IDRecorder IS NULL) AND
			(@EnabledDevicesOnly = 0 OR (h.[Enabled] = 1 AND d.[Enabled] = 1)) AND
			(
				(@CameraOverallAllow = 1 AND @CameraOverallDeny = 0 AND d.DeviceType = 'Camera') OR
				(@MicrophoneOverallAllow = 1 AND @MicrophoneOverallDeny = 0 AND d.DeviceType = 'AudioInput') OR
				(@SpeakerOverallAllow = 1 AND @SpeakerOverallDeny = 0 AND d.DeviceType = 'AudioOutput')
				OR
				(
					(spd.SecurityAttributeID = 'DEVICEBOOKMARKVIEW' AND @CameraOverallDeny = 0) OR
					(spd.SecurityAttributeID = 'DEVICEMICBOOKMARKVIEW' AND @MicrophoneOverallDeny = 0) OR
					(spd.SecurityAttributeID = 'DEVICESPEAKERBOOKMARKVIEW' AND @SpeakerOverallDeny = 0)
				)
			)
		END
	END
	ELSE IF(@SecurityGroupActionName = 'READ_EVIDENCE_LOCK')
	BEGIN
		IF(@IsAdmin = 1)
		BEGIN
			INSERT @Devices 
			SELECT IDDevice
			FROM Devices
			INNER JOIN [Hardware] ON [Devices].[IDHardware] = [Hardware].[IDHardware]
			WHERE
				([Hardware].[IDRecorder] = @IDRecorder OR @IDRecorder IS NULL) AND
				([Hardware].[Enabled] = 1 AND [Devices].[Enabled] = 1 OR @EnabledDevicesOnly = 0) AND
				([Devices].[DeviceType] = 'Camera' OR [Devices].[DeviceType] = 'AudioInput' OR [Devices].[DeviceType] = 'AudioOutput' OR [Devices].[DeviceType] = 'Metadata')
		END
		ELSE
		BEGIN
			SELECT @CameraOverallAllow = COALESCE(MAX(CASE 
					WHEN [access_deny] = 0 AND namespace_id = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85' THEN 1
					ELSE 0
					END), 0),
				@CameraOverallDeny = COALESCE(MAX(CASE 
					WHEN [access_deny] = 1 AND namespace_id = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85' THEN 1
					ELSE 0
					END), 0),
				@MicrophoneOverallAllow = COALESCE(MAX(CASE 
					WHEN [access_deny] = 0 AND namespace_id = '15F48F88-CA89-4926-9A84-2B02864EC77A' THEN 1
					ELSE 0
					END), 0),
				@MicrophoneOverallDeny =COALESCE(MAX(CASE 
					WHEN [access_deny] = 1 AND namespace_id = '15F48F88-CA89-4926-9A84-2B02864EC77A' THEN 1
					ELSE 0
					END), 0),
				@SpeakerOverallAllow = COALESCE(MAX(CASE 
					WHEN [access_deny] = 0 AND namespace_id = '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD' THEN 1
					ELSE 0
					END), 0),
				@SpeakerOverallDeny =COALESCE(MAX(CASE 
					WHEN [access_deny] = 1 AND namespace_id = '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD' THEN 1
					ELSE 0
					END), 0),
				@MetadataOverallAllow = COALESCE(MAX(CASE 
					WHEN [access_deny] = 0 AND namespace_id = 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D' THEN 1
					ELSE 0
					END), 0),
				 @MetadataOverallDeny =COALESCE(MAX(CASE 
					WHEN [access_deny] = 1 AND namespace_id = 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D' THEN 1
					ELSE 0
					END), 0)
			FROM [SecurityObject] so
					INNER JOIN [SecurityAccessEntry] sae ON so.[security_object_id] = sae.[security_object_id]
					INNER JOIN @RoleIdTable roles ON sae.[user_sid] = roles.ID
					WHERE so.[object_id] = '/'
					AND sae.[action_name] = 'READ_EVIDENCE_LOCK'
					AND	namespace_id IN 
					('623D03F8-C5D5-46BC-A2F4-4C03562D4F85'
					,'15F48F88-CA89-4926-9A84-2B02864EC77A'
					,'48B602BC-E752-4BBF-8E2A-7DE01F53A6DD'
					,'EDE4D51C-F691-4894-9C0B-C3AE096DC04D')
			INSERT @Devices 
			SELECT DISTINCT (d.IDDevice)
			FROM [Devices] d
			INNER JOIN [Hardware] h ON d.[IDHardware] = h.[IDHardware]
			LEFT JOIN
			(
				SELECT IDDevice, SecurityAttributeID
				FROM [SecurityPermissionsDevice]
				WHERE [Value] = 'True' 
				AND (SecurityAttributeID = 'DEVICEMARKEDDATAVIEW' OR
					 SecurityAttributeID = 'DEVICEMICMARKEDDATAVIEW' OR
					 SecurityAttributeID = 'DEVICESPEAKERMARKEDDATAVIEW' OR
					 SecurityAttributeID = 'DEVICEMETADATAMARKEDDATAVIEW') 
				AND IDRole IN (SELECT ID FROM @RoleIdTable)
			
			) spd ON spd.IDDevice = d.IDDevice
			WHERE 
			(h.[IDRecorder] = @IDRecorder OR @IDRecorder IS NULL) AND
			(@EnabledDevicesOnly = 0 OR (h.[Enabled] = 1 AND d.[Enabled] = 1)) AND
			(
				(@CameraOverallAllow = 1 AND @CameraOverallDeny = 0 AND d.DeviceType = 'Camera') OR
				(@MicrophoneOverallAllow = 1 AND @MicrophoneOverallDeny = 0 AND d.DeviceType = 'AudioInput') OR
				(@SpeakerOverallAllow = 1 AND @SpeakerOverallDeny = 0 AND d.DeviceType = 'AudioOutput') OR
				(@MetadataOverallAllow = 1 AND @MetadataOverallDeny = 0 AND d.DeviceType = 'Metadata')
				OR
				(
					(spd.SecurityAttributeID = 'DEVICEMARKEDDATAVIEW' AND @CameraOverallDeny = 0) OR
					(spd.SecurityAttributeID = 'DEVICEMICMARKEDDATAVIEW' AND @MicrophoneOverallDeny = 0) OR
					(spd.SecurityAttributeID = 'DEVICESPEAKERMARKEDDATAVIEW' AND @SpeakerOverallDeny = 0) OR
					(spd.SecurityAttributeID = 'DEVICEMETADATAMARKEDDATAVIEW' AND @MetadataOverallDeny = 0)
				)
			)
		END
	END
	RETURN
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [SecurityAvailableDevicesTable] ( @RoleIdTable tvp_StringTableGuidType READONLY, @SecurityGroupActionName VARCHAR(100), @EnabledDevicesOnly BIT, @IDRecorder GuidType)
RETURNS @Devices TABLE (IDDevice GuidType PRIMARY KEY)
AS
BEGIN
	DECLARE @IsAdmin BIT
	DECLARE @CameraOverallAllow BIT
	DECLARE @MicrophoneOverallAllow BIT
	DECLARE @SpeakerOverallAllow BIT
	DECLARE @OutputOverallAllow BIT
	DECLARE @InputOverallAllow BIT
	DECLARE @MetadataOverallAllow BIT
	DECLARE @CameraOverallDeny BIT
	DECLARE @MicrophoneOverallDeny BIT
	DECLARE @SpeakerOverallDeny BIT
	DECLARE @OutputOverallDeny BIT
	DECLARE @InputOverallDeny BIT
	DECLARE @MetadataOverallDeny BIT
	
	IF(EXISTS(SELECT ID FROM @RoleIdTable roles2 WHERE ID = '1f30d14b-ba91-4e8a-8838-eda17931f8fc'))
	BEGIN
		SET @IsAdmin = 1
	END
	ELSE
	BEGIN
		SET @IsAdmin = 0
	END
	IF(@SecurityGroupActionName = 'GENERIC_READ')
	BEGIN
		IF(@IsAdmin = 1)
		BEGIN
			INSERT @Devices 
			SELECT IDDevice
			FROM Devices
			INNER JOIN [Hardware] ON [Devices].[IDHardware] = [Hardware].[IDHardware]
			WHERE
				([Hardware].[IDRecorder] = @IDRecorder OR @IDRecorder IS NULL) AND
				([Hardware].[Enabled] = 1 AND [Devices].[Enabled] = 1 OR @EnabledDevicesOnly = 0)
		END
		ELSE
		BEGIN
			SELECT @CameraOverallAllow = COALESCE(MAX(CASE 
					WHEN [access_deny] = 0 AND namespace_id = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85' THEN 1
					ELSE 0
					END), 0),
				@CameraOverallDeny = COALESCE(MAX(CASE 
					WHEN [access_deny] = 1 AND namespace_id = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85' THEN 1
					ELSE 0
					END), 0),
				@MicrophoneOverallAllow = COALESCE(MAX(CASE 
					WHEN [access_deny] = 0 AND namespace_id = '15F48F88-CA89-4926-9A84-2B02864EC77A' THEN 1
					ELSE 0
					END),0),
				@MicrophoneOverallDeny =COALESCE(MAX(CASE 
					WHEN [access_deny] = 1 AND namespace_id = '15F48F88-CA89-4926-9A84-2B02864EC77A' THEN 1
					ELSE 0
					END),0),
				@SpeakerOverallAllow = COALESCE(MAX(CASE 
					WHEN [access_deny] = 0 AND namespace_id = '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD' THEN 1
					ELSE 0
					END),0),
				@SpeakerOverallDeny =COALESCE(MAX(CASE 
					WHEN [access_deny] = 1 AND namespace_id = '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD' THEN 1
					ELSE 0
					END),0),
				@OutputOverallAllow = COALESCE(MAX(CASE 
					WHEN [access_deny] = 0 AND namespace_id = 'A95FCE90-753D-4CB7-86FB-F8CAD9F93B67' THEN 1
					ELSE 0
					END),0),
				@OutputOverallDeny =COALESCE(MAX(CASE 
					WHEN [access_deny] = 1 AND namespace_id = 'A95FCE90-753D-4CB7-86FB-F8CAD9F93B67' THEN 1
					ELSE 0
					END),0),
				@InputOverallAllow = COALESCE(MAX(CASE 
					WHEN [access_deny] = 0 AND namespace_id = '3ED79EF0-EE2F-40B8-A49A-17E9380C5932' THEN 1
					ELSE 0
					END),0),
				@InputOverallDeny =COALESCE(MAX(CASE 
					WHEN [access_deny] = 1 AND namespace_id = '3ED79EF0-EE2F-40B8-A49A-17E9380C5932' THEN 1
					ELSE 0
					END),0),
				@MetadataOverallAllow = COALESCE(MAX(CASE 
					WHEN [access_deny] = 0 AND namespace_id = 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D' THEN 1
					ELSE 0
					END),0),
				 @MetadataOverallDeny =COALESCE(MAX(CASE 
					WHEN [access_deny] = 1 AND namespace_id = 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D' THEN 1
					ELSE 0
					END), 0)
			FROM [SecurityObject] so
					INNER JOIN [SecurityAccessEntry] sae ON so.[security_object_id] = sae.[security_object_id]
					INNER JOIN @RoleIdTable roles ON sae.[user_sid] = roles.ID
					WHERE so.[object_id] = '/'
					AND sae.[action_name] = 'GENERIC_READ'
					AND	namespace_id IN 
					('623D03F8-C5D5-46BC-A2F4-4C03562D4F85'
					,'15F48F88-CA89-4926-9A84-2B02864EC77A'
					,'48B602BC-E752-4BBF-8E2A-7DE01F53A6DD'
					,'3ED79EF0-EE2F-40B8-A49A-17E9380C5932'
					,'A95FCE90-753D-4CB7-86FB-F8CAD9F93B67'
					,'EDE4D51C-F691-4894-9C0B-C3AE096DC04D')
			INSERT @Devices 
			SELECT DISTINCT (d.IDDevice)
			FROM [Devices] d
			INNER JOIN [Hardware] h ON d.[IDHardware] = h.[IDHardware]
			LEFT JOIN
			(
				SELECT * FROM [SecurityPermissionsDevice]
				WHERE [Value] = 'True' 
				AND SecurityAttributeID IN 
				('DEVICEVISIBLE',
				 'DEVICEMICVISIBLE',
				 'DEVICESPEAKERVISIBLE',
				 'DEVICEINPUTVISIBLE',
				 'DEVICEOUTPUTVISIBLE',
				 'DEVICEMETADATAVISIBLE')
				AND IDRole IN (SELECT ID FROM @RoleIdTable)
			) spd ON spd.IDDevice = d.IDDevice
			WHERE 
			(h.[IDRecorder] = @IDRecorder OR @IDRecorder IS NULL) AND
			(@EnabledDevicesOnly = 0 OR (h.[Enabled] = 1 AND d.[Enabled] = 1)) AND
			(
				(@CameraOverallAllow = 1 AND @CameraOverallDeny = 0 AND d.DeviceType = 'Camera') OR
				(@MicrophoneOverallAllow = 1 AND @MicrophoneOverallDeny = 0 AND d.DeviceType = 'AudioInput') OR
				(@SpeakerOverallAllow = 1 AND @SpeakerOverallDeny = 0 AND d.DeviceType = 'AudioOutput') OR
				(@InputOverallAllow = 1 AND @InputOverallDeny = 0 AND d.DeviceType = 'Input') OR
				(@OutputOverallAllow = 1 AND @OutputOverallDeny = 0 AND d.DeviceType = 'Output') OR
				(@MetadataOverallAllow = 1 AND @MetadataOverallDeny = 0 AND d.DeviceType = 'Metadata')
				OR
				(
					((spd.SecurityAttributeID = 'DEVICEVISIBLE' AND  @CameraOverallDeny = 0) OR
					(spd.SecurityAttributeID = 'DEVICEMICVISIBLE'  AND @MicrophoneOverallDeny = 0) OR
					(spd.SecurityAttributeID = 'DEVICESPEAKERVISIBLE' AND @SpeakerOverallDeny = 0) OR
					(spd.SecurityAttributeID = 'DEVICEINPUTVISIBLE' AND @InputOverallDeny = 0) OR
					(spd.SecurityAttributeID = 'DEVICEOUTPUTVISIBLE' AND @OutputOverallDeny = 0) OR
					(spd.SecurityAttributeID = 'DEVICEMETADATAVISIBLE' AND @MetadataOverallDeny = 0))
				)
			)
		END
	END
	ELSE IF(@SecurityGroupActionName = 'READ_BOOKMARKS')
	BEGIN
		IF(@IsAdmin = 1)
		BEGIN
			INSERT @Devices 
			SELECT IDDevice
			FROM Devices
			INNER JOIN [Hardware] ON [Devices].[IDHardware] = [Hardware].[IDHardware]
			WHERE
				([Hardware].[IDRecorder] = @IDRecorder OR @IDRecorder IS NULL) AND
				([Hardware].[Enabled] = 1 AND [Devices].[Enabled] = 1 OR @EnabledDevicesOnly = 0) AND
				([Devices].[DeviceType] = 'Camera' OR [Devices].[DeviceType] = 'AudioInput' OR [Devices].[DeviceType] = 'AudioOutput')
		END
		ELSE
		BEGIN
			SELECT @CameraOverallAllow = COALESCE(MAX(CASE 
					WHEN [access_deny] = 0 AND namespace_id = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85' THEN 1
					ELSE 0
					END),0),
				@CameraOverallDeny = COALESCE(MAX(CASE 
					WHEN [access_deny] = 1 AND namespace_id = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85' THEN 1
					ELSE 0
					END),0),
				@MicrophoneOverallAllow = COALESCE(MAX(CASE 
					WHEN [access_deny] = 0 AND namespace_id = '15F48F88-CA89-4926-9A84-2B02864EC77A' THEN 1
					ELSE 0
					END),0),
				@MicrophoneOverallDeny =COALESCE(MAX(CASE 
					WHEN [access_deny] = 1 AND namespace_id = '15F48F88-CA89-4926-9A84-2B02864EC77A' THEN 1
					ELSE 0
					END),0),
				@SpeakerOverallAllow = COALESCE(MAX(CASE 
					WHEN [access_deny] = 0 AND namespace_id = '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD' THEN 1
					ELSE 0
					END),0),
				@SpeakerOverallDeny =COALESCE(MAX(CASE 
					WHEN [access_deny] = 1 AND namespace_id = '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD' THEN 1
					ELSE 0
					END), 0)
			FROM [SecurityObject] so
					INNER JOIN [SecurityAccessEntry] sae ON so.[security_object_id] = sae.[security_object_id]
					INNER JOIN @RoleIdTable roles ON sae.[user_sid] = roles.ID
					WHERE so.[object_id] = '/'
					AND sae.[action_name] = 'READ_BOOKMARKS'
					AND	namespace_id IN 
					('623D03F8-C5D5-46BC-A2F4-4C03562D4F85'
					,'15F48F88-CA89-4926-9A84-2B02864EC77A'
					,'48B602BC-E752-4BBF-8E2A-7DE01F53A6DD'
					)
			INSERT @Devices 
			SELECT DISTINCT (d.IDDevice)
			FROM [Devices] d
			INNER JOIN [Hardware] h ON d.[IDHardware] = h.[IDHardware]
			LEFT JOIN
			(
				SELECT * FROM [SecurityPermissionsDevice]
				WHERE [Value] = 'True' 
				AND (SecurityAttributeID = 'DEVICEBOOKMARKVIEW' OR
					 SecurityAttributeID = 'DEVICEMICBOOKMARKVIEW' OR
					 SecurityAttributeID = 'DEVICESPEAKERBOOKMARKVIEW')
				AND IDRole IN (SELECT ID FROM @RoleIdTable)
			
			) spd ON spd.IDDevice = d.IDDevice
			WHERE 
			(h.[IDRecorder] = @IDRecorder OR @IDRecorder IS NULL) AND
			(@EnabledDevicesOnly = 0 OR (h.[Enabled] = 1 AND d.[Enabled] = 1)) AND
			(
				(@CameraOverallAllow = 1 AND @CameraOverallDeny = 0 AND d.DeviceType = 'Camera') OR
				(@MicrophoneOverallAllow = 1 AND @MicrophoneOverallDeny = 0 AND d.DeviceType = 'AudioInput') OR
				(@SpeakerOverallAllow = 1 AND @SpeakerOverallDeny = 0 AND d.DeviceType = 'AudioOutput')
				OR
				(
					(spd.SecurityAttributeID = 'DEVICEBOOKMARKVIEW' AND @CameraOverallDeny = 0) OR
					(spd.SecurityAttributeID = 'DEVICEMICBOOKMARKVIEW' AND @MicrophoneOverallDeny = 0) OR
					(spd.SecurityAttributeID = 'DEVICESPEAKERBOOKMARKVIEW' AND @SpeakerOverallDeny = 0)
				)
			)
		END
	END
	ELSE IF(@SecurityGroupActionName = 'READ_EVIDENCE_LOCK')
	BEGIN
		IF(@IsAdmin = 1)
		BEGIN
			INSERT @Devices 
			SELECT IDDevice
			FROM Devices
			INNER JOIN [Hardware] ON [Devices].[IDHardware] = [Hardware].[IDHardware]
			WHERE
				([Hardware].[IDRecorder] = @IDRecorder OR @IDRecorder IS NULL) AND
				([Hardware].[Enabled] = 1 AND [Devices].[Enabled] = 1 OR @EnabledDevicesOnly = 0) AND
				([Devices].[DeviceType] = 'Camera' OR [Devices].[DeviceType] = 'AudioInput' OR [Devices].[DeviceType] = 'AudioOutput' OR [Devices].[DeviceType] = 'Metadata')
		END
		ELSE
		BEGIN
			SELECT @CameraOverallAllow = COALESCE(MAX(CASE 
					WHEN [access_deny] = 0 AND namespace_id = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85' THEN 1
					ELSE 0
					END), 0),
				@CameraOverallDeny = COALESCE(MAX(CASE 
					WHEN [access_deny] = 1 AND namespace_id = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85' THEN 1
					ELSE 0
					END), 0),
				@MicrophoneOverallAllow = COALESCE(MAX(CASE 
					WHEN [access_deny] = 0 AND namespace_id = '15F48F88-CA89-4926-9A84-2B02864EC77A' THEN 1
					ELSE 0
					END), 0),
				@MicrophoneOverallDeny =COALESCE(MAX(CASE 
					WHEN [access_deny] = 1 AND namespace_id = '15F48F88-CA89-4926-9A84-2B02864EC77A' THEN 1
					ELSE 0
					END), 0),
				@SpeakerOverallAllow = COALESCE(MAX(CASE 
					WHEN [access_deny] = 0 AND namespace_id = '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD' THEN 1
					ELSE 0
					END), 0),
				@SpeakerOverallDeny =COALESCE(MAX(CASE 
					WHEN [access_deny] = 1 AND namespace_id = '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD' THEN 1
					ELSE 0
					END), 0),
				@MetadataOverallAllow = COALESCE(MAX(CASE 
					WHEN [access_deny] = 0 AND namespace_id = 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D' THEN 1
					ELSE 0
					END), 0),
				 @MetadataOverallDeny =COALESCE(MAX(CASE 
					WHEN [access_deny] = 1 AND namespace_id = 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D' THEN 1
					ELSE 0
					END), 0)
			FROM [SecurityObject] so
					INNER JOIN [SecurityAccessEntry] sae ON so.[security_object_id] = sae.[security_object_id]
					INNER JOIN @RoleIdTable roles ON sae.[user_sid] = roles.ID
					WHERE so.[object_id] = '/'
					AND sae.[action_name] = 'READ_EVIDENCE_LOCK'
					AND	namespace_id IN 
					('623D03F8-C5D5-46BC-A2F4-4C03562D4F85'
					,'15F48F88-CA89-4926-9A84-2B02864EC77A'
					,'48B602BC-E752-4BBF-8E2A-7DE01F53A6DD'
					,'EDE4D51C-F691-4894-9C0B-C3AE096DC04D')
			INSERT @Devices 
			SELECT DISTINCT (d.IDDevice)
			FROM [Devices] d
			INNER JOIN [Hardware] h ON d.[IDHardware] = h.[IDHardware]
			LEFT JOIN
			(
				SELECT IDDevice, SecurityAttributeID
				FROM [SecurityPermissionsDevice]
				WHERE [Value] = 'True' 
				AND (SecurityAttributeID = 'DEVICEMARKEDDATAVIEW' OR
					 SecurityAttributeID = 'DEVICEMICMARKEDDATAVIEW' OR
					 SecurityAttributeID = 'DEVICESPEAKERMARKEDDATAVIEW' OR
					 SecurityAttributeID = 'DEVICEMETADATAMARKEDDATAVIEW') 
				AND IDRole IN (SELECT ID FROM @RoleIdTable)
			
			) spd ON spd.IDDevice = d.IDDevice
			WHERE 
			(h.[IDRecorder] = @IDRecorder OR @IDRecorder IS NULL) AND
			(@EnabledDevicesOnly = 0 OR (h.[Enabled] = 1 AND d.[Enabled] = 1)) AND
			(
				(@CameraOverallAllow = 1 AND @CameraOverallDeny = 0 AND d.DeviceType = 'Camera') OR
				(@MicrophoneOverallAllow = 1 AND @MicrophoneOverallDeny = 0 AND d.DeviceType = 'AudioInput') OR
				(@SpeakerOverallAllow = 1 AND @SpeakerOverallDeny = 0 AND d.DeviceType = 'AudioOutput') OR
				(@MetadataOverallAllow = 1 AND @MetadataOverallDeny = 0 AND d.DeviceType = 'Metadata')
				OR
				(
					(spd.SecurityAttributeID = 'DEVICEMARKEDDATAVIEW' AND @CameraOverallDeny = 0) OR
					(spd.SecurityAttributeID = 'DEVICEMICMARKEDDATAVIEW' AND @MicrophoneOverallDeny = 0) OR
					(spd.SecurityAttributeID = 'DEVICESPEAKERMARKEDDATAVIEW' AND @SpeakerOverallDeny = 0) OR
					(spd.SecurityAttributeID = 'DEVICEMETADATAMARKEDDATAVIEW' AND @MetadataOverallDeny = 0)
				)
			)
		END
	END
	
	RETURN
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [SecurityIsPermittedByObjectAction] ( @RoleIds XML, @namespace_id UNIQUEIDENTIFIER, @action_name NVARCHAR(260))
RETURNS @SecurityObjects TABLE (object_id NVARCHAR(300))
AS
BEGIN
	DECLARE @RoleIdTable TABLE (ID GuidType)
	INSERT INTO @RoleIdTable
	SELECT ID FROM XmlToIdTable(@RoleIds)
	-- Temporary table to hold objects
	DECLARE @tempSecurityObjects TABLE
	(
		[object_id] NVARCHAR(300)
	)
	-- Temporaty table to hold access entries
	DECLARE @tempAccessEntry TABLE
	(
		[access_entry_id] INT,
		[user_sid] [SidType],
		[access_deny] BIT
	)
	DECLARE @is_permitted BIT
	DECLARE @is_member BIT
	DECLARE @access_entry_id INT
	DECLARE @access_entry_sid [SidType]
	DECLARE @access_deny BIT
	DECLARE @security_object_id UNIQUEIDENTIFIER
	DECLARE @security_inherits_from_id NVARCHAR(300)
	
	DECLARE @object_id NVARCHAR(300)
	-- Verify if user is member of the administrator role
	-- If true, return is_permitted = true
	DECLARE @IsAdmin BIT
	IF(EXISTS(SELECT ID FROM @RoleIdTable roles2 WHERE ID = '1f30d14b-ba91-4e8a-8838-eda17931f8fc'))
	BEGIN
		SET @IsAdmin = 1
	END
	ELSE
	BEGIN
		SET @IsAdmin = 0
	END
	IF(@IsAdmin = 1)
	BEGIN
		INSERT @SecurityObjects
		SELECT UPPER([object_id])
		FROM [SecurityObject] 
		WHERE [namespace_id] = @namespace_id
		AND [object_id] <> N'/'
		RETURN
	END
	ELSE
	BEGIN
		DECLARE object_cursor CURSOR FOR
		SELECT [security_object_id], [inherits_from_id], [object_id] FROM [SecurityObject] WHERE [namespace_id] = @namespace_id
		OPEN object_cursor
		FETCH NEXT FROM object_cursor INTO @security_object_id, @security_inherits_from_id, @object_id;
		WHILE @@FETCH_STATUS = 0
		BEGIN
			-- Loop through the current security object and its inheritance
			WHILE(@security_object_id IS NOT NULL)
			BEGIN
				-- Copy all access entries on the current object
				INSERT INTO @tempAccessEntry([access_entry_id], [user_sid], [access_deny])
				SELECT [access_entry_id], [user_sid], [access_deny]
				FROM [SecurityAccessEntry]
				WHERE [security_object_id] = @security_object_id
					AND [action_name] = @action_name
				WHILE(1 = 1)
				BEGIN
					SET @access_entry_id = NULL
					SELECT TOP 1 
						@access_entry_id = [access_entry_id], 
						@access_entry_sid = [user_sid], 
						@access_deny = [access_deny]
					FROM @tempAccessEntry
					-- Check if the is more entries
					IF(@access_entry_id IS NULL)
					BREAK
					-- Check if the access entry sid matches the users sid
					-- If it does the user is permitted unless denied is specified
					IF(EXISTS(SELECT ID FROM @RoleIdTable roles2 WHERE ID = @access_entry_sid))
					BEGIN
						SET @is_permitted = ~@access_deny
					END
			
					-- If the user is denied we do not want to check
					-- anything else. The used is denied access - proceed with next object
					IF(@is_permitted = 0)
					BEGIN
						GOTO fetch_next
					END
					-- Now we can delete the access entry we have examined above
					DELETE FROM @tempAccessEntry WHERE [access_entry_id] = @access_entry_id
				END
				-- Check if the security object is an inherited object
				IF(@security_inherits_from_id IS NULL)
				BEGIN
					IF(@is_permitted = 1)
					BEGIN
						INSERT @tempSecurityObjects
						VALUES (@object_id)
					END
					BREAK
				END
				ELSE
				BEGIN
					SET @security_object_id = NULL
					SELECT
						@security_object_id = [security_object_id],
						@security_inherits_from_id = [inherits_from_id]
					FROM [SecurityObject]
					WHERE [security_object_id] = @security_inherits_from_id
				END
			END
		
			fetch_next:
		
			SELECT @is_permitted = NULL
		
			FETCH NEXT FROM object_cursor
			INTO @security_object_id, @security_inherits_from_id, @object_id;
		END
		CLOSE object_cursor
		DEALLOCATE object_cursor
		
		INSERT @SecurityObjects
		SELECT UPPER([object_id])
		FROM @tempSecurityObjects
		WHERE [object_id] <> N'/'
	END
	RETURN
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [ShouldReThrowError](@ERROR_NUMBER INT, @ERROR_SEVERITY INT)
RETURNS BIT
AS
BEGIN
	IF (@ERROR_NUMBER >= 60000)
		RETURN 1
	IF(@ERROR_NUMBER = 1225 AND @ERROR_SEVERITY = 20)
		RETURN 1
	IF (@ERROR_NUMBER = 1205
		OR @ERROR_NUMBER = 8628
		OR @ERROR_NUMBER = 8645
		OR @ERROR_NUMBER = 2801
		OR @ERROR_NUMBER = 233
		OR @ERROR_NUMBER = 10053
		OR @ERROR_NUMBER = 10054
		OR @ERROR_NUMBER = -2
		OR @ERROR_NUMBER = 10060
		OR @ERROR_NUMBER = 983
		OR @ERROR_NUMBER = 976
	)
		RETURN 1
	RETURN 0
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [StringValueChanging]
(
	@value1 AS NVARCHAR(MAX),
	@value2 AS NVARCHAR(MAX)
)
RETURNS BIT
AS BEGIN
	DECLARE @result BIT
	DECLARE @temp1 NVARCHAR(4000)
	DECLARE @temp2 NVARCHAR(4000)
	DECLARE @index INT
	IF (@value1 IS NULL AND @value2 IS NULL)			-- Both are NULL
	BEGIN
		SET @result = 0
	END
	ELSE IF (@value1 IS NULL OR @value2 IS NULL)		-- Only one of them is NULL
	BEGIN
		SET @result = 1
	END
	ELSE												-- None of them are NULL
	BEGIN
		IF (LEN(@value1) <> LEN(@value2))
			SET @result = 1
		ELSE
		BEGIN
			SET @index = 0
			WHILE @index < LEN(@value1) -- We already know that @value1 and @value2 have same length
			BEGIN
				SET @temp1 = SUBSTRING(@value1, @index, @index + 4000)
				SET @temp2 = SUBSTRING(@value2, @index, @index + 4000)
				IF ((@temp1 <> @temp2) OR HASHBYTES('SHA1', @temp1) <> HASHBYTES('SHA1', @temp2))
				BEGIN
					SET @result = 1
					BREAK
				END
				SET @result = 0
				SET @index = @index + 4000
			END
		END
	END
	RETURN @result
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION dbo.udfRoleMembers()
RETURNS @roleMembers TABLE 
(
    [IDRole] [GuidType],
    [MemberSID] [SIDType]
)
AS
BEGIN
	DECLARE @currentLevel INT
	SET @currentLevel = 0
	;WITH GroupList([IDRole], [MemberSID], [MemberLevel], [IsCycle], [Path]) AS
	(
		SELECT 
			sm.[Container], 
			sm.[MemberSID], 
			@currentLevel, 
			0 AS IsCycle, 
			'.' + CAST(sm.[MemberSID] AS VARCHAR(MAX)) + '.' AS [Path]
		FROM [SecurityMembership] sm
		UNION ALL
		SELECT 
			sm.[Container], 
			gl.[MemberSID], 
			gl.[MemberLevel] + 1,
			CASE WHEN gl.[Path] LIKE '%.' + CAST(sm.[MemberSID] AS VARCHAR(MAX)) + '.%' THEN 1 ELSE 0 END AS IsCycle,
			gl.[Path] + CAST(sm.[MemberSID] AS VARCHAR(MAX)) + '.' AS [Path]
		FROM [SecurityMembership] sm
		INNER JOIN GroupList gl ON sm.[MemberSID] = gl.[IDRole]
		WHERE gl.[MemberLevel] < 100 AND gl.[IsCycle] = 0
	)
	INSERT @roleMembers
	SELECT DISTINCT [IDRole], [MemberSID]
	FROM GroupList
	INNER JOIN [SecurityIdentity] si ON GroupList.[IDRole] = si.[SID] AND si.[Type] = 4
	ORDER BY IDRole
	RETURN
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION ufn_query_performance_data(@start_time DATETIME, @end_time DATETIME, @number_of_values INT)
RETURNS TABLE
AS
RETURN
(
	WITH TimeCTE
	AS
	(
		SELECT @start_time AS [collection_time]
		UNION ALL
		SELECT DATEADD(mi, DATEDIFF(MINUTE, @start_time, @end_time) / @number_of_values, [collection_time])
		FROM TimeCTE
		WHERE [collection_time] <= DATEADD(mi, -DATEDIFF(MINUTE, @start_time, @end_time) / @number_of_values, @end_time)
	)
	SELECT 
		c.[collection_time],
		pci.[pkid] as [instance_id],
		AVG(CASE WHEN pct.[calculated_value] IS NULL THEN 0 ELSE pct.[calculated_value] END) AS [calculated_value],
		MAX(CASE WHEN pct.[calculated_value] IS NULL THEN 0 ELSE pct.[calculated_value] END) AS [calculated_value_max],
		COUNT(pct.[calculated_value]) AS [number_of_values]
	FROM TimeCTE c
	CROSS JOIN [PerformanceCounterInstance] pci
	LEFT JOIN [PerformanceCounterValue] pct ON 
					pct.[collection_time] > DATEADD(mi, -DATEDIFF(MINUTE, @start_time, @end_time) / @number_of_values, c.[collection_time]) 
			AND pct.[collection_time] <= c.[collection_time]
			AND pci.[pkid] = pct.instance_id
	GROUP BY pci.[pkid], c.[collection_time]
	--OPTION(MAXRECURSION 10000);
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION dbo.ufnRoleMembers(@member_sid VARCHAR(256))
RETURNS @roleMembers TABLE 
(
    [IDRole] [GuidType],
    [MemberSID] [SIDType]
)
AS
BEGIN
	DECLARE @currentLevel INT
	SET @currentLevel = 0
	;WITH GroupList([Container], [MemberSID], [MemberLevel], [IsCycle], [Path]) AS
	(
		SELECT 
			sm.[Container], 
			sm.[MemberSID], 
			@currentLevel, 
			0 AS IsCycle, 
			'.' + CAST(sm.[MemberSID] AS VARCHAR(MAX)) + '.' AS [Path]
		FROM [SecurityMembership] sm
		WHERE MemberSID = @member_sid
		UNION ALL
		SELECT 
			sm.[Container], 
			gl.[MemberSID], 
			gl.[MemberLevel] + 1,
			CASE WHEN gl.[Path] LIKE '%.' + CAST(sm.[MemberSID] AS VARCHAR(MAX)) + '.%' THEN 1 ELSE 0 END AS IsCycle,
			gl.[Path] + CAST(sm.[MemberSID] AS VARCHAR(MAX)) + '.' AS [Path]
		FROM [SecurityMembership] sm
		INNER JOIN GroupList gl ON sm.[MemberSID] = gl.[Container]
		WHERE gl.[MemberLevel] < 100 AND gl.[IsCycle] = 0
	)
	INSERT @roleMembers
	SELECT DISTINCT sr.[IDRole], [MemberSID]
	FROM GroupList
	INNER JOIN [SecurityRoles] sr ON GroupList.[Container] = sr.[IDRole]
	ORDER BY IDRole
	RETURN
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [XmlToIdTable] ( @IDs XML )
RETURNS TABLE 
AS
RETURN 
(
	SELECT [ID] = idnode.value('.', 'GuidType')
	FROM @IDs.nodes('Ids/Id') AS IdCollection(idnode)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [XmlToStringTable] (@IDs XML)
RETURNS TABLE 
AS
RETURN 
(
	SELECT [Result] = idnode.value('.', 'nvarchar(256)')
	FROM @IDs.nodes('Ids/Id') AS IdCollection(idnode)
)

GO
