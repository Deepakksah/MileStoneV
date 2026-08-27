-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Add foreign key constraint to FailoverRecorders
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_FailoverRecorders_FailoverGroups]') AND parent_object_id = OBJECT_ID(N'[FailoverRecorders]'))
ALTER TABLE [FailoverRecorders] WITH CHECK 
ADD CONSTRAINT [FK_FailoverRecorders_FailoverGroups] FOREIGN KEY([IDFailoverGroup])
REFERENCES [FailoverGroups] ([IDFailoverGroup])
GO

ALTER TABLE [FailoverRecorders]
ALTER COLUMN [Description] [DescriptionType] NULL
GO

ALTER TABLE [FailoverRecorders]
ALTER COLUMN [HostName] [UriType] NULL
GO
