using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace System.Collections.ObjectModel;

public static class ReadOnlyCollectionExtensions
{
	private class ObservableCollectionClosure<T> : IWeakEventListener where T : class
	{
		private readonly ObservableCollection<T> _target;

		private readonly ReadOnlyCollection<T> _source;

		private readonly Func<T, bool> _predicate;

		private readonly bool[] _visibility;

		public ObservableCollection<T> Target => _target;

		public ObservableCollectionClosure(ReadOnlyCollection<T> source, Func<T, bool> predicate)
		{
			_source = source;
			_predicate = predicate;
			_visibility = source.Select(predicate).ToArray();
			_target = new ObservableCollection<T>(source.Where((T item, int idx) => _visibility[idx]));
			_target.CollectionChanged += _target_CollectionChanged;
			_source.OfType<INotifyPropertyChanged>().ForEach(delegate(INotifyPropertyChanged item)
			{
				PropertyChangedEventManager.AddListener(item, this, string.Empty);
			});
		}

		public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			if (sender is T val)
			{
				int num = _source.IndexOf(val);
				bool flag = _predicate(val);
				if (flag != _visibility[num])
				{
					_visibility[num] = flag;
					if (flag)
					{
						num--;
						while (num >= 0 && !_visibility[num])
						{
							num--;
						}
						if (num >= 0)
						{
							T item = _source[num];
							int index = _target.IndexOf(item) + 1;
							_target.Insert(index, val);
						}
						else
						{
							_target.Insert(0, val);
						}
					}
					else
					{
						_target.Remove(val);
					}
				}
			}
			return true;
		}

		private void _target_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
		}
	}

	public static ObservableCollection<T> ToObservableCollection<T>(this ReadOnlyCollection<T> @this, Func<T, bool> predicate) where T : class
	{
		return new ObservableCollectionClosure<T>(@this, predicate).Target;
	}
}
