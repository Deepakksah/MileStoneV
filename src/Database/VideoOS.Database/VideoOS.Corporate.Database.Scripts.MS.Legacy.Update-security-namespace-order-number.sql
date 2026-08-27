-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

UPDATE SecurityNamespace
SET order_no = nom.new_order_no
FROM 
(
	SELECT ROW_NUMBER() OVER(ORDER BY order_no, display_name) AS new_order_no, namespace_id AS namespace_id2 
	FROM SecurityNamespace
) AS nom
WHERE namespace_id = nom.namespace_id2
GO