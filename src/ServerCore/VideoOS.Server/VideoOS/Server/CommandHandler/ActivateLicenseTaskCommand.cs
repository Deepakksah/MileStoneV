using System.Xml.XPath;

namespace VideoOS.Server.CommandHandler;

public sealed class ActivateLicenseTaskCommand : SystemTaskCommandBase
{
	public string UserName { get; private set; }

	public string Password { get; private set; }

	public ActivateLicenseTaskCommand(string taskXml)
		: base(taskXml)
	{
	}

	protected override void Initialize(XPathNavigator taskNode)
	{
		base.Initialize(taskNode);
		UserName = ReadTaskStringNodeValue(taskNode, "userName");
		Password = ReadTaskStringNodeValue(taskNode, "password");
	}
}
