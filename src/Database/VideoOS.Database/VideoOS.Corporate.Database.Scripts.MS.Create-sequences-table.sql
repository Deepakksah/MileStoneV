-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Sequences]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
CREATE TABLE [dbo].[Sequences] (
	[Name] [NameType] NOT NULL ,
	[Sequence] [bigint] NOT NULL,
	PRIMARY KEY(Name)
) 
GO

-- Insert the default value
INSERT INTO [dbo].[Sequences] ([Name], [Sequence]) VALUES ('IdpEventLogIdSequence', 0)
GO