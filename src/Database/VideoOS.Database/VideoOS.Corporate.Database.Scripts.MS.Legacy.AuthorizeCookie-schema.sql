-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[AuthorizationCookie]') AND type in (N'U'))
CREATE TABLE [AuthorizationCookie]
(
	[AuthorizationId] NVARCHAR(256) UNIQUE NOT NULL,
	[SID] NVARCHAR(256) NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	[ValidUntil] DATETIME NOT NULL,
	[ActionType] NVARCHAR(20) NOT NULL,

	CONSTRAINT [PK_AuthorizeCookie] PRIMARY KEY NONCLUSTERED
	(
		[AuthorizationId] ASC
	),
) ON [PRIMARY]
GO




