using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using VideoOS.Platform.UI.Controls;
using VideoOS.Platform.Util.Images;

namespace VideoOS.Platform.Search;

public abstract class SearchFilterCategory
{
	private VideoOSIconSourceBase _iconSource;

	public Guid SearchAgentPluginId { get; public set; }

	public string Name { get; }

	[Obsolete("This property is deprecated. Windows Forms controls are no longer supported. Use IconSource instead.", false)]
	public Icon Icon { get; }

	public VideoOSIconSourceBase IconSource
	{
		get
		{
			return _iconSource ?? VideoOS.Platform.Util.Images.ImageConverter.ConvertIconToIconSourceBase(Icon);
		}
		private set
		{
			_iconSource = value;
		}
	}

	public IEnumerable<SearchFilter> Filters { get; }

	[Obsolete("This constructor is deprecated. Windows Forms controls are no longer supported. Use SearchFilterCategory(string, VideoOSIconSourceBase, IEnumerable<SearchFilter>) instead.", false)]
	protected SearchFilterCategory(string name, Icon icon, IEnumerable<SearchFilter> filters)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			throw new ArgumentException("Cannot be null or empty", "name");
		}
		Name = name;
		Icon = icon;
		Filters = filters ?? Enumerable.Empty<SearchFilter>();
	}

	protected SearchFilterCategory(string name, VideoOSIconSourceBase iconSource, IEnumerable<SearchFilter> filters)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			throw new ArgumentException("Cannot be null or empty", "name");
		}
		Name = name;
		IconSource = iconSource;
		Filters = filters ?? Enumerable.Empty<SearchFilter>();
	}
}
