using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace VideoOS.Common.Controls;

public class DataGridViewTextAndImageCell : DataGridViewTextBoxCell
{
	private Image imageValue;

	private int imageOffset;

	private bool showAsLink;

	private Color saveColor;

	public bool ShowAsLink
	{
		get
		{
			return showAsLink;
		}
		set
		{
			if (showAsLink != value)
			{
				showAsLink = value;
				if (showAsLink)
				{
					Font font = base.InheritedStyle.Font;
					base.Style.Font = new Font(font, FontStyle.Underline);
					saveColor = base.Style.ForeColor;
					base.Style.ForeColor = SystemColors.HotTrack;
				}
				else
				{
					base.Style.Font = new Font(base.InheritedStyle.Font, FontStyle.Regular);
					base.Style.ForeColor = saveColor;
				}
			}
		}
	}

	public Image Image
	{
		get
		{
			if (imageValue != null || OwningTextAndImageColumn == null)
			{
				return imageValue;
			}
			return OwningTextAndImageColumn.Image;
		}
		set
		{
			if (imageValue != value)
			{
				imageValue = value;
				CalculatePadding();
			}
		}
	}

	public int ImageOffset
	{
		get
		{
			if (imageOffset > 0 || OwningTextAndImageColumn == null)
			{
				return imageOffset;
			}
			return OwningTextAndImageColumn.ImageOffset;
		}
		set
		{
			if (imageOffset != value)
			{
				imageOffset = value;
				CalculatePadding();
			}
		}
	}

	private DataGridViewTextAndImageColumn OwningTextAndImageColumn => base.OwningColumn as DataGridViewTextAndImageColumn;

	public override object Clone()
	{
		DataGridViewTextAndImageCell obj = base.Clone() as DataGridViewTextAndImageCell;
		obj.imageValue = imageValue;
		obj.imageOffset = imageOffset;
		return obj;
	}

	protected override void OnMouseEnter(int rowIndex)
	{
		base.OnMouseEnter(rowIndex);
		if (showAsLink)
		{
			base.DataGridView.Cursor = Cursors.Hand;
		}
	}

	protected override void OnMouseLeave(int rowIndex)
	{
		base.OnMouseLeave(rowIndex);
		if (showAsLink)
		{
			base.DataGridView.Cursor = Cursors.Default;
		}
	}

	private void CalculatePadding()
	{
		Padding padding = base.InheritedStyle.Padding;
		base.Style.Padding = new Padding(Image.Size.Width + ImageOffset, padding.Top, padding.Right, padding.Bottom);
	}

	protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
	{
		base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
		if (Image != null)
		{
			if (graphics == null)
			{
				throw new ArgumentNullException("graphics");
			}
			GraphicsContainer container = graphics.BeginContainer();
			graphics.SetClip(cellBounds);
			graphics.DrawImageUnscaled(Image, cellBounds.Location.X + ImageOffset, cellBounds.Location.Y + cellBounds.Height / 2 - Image.Height / 2);
			graphics.EndContainer(container);
		}
	}
}
