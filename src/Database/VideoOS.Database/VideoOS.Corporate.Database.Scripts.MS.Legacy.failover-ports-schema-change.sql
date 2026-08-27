-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Add foreign key constraint to FailoverRecorders
ALTER TABLE [FailoverServers]
ALTER COLUMN [UdpPort] [int] NOT NULL
GO

ALTER TABLE [Recorders]
ALTER COLUMN [FailoverUdpPort] [int] NULL
GO

UPDATE [Recorders] SET [FailoverUdpPort] = 11000 WHERE IsNull([FailoverUdpPort], 0) = 0
GO