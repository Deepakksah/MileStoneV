-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Define helper stored procedure which will we be cleaned up at the end

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Temp_Update_MipItem]') AND type in (N'P', N'PC'))
DROP PROCEDURE [Temp_Update_MipItem]
GO

CREATE PROCEDURE [Temp_Update_MipItem]
	@ItemId UNIQUEIDENTIFIER,
	@ItemOldId UNIQUEIDENTIFIER
AS
BEGIN
	INSERT INTO [Mip].[Items] ([ItemId], [Name], [Kind], [ParentId], [ServerId], [ServerScheme], [ServerHost], [ServerPort], [ServerType], [Enabled], [Properties], [GisPoint], [LastModified], [Encrypted])
	SELECT @ItemId, i.[Name], i.[Kind], i.[ParentId], i.[ServerId], i.[ServerScheme], i.[ServerHost], i.[ServerPort], i.[ServerType], i.[Enabled], i.[Properties], i.[GisPoint], i.[LastModified], i.[Encrypted]
	FROM [Mip].[Items] i
	WHERE i.ItemId = @ItemOldId

	DELETE
	FROM [Mip].[Items]
	WHERE [ItemId] = @ItemOldId
END
GO

DECLARE @RegistrationId [GuidType]		=	'AD5EFDCE-3B21-47D5-A6A8-657E43A3E638';
DECLARE @RegistrationIdOld [GuidType]	=	'E125597A-ECAF-40A1-99E6-24BB9D45A09E';
IF NOT EXISTS(SELECT * FROM [Mip].[Items] WHERE [ItemId] = @RegistrationId)
BEGIN
	IF EXISTS(SELECT * FROM [Mip].[Items] WHERE [ItemId] = @RegistrationIdOld)
	BEGIN
		EXEC [Temp_Update_MipItem] @RegistrationId, @RegistrationIdOld
	END
END
GO

-- Cleanup helper stored procedure
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Temp_Update_MipItem]') AND type in (N'P', N'PC'))
DROP PROCEDURE [Temp_Update_MipItem]
GO
