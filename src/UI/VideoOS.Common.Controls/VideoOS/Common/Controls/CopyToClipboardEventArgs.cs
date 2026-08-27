using System;

namespace VideoOS.Common.Controls;

public sealed class CopyToClipboardEventArgs : EventArgs
{
	private string _clipboardText;

	private bool _eventHandled;

	public string ClipboardText => _clipboardText;

	public bool EventHandled
	{
		get
		{
			return _eventHandled;
		}
		set
		{
			_eventHandled = value;
		}
	}

	public CopyToClipboardEventArgs()
	{
		_clipboardText = string.Empty;
		_eventHandled = false;
	}

	public CopyToClipboardEventArgs(string clipboardText)
	{
		_clipboardText = clipboardText;
		_eventHandled = false;
	}
}
