using System;
using System.Collections.Generic;
using System.Drawing;

namespace VideoOS.UI.Common.WPF.FilterTree;

public interface IFilterTreeItemContent
{
	IFilterTreeItemContent TopLevelParent { get; }

	IFilterTreeItemContent Parent { get; }

	IEnumerable<IFilterTreeItemContent> Children { get; }

	Guid Id { get; }

	string Name { get; }

	Image Icon { get; }

	bool IsCustomItem { get; }
}
