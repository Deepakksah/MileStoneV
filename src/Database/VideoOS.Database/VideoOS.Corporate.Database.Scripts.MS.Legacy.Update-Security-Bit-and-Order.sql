-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

UPDATE [SecurityAction]
SET security_bit = secBit.securityBit, order_no = secbit.new_order_no
FROM
(
	SELECT 
		namespace_id
		,name
		,CASE
			WHEN RANK() OVER (PARTITION by namespace_id ORDER BY order_no, name) = 1 THEN 1
			ELSE POWER(2, RANK() OVER (PARTITION by namespace_id ORDER BY order_no, name) - 1)
		 END as securityBit
		 , RANK() OVER(PARTITION BY namespace_id ORDER BY order_no, name) - 1 as new_order_no
	FROM SecurityAction 
) as secbit
WHERE [SecurityAction].namespace_id = secbit.namespace_id
AND [SecurityAction].name = secbit.name
GO
