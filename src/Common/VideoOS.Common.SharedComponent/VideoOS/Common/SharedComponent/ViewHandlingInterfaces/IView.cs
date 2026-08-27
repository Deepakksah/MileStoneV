using System;

namespace VideoOS.Common.SharedComponent.ViewHandlingInterfaces;

public interface IView
{
	Guid Id { get; }

	string Name { get; set; }

	string DefinitionXml { get; set; }

	IViewItems ViewItems { get; }

	void AddViewItem(int position, Guid id, string definitionXml);

	void Delete();

	void Create();

	void Update();
}
