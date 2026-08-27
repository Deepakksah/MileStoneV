-- This header is used by the dbupdate tool]
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on] The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes]
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[markeddata_query_search]') AND type in (N'P', N'PC'))
DROP PROCEDURE [markeddata_query_search]
GO

CREATE PROCEDURE [markeddata_query_search]
	@RoleIds tvp_StringTableGuidType READONLY,
	@IDDevices tvp_StringTableGuidType READONLY,					-- Optional
	@Users tvp_StringTableNvarchar READONLY,						-- Optional
	@SearchText as NVarChar(512),		-- Optional
	@SearchHeader AS BIT,               -- Optional
	@SearchDescription AS BIT,			-- Optional
	@SearchReference AS BIT,			-- Optional
	@SearchUser AS BIT,					-- Optional
	@Header AS NVARCHAR(512),			-- Optional
	@Description AS NVARCHAR(512),		-- Optional
	@CreatedFromTime as DateTime = NULL,-- Optional
	@CreatedToTime as DateTime = NULL,	-- Optional
	@MarkedDataFromTime as BigInt,		-- Optional
	@MarkedDataToTime as BigInt,		-- Optional
	@TagTimeFromTime as BigInt,			-- Optional
	@TagTimeToTime as BigInt,			-- Optional
	@RetentionExpireFromTime as BigInt,	-- Optional
	@RetentionExpireToTime as BigInt,	-- Optional
	@PageIndex as BigInt,	
	@PageSize as BigInt,	
	@SortOrder as NVarchar(50),
	@SortAscending as Bit,
	@MarkedDataType as INT	
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @NoUsers BIT
	SET @NoUsers = 0

	IF((SELECT COUNT(ID) FROM @Users) = 0)
	BEGIN
		SET @NoUsers = 1
	END

	DECLARE @TargetSecurity NCHAR(256)
	IF @MarkedDataType = 8 
		SET @TargetSecurity = N'READ_BOOKMARKS'
	ELSE IF @MarkedDataType = 2
		SET @TargetSecurity = N'READ_EVIDENCE_LOCK'
	ELSE SET @TargetSecurity = N'NONE'
	
	DECLARE @SearchStrLike as NVarChar(516)
	SELECT @SearchStrLike = N'%' + REPLACE(@SearchText, '[', '[[]') + N'%'

	DECLARE @HeaderStrLike as NVarChar(516)
	SELECT @HeaderStrLike = N'%' + REPLACE(@Header, '[', '[[]') + N'%'

	DECLARE @DescriptionStrLike as NVarChar(516)
	SELECT @DescriptionStrLike = N'%' + REPLACE(@Description, '[', '[[]') + N'%'

	DECLARE @AvailableDevices TABLE(ID GuidType)
	
	IF @TargetSecurity = N'NONE'
	BEGIN
		INSERT INTO @AvailableDevices(ID)
		SELECT [Devices].IDDevice
		FROM [Devices]
	END
	ELSE
	BEGIN 
		IF((SELECT COUNT(ID) FROM @IDDevices) > 0)
		BEGIN
			INSERT INTO @AvailableDevices(ID)
			SELECT sad.IDDevice
			FROM [SecurityAvailableDevicesTable](@RoleIds, @TargetSecurity, 0, NULL) as sad
			INNER JOIN @IDDevices d ON sad.IDDevice = d.ID
			INNER JOIN [Devices] ON [Devices].IDDevice = d.ID
		END
		ELSE
		BEGIN
			INSERT INTO @AvailableDevices(ID)
			SELECT sad.IDDevice
			FROM [SecurityAvailableDevicesTable](@RoleIds, @TargetSecurity, 0, NULL) as sad
			INNER JOIN [Devices] ON [Devices].IDDevice = sad.IDDevice
		END
	END 
	SELECT
		  [IDMarkedData]
		, [IDDevices] 
		, [LastModified]
		, [CreatedDate]
		, [Reference]
		, [UserName]
		, [Header]
		, [Description]
		, [MarkDataType]
		, [StartTime]
		, [TagTime]
		, [EndTime]
		, [RetentionType]
		, [RetentionUnits]
		, [RetentionExpire]
		, [ProtectionSize]
	  FROM 
	  (
			SELECT CASE
				WHEN @SortOrder='Header' AND @SortAscending = 1 THEN ROW_NUMBER() OVER (ORDER BY [MarkedData].[Header])
				WHEN @SortOrder='Header' AND @SortAscending = 0	THEN ROW_NUMBER() OVER (ORDER BY [MarkedData].[Header] DESC)
				WHEN @SortOrder='Description' AND @SortAscending = 1 THEN ROW_NUMBER() OVER (ORDER BY [MarkedData].[Description])
				WHEN @SortOrder='Description' AND @SortAscending = 0	THEN ROW_NUMBER() OVER (ORDER BY [MarkedData].[Description] DESC)
				WHEN @SortOrder='StartTime' AND @SortAscending = 1	THEN ROW_NUMBER() OVER (ORDER BY [MarkedData].[StartTime])  
				WHEN @SortOrder='StartTime' AND @SortAscending = 0	THEN ROW_NUMBER() OVER (ORDER BY [MarkedData].[StartTime] DESC)
				WHEN @SortOrder='TagTime' AND @SortAscending = 1	THEN ROW_NUMBER() OVER (ORDER BY [MarkedData].[TagTime])
				WHEN @SortOrder='TagTime' AND @SortAscending = 0	THEN ROW_NUMBER() OVER (ORDER BY [MarkedData].[TagTime] DESC) 
				WHEN @SortOrder='EndTime' AND @SortAscending = 1	THEN ROW_NUMBER() OVER (ORDER BY [MarkedData].[EndTime]) 
				WHEN @SortOrder='EndTime' AND @SortAscending = 0	THEN ROW_NUMBER() OVER (ORDER BY [MarkedData].[EndTime] DESC) 
				WHEN @SortOrder='CreateTime' AND @SortAscending = 1	THEN ROW_NUMBER() OVER (ORDER BY [MarkedData].[CreatedDate])
				WHEN @SortOrder='CreateTime' AND @SortAscending = 0	THEN ROW_NUMBER() OVER (ORDER BY [MarkedData].[CreatedDate] DESC) 
				WHEN @SortOrder='RetentionExpireTime' AND @SortAscending = 1	THEN ROW_NUMBER() OVER (ORDER BY [MarkedData].[RetentionExpire])
				WHEN @SortOrder='RetentionExpireTime' AND @SortAscending = 0	THEN ROW_NUMBER() OVER (ORDER BY [MarkedData].[RetentionExpire] DESC)
				WHEN @SortOrder='UserName' AND @SortAscending = 1	THEN ROW_NUMBER() OVER (ORDER BY [MarkedData].[UserName])
				WHEN @SortOrder='UserName' AND @SortAscending = 0	THEN ROW_NUMBER() OVER (ORDER BY [MarkedData].[UserName] DESC) 
				WHEN @SortOrder='Size' AND @SortAscending = 1	THEN ROW_NUMBER() OVER (ORDER BY [MarkedData].[ProtectionSize])
				WHEN @SortOrder='Size' AND @SortAscending = 0	THEN ROW_NUMBER() OVER (ORDER BY [MarkedData].[ProtectionSize] DESC)
				ELSE ROW_NUMBER() OVER (ORDER BY [MarkedData].[CreatedDate])
			END AS RowNum
			, [MarkedData].[IDMarkedData]
			, (
				SELECT IDDevice, Size
				FROM [MarkedDataDevices] mdd 
				WHERE [IDMarkedData] = [MarkedData].[IDMarkedData] 
				FOR XML PATH('Device'), ROOT('MarkedDataDevices'), Elements
			  ) AS IDDevices
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
		FROM 
			[MarkedData]
		WHERE
			EXISTS(SELECT IDDevice FROM [MarkedDataDevices] mdd INNER JOIN @AvailableDevices ad ON mdd.IDDevice = ad.ID WHERE [IDMarkedData] = [MarkedData].[IDMarkedData]) AND
			((IsNull(@CreatedFromTime, 0) = 0) OR (@CreatedFromTime < CreatedDate)) AND
			((IsNull(@CreatedToTime, 0) = 0) OR (@CreatedToTime > CreatedDate)) 
			AND
			(((@MarkedDataFromTime = 0) AND (@MarkedDataToTime = 0)) OR 
			(StartTime BETWEEN @MarkedDataFromTime AND @MarkedDataToTime) OR (EndTime BETWEEN @MarkedDataFromTime AND @MarkedDataToTime) OR
			(@MarkedDataFromTime BETWEEN StartTime AND EndTime) OR (@MarkedDataToTime BETWEEN StartTime AND EndTime)) 
			AND
			((@TagTimeFromTime = 0) OR (@TagTimeFromTime <= TagTime)) AND
			((@TagTimeToTime = 0) OR (@TagTimeToTime >= TagTime)) 
			AND
			((@RetentionExpireFromTime = 0) OR (RetentionType <> 0 AND @RetentionExpireFromTime <= RetentionExpire)) AND
			((@RetentionExpireToTime = 0) OR (RetentionType <> 0 AND @RetentionExpireToTime >= RetentionExpire)) 
			AND
			((@NoUsers = 1) OR [MarkedData].[UserName] IN (SELECT [ID] FROM @Users)) 
			AND
			((@Header IS NULL) OR ([MarkedData].Header LIKE @HeaderStrLike))
			AND
			((@Description IS NULL) OR ([MarkedData].[Description] LIKE @DescriptionStrLike))
			AND
			((@SearchText IS NULL) OR (@SearchReference = 1 AND [MarkedData].Reference = @SearchText) OR (@SearchUser = 1 AND [MarkedData].[UserName]= @SearchText) OR (@SearchHeader = 1 AND [MarkedData].Header LIKE @SearchStrLike) OR (@SearchDescription = 1 AND [MarkedData].[Description] LIKE @SearchStrLike))
			AND
			((@MarkedDataType = 0) OR ([MarkDataType] = @MarkedDataType))
		GROUP BY
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

	) AS DT
	WHERE RowNum BETWEEN @PageIndex * @PageSize + 1 AND @PageIndex * @PageSize + @PageSize
	ORDER BY RowNum
END
GO