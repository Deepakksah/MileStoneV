-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @CurrentTime DateTime
SET @CurrentTime = GETUTCDATE()
--replacing from MD5 to SHA1 Guid
--original guid in script "07.0.55.Default Access Control Rule.sql"
IF EXISTS(SELECT * FROM [Rules] WHERE [IDRule] = '3D60DF41-AA8C-49AF-A06D-AA30F6CE4360') 
BEGIN
UPDATE [RULES] 
SET 
[LastModified] = @CurrentTime, 
[Definition] = CAST(Replace(Cast([Definition] as varchar(MAX)),'7edbf312-8857-79c1-ea32-14b86f6334a8','7ff4e6d4-9078-5716-a83c-3ff5a2b35cb2') as XML)
WHERE [IDRule] = '3D60DF41-AA8C-49AF-A06D-AA30F6CE4360'
END
GO