using System.Collections;
using System.Runtime;

namespace VideoOS.UI.Common.WPF.Utils;

public interface IObservableCollectionEx : IList, ICollection, IEnumerable
{
	[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
	void Move(int oldIndex, int newIndex);

	void Sort();
}
