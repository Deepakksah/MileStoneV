using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using VideoOS.TrayController.Communication;
using VideoOS.TrayControllerControls.Properties;

namespace VideoOS.TrayControllerControls;

public class StatusMessagesForm : Form
{
	private delegate void CloseFormCallback();

	private delegate void HideFormCallback();

	private delegate void ReceiveMessagesCallback(List<LogMessage> messages, bool inSession);

	private object _lockObject = new object();

	private ListView _listViewMessages;

	private const int _maxMessagesSize = 100;

	private IContainer components;

	private Panel contentPanel;

	private Button buttonClose;

	private ToolStrip toolStrip1;

	private ToolStripButton btnCopy;

	private ToolStripButton btnAdvanced;

	private Panel panel1;

	public StatusMessagesForm()
	{
		InitializeComponent();
		InitializeContentPanel();
	}

	public void CloseForm()
	{
		if (base.InvokeRequired)
		{
			Invoke(new CloseFormCallback(CloseForm));
		}
		else
		{
			Close();
		}
	}

	public void HideForm()
	{
		if (base.InvokeRequired)
		{
			Invoke(new HideFormCallback(HideForm));
		}
		else
		{
			Hide();
		}
	}

	private void InitializeContentPanel()
	{
		_listViewMessages = new ListView();
		_listViewMessages = CreateNewMessagesList();
		_listViewMessages.Dock = DockStyle.Fill;
		contentPanel.Controls.Add(_listViewMessages);
	}

	private ListView CreateNewMessagesList()
	{
		ListView listViewMessages = new ListView();
		ColumnHeader columnHeader = new ColumnHeader();
		ColumnHeader columnHeader2 = new ColumnHeader();
		columnHeader.Text = Resources.StatusMessagesFormTimeColumn;
		columnHeader.Width = 117;
		columnHeader2.Text = Resources.StatusMessagesFormMessageColumn;
		columnHeader2.Width = 391;
		listViewMessages.Columns.AddRange(new ColumnHeader[2] { columnHeader, columnHeader2 });
		listViewMessages.Dock = DockStyle.Fill;
		listViewMessages.FullRowSelect = true;
		listViewMessages.Name = "listViewMessages";
		listViewMessages.UseCompatibleStateImageBehavior = false;
		listViewMessages.View = View.Details;
		listViewMessages.HideSelection = false;
		listViewMessages.KeyUp += KeyUpHandler;
		listViewMessages.MouseDoubleClick += delegate
		{
			ShowSelectedStatusMessage(listViewMessages);
		};
		AddContextMenu(listViewMessages);
		return listViewMessages;
	}

	private void AddContextMenu(ListView listView)
	{
		ContextMenu contextMenu = new ContextMenu();
		listView.ContextMenu = contextMenu;
		MenuItem mnuItemDetails = new MenuItem();
		MenuItem mnuItemCopyToClipboard = new MenuItem();
		mnuItemDetails.Text = Resources.StatusMessagesFormMenuItemDetails;
		mnuItemCopyToClipboard.Text = Resources.StatusMessagesFormMenuItemCopyToClipboard;
		contextMenu.MenuItems.Add(mnuItemDetails);
		contextMenu.MenuItems.Add(mnuItemCopyToClipboard);
		contextMenu.Popup += delegate
		{
			contextMenu.MenuItems[mnuItemDetails.Index].Enabled = listView.SelectedItems.Count == 1;
			contextMenu.MenuItems[mnuItemCopyToClipboard.Index].Enabled = listView.SelectedItems.Count > 0;
		};
		mnuItemDetails.Click += delegate
		{
			ShowSelectedStatusMessage(listView);
		};
		mnuItemCopyToClipboard.Click += delegate
		{
			CopyToClipboard(listView);
		};
	}

	private void ShowSelectedStatusMessage(ListView listView)
	{
		if (listView.SelectedItems.Count > 0)
		{
			using (SingleStatusMessageForm singleStatusMessageForm = new SingleStatusMessageForm(listView.SelectedItems[0].SubItems[0].Text, listView.SelectedItems[0].SubItems[1].Text))
			{
				singleStatusMessageForm.ShowDialog(this);
			}
		}
	}

	private void StatusMessageForm_Load(object sender, EventArgs e)
	{
		Show();
	}

	public void ReceiveMessages(List<LogMessage> messages, bool inSession)
	{
		if (base.InvokeRequired)
		{
			Invoke(new ReceiveMessagesCallback(ReceiveMessages), messages, inSession);
			return;
		}
		try
		{
			_listViewMessages.BeginUpdate();
			lock (_lockObject)
			{
				if (!inSession)
				{
					_listViewMessages.Items.Clear();
				}
				foreach (LogMessage message in messages)
				{
					_listViewMessages.Items.Insert(0, new ListViewItem(new string[2]
					{
						message.Timestamp.ToString(CultureInfo.InvariantCulture),
						message.Message.ToString()
					}));
					while (_listViewMessages.Items.Count > 100)
					{
						_listViewMessages.Items.RemoveAt(_listViewMessages.Items.Count - 1);
					}
				}
			}
		}
		finally
		{
			_listViewMessages.EndUpdate();
		}
	}

	private void StatusMessageForm_FormClosing(object sender, FormClosingEventArgs e)
	{
		HideForm();
		if (e.CloseReason == CloseReason.UserClosing)
		{
			e.Cancel = true;
		}
	}

	private void buttonClose_Click(object sender, EventArgs e)
	{
		Hide();
	}

	private void KeyUpHandler(object sender, KeyEventArgs e)
	{
		if ((e.KeyCode == Keys.Insert && e.Control) || (e.KeyCode == Keys.C && e.Control))
		{
			CopyToClipboard(sender as ListView);
		}
	}

	private static void CopyToClipboard(ListView listView)
	{
		if (listView == null || listView.SelectedItems.Count <= 0)
		{
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		foreach (ListViewItem selectedItem in listView.SelectedItems)
		{
			stringBuilder.Append(selectedItem.Text);
			stringBuilder.Append(' ');
			stringBuilder.Append(selectedItem.SubItems[1].Text);
			stringBuilder.AppendLine();
		}
		Clipboard.SetText(stringBuilder.ToString());
	}

	private void btnAdvanced_Click(object sender, EventArgs e)
	{
		ShowSelectedStatusMessage(_listViewMessages);
	}

	private void btnCopy_Click(object sender, EventArgs e)
	{
		CopyToClipboard(_listViewMessages);
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.TrayControllerControls.StatusMessagesForm));
		this.contentPanel = new System.Windows.Forms.Panel();
		this.buttonClose = new System.Windows.Forms.Button();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnCopy = new System.Windows.Forms.ToolStripButton();
		this.btnAdvanced = new System.Windows.Forms.ToolStripButton();
		this.panel1 = new System.Windows.Forms.Panel();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		resources.ApplyResources(this.contentPanel, "contentPanel");
		this.contentPanel.Name = "contentPanel";
		resources.ApplyResources(this.buttonClose, "buttonClose");
		this.buttonClose.Name = "buttonClose";
		this.buttonClose.Click += new System.EventHandler(buttonClose_Click);
		this.toolStrip1.AllowMerge = false;
		resources.ApplyResources(this.toolStrip1, "toolStrip1");
		this.toolStrip1.CanOverflow = false;
		this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnCopy, this.btnAdvanced });
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
		this.toolStrip1.TabStop = true;
		this.btnCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		resources.ApplyResources(this.btnCopy, "btnCopy");
		this.btnCopy.Name = "btnCopy";
		this.btnCopy.Click += new System.EventHandler(btnCopy_Click);
		this.btnAdvanced.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		resources.ApplyResources(this.btnAdvanced, "btnAdvanced");
		this.btnAdvanced.Name = "btnAdvanced";
		this.btnAdvanced.Click += new System.EventHandler(btnAdvanced_Click);
		resources.ApplyResources(this.panel1, "panel1");
		this.panel1.Name = "panel1";
		resources.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.toolStrip1);
		base.Controls.Add(this.contentPanel);
		base.Controls.Add(this.buttonClose);
		base.Name = "StatusMessagesForm";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(StatusMessageForm_FormClosing);
		base.Load += new System.EventHandler(StatusMessageForm_Load);
		base.KeyUp += new System.Windows.Forms.KeyEventHandler(KeyUpHandler);
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
