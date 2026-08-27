using System;
using System.Collections.Generic;

namespace VideoOS.Installer.Interfaces.ModelContainer;

public interface IModelContainer
{
	object Content { get; }

	IEnumerable<Type> Interfaces { get; }

	void Persist();
}
