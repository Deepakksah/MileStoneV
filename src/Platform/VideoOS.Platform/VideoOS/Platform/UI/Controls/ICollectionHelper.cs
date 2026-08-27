using System.Collections;
using System.Collections.Specialized;

namespace VideoOS.Platform.UI.Controls;

public interface ICollectionHelper
{
	void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e);

	void RaiseCollectionChanged(ICollectionChangedInfo info);

	ICollectionChangedInfo CreateCollectionChangedInfo(CollectionChange change, IEnumerable removedItems, IEnumerable addedItems);
}
