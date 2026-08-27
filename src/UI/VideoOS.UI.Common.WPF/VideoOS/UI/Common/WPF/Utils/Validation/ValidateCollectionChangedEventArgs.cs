using System.ComponentModel;

namespace VideoOS.UI.Common.WPF.Utils.Validation;

public class ValidateCollectionChangedEventArgs<T> : CancelEventArgs
{
	private readonly T _newItem;

	private readonly T _oldItem;

	private readonly int _newIndex;

	private readonly int _oldIndex;

	private readonly ValidateCollectionChangedAction _action;

	public T NewItem => _newItem;

	public T OldItem => _oldItem;

	public int NewIndex => _newIndex;

	public int OldIndex => _oldIndex;

	public ValidateCollectionChangedAction Action => _action;

	public ValidateCollectionChangedEventArgs(ValidateCollectionChangedAction action)
	{
		_action = action;
	}

	public ValidateCollectionChangedEventArgs(ValidateCollectionChangedAction action, int index, T item)
	{
		_action = action;
		switch (_action)
		{
		case ValidateCollectionChangedAction.Insert:
			_newIndex = index;
			_newItem = item;
			break;
		case ValidateCollectionChangedAction.Remove:
			_oldIndex = index;
			_oldItem = item;
			break;
		}
	}

	public ValidateCollectionChangedEventArgs(ValidateCollectionChangedAction action, int index, T oldItem, T newItem)
	{
		_action = action;
		_oldIndex = (_newIndex = index);
		_oldItem = oldItem;
		_newItem = newItem;
	}
}
