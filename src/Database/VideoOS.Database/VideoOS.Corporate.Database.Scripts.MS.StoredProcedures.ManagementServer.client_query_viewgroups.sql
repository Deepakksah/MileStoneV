-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[client_query_viewgroups]') AND type in (N'P', N'PC'))
DROP PROCEDURE [client_query_viewgroups]
GO

CREATE PROCEDURE [client_query_viewgroups]
	@SID [SIDType],
	@IDViewGroupType [GuidType]
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRANSACTION

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		IF NOT EXISTS(SELECT * FROM [ViewGroups] WHERE OwnerSID = @SID)
		BEGIN
			INSERT [ViewGroups] ([IDViewGroup], [Name], [Type], [OwnerSID], [LastModified])
			VALUES(NEWID(), 'Private', 0, @SID, @CurrentUtcTime)
		END

		SELECT 
			vg.[IDViewGroup],
			vg.[Name], 
			vg.[Type], 
			vg.[OwnerSID], 
			COALESCE(vgd.[ModifiedDate], @CurrentUtcTime) AS [LastModified],
			COALESCE(vgd.[ModifiedUser], '') AS [LastModifiedUser]
		FROM [ViewGroups] vg
		LEFT JOIN [ViewGroupData] vgd ON vg.[IDViewGroup] = vgd.[IDViewGroup]
			AND vgd.[IDViewGroupType] = @IDViewGroupType
		WHERE vg.[OwnerSID] = 'S-1-1-0' OR vg.[OwnerSID] = @SID

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH 
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION
		END;
		DECLARE @shouldReThrowError BIT = dbo.[ShouldReThrowError](ERROR_NUMBER(), ERROR_SEVERITY())
		IF(@shouldReThrowError = 1)
			THROW;
		SET @ErrorArgs = dbo.[FormatErrorMessage](ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE())
		;THROW 60000, @ErrorArgs, 1;
	END CATCH;
END
GO