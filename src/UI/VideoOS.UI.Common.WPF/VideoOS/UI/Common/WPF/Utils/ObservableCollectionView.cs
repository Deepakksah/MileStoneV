using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace VideoOS.UI.Common.WPF.Utils;

public class ObservableCollectionView : ListCollectionView
{
	public ObservableCollectionView(IList list)
		: base(list)
	{
		if (list == null)
		{
			return;
		}
		foreach (object item in list)
		{
			(item as INotifyPropertyChanged).PropertyChanged += item_PropertyChanged;
		}
	}

	protected override void ProcessCollectionChanged(NotifyCollectionChangedEventArgs args)
	{
		base.ProcessCollectionChanged(args);
		IList list = new object[0];
		IList list2 = new object[0];
		if (args.Action == NotifyCollectionChangedAction.Reset)
		{
			list2 = this.Cast<object>().ToList();
		}
		else
		{
			if (args.NewItems != null)
			{
				list = args.NewItems;
			}
			if (args.OldItems != null)
			{
				list2 = args.OldItems;
			}
		}
		foreach (object item in list)
		{
			(item as INotifyPropertyChanged).PropertyChanged += item_PropertyChanged;
		}
		foreach (object item2 in list2)
		{
			(item2 as INotifyPropertyChanged).PropertyChanged -= item_PropertyChanged;
		}
	}

	private void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		base.ProcessCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, sender, sender));
	}
}
