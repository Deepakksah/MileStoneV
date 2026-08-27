-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

BEGIN
	BEGIN TRAN
	BEGIN TRY
		ALTER TABLE [dbo].[PerfomanceCounterStateSource] DROP CONSTRAINT FK_PerfomanceCounterStateSource_PerfomanceCounterStateRule
		ALTER TABLE [dbo].[PerfomanceCounterStateOverride] DROP CONSTRAINT FK_PerfomanceCounterStateOverride_PerfomanceCounterStateSource

		IF ( object_id(N'[dbo].[PerfomanceCounterStateOverride]', N'U') is not null )
		BEGIN
			DROP TABLE [dbo].[PerfomanceCounterStateOverride]
		END
		IF ( object_id(N'[dbo].[PerfomanceCounterStateRule]', N'U') is not null )
		BEGIN
			DROP TABLE [dbo].[PerfomanceCounterStateRule]
		END
		IF ( object_id(N'[dbo].[PerfomanceCounterStateSource]', N'U') is not null )
		BEGIN
			DROP TABLE [dbo].[PerfomanceCounterStateSource]
		END
		IF ( object_id(N'[dbo].[PerfomanceCounterType]', N'U') is not null )
		BEGIN
			DROP TABLE [dbo].[PerfomanceCounterType]
		END	
	COMMIT TRAN
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN
	END CATCH	
END
GO



