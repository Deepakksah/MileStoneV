using System;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public class CommandTrigger
{
	public event EventHandler CommandTriggered = delegate
	{
	};

	public void TriggerCommand()
	{
		OnCommandTriggered();
	}

	public void OnCommandTriggered()
	{
		this.CommandTriggered(this, EventArgs.Empty);
	}
}
