-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SystemDashboardConfiguration]') AND type = 'U')
DROP TABLE [SystemDashboardConfiguration]

CREATE TABLE [SystemDashboardConfiguration]
(
	[id] INT NOT NULL,
	[name] NVARCHAR(100) NOT NULL,
	[state_source_type] NVARCHAR(100) NOT NULL,
	[single_state_source_id] NVARCHAR(100) NULL,
	[event_types] NVARCHAR(MAX) NULL

	CONSTRAINT [PK_SystemDashboardConfiguration] PRIMARY KEY NONCLUSTERED
	(
		[id] ASC
	)
	 
) ON [PRIMARY]
GO
