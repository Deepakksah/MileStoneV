-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_compressor]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_compressor]
GO

CREATE PROCEDURE [update_compressor]
		@handler [int],
		@idCodec [GuidType],
		@flags [int],
		@quality [int],
		@keyFrames [int],
		@dataRate [int],
		@configData VARBINARY(255)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @lastModified DATETIME

	SELECT @lastModified = [LastModified]
		FROM [Compressors]
		WHERE @handler = [Handler]

	if (@lastModified IS NULL)
	BEGIN
		INSERT [Compressors] (
			[LastModified],
			[Handler],
			[IDCodec],
			[Flags],
			[Quality],
			[KeyFrames],
			[DataRate],
			[ConfigData]
			)
		VALUES (
			GETUTCDATE(),
			@handler,
			@idCodec,
			@flags,
			@quality,
			@keyFrames,
			@dataRate,
			@configData
			)
	END
	ELSE
	BEGIN
		UPDATE [Compressors] 
		SET
			[LastModified] = GETUTCDATE(),
			[IDCodec] = @idCodec,
			[Flags] = @flags,
			[Quality] = @quality,
			[KeyFrames] = @keyFrames,
			[DataRate] = @dataRate,
			[ConfigData] = @configData
		WHERE @handler = [Handler]
	END
END
GO
