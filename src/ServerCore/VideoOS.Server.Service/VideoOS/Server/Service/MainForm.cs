using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace VideoOS.Server.Service;

public class MainForm : Form
{
	private ServerService _service;

	private Timer timerStatus;

	private Label labelStatus;

	private Label label1;

	private Button buttonStop;

	private IContainer components;

	public MainForm(ServerService service)
	{
		_service = service;
		InitializeComponent();
		timerStatus.Start();
		base.ActiveControl = labelStatus;
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
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Server.Service.MainForm));
		this.timerStatus = new System.Windows.Forms.Timer(this.components);
		this.labelStatus = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.buttonStop = new System.Windows.Forms.Button();
		base.SuspendLayout();
		this.timerStatus.Interval = 500;
		this.timerStatus.Tick += new System.EventHandler(timerStatus_Tick);
		resources.ApplyResources(this.labelStatus, "labelStatus");
		this.labelStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.labelStatus.Name = "labelStatus";
		resources.ApplyResources(this.label1, "label1");
		this.label1.Name = "label1";
		resources.ApplyResources(this.buttonStop, "buttonStop");
		this.buttonStop.Name = "buttonStop";
		this.buttonStop.Click += new System.EventHandler(buttonStop_Click);
		base.AcceptButton = this.buttonStop;
		resources.ApplyResources(this, "$this");
		base.Controls.Add(this.labelStatus);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.buttonStop);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.MaximizeBox = false;
		base.Name = "MainForm";
		base.Closing += new System.ComponentModel.CancelEventHandler(MainForm_Closing);
		base.Load += new System.EventHandler(MainForm_Load);
		base.ResumeLayout(false);
	}

	private void buttonStop_Click(object sender, EventArgs e)
	{
		Close();
	}

	[DebuggerStepThrough]
	private void timerStatus_Tick(object sender, EventArgs e)
	{
		labelStatus.Text = _service.State.ServiceStatusText;
	}

	private void MainForm_Closing(object sender, CancelEventArgs e)
	{
		try
		{
			_service.StopFromWindow();
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
		}
	}

	private void MainForm_Load(object sender, EventArgs e)
	{
		_service.StartFromWindow();
	}
}
