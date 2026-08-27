-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[append_to_file]') AND type in (N'P', N'PC'))
DROP PROCEDURE [append_to_file]
GO

CREATE PROCEDURE append_to_file(@FileName VARCHAR(255), @Text1 VARCHAR(255)) AS
BEGIN

	DECLARE @OLEResult INT
	DECLARE @FS INT
	DECLARE @FileID INT
	
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
  
	EXECUTE @OLEResult = sp_OACreate 'Scripting.FileSystemObject', @FS OUT
	IF @OLEResult <> 0
	BEGIN
		;THROW 60046, @procedureName, 1;
	END

	--Open a file
	EXECUTE @OLEResult = sp_OAMethod @FS, 'OpenTextFile', @FileID OUT, @FileName, 8, 1
	IF @OLEResult <> 0
	BEGIN
		;THROW 60105, @procedureName, 1;
	END

	--Write Text1
	EXECUTE @OLEResult = sp_OAMethod @FileID, 'WriteLine', Null, @Text1
	IF @OLEResult <> 0
	BEGIN
		;THROW 60105, @procedureName, 1;
	END

	EXECUTE @OLEResult = sp_OADestroy @FileID
	EXECUTE @OLEResult = sp_OADestroy @FS
	
	RETURN 0
END
GO
