-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[bookmarks_delete_by_device_id_before_time]') AND type in (N'P', N'PC'))
DROP PROCEDURE [bookmarks_delete_by_device_id_before_time]
GO

CREATE PROCEDURE [bookmarks_delete_by_device_id_before_time]
	@IDDevice [GuidType],
	@BeforeTime [BigInt]
AS
BEGIN
	SET NOCOUNT ON
 
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRAN

		DECLARE @ids TABLE(ID uniqueidentifier)

		DELETE B_MDD 
		OUTPUT DELETED.IDMarkedData INTO @ids
		FROM MarkedDataDevices AS B_MDD 
		INNER JOIN MarkedData AS B_MD ON B_MD.IDMarkedData = B_MDD.IDMarkedData
		WHERE 
			B_MDD.IDDevice = @IDDevice AND
			B_MD.MarkDataType = 8 AND
			B_MD.EndTime < @BeforeTime

		-- Reduce ids to those without any references in MarkedDataDevices
		DELETE ids FROM @ids AS ids
		INNER JOIN MarkedDataDevices AS B_MDD ON ids.ID = B_MDD.IDMarkedData

		DELETE B_MD FROM [MarkedData] AS B_MD
		INNER JOIN @ids AS ids ON ids.ID = B_MD.IDMarkedData

		COMMIT TRAN
	END TRY
	BEGIN CATCH 
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION
		END;
		THROW; -- Used from ServerService, where Error 60000 is NOT expected
	END CATCH;
END
GO