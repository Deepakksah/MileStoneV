using System;
using System.Globalization;
using System.Xml;
using VideoOS.Platform.Login;

namespace VideoOS.Platform.Util.ToolkitXmlProviders;

public class IsSourceXmlProvider : BaseToolkitXmlProvider
{
	public FQID Fqid { get; set; }

	public Guid StreamId { get; set; }

	public override ToolkitProviders ToolkitProvider => ToolkitProviders.IsSource;

	public override ToolkitTypes ToolkitType => ToolkitTypes.Source;

	protected override void WriteTransformationStart(XmlWriter xw)
	{
		LoginSettings loginSettings = LoginSettingsCache.GetLoginSettings(Fqid);
		if (loginSettings == null)
		{
			throw new CommunicationMIPException("LoginSettings not found");
		}
		UriBuilder uriBuilder = new UriBuilder(Fqid.ServerId.ServerScheme, Fqid.ServerId.ServerHostname, Fqid.ServerId.Serverport);
		xw.WriteElementString("media_type", Fqid.ToMedia());
		xw.WriteElementString("server_uri", uriBuilder.ToString());
		xw.WriteStartElement("device_id");
		if (StreamId != Guid.Empty)
		{
			xw.WriteAttributeString("stream_id", StreamId.ToString());
		}
		xw.WriteString(Fqid.ObjectId.ToString().ToUpper(CultureInfo.InvariantCulture));
		xw.WriteEndElement();
		xw.WriteStartElement("authorization");
		xw.WriteAttributeString("method", "token");
		xw.WriteStartElement("token");
		xw.WriteAttributeString("update_key", "token_key");
		xw.WriteString(loginSettings.Token);
		xw.WriteEndElement();
		xw.WriteEndElement();
		xw.WriteElementString("maximum_queue_size", "30");
	}

	protected override void WriteTransformationEnd(XmlWriter xw)
	{
	}

	public override void UpdateConfiguration(Action<string, string> configurationUpdater)
	{
		throw new NotImplementedException();
	}
}
