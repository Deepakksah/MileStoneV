-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_query_mip_objects_by_kind]') AND type = 'P')
DROP PROCEDURE [admin_query_mip_objects_by_kind]
GO

CREATE PROCEDURE [admin_query_mip_objects_by_kind]
	@Kind [UNIQUEIDENTIFIER],
	@SubKind varchar(36)
AS
BEGIN
	SET NOCOUNT ON

	IF (@SubKind IS NULL OR @SubKind = '')
	BEGIN
	SELECT
		m.[IDMIPObject],
		m.[Name],
		m.[Kind],
		m.[SubKind],
		m.[PluginId],
		m.[CustomData],
		m.[LastModified],
		m.[CreatedDate]
	FROM
		[MIPObject] m
	WHERE m.[Kind] = @Kind
	END ELSE
	BEGIN
	SELECT
		m.[IDMIPObject],
		m.[Name],
		m.[Kind],
		m.[SubKind],
		m.[PluginId],
		m.[CustomData],
		m.[LastModified],
		m.[CreatedDate]
	FROM
		[MIPObject] m
	WHERE m.[Kind] = @Kind AND m.[SubKind] = @SubKind
	END
END
GO