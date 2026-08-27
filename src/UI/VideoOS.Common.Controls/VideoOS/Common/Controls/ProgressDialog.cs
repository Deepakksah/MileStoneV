using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.ServiceModel;
using System.Windows.Forms;
using VideoOS.Common.Controls.Properties;
using VideoOS.Common.Integration;

namespace VideoOS.Common.Controls;

public class ProgressDialog : Form
{
	protected class ProgressEventArgs : EventArgs
	{
		public int ProgressSubtaskValue { get; private set; }

		public int ProgressTotalValue { get; private set; }

		public string SubtaskStatusMessage { get; private set; }

		public bool CancelEnabled { get; private set; }

		public ProgressEventArgs(int progressValue, int progressTotalValue, string subtaskStatusMessage, bool cancelEnabled)
		{
			ProgressSubtaskValue = progressValue;
			ProgressTotalValue = progressTotalValue;
			SubtaskStatusMessage = subtaskStatusMessage;
			CancelEnabled = cancelEnabled;
		}
	}

	protected class SubtaskCompletedEventArgs : EventArgs
	{
		public int ProgressTotalValue { get; private set; }

		public SubtaskCompletedEventArgs(int progressTotalValue)
		{
			ProgressTotalValue = progressTotalValue;
		}
	}

	protected class CompletedEventArgs : EventArgs
	{
		public string SubtaskStatusMessage { get; private set; }

		public string TotalStatusMessage { get; private set; }

		public CompletedEventArgs(string subtaskStatusMessage, string totalStatusMessage)
		{
			SubtaskStatusMessage = subtaskStatusMessage;
			TotalStatusMessage = totalStatusMessage;
		}
	}

	protected class FailedEventArgs : EventArgs
	{
		public ExceptionDetail RestoreException { get; private set; }

		public string ErrorMessage { get; private set; }

		public bool UnauthorizedAccessException { get; private set; }

		public FailedEventArgs(Exception exception, string errorMessage)
		{
			RestoreException = new ExceptionDetail(exception);
			ErrorMessage = errorMessage;
			UnauthorizedAccessException = false;
			if (exception is FaultException)
			{
				FaultException ex = exception as FaultException;
				if (ex.Code != null && string.Equals("51000", ex.Code.Name, StringComparison.OrdinalIgnoreCase))
				{
					UnauthorizedAccessException = true;
				}
			}
		}

		public FailedEventArgs(ExceptionDetail exception, bool unauthorizedAccessException, string errorMessage)
		{
			RestoreException = exception;
			ErrorMessage = errorMessage;
			UnauthorizedAccessException = unauthorizedAccessException;
		}
	}

	protected class AbortingEventArgs : EventArgs
	{
		public bool IsRunning { get; private set; }

		public AbortingEventArgs(bool isRunning)
		{
			IsRunning = isRunning;
		}
	}

	private bool finished;

	private volatile bool _aborting;

	private volatile bool _startAbort;

	private volatile bool _unauthorizedAccess;

	private IContainer components;

	private LogoHorizontalResizable logoHorizontalResizable1;

	private ProgressBar procSubtask;

	private ProgressBar procTotal;

	private Label lblSubtask;

	private Label lblTotal;

	private Button cmdCancel;

	protected bool Aborting
	{
		get
		{
			return _aborting;
		}
		set
		{
			_aborting = value;
		}
	}

	protected bool StartAbort
	{
		get
		{
			return _startAbort;
		}
		set
		{
			_startAbort = value;
		}
	}

	protected bool UnauthorizedAccess
	{
		get
		{
			return _unauthorizedAccess;
		}
		set
		{
			_unauthorizedAccess = value;
		}
	}

	protected string Caption
	{
		get
		{
			return Text;
		}
		set
		{
			Text = value;
		}
	}

	protected event EventHandler<AbortingEventArgs> Abort;

	public ProgressDialog()
	{
		InitializeComponent();
	}

	protected void OnProgress(object sender, ProgressEventArgs e)
	{
		if (base.InvokeRequired)
		{
			Invoke(new EventHandler<ProgressEventArgs>(OnProgress), sender, e);
		}
		else if (e != null)
		{
			procSubtask.Value = e.ProgressSubtaskValue;
			procSubtask.Refresh();
			procTotal.Value = e.ProgressTotalValue;
			procTotal.Refresh();
			lblSubtask.Text = e.SubtaskStatusMessage;
			cmdCancel.Enabled = e.CancelEnabled;
		}
	}

	protected void OnSubtaskCompleted(object sender, SubtaskCompletedEventArgs e)
	{
		if (base.InvokeRequired)
		{
			Invoke(new EventHandler<SubtaskCompletedEventArgs>(OnSubtaskCompleted), sender, e);
		}
		else if (e != null)
		{
			procSubtask.Value = 100;
			procTotal.Value = e.ProgressTotalValue;
			Refresh();
		}
	}

	protected void OnFailed(object sender, FailedEventArgs e)
	{
		if (base.InvokeRequired)
		{
			Invoke(new EventHandler<FailedEventArgs>(OnFailed), sender, e);
		}
		else if (e != null)
		{
			if (e.UnauthorizedAccessException)
			{
				UnauthorizedAccess = true;
				finished = true;
				Utils.ShowMessageAsError(OemResources.Instance.Manager.ApplicationName, Resources.MsgBoxAccountMissingPermissions, this);
			}
			else
			{
				Utils.ShowMessageAsError(OemResources.Instance.Manager.ApplicationName, string.Format(CultureInfo.CurrentCulture, Resources.MsgBoxUnknowServerError, e.ErrorMessage), this);
			}
		}
	}

	protected void OnCompleted(object sender, CompletedEventArgs e)
	{
		if (base.InvokeRequired)
		{
			Invoke(new EventHandler<CompletedEventArgs>(OnCompleted), sender, e);
		}
		else if (e != null)
		{
			procSubtask.Value = 100;
			procTotal.Value = 100;
			cmdCancel.Enabled = true;
			cmdCancel.Text = Resources.ProgressDialogClose;
			lblTotal.Text = e.TotalStatusMessage;
			lblSubtask.Text = e.SubtaskStatusMessage;
			finished = true;
			Refresh();
			cmdCancel.Focus();
		}
	}

	private void cmdCancel_Click(object sender, EventArgs e)
	{
		if (finished)
		{
			if (!UnauthorizedAccess)
			{
				this.Abort(null, new AbortingEventArgs(!finished));
			}
			base.DialogResult = DialogResult.OK;
			return;
		}
		StartAbort = true;
		if (Utils.ShowMessageYesNoButtons(Resources.ProgressDialogCancelFormTitle, Resources.ProgressDialogCancelFormText, MessageBoxIcon.Exclamation, this) == DialogResult.Yes)
		{
			Aborting = true;
			if (!UnauthorizedAccess)
			{
				this.Abort(null, new AbortingEventArgs(!finished));
			}
			base.DialogResult = DialogResult.Cancel;
		}
		else
		{
			StartAbort = false;
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	private void InitializeComponent()
	{
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Common.Controls.ProgressDialog));
		this.logoHorizontalResizable1 = new VideoOS.Common.Controls.LogoHorizontalResizable();
		this.procSubtask = new System.Windows.Forms.ProgressBar();
		this.procTotal = new System.Windows.Forms.ProgressBar();
		this.lblSubtask = new System.Windows.Forms.Label();
		this.lblTotal = new System.Windows.Forms.Label();
		this.cmdCancel = new System.Windows.Forms.Button();
		base.SuspendLayout();
		componentResourceManager.ApplyResources(this.logoHorizontalResizable1, "logoHorizontalResizable1");
		this.logoHorizontalResizable1.Name = "logoHorizontalResizable1";
		componentResourceManager.ApplyResources(this.procSubtask, "procSubtask");
		this.procSubtask.Name = "procSubtask";
		componentResourceManager.ApplyResources(this.procTotal, "procTotal");
		this.procTotal.Name = "procTotal";
		componentResourceManager.ApplyResources(this.lblSubtask, "lblSubtask");
		this.lblSubtask.Name = "lblSubtask";
		componentResourceManager.ApplyResources(this.lblTotal, "lblTotal");
		this.lblTotal.Name = "lblTotal";
		componentResourceManager.ApplyResources(this.cmdCancel, "cmdCancel");
		this.cmdCancel.Name = "cmdCancel";
		this.cmdCancel.UseVisualStyleBackColor = true;
		this.cmdCancel.Click += new System.EventHandler(cmdCancel_Click);
		componentResourceManager.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.cmdCancel);
		base.Controls.Add(this.lblTotal);
		base.Controls.Add(this.lblSubtask);
		base.Controls.Add(this.procTotal);
		base.Controls.Add(this.procSubtask);
		base.Controls.Add(this.logoHorizontalResizable1);
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "ProgressDialog";
		base.ShowIcon = false;
		base.ResumeLayout(false);
	}
}
