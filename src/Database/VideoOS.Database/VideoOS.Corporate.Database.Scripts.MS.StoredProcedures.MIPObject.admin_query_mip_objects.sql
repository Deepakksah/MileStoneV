-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_query_mip_objects]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_query_mip_objects]
GO

CREATE PROCEDURE [admin_query_mip_objects]
	@FilterOnServerGroup [bit],
	@IDServerGroup [GuidType] = ''
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

	IF (@FilterOnServerGroup = 0)
	BEGIN
		SELECT
			m.[IDMIPObject],
			m.[Name],
			m.[Kind],
			m.[SubKind],
			m.[PluginId],
			m.[CustomData],
			m.[LastModified],
			m.[CreatedDate],
			COALESCE( sg.[RelationIds], sgm.IDServerGroup) AS [RelationIds]
		FROM 
			[MIPObject] m
			LEFT JOIN [ServerGroupMembership] sgm ON sgm.IDObject = m.[IDMIPObject]
			LEFT JOIN [ServerGroup] sg ON sg.IDServerGroup = sgm.IDServerGroup
	END
	ELSE
	BEGIN
		IF(@IDServerGroup IS NULL OR @IDServerGroup = '')
		BEGIN
			SELECT
				m.[IDMIPObject],
				m.[Name],
				m.[Kind],
				m.[SubKind],
				m.[PluginId],
				m.[CustomData],
				m.[LastModified],
				m.[CreatedDate],
				'' AS [RelationIds]
			FROM 
				[MIPObject] m
				LEFT JOIN [ServerGroupMembership] sgm ON sgm.IDObject = m.[IDMIPObject]
			WHERE
				sgm.IDServerGroup IS NULL
		END
		ELSE
		BEGIN
			SELECT
				m.[IDMIPObject],
				m.[Name],
				m.[Kind],
				m.[SubKind],
				m.[PluginId],
				m.[CustomData],
				m.[LastModified],
				m.[CreatedDate],
				COALESCE( sg.[RelationIds], sgm.IDServerGroup) AS [RelationIds]
			FROM 
				[MIPObject] m
				LEFT JOIN [ServerGroupMembership] sgm ON sgm.IDObject = m.[IDMIPObject]
				LEFT JOIN [ServerGroup] sg ON sg.IDServerGroup = sgm.IDServerGroup
			WHERE
				sgm.IDServerGroup = @IDServerGroup
		END
	END
END
GO