-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SecurityAccessCache]') AND type = 'U')
DROP TABLE [SecurityAccessCache]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SecurityAccessEntriesTimeProfile]') AND type = 'U')
DROP TABLE [SecurityAccessEntriesTimeProfile]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SecurityAccessEntries]') AND type = 'U')
DROP TABLE [SecurityAccessEntries]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SecurityObjects]') AND type = 'U')
DROP TABLE [SecurityObjects]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SecurityActions]') AND type = 'U')
DROP TABLE [SecurityActions]
GO

-- Old table that have not been used for a long time
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ViewGroups]') AND type = 'U')
DROP TABLE [ViewGroups]
GO

-- Old table that have not been used for a long time
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ViewGroupData]') AND type = 'U')
DROP TABLE [ViewGroupData]
GO
