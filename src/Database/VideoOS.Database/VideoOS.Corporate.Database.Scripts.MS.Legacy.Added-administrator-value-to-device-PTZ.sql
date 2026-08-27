-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
UPDATE [SecurityAttributes]
SET [Definition] = 
'<attributedefinition>
	<type>integer</type>
	<data>
		<datatype>int</datatype>
		<minvalue>0</minvalue>
		<maxvalue>32000</maxvalue>
		<resolution>1</resolution>
		<defaultvalue>3000</defaultvalue>
		<administratorvalue>32000</administratorvalue>
		<aggregation>max</aggregation>
	</data>
</attributedefinition>'
WHERE [SecurityAttributeID] = 'DEVICEPTZPRIORITY'
GO
