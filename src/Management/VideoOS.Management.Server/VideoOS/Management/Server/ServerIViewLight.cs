using System;
using VideoOS.Common.SharedComponent.ViewHandlingInterfaces;

namespace VideoOS.Management.Server;

public class ServerIViewLight : IView
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public string DefinitionXml { get; set; }

	public IViewItems ViewItems { get; set; }

	public void AddViewItem(int position, Guid id, string definitionXml)
	{
		throw new NotSupportedException();
	}

	public void Create()
	{
		throw new NotSupportedException();
	}

	public void Delete()
	{
		throw new NotSupportedException();
	}

	public void Update()
	{
		throw new NotSupportedException();
	}
}
