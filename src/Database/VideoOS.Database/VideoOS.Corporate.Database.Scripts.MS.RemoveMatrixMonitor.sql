-- This header is used by the dbupdate tool.
-- The TARGET poperty is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS (SELECT 1
			 FROM sys.columns 
			WHERE object_id = OBJECT_ID(N'[dbo].[Matrix]') 
			  AND name = N'Type')
BEGIN
  -- Remove all rows from Matrix that don't have Type = 2
  EXEC sys.sp_executesql N'DELETE FROM [dbo].[Matrix]
							WHERE [Type] != 2'

  -- Remove Type column
  ALTER TABLE [dbo].[Matrix]
   DROP COLUMN [Type]
END
GO