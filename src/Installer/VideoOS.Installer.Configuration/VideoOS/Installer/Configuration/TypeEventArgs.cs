using System;

namespace VideoOS.Installer.Configuration;

public class TypeEventArgs : EventArgs
{
	public Type InterfaceType { get; }

	public TypeEventArgs(Type interfaceType)
	{
		InterfaceType = interfaceType;
	}
}
