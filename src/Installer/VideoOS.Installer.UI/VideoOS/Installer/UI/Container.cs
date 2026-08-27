namespace VideoOS.Installer.UI;

public class Container<T>
{
	public T Value { get; }

	public Container(T containedValue)
	{
		Value = containedValue;
	}
}
