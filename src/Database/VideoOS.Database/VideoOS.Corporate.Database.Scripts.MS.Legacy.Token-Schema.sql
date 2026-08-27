-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Token]') AND type in (N'U'))
BEGIN
	CREATE TABLE [Token] (
		[TokenId] NVARCHAR(255) NOT NULL,
		[SID] SIDType NOT NULL,
		[Expires] bigint NOT NULL,
		[RegistrationTime] DateTime NOT NULL,

		CONSTRAINT [PK_Token] PRIMARY KEY NONCLUSTERED
		(
			[TokenId] ASC
		)
	)
END


GO
