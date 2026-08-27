using System.Xml.XPath;

namespace VideoOS.Server.CommandHandler;

public sealed class DatabaseRepairingCommand : DatabaseTaskCommandBase
{
	public const string TaskTypeId = "Database.Repairing2";

	public DatabaseRepairingCommand(string taskXml)
		: base(taskXml)
	{
		base.MaxRetryCount = 5;
	}

	protected override void Initialize(XPathNavigator taskNode)
	{
		base.Initialize(taskNode);
	}
}
