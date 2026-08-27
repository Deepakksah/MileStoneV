---- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SecurityAccessEntriesTimeProfile]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[SecurityAccessEntriesTimeProfile]
	(
		[IDAccessEntry] INT NOT NULL,
		[IDTimeProfile] [dbo].[GuidType] NULL,
		[IsDefaultTimeProfile] [bit] NOT NULL,
		 CONSTRAINT [PK_SecurityAccessEntriesTimeProfile] PRIMARY KEY CLUSTERED 
		(
			[IDAccessEntry] ASC
		),
		CONSTRAINT [FK_SecurityAccessEntriesTimeProfile_SecurityAccessEntries] FOREIGN KEY
		(
			[IDAccessEntry] 
		)
		REFERENCES [SecurityAccessEntries] ([IDAccessEntry])
		ON DELETE CASCADE
	) ON [PRIMARY]
END
GO

INSERT INTO SecurityAccessEntriesTimeProfile(IDAccessEntry, IDTimeProfile, IsDefaultTimeProfile)
SELECT sae.IDAccessEntry, NULL, 1 
FROM SecurityAccessEntries sae
INNER JOIN SecurityObjects so on sae.IDSecurityObject = so.IDSecurityObject
WHERE so.IDClass ='VIDEO_WALL_MONITOR'
AND sae.IDAction = 'APPLYLAYOUT'
AND sae.Deleted = 0
AND sae.SID IN (SELECT IDRole FROM SecurityRoles)
GO

INSERT INTO SecurityAccessEntriesTimeProfile(IDAccessEntry, IDTimeProfile, IsDefaultTimeProfile)
SELECT sae.IDAccessEntry, NULL, 1 
FROM SecurityAccessEntries sae
INNER JOIN SecurityObjects so on sae.IDSecurityObject = so.IDSecurityObject
WHERE so.IDClass ='VIDEO_WALL_PRESET'
AND sae.IDAction = 'ACTIVATE'
AND sae.Deleted = 0
AND sae.SID IN (SELECT IDRole FROM SecurityRoles)
GO

INSERT INTO SecurityAccessEntriesTimeProfile(IDAccessEntry, IDTimeProfile, IsDefaultTimeProfile)
SELECT sae.IDAccessEntry, NULL, 1 
FROM SecurityAccessEntries sae
INNER JOIN SecurityObjects so on sae.IDSecurityObject = so.IDSecurityObject
WHERE so.IDClass ='USER_DEFINED_EVENT'
AND sae.IDAction = 'TRIGGER_EVENT'
AND sae.Deleted = 0
AND sae.SID IN (SELECT IDRole FROM SecurityRoles)
GO
