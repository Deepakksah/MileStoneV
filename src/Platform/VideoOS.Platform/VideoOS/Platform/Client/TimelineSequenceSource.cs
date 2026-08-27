using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.Client;

public abstract class TimelineSequenceSource
{
	public enum TimelineSequenceSourceType
	{
		Ribbon,
		Marker
	}

	public abstract Guid Id { get; }

	public abstract string Title { get; }

	public abstract TimelineSequenceSourceType SourceType { get; }

	[Obsolete("This property is deprecated. Windows Forms controls are no longer supported. Use RibbonContentColorBrush instead.", false)]
	public virtual System.Drawing.Color RibbonContentColor => System.Drawing.Color.SkyBlue;

	public virtual System.Windows.Media.Brush RibbonContentColorBrush
	{
		get
		{
			_ = RibbonContentColor;
			if (RibbonContentColor != System.Drawing.Color.SkyBlue)
			{
				SolidColorBrush solidColorBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(RibbonContentColor.A, RibbonContentColor.R, RibbonContentColor.G, RibbonContentColor.B));
				solidColorBrush.Freeze();
				return solidColorBrush;
			}
			return System.Windows.Media.Brushes.SkyBlue;
		}
	}

	[Obsolete("This property is deprecated. Windows Forms controls are no longer supported. Use MarkerIconSource instead.", false)]
	public virtual System.Drawing.Image MarkerIcon => null;

	public virtual System.Windows.Media.Imaging.BitmapSource MarkerIconSource
	{
		get
		{
			if (MarkerIcon != null)
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					MarkerIcon.Save(memoryStream, ImageFormat.Png);
					memoryStream.Position = 0L;
					BitmapImage bitmapImage = new BitmapImage();
					bitmapImage.BeginInit();
					bitmapImage.StreamSource = memoryStream;
					bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
					bitmapImage.EndInit();
					bitmapImage.Freeze();
					return bitmapImage;
				}
			}
			return null;
		}
	}

	public event EventHandler<TimelineSequencesEventArgs> SequencesRetrieved;

	public abstract void StartGetSequences(IEnumerable<TimeInterval> intervals);

	[Obsolete("This method is deprecated. Windows Forms controls are no longer supported. Use GetPreviewWpfUserControl(object) instead.", false)]
	public virtual System.Windows.Forms.UserControl GetPreviewUserControl(object dataId)
	{
		return null;
	}

	public virtual System.Windows.Controls.UserControl GetPreviewWpfUserControl(object dataId)
	{
		return null;
	}

	protected void OnSequencesRetrieved(IEnumerable<TimelineSourceQueryResult> results)
	{
		this.SequencesRetrieved?.Invoke(this, new TimelineSequencesEventArgs(results));
	}
}
