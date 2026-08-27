-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Bookmarks]') AND type in (N'U'))
BEGIN
	INSERT INTO [MarkedData]
	SELECT [Bookmarks].[IDBookmark]
		, GetUtcDate()
		, GetUtcDate()
		, [Bookmarks].[Reference]
		, [Bookmarks].[User]
		, [Bookmarks].[Header]
		, [Bookmarks].[Description]
		, 8
		, [Bookmarks].[TimeBegin]
		, [Bookmarks].[TimeTrigged]
		, [Bookmarks].[TimeEnd]
		, 0
		, 0
		, 0
		, CAST(GetUtcDate() as bigint)
		, 0
	FROM [Bookmarks]

	INSERT INTO [MarkedDataDevices]
	SELECT [Bookmarks].[IDBookmark]
		, [Bookmarks].[IDDevice]
	FROM [Bookmarks]

	DROP TABLE Bookmarks
END

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_bookmark]') AND type in (N'P', N'PC'))
DROP PROCEDURE create_bookmark
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_bookmark]') AND type in (N'P', N'PC'))
	DROP PROCEDURE delete_bookmark
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_bookmarks_by_device_id]') AND type in (N'P', N'PC'))
	DROP PROCEDURE delete_bookmarks_by_device_id
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_bookmarks_by_device_id_before_time]') AND type in (N'P', N'PC'))
	DROP PROCEDURE delete_bookmarks_by_device_id_before_time
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_bookmark]') AND type in (N'P', N'PC'))
	DROP PROCEDURE query_bookmark
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_bookmarks]') AND type in (N'P', N'PC'))
	DROP PROCEDURE query_bookmarks
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_bookmark]') AND type in (N'P', N'PC'))
	DROP PROCEDURE update_bookmark
GO





