using System.Xml.XPath;

namespace VideoOS.Server.CommandHandler;

public sealed class DeleteIdpUserTaskCommand : SystemTaskCommandBase
{
	public string UserId { get; private set; }

	public DeleteIdpUserTaskCommand(string taskXml)
		: base(taskXml)
	{
	}

	protected override void Initialize(XPathNavigator taskNode)
	{
		base.Initialize(taskNode);
		UserId = ReadTaskStringNodeValue(taskNode, "id");
	}
}
