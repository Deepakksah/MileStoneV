using System;

namespace VideoOS.Common.SharedComponent.Data.ViewLayout;

[Serializable]
public class ViewLayoutGroup
{
	private Guid _id;

	private string _name;

	public Guid Id
	{
		get
		{
			return _id;
		}
		set
		{
			_id = value;
		}
	}

	public string Name
	{
		get
		{
			return _name;
		}
		set
		{
			_name = value;
		}
	}
}
