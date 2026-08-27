using System;

namespace VideoOS.Platform.Util.ToolkitXmlProviders;

public static class ToMediaConverter
{
	public static string ToMedia(this Item item)
	{
		return item.FQID.ToMedia();
	}

	public static string ToMedia(this FQID fqid)
	{
		return fqid.Kind.ToMedia();
	}

	public static string ToMedia(this Guid kind)
	{
		if (kind == Kind.Camera)
		{
			return "VIDEO";
		}
		if (kind == Kind.Microphone || kind == Kind.Speaker)
		{
			return "AUDIO";
		}
		if (kind == Kind.Metadata)
		{
			return "METADATA";
		}
		throw new MIPException("Toolkit: Unknown media type");
	}
}
