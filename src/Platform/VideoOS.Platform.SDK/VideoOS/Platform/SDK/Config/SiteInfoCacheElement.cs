using System;
using System.Xml;

namespace VideoOS.Platform.SDK.Config;

public class SiteInfoCacheElement
{
	public string UserName;

	public bool Basic;

	public Uri ServerUri;

	public bool Unauthorized;

	public XmlDocument XmlDocument;

	public DateTime LoadedDateTime;
}
