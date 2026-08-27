-- This header is used by the dbupdate tool]
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on] The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes]
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[bookmarks_query_search_no_device_filter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [bookmarks_query_search_no_device_filter]
GO

CREATE PROCEDURE [bookmarks_query_search_no_device_filter]
	@RoleIds tvp_StringTableGuidType READONLY,
	@DeviceTypes tvp_StringTableNvarchar READONLY,
	@Users tvp_StringTableNvarchar READONLY,		-- Optional
	@SearchStr AS NVARCHAR(512),					-- Optional
	@SearchHeader AS BIT,							-- Optional
	@SearchDescription AS BIT,						-- Optional
	@SearchReference AS BIT,						-- Optional
	@SearchUser AS BIT,								-- Optional
	@Header AS NVARCHAR(512),						-- Optional
	@Description AS NVARCHAR(512),					-- Optional
	@CenterTime AS BIGINT,							-- n/a, if FromBookmarkId is given
	@TimeLimit AS BIGINT,
	@CountLimit AS INT,
	@FromBookmarkId as GuidType						-- Optional
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @NoUsers BIT
	SET @NoUsers = 0
	IF((SELECT COUNT(ID) FROM @Users) = 0)
	BEGIN
		SET @NoUsers = 1
	END

	IF(@FromBookmarkId IS NOT NULL)
	BEGIN
		SELECT @CenterTime = (SELECT [MarkedData].TagTime FROM [MarkedData] WHERE [MarkedData].IDMarkedData = @FromBookmarkId) 		
	END

	DECLARE @EndTime BIGINT = @CenterTime + @TimeLimit;
	
	CREATE TABLE #AvailableDevices (ID UNIQUEIDENTIFIER, PRIMARY KEY (ID))
	
	INSERT INTO #AvailableDevices(ID)
		SELECT	[Devices].IDDevice
		FROM	[SecurityAvailableDevicesTable](@RoleIds, 'READ_BOOKMARKS', 0, NULL) as sad
				INNER JOIN [Devices] ON [Devices].IDDevice = sad.IDDevice
				INNER JOIN @DeviceTypes deviceTypes ON [Devices].[DeviceType] = deviceTypes.[ID]

	DECLARE @MarkedDataResult TABLE 
	(
		IDMarkedData		UNIQUEIDENTIFIER
		,LastModified		DATETIME
		,CreatedDate		DATETIME
		,Reference			NVARCHAR(256)
		,UserName			NVARCHAR(256)
		,Header				NVARCHAR(1024)
		,Description		NVARCHAR(MAX)
		,MarkDataType		INT
		,StartTime			BIGINT
		,TagTime			BIGINT
		,EndTime			BIGINT
		,RetentionType		INT
		,RetentionUnits		INT
		,RetentionExpire	BIGINT
		,ProtectionSize		BIGINT
		,PRIMARY KEY (IDMarkedData)
	)

	DECLARE @SearchStrLike as NVarChar(516)
	SELECT @SearchStrLike = N'%' + REPLACE(@SearchStr, '[', '[[]') + N'%'

	DECLARE @HeaderStrLike as NVarChar(516)
	SELECT @HeaderStrLike = N'%' + REPLACE(@Header, '[', '[[]') + N'%'

	DECLARE @DescriptionStrLike as NVarChar(516)
	SELECT @DescriptionStrLike = N'%' + REPLACE(@Description, '[', '[[]') + N'%'

	IF(@CenterTime < @EndTime)
	BEGIN
		INSERT INTO @MarkedDataResult(IDMarkedData, LastModified, CreatedDate, Reference, UserName, Header, Description, MarkDataType, StartTime, TagTime, EndTime, RetentionType, RetentionUnits, RetentionExpire, ProtectionSize)
		-- Forward search
		SELECT DISTINCT TOP(@CountLimit) 
				  [MarkedData].[IDMarkedData]
				, [MarkedData].[LastModified]
				, [MarkedData].[CreatedDate]
				, [MarkedData].[Reference]
				, [MarkedData].[UserName]
				, [MarkedData].[Header]
				, [MarkedData].[Description]
				, [MarkedData].[MarkDataType]
				, [MarkedData].[StartTime]
				, [MarkedData].[TagTime]
				, [MarkedData].[EndTime]
				, [MarkedData].[RetentionType]
				, [MarkedData].[RetentionUnits]
				, [MarkedData].[RetentionExpire]
				, [MarkedData].[ProtectionSize]
		FROM	[MarkedData] 
				INNER JOIN [MarkedDataDevices] mdd  ON mdd.[IDMarkedData] = [MarkedData].[IDMarkedData]	
				INNER JOIN #AvailableDevices ad ON mdd.IDDevice = ad.ID
		WHERE	[MarkedData].StartTime <= @EndTime AND [MarkedData].EndTime >= @CenterTime AND
				((@FromBookmarkId IS NULL) OR ([MarkedData].TagTime > @CenterTime) OR ([MarkedData].IDMarkedData > @FromBookmarkId AND [MarkedData].TagTime = @CenterTime )) AND
				[MarkedData].MarkDataType = 8 AND
				((@NoUsers = 1) OR [MarkedData].[UserName] IN (SELECT [ID] FROM @Users)) AND
				((@Header IS NULL) OR ([MarkedData].Header LIKE @HeaderStrLike)) AND
				((@Description IS NULL) OR ([MarkedData].[Description] LIKE @DescriptionStrLike)) AND
				((@SearchStr IS NULL) OR (@SearchReference = 1 AND [MarkedData].Reference = @SearchStr) OR (@SearchUser = 1 AND [MarkedData].[UserName]= @SearchStr) OR (@SearchHeader = 1 AND [MarkedData].Header LIKE @SearchStrLike) OR (@SearchDescription = 1 AND [MarkedData].[Description] LIKE @SearchStrLike))

		ORDER BY
				[MarkedData].TagTime ASC,
				[MarkedData].IDMarkedData ASC

		--Selecting marked data as result. We need to order again, because the data is stored unordered in @MarkedDataResult
		SELECT	[IDMarkedData],
				[LastModified],
				[CreatedDate],
				[Reference],
				[UserName],
				[Header],
				[Description],
				[MarkDataType],
				[StartTime],
				[TagTime],
				[EndTime],
				[RetentionType],
				[RetentionUnits],
				[RetentionExpire],
				[ProtectionSize] 
		FROM	@MarkedDataResult
		ORDER BY
				[TagTime] ASC, 
				[IDMarkedData] ASC
	END
	ELSE
	BEGIN
		INSERT INTO @MarkedDataResult(IDMarkedData, LastModified, CreatedDate, Reference, UserName, Header, Description, MarkDataType, StartTime, TagTime, EndTime, RetentionType, RetentionUnits, RetentionExpire, ProtectionSize)
		-- Forward search
		SELECT DISTINCT TOP(@CountLimit) 
				 [MarkedData].[IDMarkedData]
				,[MarkedData].[LastModified]
				,[MarkedData].[CreatedDate]
				,[MarkedData].[Reference]
				,[MarkedData].[UserName]
				,[MarkedData].[Header]
				,[MarkedData].[Description]
				,[MarkedData].[MarkDataType]
				,[MarkedData].[StartTime]
				,[MarkedData].[TagTime]
				,[MarkedData].[EndTime]
				,[MarkedData].[RetentionType]
				,[MarkedData].[RetentionUnits]
				,[MarkedData].[RetentionExpire]
				,[MarkedData].[ProtectionSize]
		FROM 	[MarkedData]
				INNER JOIN [MarkedDataDevices] mdd ON mdd.[IDMarkedData] = [MarkedData].[IDMarkedData]
				INNER JOIN #AvailableDevices ad  ON mdd.IDDevice = ad.ID 
		WHERE	[MarkedData].StartTime <= @CenterTime AND [MarkedData].EndTime >= @EndTime AND
				((@FromBookmarkId IS NULL) OR ([MarkedData].TagTime < @CenterTime) OR ([MarkedData].IDMarkedData < @FromBookmarkId AND [MarkedData].TagTime = @CenterTime )) AND
				[MarkedData].MarkDataType = 8 AND
				((@NoUsers = 1) OR [MarkedData].[UserName] IN (SELECT [ID] FROM @Users)) AND
				((@Header IS NULL) OR ([MarkedData].Header LIKE @HeaderStrLike)) AND
				((@Description IS NULL) OR ([MarkedData].[Description] LIKE @DescriptionStrLike)) AND
				((@SearchStr IS NULL) OR (@SearchReference = 1 AND [MarkedData].Reference = @SearchStr) OR (@SearchUser = 1 AND [MarkedData].[UserName]= @SearchStr) OR (@SearchHeader = 1 AND [MarkedData].Header LIKE @SearchStrLike) OR (@SearchDescription = 1 AND [MarkedData].[Description] LIKE @SearchStrLike))

		ORDER BY 
				[MarkedData].[TagTime] DESC
				, [MarkedData].[IDMarkedData] DESC

		--Selecting marked data as result. We need to order again, because the data is stored unordered in @MarkedDataResult
		SELECT	[IDMarkedData],
				[LastModified],
				[CreatedDate],
				[Reference],
				[UserName],
				[Header],
				[Description],
				[MarkDataType],
				[StartTime],
				[TagTime],
				[EndTime],
				[RetentionType],
				[RetentionUnits],
				[RetentionExpire],
				[ProtectionSize] 
		FROM	@MarkedDataResult
		ORDER BY
				[TagTime] DESC,
				[IDMarkedData] DESC
	END
	
	--Selecting marked data devices as result 
	SELECT	mdd.IDMarkedData, mdd.IDDevice, mdd.Size 
	FROM	[MarkedDataDevices] as mdd 
			INNER JOIN @MarkedDataResult as mdr ON mdd.[IDMarkedData] = mdr.[IDMarkedData]
			INNER JOIN #AvailableDevices AS ad ON mdd.IDDevice = ad.ID 
	OPTION (LOOP JOIN)
END
GO