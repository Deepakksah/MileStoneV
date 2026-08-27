using System;

namespace VideoOS.Installer.UI;

public class TreckChangedEventArgs : EventArgs
{
	private string _treckKey;

	public string TreckKey => _treckKey;

	public TreckChangedEventArgs(string treckKey)
	{
		_treckKey = treckKey;
	}
}
