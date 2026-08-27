-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_dbo_driver_numbers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_dbo_driver_numbers]
GO

CREATE PROCEDURE [dbo].[update_dbo_driver_numbers]
  @id_drivers tvp_StringTableGuidType READONLY
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO dbo.DriverNumbers (IDDriver)
	(
		SELECT ID AS [IDDriver] FROM @id_drivers
		EXCEPT 
		SELECT [IDDriver] FROM [dbo].[DriverNumbers]
	)
	SELECT * FROM dbo.DriverNumbers
	WHERE IDDriver IN (SELECT ID FROM @id_drivers)
END
GO