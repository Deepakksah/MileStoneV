using System.Xml.XPath;

namespace VideoOS.Server.CommandHandler;

public sealed class ChangeIdpAdminTaskCommand : SystemTaskCommandBase
{
	public string UserId { get; private set; }

	public ChangeIdpAdminTaskCommand(string taskXml)
		: base(taskXml)
	{
	}

	protected override void Initialize(XPathNavigator taskNode)
	{
		base.Initialize(taskNode);
		UserId = ReadTaskStringNodeValue(taskNode, "id");
	}
}
