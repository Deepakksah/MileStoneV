using System.Drawing;
using System.Windows.Forms;

namespace VideoOS.Installer.Common.Utilities;

public class LabelTextAdaptor
{
	private Label _label { get; }

	public LabelTextAdaptor(Label label)
	{
		_label = label;
	}

	public void AdaptingFontSize()
	{
		if (_label != null && _label.Font.Size > 1f)
		{
			Size size = TextRenderer.MeasureText(_label.Text, _label.Font, _label.Size, TextFormatFlags.WordBreak);
			if (size.Width > _label.Size.Width || size.Height > _label.Size.Height)
			{
				_label.Font = new Font(_label.Font.Name, _label.Font.Size - 1f, _label.Font.Style);
				AdaptingFontSize();
			}
		}
	}
}
