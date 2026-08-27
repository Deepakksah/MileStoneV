using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class Compressor : IConfigurationItem
{
	public DateTime LastModified
	{
		get
		{
			if (DateTime.TryParse(base.Wrapper.GetPropertyValue("LastModified"), out var result))
			{
				return result;
			}
			return DateTime.MinValue;
		}
	}

	public int AviGenerationHandler
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("AviGenerationHandler");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
	}

	public int AviGenerationFlags
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("AviGenerationFlags");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
	}

	public int AviGenerationQuality
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("AviGenerationQuality");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("AviGenerationQuality", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public int AviGenerationKeyFrames
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("AviGenerationKeyFrames");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("AviGenerationKeyFrames", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public int AviGenerationDataRate
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("AviGenerationDataRate");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("AviGenerationDataRate", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public string AviGenerationConfigData => base.Wrapper.GetPropertyValue("AviGenerationConfigData");

	public bool AviGenerationSelected
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("AviGenerationSelected");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("AviGenerationSelected", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Compressor(ServerId serverId, ConfigurationItem compressor)
		: base(serverId, compressor)
	{
	}

	public Compressor(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public Compressor(FQID fqid)
		: base(fqid)
	{
	}
}
