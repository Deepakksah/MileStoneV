-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[VideoWall]') AND name = N'statustext')
BEGIN
	ALTER TABLE [VideoWall]
	ADD [statustext] BIT NULL
END
GO

UPDATE VideoWall
SET [statustext] = 0
WHERE [statustext] IS NULL
GO

ALTER TABLE [VideoWall]
ALTER COLUMN [statustext] BIT NOT NULL
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[VideoWall]') AND name = N'titlebar')
BEGIN
	ALTER TABLE [VideoWall]
	ADD [titlebar] VARCHAR(50) NULL
END
GO

UPDATE VideoWall
SET [titlebar] = 'None'
WHERE [titlebar] IS NULL
GO

ALTER TABLE [VideoWall]
ALTER COLUMN [titlebar] VARCHAR(50) NOT NULL
GO
