namespace VideoOS.Management.Server;

public interface ITokenStore<T>
{
	T this[string token] { get; set; }

	bool TryGetValue(string token, out T referencedObject);

	bool Remove(string token, bool removeChildren);

	void Clear();
}
