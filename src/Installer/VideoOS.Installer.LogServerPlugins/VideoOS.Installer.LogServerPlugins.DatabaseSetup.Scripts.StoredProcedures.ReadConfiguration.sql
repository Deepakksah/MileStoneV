IF OBJECT_ID ( '[LogServer].[ReadConfiguration]', 'P' ) IS NOT NULL 
    DROP PROCEDURE [LogServer].[ReadConfiguration];
GO

CREATE PROCEDURE [LogServer].[ReadConfiguration]
AS
BEGIN
	SELECT [Name], [Value] 
	FROM [LogServer].[Configuration]
END
GO