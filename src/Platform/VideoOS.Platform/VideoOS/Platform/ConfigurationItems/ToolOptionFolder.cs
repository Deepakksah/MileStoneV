using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ToolOptionFolder : IConfigurationItem
{
	public static Guid RecordingServerToolOptionId = new Guid("F88BEA98-FD49-406B-A504-C2D2EE7E01DE");

	public static Guid ServerLogToolOptionId = new Guid("1B6516E0-D257-4EF6-B642-E50C650DEBBB");

	public static Guid BookmarkToolOptionId = new Guid("CEA9E61B-55E9-4231-BC92-D7266A66B880");

	public static Guid PrivacySettingsToolOptionId = new Guid("A36B973E-EB89-4CD9-8F44-5628C1E7F032");

	public static Guid CustomerDashboardToolOptionId = new Guid("4D2859D2-90D9-4192-AFC3-3C435241AF0E");

	public static Guid MailServerToolOptionId = new Guid("C92B9146-837F-4920-AF0C-2FC3BA847B9E");

	public static Guid NetworkToolOptionId = new Guid("DDC5089D-0837-4E4A-8F9D-14AD4385E2C5");

	public static Guid AviGenerationToolOptionId = new Guid("4EBABFAB-EC77-4888-B391-157896731975");

	public ICollection<IConfigurationItem> ToolOptions
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<IConfigurationItem> list = new List<IConfigurationItem>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				string[] array3 = configurationItem.Path.Split('[', ']');
				Guid guid = new Guid(array3[1]);
				if (guid == RecordingServerToolOptionId)
				{
					list.Add(new ToolOptionRecordingServer(base.ServerId, configurationItem));
				}
				if (guid == ServerLogToolOptionId)
				{
					list.Add(new ToolOptionServerLog(base.ServerId, configurationItem));
				}
				if (guid == BookmarkToolOptionId)
				{
					list.Add(new ToolOptionBookmark(base.ServerId, configurationItem));
				}
				if (guid == PrivacySettingsToolOptionId)
				{
					list.Add(new ToolOptionPrivacySetting(base.ServerId, configurationItem));
				}
				if (guid == CustomerDashboardToolOptionId)
				{
					list.Add(new ToolOptionCustomerDashbord(base.ServerId, configurationItem));
				}
				if (guid == MailServerToolOptionId)
				{
					list.Add(new ToolOptionMailServer(base.ServerId, configurationItem));
				}
				if (guid == NetworkToolOptionId)
				{
					list.Add(new ToolOptionNetwork(base.ServerId, configurationItem));
				}
				if (guid == AviGenerationToolOptionId)
				{
					list.Add(new ToolOptionAviGeneration(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public ToolOptionFolder(ServerId serverId, ConfigurationItem toolOptionFolder)
		: base(serverId, toolOptionFolder)
	{
	}

	public ToolOptionFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public ToolOptionFolder(FQID fqid)
		: base(fqid)
	{
	}

	public ToolOptionFolder()
		: base(EnvironmentManager.Instance.MasterSite.ServerId, "/ToolOptionFolder")
	{
	}
}
