using System;
using VideoOS.Mobile.Management;
using VideoOS.Mobile.Portable.Version;
using VideoOS.Mobile.SDK.Portable.Server.Base;

namespace VideoOS.Mobile.LoginThread;

public class MobileServerLoginConfigSpecific : ILoginConfigSpecific
{
	public Guid ServerTypeId => VideoOS.Mobile.SDK.Portable.Server.Base.Constants.MobileServiceType;

	public Guid ServerPluginId => Definitions.MobilePluginId;

	public Guid ServerItemType => Definitions.MobileKind;

	public string CommunicationAlias => "XProtectMobile/Communication";

	public string ServerVersion => VideoOS.Mobile.Portable.Version.Constants.ServerVersion;

	public string ServerDescription => "Mobile Server Service";
}
