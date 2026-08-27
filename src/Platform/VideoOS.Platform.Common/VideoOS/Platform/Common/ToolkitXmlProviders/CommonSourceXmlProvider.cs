using System;
using VideoOS.Platform.Util.ToolkitXmlProviders;

namespace VideoOS.Platform.Common.ToolkitXmlProviders;

public class CommonSourceXmlProvider : ProxyXmlProvider
{
	public CommonSourceXmlProvider(FQID fqid, Guid streamId)
		: this(GetRelevantProvider(fqid, streamId))
	{
	}

	private CommonSourceXmlProvider(BaseToolkitXmlProvider inner)
		: base(inner)
	{
	}

	public static BaseToolkitXmlProvider GetRelevantProvider(FQID fqid, Guid streamId)
	{
		if (fqid.ServerId.ServerType == "BLK")
		{
			return new XpcoDbSourceXmlProvider
			{
				Fqid = fqid
			};
		}
		return new IsSourceXmlProvider
		{
			Fqid = fqid,
			StreamId = streamId
		};
	}
}
