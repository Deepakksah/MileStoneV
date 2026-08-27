using System;
using System.Drawing;
using System.Windows.Forms;

namespace VideoOS.Common.Controls;

public class ToolStripRenderer : ToolStripSystemRenderer
{
	protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
	{
		if (e == null)
		{
			throw new ArgumentNullException("e");
		}
		e.Graphics.Clear(SystemColors.Control);
	}

	protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
	{
	}
}
