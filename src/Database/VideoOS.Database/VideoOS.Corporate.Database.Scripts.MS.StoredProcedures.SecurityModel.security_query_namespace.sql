-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_query_namespace]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_query_namespace]
GO

CREATE PROCEDURE [security_query_namespace]
	@namespace_id UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON

	IF(@namespace_id IS NULL)
	BEGIN
		SELECT 
			[namespace_id], 
			[name], 
			[display_name], 
			[separator], 
			[structure],
			[user_defined]
		FROM [SecurityNamespace]
		ORDER BY [order_no]

		SELECT
			[namespace_id],
			[name],
			[display_name],
			[visible],
			[security_bit]
		FROM [SecurityAction]
		ORDER BY [namespace_id], [order_no] ASC
	END
	ELSE
	BEGIN
		SELECT 
			[namespace_id], 
			[name], 
			[display_name], 
			[separator], 
			[structure] ,
			[user_defined]
		FROM [SecurityNamespace]
		WHERE [namespace_id] = @namespace_id

		SELECT
			[namespace_id],
			[name],
			[display_name],
			[visible],
			[security_bit]
		FROM [SecurityAction]
		WHERE [namespace_id] = @namespace_id
		ORDER BY [order_no] ASC
	END
END
GO