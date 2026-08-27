using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class CompressorFolder : IConfigurationItem
{
	public ICollection<Compressor> Compressors
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<Compressor> list = new List<Compressor>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "Compressor")
				{
					list.Add(new Compressor(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public CompressorFolder(ServerId serverId, ConfigurationItem compressorFolder)
		: base(serverId, compressorFolder)
	{
	}

	public CompressorFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public CompressorFolder(FQID fqid)
		: base(fqid)
	{
	}

	public CompressorFolder()
		: base(EnvironmentManager.Instance.MasterSite.ServerId, "/CompressorFolder")
	{
	}
}
