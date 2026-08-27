using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace VideoOS.Common.Controls;

public class WrappingCheckBox : CheckBox
{
	private Hashtable _preferredSizeHash;

	private Size _cachedSizeOfOneLineOfText;

	public WrappingCheckBox()
	{
		_cachedSizeOfOneLineOfText = Size.Empty;
		_preferredSizeHash = new Hashtable(3);
		AutoSize = true;
	}

	private void CacheTextSize()
	{
		_preferredSizeHash.Clear();
		if (string.IsNullOrEmpty(Text))
		{
			_cachedSizeOfOneLineOfText = Size.Empty;
		}
		else
		{
			_cachedSizeOfOneLineOfText = TextRenderer.MeasureText(Text, Font, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.WordBreak);
		}
	}

	public override Size GetPreferredSize(Size proposedSize)
	{
		Size preferredSize = base.GetPreferredSize(proposedSize);
		if (preferredSize.Width <= proposedSize.Width)
		{
			return preferredSize;
		}
		if ((string.IsNullOrEmpty(Text) || proposedSize.Width.Equals(int.MaxValue)) && proposedSize.Height.Equals(int.MaxValue))
		{
			return preferredSize;
		}
		Size size = preferredSize - _cachedSizeOfOneLineOfText;
		Size size2 = proposedSize - size - new Size(3, 0);
		if (!_preferredSizeHash.Contains(size2))
		{
			preferredSize = size - TextRenderer.MeasureText(Text, Font, size2, TextFormatFlags.WordBreak);
			_preferredSizeHash[size2] = preferredSize;
			return preferredSize;
		}
		return (Size)_preferredSizeHash[size2];
	}

	protected override void OnFontChanged(EventArgs e)
	{
		base.OnFontChanged(e);
		CacheTextSize();
	}

	protected override void OnTextChanged(EventArgs e)
	{
		base.OnTextChanged(e);
		CacheTextSize();
	}
}
