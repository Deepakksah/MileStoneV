using System;
using System.IO;
using System.Text;
using System.Xml;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("AxisOneClickCamera")]
public class AxisOneClickCamera : HostedHardware, ICreatable, IUpdatable, IDeletable
{
	public string OwnerAuthenticationKey
	{
		get
		{
			string result = "";
			string hostingInfo = base.HostingInfo;
			XmlDocument xmlDocument = new XmlDocument();
			try
			{
				using StringReader txtReader = new StringReader(hostingInfo);
				xmlDocument.Load(txtReader);
			}
			catch (XmlException)
			{
			}
			foreach (XmlNode attribute in xmlDocument.SelectSingleNode("/Data").Attributes)
			{
				if (string.Equals(attribute.Name, "OAK", StringComparison.OrdinalIgnoreCase))
				{
					result = attribute.Value;
				}
			}
			return result;
		}
		set
		{
			StringBuilder stringBuilder = new StringBuilder();
			XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
			xmlWriterSettings.OmitXmlDeclaration = true;
			using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, xmlWriterSettings))
			{
				xmlWriter.WriteStartElement("Data");
				xmlWriter.WriteAttributeString("OAK", value);
				xmlWriter.WriteEndElement();
				xmlWriter.Flush();
			}
			base.HostingInfo = stringBuilder.ToString();
		}
	}

	public AxisTunnelServer AxisTunnelServer
	{
		get
		{
			string propertyValue = GetPropertyValue<string>("HostingServerId");
			return GetObjectById<AxisTunnelServer>(propertyValue);
		}
	}

	public AxisOneClickCamera(AxisTunnelServer axisTunnelServer)
		: this(axisTunnelServer.Repositories, axisTunnelServer.Id)
	{
	}

	public AxisOneClickCamera(IVmoRepositories repositories, int hostingServerId)
		: this(string.Empty, new VmoPropertyCollection(), new VmoRelationCollection(), repositories, isInitialized: true)
	{
		base.Name = string.Empty;
		base.Description = string.Empty;
		base.HostingInfo = string.Empty;
		base.ManufacturerId = string.Empty;
		base.UserName = string.Empty;
		base.Password = string.Empty;
		base.CustomData = string.Empty;
		base.HostingServerId = hostingServerId;
	}

	public AxisOneClickCamera(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
	}

	public void Refresh()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.RefreshObject(base.InternalId);
	}

	public void Create()
	{
		base.Repository.CreateObject(this);
	}

	public void Update()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.UpdateObject(this);
	}

	public void Delete()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.DeleteObject(this);
	}
}
