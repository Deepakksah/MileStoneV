using System;
using System.Reflection;
using Autofac.Core.Activators.Reflection;

namespace VideoOS.Platform.SDK.Container;

public class NonPublicConstructorFinder : DefaultConstructorFinder
{
	public NonPublicConstructorFinder()
		: base((Func<Type, ConstructorInfo[]>)((Type type) => type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)))
	{
	}
}
