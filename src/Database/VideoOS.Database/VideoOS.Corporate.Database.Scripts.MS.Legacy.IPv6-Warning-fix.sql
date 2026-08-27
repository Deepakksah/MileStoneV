-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Fix IPScanRangeWarning for Recorders

DECLARE @df_name varchar(256)

SET @df_name = (select sys.default_constraints.name from sys.default_constraints 
where parent_object_id = OBJECT_ID('Recorders') and parent_column_id = (
select column_id from sys.columns
inner join sys.tables on sys.tables.object_id = sys.columns.object_id
where sys.tables.object_id = OBJECT_ID('Recorders')
and sys.columns.name = 'IPScanRangeWarning'
))

IF((@df_name IS NULL) OR (@df_name <> 'DF_IPScanRangeWarning'))
BEGIN
	IF(@df_name IS NOT NULL)
	BEGIN
		EXEC('ALTER TABLE [Recorders] DROP CONSTRAINT ' + @df_name)
	END

	ALTER TABLE [Recorders]
	ADD CONSTRAINT [DF_IPScanRangeWarning]
	DEFAULT 255 FOR [IPScanRangeWarning]
	
	UPDATE [Recorders]
	SET [IPScanRangeWarning] = 255
	WHERE [IPScanRangeWarning] IS NULL
	
	ALTER TABLE [Recorders]
	ALTER COLUMN [IPScanRangeWarning] INT NOT NULL
END
GO
