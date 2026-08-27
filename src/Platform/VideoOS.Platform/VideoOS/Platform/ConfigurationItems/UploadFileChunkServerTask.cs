using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class UploadFileChunkServerTask : ServerTask
{
	public string TransferId
	{
		get
		{
			return base.Wrapper.GetPropertyValue("TransferId");
		}
		set
		{
			base.Wrapper.SetPropertyValue("TransferId", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string ChunkData
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ChunkData");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ChunkData", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public int Offset
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("Offset");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("Offset", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public int Size
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("Size");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("Size", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public string Checksum
	{
		get
		{
			return base.Wrapper.GetPropertyValue("Checksum");
		}
		set
		{
			base.Wrapper.SetPropertyValue("Checksum", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public UploadFileChunkServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("UploadFileChunk"));
	}
}
