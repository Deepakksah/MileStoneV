using System;

namespace VideoOS.UI.Common.WPF.Utils;

public interface ISelectable
{
	bool IsSelected { get; set; }

	event EventHandler IsSelectedChanged;
}
