using VideoOS.Mobile.Portable.Common;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public enum ServerEndpointType
{
	[Description("AutomaticUrl")]
	AutomaticUrl,
	[Description("PredefinedUrl")]
	PredefinedUrl,
	[Description("EasyConnect")]
	EasyConnect
}
