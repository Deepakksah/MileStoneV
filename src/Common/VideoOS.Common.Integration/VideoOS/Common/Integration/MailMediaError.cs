namespace VideoOS.Common.Integration;

public enum MailMediaError
{
	MailMediaNoConfiguration,
	MailMediaErrorInConfiguration,
	MailMediaErrorCodecDoesNotSupportThisMedia,
	MailMediaErrorNoImagesAvailable,
	MailMediaErrorUnknownError,
	MailMediaErrorNoCodecSelected,
	MailMediaErrorConnectionBroken
}
