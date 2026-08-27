using System;
using System.IO;
using System.Xml;
using VideoOS.Platform.Login;

namespace VideoOS.Platform.Util.ToolkitXmlProviders;

public class XpcoDbSourceXmlProvider : BaseToolkitXmlProvider
{
	public FQID Fqid { get; set; }

	public override ToolkitProviders ToolkitProvider => ToolkitProviders.MediaServer;

	public override ToolkitTypes ToolkitType => ToolkitTypes.Source;

	protected override void WriteTransformationStart(XmlWriter xw)
	{
		string serverHostname = Fqid.ServerId.ServerHostname;
		if (!Directory.Exists(serverHostname))
		{
			throw new MIPException("Directory not found:" + serverHostname);
		}
		xw.WriteStartElement("table");
		xw.WriteAttributeString("include_archives", "yes");
		xw.WriteElementString("name", string.IsNullOrEmpty(Fqid.ObjectIdString) ? Fqid.ObjectId.ToString() : Fqid.ObjectIdString);
		xw.WriteStartElement("bank");
		xw.WriteElementString("path", serverHostname);
		LoginSettings loginSettings = LoginSettingsCache.GetLoginSettings(Fqid);
		if (loginSettings != null && !string.IsNullOrEmpty(loginSettings.NetworkCredential.Password))
		{
			xw.WriteStartElement("authorization");
			xw.WriteAttributeString("method", "anonymous");
			xw.WriteElementString("password", loginSettings.NetworkCredential.Password);
			xw.WriteEndElement();
		}
		xw.WriteEndElement();
		xw.WriteEndElement();
	}

	protected override void WriteTransformationEnd(XmlWriter xw)
	{
	}

	public override void UpdateConfiguration(Action<string, string> configurationUpdater)
	{
		throw new NotImplementedException();
	}
}
