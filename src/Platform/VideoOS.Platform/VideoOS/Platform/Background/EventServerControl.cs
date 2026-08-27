using VideoOS.Platform.Data;

namespace VideoOS.Platform.Background;

public class EventServerControl
{
	private static EventServerControl _instance;

	public static EventServerControl Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new EventServerControl();
			}
			return _instance;
		}
		set
		{
			_instance = value;
		}
	}

	public virtual void CreateAlarm(EventData eventData)
	{
	}

	public virtual void ItemStatusChanged(Item item)
	{
	}

	public virtual void ItemContextMenuChanged(Item item)
	{
	}
}
