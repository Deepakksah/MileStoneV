---- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

INSERT [SecurityPermissions] ([IDRole], [SecurityAttributeID], [Value], [IDTimeProfile], [IsDefaultTimeProfile], [LastModified])
SELECT r.[IDRole], 'APPLICATIONACCESSCLIENTLOGIN', 'True', NULL, 1, GETUTCDATE()
FROM [SecurityRoles] r
LEFT JOIN [SecurityPermissions] sp ON sp.[IDRole] = r.[IDRole] AND sp.[SecurityAttributeID] = 'APPLICATIONACCESSCLIENTLOGIN'
WHERE sp.[IDRole] IS NULL
           AND r.[IDRole] <> '1f30d14b-ba91-4e8a-8838-eda17931f8fc'
GO
