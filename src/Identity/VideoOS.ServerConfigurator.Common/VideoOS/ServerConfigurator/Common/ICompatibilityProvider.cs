namespace VideoOS.ServerConfigurator.Common;

public interface ICompatibilityProvider
{
	T GetCompatibleType<T>(object value);
}
