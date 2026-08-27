-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DriverNumbers]') AND type = 'U')
CREATE TABLE [dbo].[DriverNumbers]
(
	[IDDriver] UNIQUEIDENTIFIER NOT NULL,
	[DriverNumber] INT IDENTITY(50001, 1) NOT NULL
)
GO

ALTER TABLE [dbo].[DriverNumbers] ADD CONSTRAINT [PK_DriverNumbers] PRIMARY KEY NONCLUSTERED
(
	[IDDriver] ASC
)
GO

CREATE UNIQUE CLUSTERED INDEX CIX_DriverNumbers ON [dbo].[DriverNumbers]([DriverNumber])
GO