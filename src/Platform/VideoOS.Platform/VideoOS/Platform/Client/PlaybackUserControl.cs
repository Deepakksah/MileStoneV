using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using VideoOS.Platform.Telemetry;

namespace VideoOS.Platform.Client;

[Obsolete("This class is deprecated. Windows Forms controls are no longer supported. Use PlaybackWpfUserControl instead.", false)]
public class PlaybackUserControl : UserControl
{
	private IContainer components;

	public virtual bool ShowTallUserControl
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public virtual bool ShowSpeedControl
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public virtual bool ShowTimeSpanControl
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public virtual DateTime SelectionFromTime
	{
		get
		{
			return DateTime.UtcNow;
		}
		set
		{
		}
	}

	public virtual DateTime SelectionToTime
	{
		get
		{
			return DateTime.UtcNow;
		}
		set
		{
		}
	}

	public virtual TimeSpan TimeSpan
	{
		get
		{
			return TimeSpan.FromHours(1.0);
		}
		set
		{
		}
	}

	public PlaybackUserControl()
	{
		InitializeComponent();
	}

	public virtual void Init(FQID playbackControllerFQID)
	{
		new TrackPageViewData(typeof(PlaybackUserControl), deprecated: true).Send();
	}

	public virtual void Close()
	{
	}

	public virtual void SetCameras(List<FQID> sources)
	{
	}

	public virtual void SetEnabled(bool enable)
	{
	}

	public virtual void SetSequence(DateTime startTime, DateTime endTime)
	{
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		base.SuspendLayout();
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Name = "PlaybackUserControl";
		base.Size = new System.Drawing.Size(465, 75);
		base.ResumeLayout(false);
	}
}
