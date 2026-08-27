using System.Xml.XPath;
using VideoOS.Common.Integration;

namespace VideoOS.Server.CommandHandler;

public abstract class SystemCallContextInfoTaskCommandBase : SystemTaskCommandBase
{
	public ICallContextInfo CallContextInfo { get; public set; }

	protected SystemCallContextInfoTaskCommandBase(string taskXml)
		: base(taskXml)
	{
	}

	protected override void Initialize(XPathNavigator taskNode)
	{
		base.Initialize(taskNode);
		XPathNavigator taskNode2 = taskNode.SelectSingleNode("callcontext");
		string locationIpAddress = ReadTaskStringNodeValue(taskNode2, "locationipaddress");
		string userDomainAccountName = ReadTaskStringNodeValue(taskNode2, "userdomainaccountname");
		string userSid = ReadTaskStringNodeValue(taskNode2, "usersid");
		CallContextInfo = new TaskCallContextInfo(locationIpAddress, userDomainAccountName, userSid);
	}
}
