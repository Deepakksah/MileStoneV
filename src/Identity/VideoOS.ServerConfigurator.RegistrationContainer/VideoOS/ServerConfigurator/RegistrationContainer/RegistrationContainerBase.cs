using System;
using System.IO;

namespace VideoOS.ServerConfigurator.RegistrationContainer;

public abstract class RegistrationContainerBase : MarshalByRefObject, IRegistrationContainer
{
	protected object RegistrarObject { get; private set; }

	public virtual void CreateInstance(string assemblyLocation, string assemblyQualifiedName)
	{
		AssemblyQualifiedName assemblyQualifiedName2 = new AssemblyQualifiedName(assemblyQualifiedName);
		string text = Path.Combine(assemblyLocation, assemblyQualifiedName2.AssemblyFileName);
		AssemblyAuthenticator.Authenticate(text);
		RegistrarObject = Activator.CreateInstanceFrom(text, assemblyQualifiedName2.FullTypeName)?.Unwrap();
	}
}
