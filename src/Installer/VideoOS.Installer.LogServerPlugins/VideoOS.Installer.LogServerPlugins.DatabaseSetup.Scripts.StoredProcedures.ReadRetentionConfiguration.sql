IF OBJECT_ID ( '[LogServer].[ReadRetentionConfiguration]', 'P' ) IS NOT NULL 
    DROP PROCEDURE [LogServer].[ReadRetentionConfiguration];
GO

CREATE PROCEDURE [LogServer].[ReadRetentionConfiguration]
AS
BEGIN
	SELECT [LogTypeId], [RetentionDays] 
	FROM [LogServer].[RetentionConfiguration]
END
GO