using System;
using VideoOS.Mobile.Utilities;
using VideoOS.Platform.Messaging;

namespace VideoOS.Mobile.LoginThread;

public class VmsConfigChangeChecker : VmsChangeChecker
{
	public static VmsConfigChangeChecker Instance { get; } = new VmsConfigChangeChecker();

	public IItemManagerAddRemove<object, EventHandler> VmsConfigurationChanged => GetHandler(NotificationType.Communicator, MessageId.System.SystemConfigurationChangedIndication);

	public IItemManagerAddRemove<object, EventHandler> MsConfigurationChanged => GetHandler(NotificationType.Receiver, "Server.ConfigurationChangedIndication");

	public IItemManagerAddRemove<object, EventHandler> VmsLicenseChanged => GetHandler(NotificationType.Receiver, MessageId.System.LicenseChangedIndication);

	private VmsConfigChangeChecker()
	{
	}
}
