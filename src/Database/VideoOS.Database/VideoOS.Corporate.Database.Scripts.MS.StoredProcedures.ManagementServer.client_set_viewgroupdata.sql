-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[client_set_viewgroupdata]') AND type in (N'P', N'PC'))
DROP PROCEDURE [client_set_viewgroupdata]
GO

CREATE PROCEDURE [client_set_viewgroupdata]
	@IDViewGroup [GuidType],
	@IDViewGroupType [GuidType],
	@User NVARCHAR(512),
	@DataVersion INT,
	@XmlVersion INT,
	@Data NTEXT
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

		IF NOT EXISTS(
			SELECT * 
			FROM [ViewGroupData] 
			WHERE [IDViewGroup] = @IDViewGroup AND [IDViewGroupType] = @IDViewGroupType
		)
		BEGIN
			INSERT [ViewGroupData] (
				[IDViewGroup], 
				[IDViewGroupType], 
				[DataVersion], 
				[XmlVersion],
				[GroupDataXml],
				[ModifiedDate],
				[ModifiedUser],
				[LastModified]
			)
			VALUES (
				@IDViewGroup,
				@IDViewGroupType,
				@DataVersion,
				@XmlVersion,
				@Data,
				@CurrentUtcTime,
				@User,
				@CurrentUtcTime
			)
		END
		ELSE
		BEGIN
			UPDATE [ViewGroupData]
			SET [DataVersion] = @DataVersion,
				[XmlVersion] = @XmlVersion,
				[GroupDataXml] = @Data,
				[ModifiedDate] = @CurrentUtcTime,
				[ModifiedUser] = @User,
				[LastModified] = @CurrentUtcTime
			WHERE [IDViewGroup] = @IDViewGroup AND [IDViewGroupType] = @IDViewGroupType
		END

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