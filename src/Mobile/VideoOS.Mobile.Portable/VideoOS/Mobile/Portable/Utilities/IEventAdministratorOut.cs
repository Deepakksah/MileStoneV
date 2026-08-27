using System.Collections.Generic;

namespace VideoOS.Mobile.Portable.Utilities;

public interface IEventAdministratorOut<TKey, TEvent, TClient>
{
	TClient this[TKey key] { get; set; }

	List<TKey> Keys { get; }

	void AddClient(ref TKey tKey, ClientEvent<TKey, TEvent> OnEvent, TClient tClient);

	TClient RemoveClient(TKey tKey);
}
