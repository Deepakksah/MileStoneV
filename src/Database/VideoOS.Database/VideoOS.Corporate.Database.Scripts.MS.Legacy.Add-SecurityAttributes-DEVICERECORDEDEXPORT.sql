---- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
IF NOT EXISTS (SELECT TOP 1 * FROM [SecurityAttributes] WHERE SecurityAttributeID = 'DEVICERECORDEDEXPORT')
BEGIN
	INSERT INTO [SecurityAttributes]
	(
		 [LastModified]
		,[SecurityAttributeID]
		,[Name]
		,[Description]
		,[ParentSecurityAttributeName]
		,[SecurityType]
		,[HasObject]
		,[DisplayOrder]
		,[Definition]
		,[TimeProfileBased]
		,[TimeBackwardBased]
	)
	VALUES
	(
		GETDATE()
		,'DEVICERECORDEDEXPORT'
		,'Export video'
		,'Export video'
		,'DEVICERECORDED'
		,'Camera'
		, 0
		, 56
		, ''
		, 0
		, 0
	)
END

INSERT INTO [SecurityPermissionsDevice]
           ([LastModified]
           ,[SecurityAttributeID]
           ,[IDRole]
           ,[IDDevice]
           ,[Value]
           ,[IDTimeProfile]
           ,[TimeBackward]
           ,[IsDefaultTimeProfile])
SELECT DISTINCT 
		GETDATE() AS LastModified
	  ,'DEVICERECORDEDEXPORT' AS [SecurityAttributeID]
      ,[IDRole]
      ,[IDDevice]
      ,[Value]
      ,[IDTimeProfile]
      ,[TimeBackward]
      ,[IsDefaultTimeProfile]
FROM [SecurityPermissionsDevice]
WHERE SecurityAttributeID IN ('DEVICERECORDEDEXPORTAVI','DEVICERECORDEDEXPORTDATABASE')

DELETE FROM [SecurityPermissionsDevice]
WHERE SecurityAttributeID IN ('DEVICERECORDEDEXPORTAVI','DEVICERECORDEDEXPORTDATABASE')

DELETE FROM SecurityAttributes
WHERE SecurityAttributeID IN ('DEVICERECORDEDEXPORTAVI','DEVICERECORDEDEXPORTDATABASE')
GO

