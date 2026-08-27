-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Add column to SecurityRolesIdentity
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[SecurityRolesIdentity]') AND name = N'IsStatic')
ALTER TABLE [SecurityRolesIdentity]
  ADD [IsStatic] BIT NULL 
GO

-- Add column to SecurityRolesIdentity
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[SecurityRolesIdentity]') AND name = N'IsDynamic')
ALTER TABLE [SecurityRolesIdentity]
  ADD [IsDynamic] [BIT] NULL
GO


UPDATE	[SecurityRolesIdentity]
SET		[IsStatic] = 1, [IsDynamic] = 0
WHERE	[IsStatic] IS NULL
GO


ALTER TABLE [SecurityRolesIdentity]
  ALTER COLUMN [IsStatic] [BIT] NOT NULL
GO

ALTER TABLE [SecurityRolesIdentity]
  ALTER COLUMN [IsDynamic] [BIT] NOT NULL
GO
