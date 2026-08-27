-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

UPDATE rsd SET rsd.[edgestorage_enabled] = 0, rsd.[last_modified] = GETUTCDATE()
FROM [RecordingStorageDevice] rsd
INNER JOIN [Devices] d on rsd.[device_id] = d.[IDDevice]
WHERE (rsd.[edgestorage_enabled] = 1 AND NOT
	   d.[Settings].exist('/properties/settings/setting[@typeid="E89CDFA5-491F-4B14-9D28-FB98F3948449"]/value//text()[contains(lower-case(.), "true")]') = 1)

GO

