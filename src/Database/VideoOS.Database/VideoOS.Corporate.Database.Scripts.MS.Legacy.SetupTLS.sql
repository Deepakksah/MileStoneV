---- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

EXEC sp_configure 'clr enabled' , '1';
RECONFIGURE;
GO

IF NOT EXISTS (SELECT * FROM [License] WHERE [IDLicense] = '968FBC5E-99BD-4E08-B711-8A73D2A37461')
BEGIN

DECLARE @CurrentTime DateTime
SET @CurrentTime = GETUTCDATE()

DECLARE @Data AS NVARCHAR(MAX)
SET @Data = dbo.CreateTLSData('{MasterEncryptionKey}')
INSERT INTO [License] 
	([LastModified], [IDLicense], [Data]) 
VALUES 
	(@CurrentTime, '968FBC5E-99BD-4E08-B711-8A73D2A37461', @Data)

END
GO

EXEC sp_configure 'clr enabled' , '0';
RECONFIGURE;
GO
