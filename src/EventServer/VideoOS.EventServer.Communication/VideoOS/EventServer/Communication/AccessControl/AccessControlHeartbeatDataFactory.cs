using System;
using System.Text;
using System.Xml;
using VideoOS.EventServer.Communication.Configuration;

namespace VideoOS.EventServer.Communication.AccessControl;

public static class AccessControlHeartbeatDataFactory
{
	private const string _accessControlHeartbeatDataType = "AccessControlCredentials";

	public static HeartbeatData CreateAccessControlHeartbeatData(Guid accessControlSystemId, string username, string password)
	{
		HeartbeatData heartbeatData = new HeartbeatData();
		heartbeatData.TypeId = "AccessControlCredentials";
		StringBuilder stringBuilder = new StringBuilder();
		using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder))
		{
			xmlWriter.WriteStartElement("AccessControlCredentials");
			xmlWriter.WriteElementString("SystemId", accessControlSystemId.ToString());
			xmlWriter.WriteElementString("Username", username);
			xmlWriter.WriteElementString("Password", password);
			xmlWriter.WriteEndElement();
			xmlWriter.Flush();
		}
		heartbeatData.Data = stringBuilder.ToString();
		return heartbeatData;
	}
}
