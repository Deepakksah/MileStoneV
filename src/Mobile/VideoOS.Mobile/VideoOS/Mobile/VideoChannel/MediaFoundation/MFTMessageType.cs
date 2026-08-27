namespace VideoOS.Mobile.VideoChannel.MediaFoundation;

public enum MFTMessageType
{
	CommandDrain = 1,
	CommandFlush = 0,
	NotifyBeginStreaming = 268435456,
	NotifyEndOfStream = 268435458,
	NotifyEndStreaming = 268435457,
	NotifyStartOfStream = 268435459,
	SetD3DManager = 2
}
