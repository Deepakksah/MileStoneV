using System;
using System.Windows.Forms;

namespace VideoOS.Platform.Admin;

public class TabUserControl : UserControl
{
	public event EventHandler ConfigurationChangedByUser;

	public virtual void Init()
	{
	}

	public virtual void Close()
	{
	}

	public void FireConfigurationChanged()
	{
		if (this.ConfigurationChangedByUser != null)
		{
			this.ConfigurationChangedByUser(this, new EventArgs());
		}
	}

	public virtual bool ValidateAndSave()
	{
		return true;
	}
}
