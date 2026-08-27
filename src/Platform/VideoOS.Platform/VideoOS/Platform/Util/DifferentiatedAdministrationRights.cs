using System.Windows;
using VideoOS.Platform.Admin;

namespace VideoOS.Platform.Util;

public class DifferentiatedAdministrationRights
{
	public virtual bool IsReadAllowed(ItemNode itemNode)
	{
		return true;
	}

	public virtual bool IsCreateAllowed(ItemNode itemNode)
	{
		return true;
	}

	public virtual bool IsDeleteAllowed(ItemNode itemNode)
	{
		return true;
	}

	public virtual bool IsEditAllowed(ItemNode itemNode)
	{
		return true;
	}

	public virtual void ShowNotAllowedDialog()
	{
		MessageBox.Show("You are not allowed to perform this action");
	}
}
