-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[fn_SIDToString]') AND type in (N'FN'))
DROP FUNCTION [fn_SIDToString]
GO

CREATE FUNCTION fn_SIDToString
(
  @BinSID AS VARBINARY(100)
)
RETURNS VARCHAR(100)
AS BEGIN

  IF LEN(@BinSID) % 4 <> 0 RETURN(NULL)

  DECLARE @StringSID VARCHAR(100)
  DECLARE @i AS INT
  DECLARE @j AS INT

  SELECT @StringSID = 'S-'
     + CONVERT(VARCHAR, CONVERT(INT, CONVERT(VARBINARY, SUBSTRING(@BinSID, 1, 1)))) 
  SELECT @StringSID = @StringSID + '-'
     + CONVERT(VARCHAR, CONVERT(INT, CONVERT(VARBINARY, SUBSTRING(@BinSID, 3, 6))))

  SET @j = 9
  SET @i = LEN(@BinSID)

  WHILE @j < @i
  BEGIN
    DECLARE @val BINARY(4)
    SELECT @val = SUBSTRING(@BinSID, @j, 4)
    SELECT @StringSID = @StringSID + '-'
      + CONVERT(VARCHAR, CONVERT(BIGINT, CONVERT(VARBINARY, REVERSE(CONVERT(VARBINARY, @val))))) 
    SET @j = @j + 4
  END
  RETURN ( @StringSID ) 
END
GO
