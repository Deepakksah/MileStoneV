using System;
using VideoOS.Platform.UI.Controls;

namespace VideoOS.Platform.Client;

public abstract class ClientAction
{
	public abstract Guid Id { get; }

	public abstract string Name { get; }

	public virtual VideoOSIconSourceBase Icon { get; }

	public abstract void Init();

	public abstract void Close();

	public abstract void Activated();
}
