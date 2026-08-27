using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class LiveMessageResponse : BaseCommandResponse
{
	public IEnumerable<NotificationData> Notifications { get; private set; }

	public Guid ServerConfigurationId { get; private set; }

	public LiveMessageResponse(Command command)
		: base(command)
	{
		Dictionary<string, string> dictionary = command.GetDictionary(Command.ParamsType.Output);
		ServerConfigurationId = (dictionary.ContainsKey("ConfigurationId") ? ParamsHelper.GuidConvertion(dictionary["ConfigurationId"]) : Guid.Empty);
		Notifications = ((command.Items == null) ? null : (from item in command.Items
			where item.GetItemType() == SerializableItemTypes.Notification
			select SerializableNotificationData.GetNotificationData(item)).ToList());
	}

	public LiveMessageResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}
