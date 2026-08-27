using System.Drawing;

namespace VideoOS.Platform.Client;

public class Theme
{
	public virtual ThemeType ThemeType
	{
		get
		{
			return ThemeType.Light;
		}
		set
		{
		}
	}

	public virtual Color BorderColor { get; private set; }

	public virtual Color BackgroundColor { get; private set; }

	public virtual Color GradientStartColor { get; private set; }

	public virtual Color GradientStopColor { get; private set; }

	public virtual Color TitleBarGradientStartColor { get; private set; }

	public virtual Color TitleBarGradientStopColor { get; private set; }

	public virtual Color TitleBarTextColor { get; private set; }

	public virtual Color TextColor { get; private set; }

	public virtual Color ViewItemHeaderColor { get; private set; }

	public virtual Color ViewItemSelectedHeaderColor { get; private set; }

	public virtual Color ViewItemHeaderTextColor { get; private set; }

	public virtual Color ViewItemSelectedHeaderTextColor { get; private set; }

	public virtual Color SeparatorLinesColor { get; private set; }

	public Theme()
	{
		BorderColor = Color.FromArgb(238, 238, 238);
		GradientStartColor = Color.FromArgb(220, 220, 220);
		GradientStopColor = Color.FromArgb(255, 255, 255);
		BackgroundColor = Color.FromArgb(229, 229, 229);
		TitleBarGradientStartColor = Color.FromArgb(255, 255, 255);
		TitleBarGradientStopColor = Color.FromArgb(215, 215, 215);
		TitleBarTextColor = Color.FromArgb(49, 49, 49);
		TextColor = Color.FromArgb(49, 49, 49);
		ViewItemHeaderColor = Color.FromArgb(252, 252, 252);
		ViewItemSelectedHeaderColor = Color.FromArgb(53, 53, 53);
		ViewItemHeaderTextColor = Color.FromArgb(49, 49, 49);
		ViewItemSelectedHeaderTextColor = Color.FromArgb(255, 255, 255);
		SeparatorLinesColor = Color.FromArgb(192, 192, 192);
	}
}
