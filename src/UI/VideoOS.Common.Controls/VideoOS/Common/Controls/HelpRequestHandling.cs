using System;

namespace VideoOS.Common.Controls;

public class HelpRequestHandling
{
	private static bool _handledByWpf;

	private DateTime _timestamp;

	private bool _handled;

	private bool _tabSpecificHelp;

	public static bool HandledByWpf
	{
		get
		{
			return _handledByWpf;
		}
		set
		{
			_handledByWpf = value;
		}
	}

	public bool IsHandled
	{
		get
		{
			if (DateTime.Now - _timestamp > TimeSpan.FromSeconds(1.0))
			{
				_handled = false;
			}
			if (!_handled)
			{
				return _handledByWpf;
			}
			return true;
		}
	}

	public bool TabSpecificHelp
	{
		get
		{
			if (_tabSpecificHelp)
			{
				return !_handledByWpf;
			}
			return false;
		}
	}

	public HelpRequestHandling(bool handled, bool tabSpecificHelp)
	{
		_timestamp = DateTime.Now;
		_handled = handled;
		_tabSpecificHelp = tabSpecificHelp;
	}
}
