namespace VideoOS.ServerConfigurator.RegistrationContainer;

public interface IRegistrationContainer
{
	void CreateInstance(string assemblyLocation, string assemblyQualifiedName);
}
