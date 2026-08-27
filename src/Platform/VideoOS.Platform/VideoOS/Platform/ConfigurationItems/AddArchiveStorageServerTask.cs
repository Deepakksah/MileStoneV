using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class AddArchiveStorageServerTask : ServerTask
{
	public string Name
	{
		get
		{
			return base.Wrapper.GetPropertyValue("Name");
		}
		set
		{
			base.Wrapper.SetPropertyValue("Name", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string Description
	{
		get
		{
			return base.Wrapper.GetPropertyValue("Description");
		}
		set
		{
			base.Wrapper.SetPropertyValue("Description", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string DiskPath
	{
		get
		{
			return base.Wrapper.GetPropertyValue("DiskPath");
		}
		set
		{
			base.Wrapper.SetPropertyValue("DiskPath", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public double TargetFramerate
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("TargetFramerate");
			double result = 0.0;
			if (double.TryParse(propertyValue, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				return result;
			}
			return 0.0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("TargetFramerate", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public bool FramerateReductionEnabled
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("FramerateReductionEnabled");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("FramerateReductionEnabled", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public int RetainMinutes
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("RetainMinutes");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("RetainMinutes", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public int MaxSize
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("MaxSize");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("MaxSize", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public AddArchiveStorageServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("AddArchiveStorage"));
	}
}
