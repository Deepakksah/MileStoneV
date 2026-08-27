using System;

namespace VideoOS.Mobile.UI.Controls.TreeView;

public interface ITreeViewModelItem
{
	string DisplayName { get; }

	Guid Id { get; }
}
