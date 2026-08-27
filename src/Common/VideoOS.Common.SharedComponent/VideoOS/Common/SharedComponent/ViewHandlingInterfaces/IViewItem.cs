using System;

namespace VideoOS.Common.SharedComponent.ViewHandlingInterfaces;

public interface IViewItem
{
	Guid Id { get; }

	int Position { get; set; }

	string DefinitionXml { get; set; }

	void ChangeId(Guid id);

	void Update();
}
