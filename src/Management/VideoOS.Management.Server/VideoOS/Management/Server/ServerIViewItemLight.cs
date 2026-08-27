using System;
using VideoOS.Common.SharedComponent.ViewHandlingInterfaces;

namespace VideoOS.Management.Server;

public class ServerIViewItemLight : IViewItem
{
	public Guid Id { get; set; }

	public string DefinitionXml { get; set; }

	public int Position { get; set; }

	public void ChangeId(Guid id)
	{
		throw new NotSupportedException();
	}

	public void Update()
	{
		throw new NotSupportedException();
	}
}
