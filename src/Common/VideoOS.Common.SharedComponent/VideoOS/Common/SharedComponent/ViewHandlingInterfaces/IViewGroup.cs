using System;

namespace VideoOS.Common.SharedComponent.ViewHandlingInterfaces;

public interface IViewGroup
{
	Guid Id { get; }

	string Name { get; set; }

	ICustomProperties CustomProperties { get; }

	IViewGroups ViewGroups { get; }

	IViews Views { get; }

	void Delete();

	void Create();

	void Update();

	ICustomProperty NewCustomProperty(string key, string value);

	IViewGroup NewViewGroup(string name, Guid id);

	IViewGroup NewViewGroup(string name);

	IView NewView(string name, Guid id);
}
