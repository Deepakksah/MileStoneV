using System;
using System.Collections.Generic;

namespace VideoOS.Installer.Interfaces.ModelContainer;

public abstract class ModelContainer<T> : IModelContainer
{
	public T Content { get; set; }

	public Type[] Interfaces { get; }

	object IModelContainer.Content => Content;

	IEnumerable<Type> IModelContainer.Interfaces => Interfaces;

	protected ModelContainer(T content, params Type[] interfaces)
	{
		Content = content;
		Interfaces = interfaces;
	}

	protected abstract void Persist();

	void IModelContainer.Persist()
	{
		Persist();
	}
}
