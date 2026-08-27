-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

ALTER TABLE Recorders
ALTER COLUMN MulticastSettings XML NOT NULL
GO

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[RecordingServerServiceConfigurable]') AND type in (N'U'))
BEGIN
	CREATE TABLE [RecordingServerServiceConfigurable]
	(
		[LastModified] DATETIME NOT NULL, 
		[PKID] [GuidType] NOT NULL,
		[MulticastServerAddress] NVARCHAR(256) NOT NULL,

		CONSTRAINT [PK_RecordingServerServiceConfigurable] PRIMARY KEY NONCLUSTERED
		(
			[PKID] ASC
		),

		CONSTRAINT [FK_RecordingServerServiceConfigurable_RecordingServerService] FOREIGN KEY([PKID]) REFERENCES [RecordingServerService] ([pkid]),

	) ON [PRIMARY]

	
	DECLARE @CurrentTime DateTime
	SET @CurrentTime = GETUTCDATE()

	-- Insert configurable values for standard recorders
	INSERT INTO RecordingServerServiceConfigurable([LastModified], [PKID], [MulticastServerAddress])
	SELECT @CurrentTime, rssr.recording_server_service_id, COALESCE(r.MulticastSettings.value('(properties/settings/setting[name = "ServerAddress"]/value)[1]', 'NVARCHAR(MAX)'), '0.0.0.0')
	FROM [RecordingServerServiceRecorder] as rssr 
	INNER JOIN [Recorders] as r ON r.IDRecorder = rssr.recorder_id
	WHERE rssr.[type] = 0

	-- Insert configurable default values for failover and hotstandby recorders
	INSERT INTO RecordingServerServiceConfigurable([LastModified], [PKID], [MulticastServerAddress])
	SELECT @CurrentTime, rss.pkid, '0.0.0.0'
	FROM [RecordingServerService] as rss 
	LEFT OUTER JOIN [RecordingServerServiceRecorder] as rssr ON rss.pkid = rssr.recording_server_service_id
	WHERE rssr.[type] is null or rssr.[type] <> 0

	-- Remove multicast address and port from the recorder
	UPDATE Recorders
	SET MulticastSettings.modify('delete properties/settings/setting[name = "ServerAddress" or name = "ServerPort"]')
END

GO



