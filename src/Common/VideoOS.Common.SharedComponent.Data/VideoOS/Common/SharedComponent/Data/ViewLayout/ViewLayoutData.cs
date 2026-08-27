using System.Drawing;

namespace VideoOS.Common.SharedComponent.Data.ViewLayout;

public class ViewLayoutData
{
	private Image _icon;

	private string _name = "";

	private ViewLayout _layout;

	private ViewLayoutGroup _group;

	public Image Icon => _icon;

	public string Name => _name;

	public ViewLayout Layout => _layout;

	public ViewLayoutGroup Group => _group;

	public ViewLayoutData(Image icon, string name, ViewLayout layout, ViewLayoutGroup group)
	{
		_icon = icon;
		_name = name;
		_layout = layout;
		_group = group;
	}
}
