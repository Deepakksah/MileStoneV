using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ToolOptionBookmark : IConfigurationItem
{
	public string BookmarkIdPrefix
	{
		get
		{
			return base.Wrapper.GetPropertyValue("BookmarkIdPrefix");
		}
		set
		{
			base.Wrapper.SetPropertyValue("BookmarkIdPrefix", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public int BookmarkPreTime
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("BookmarkPreTime");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("BookmarkPreTime", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public int BookmarkPostTime
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("BookmarkPostTime");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("BookmarkPostTime", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public ToolOptionBookmark(ServerId serverId, ConfigurationItem toolOptionBookmark)
		: base(serverId, toolOptionBookmark)
	{
	}

	public ToolOptionBookmark(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public ToolOptionBookmark(FQID fqid)
		: base(fqid)
	{
	}
}
