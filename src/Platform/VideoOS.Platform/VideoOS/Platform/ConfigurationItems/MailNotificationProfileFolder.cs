using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class MailNotificationProfileFolder : IConfigurationItem
{
	public ICollection<MailNotificationProfile> MailNotificationProfiles
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<MailNotificationProfile> list = new List<MailNotificationProfile>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "MailNotificationProfile")
				{
					list.Add(new MailNotificationProfile(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public MailNotificationProfileFolder(ServerId serverId, ConfigurationItem mailNotificationProfileFolder)
		: base(serverId, mailNotificationProfileFolder)
	{
	}

	public MailNotificationProfileFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public MailNotificationProfileFolder(FQID fqid)
		: base(fqid)
	{
	}

	public MailNotificationProfileFolder()
		: base(EnvironmentManager.Instance.MasterSite.ServerId, "/MailNotificationProfileFolder")
	{
	}

	public ServerTask AddMailNotificationProfile(string name, string description, string subject, string messageText, Collection<string> recipients, int timeBetweenMails, bool includeImages, int numberOfImages, bool embedImages, int timeBetweenImages, bool includeAVI, int timeBeforeEvent, int timeAfterEvent, int frameRate)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddMailNotificationProfile");
			configurationApiItemWrapper.SetProperty("Name", name);
			configurationApiItemWrapper.SetProperty("Description", description);
			configurationApiItemWrapper.SetProperty("Subject", subject);
			configurationApiItemWrapper.SetProperty("MessageText", messageText);
			configurationApiItemWrapper.SetProperty("Recipients", recipients);
			configurationApiItemWrapper.SetProperty("TimeBetweenMails", timeBetweenMails.ToString(CultureInfo.InvariantCulture));
			configurationApiItemWrapper.SetProperty("IncludeImages", includeImages.ToString());
			configurationApiItemWrapper.SetProperty("NumberOfImages", numberOfImages.ToString(CultureInfo.InvariantCulture));
			configurationApiItemWrapper.SetProperty("EmbedImages", embedImages.ToString());
			configurationApiItemWrapper.SetProperty("TimeBetweenImages", timeBetweenImages.ToString(CultureInfo.InvariantCulture));
			configurationApiItemWrapper.SetProperty("IncludeAVI", includeAVI.ToString());
			configurationApiItemWrapper.SetProperty("TimeBeforeEvent", timeBeforeEvent.ToString(CultureInfo.InvariantCulture));
			configurationApiItemWrapper.SetProperty("TimeAfterEvent", timeAfterEvent.ToString(CultureInfo.InvariantCulture));
			configurationApiItemWrapper.SetProperty("FrameRate", frameRate.ToString(CultureInfo.InvariantCulture));
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddMailNotificationProfile"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "MailNotificationProfileFolder", "Unable to call method 'AddMailNotificationProfile':" + ex.Message);
		}
		return null;
	}

	public AddMailNotificationProfileServerTask AddMailNotificationProfile()
	{
		try
		{
			return new AddMailNotificationProfileServerTask(base.Wrapper.ExecuteMethod("AddMailNotificationProfile"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "MailNotificationProfileFolder", "Unable to call method 'AddMailNotificationProfile':" + ex.Message);
		}
		return null;
	}

	public ServerTask RemoveMailNotificationProfile(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemoveMailNotificationProfile");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemoveMailNotificationProfile"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "MailNotificationProfileFolder", "Unable to call method 'RemoveMailNotificationProfile':" + ex.Message);
		}
		return null;
	}

	public RemoveMailNotificationProfileServerTask RemoveMailNotificationProfile()
	{
		try
		{
			return new RemoveMailNotificationProfileServerTask(base.Wrapper.ExecuteMethod("RemoveMailNotificationProfile"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "MailNotificationProfileFolder", "Unable to call method 'RemoveMailNotificationProfile':" + ex.Message);
		}
		return null;
	}
}
