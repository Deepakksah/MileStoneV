using System.Collections;

namespace VideoOS.Installer.UI.Framework;

public class PageCollection : CollectionBase
{
	private Wizard _parent;

	public Wizard Parent => _parent;

	public WizardPage this[int index]
	{
		get
		{
			return (WizardPage)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public PageCollection(Wizard parent)
	{
		_parent = parent;
	}

	public int Add(WizardPage value)
	{
		return base.List.Add(value);
	}

	public void AddRange(WizardPage[] pages)
	{
		foreach (WizardPage value in pages)
		{
			Add(value);
		}
	}

	public int IndexOf(WizardPage value)
	{
		return base.List.IndexOf(value);
	}

	public void Insert(int index, WizardPage value)
	{
		base.List.Insert(index, value);
	}

	public void Remove(WizardPage value)
	{
		base.List.Remove(value);
	}

	public bool Contains(WizardPage value)
	{
		return base.List.Contains(value);
	}

	protected override void OnInsertComplete(int index, object value)
	{
		base.OnInsertComplete(index, value);
	}

	protected override void OnRemoveComplete(int index, object value)
	{
		base.OnRemoveComplete(index, value);
		if (_parent.PageIndex == index)
		{
			if (index < base.InnerList.Count)
			{
				_parent.PageIndex = index;
			}
			else
			{
				_parent.PageIndex = base.InnerList.Count - 1;
			}
		}
	}
}
