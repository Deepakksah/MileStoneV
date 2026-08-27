-- This header is used by the dbupdate7 tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[FormatErrorMessage]') AND type in (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [FormatErrorMessage]
GO

CREATE FUNCTION [FormatErrorMessage](@ERROR_NUMBER INT, @ERROR_SEVERITY INT, @ERROR_STATE INT, @ERROR_PROCEDURE NVARCHAR(MAX), @ERROR_LINE INT, @ERROR_MESSAGE NVARCHAR(MAX))
RETURNS NVARCHAR(MAX)
AS
BEGIN
	DECLARE @errorMessage NVARCHAR(MAX)

	SELECT @errorMessage = N': %%Status=' + CAST(@ERROR_NUMBER AS NVARCHAR(10)) 
		+ N': %%Severity=' + CAST(@ERROR_SEVERITY AS NVARCHAR(10)) 
		+ N': %%State=' + CAST(@ERROR_STATE AS NVARCHAR(10)) 
		+ N': %%Procedure=' + @@SERVERNAME + '.' + db_name() + '..' + @ERROR_PROCEDURE
		+ N': %%Line=' + CAST(@ERROR_LINE AS NVARCHAR(10)) 
		+ N': %%Message=' + @ERROR_MESSAGE;
	RETURN @errorMessage
END
GO