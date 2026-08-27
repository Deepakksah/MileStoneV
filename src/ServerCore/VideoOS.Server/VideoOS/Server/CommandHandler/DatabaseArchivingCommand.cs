using System;
using System.Xml.XPath;

namespace VideoOS.Server.CommandHandler;

public sealed class DatabaseArchivingCommand : DatabaseTaskCommandBase
{
	public const string TaskTypeId = "Database.Archiving";

	public Guid SourceStorageId { get; private set; }

	public Guid TargetStorageId { get; private set; }

	public DatabaseArchivingCommand(string taskXml)
		: base(taskXml)
	{
		base.MaxRetryCount = 5;
	}

	protected override void Initialize(XPathNavigator taskNode)
	{
		base.Initialize(taskNode);
		SourceStorageId = ReadTaskGuidNodeValue(taskNode, "idStorageSource");
		TargetStorageId = ReadTaskGuidNodeValue(taskNode, "idStorageTarget");
	}
}
