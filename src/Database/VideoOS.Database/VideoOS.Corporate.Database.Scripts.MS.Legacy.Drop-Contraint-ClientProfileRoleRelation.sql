-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[PK_ClientProfileRoleRelation]') AND type in (N'PK'))
BEGIN
	ALTER TABLE ClientProfileRoleRelation
	DROP CONSTRAINT [PK_ClientProfileRoleRelation]
END
GO
