IF OBJECT_ID ( '[LogServer].[ReadVersion]', 'P' ) IS NOT NULL 
    DROP PROCEDURE [LogServer].[ReadVersion];
GO

IF OBJECT_ID ( '[LogServer].[WriteVersion]', 'P' ) IS NOT NULL 
    DROP PROCEDURE [LogServer].[WriteVersion];
GO