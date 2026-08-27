namespace VideoOS.Toolkit;

public interface IDictionaryToolkit : IToolkit
{
	void Add(string name, string configXml);

	void Remove(string name);
}
