using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Windows.Forms;
using VideoOS.Common.Controls.Properties;

namespace VideoOS.Common.Controls;

public class AdvancedInfoForm : Form
{
	private ExceptionDialogForm _exceptionForm;

	private IContainer components;

	private Button btnClose;

	private Label lblMessageComponents;

	private Label lblDetails;

	private ToolStripButton btnCopy;

	private ToolStrip toolStrip1;

	private TreeView treeMessages;

	private TextBox txtDetails;

	public ExceptionDialogForm ExceptionDialogForm{
		get
		{
			return _exceptionForm;
		}
		set
		{
			_exceptionForm = value;
		}
	}

	public AdvancedInfoForm()
	{
		InitializeComponent();
		toolStrip1.Renderer = new ToolStripRenderer();
	}

	private void AdvancedInfoForm_Load(object sender, EventArgs e)
	{
		TreeNode treeNode = new TreeNode(Resources.ExBoxAdvancedAllMessages);
		for (Exception ex = _exceptionForm.Message; ex != null; ex = ex.InnerException)
		{
			if (!(ex is MessageOnlyException))
			{
				try
				{
					TreeNode treeNode2 = new TreeNode(ex.Message);
					treeNode2.Tag = _exceptionForm.BuildAdvancedInfo(ex, AdvancedInfoType.All);
					TreeNode treeNode3 = new TreeNode(Resources.ExBoxAdvancedMessage);
					treeNode3.Tag = _exceptionForm.BuildAdvancedInfo(ex, AdvancedInfoType.Message);
					treeNode2.Nodes.Add(treeNode3);
					treeNode.Nodes.Add(treeNode2);
					try
					{
						string text = _exceptionForm.BuildAdvancedInfo(ex, AdvancedInfoType.Data);
						if (!string.IsNullOrEmpty(text))
						{
							TreeNode treeNode4 = new TreeNode(Resources.ExBoxAdvancedData);
							treeNode4.Tag = text;
							treeNode2.Nodes.Add(treeNode4);
						}
					}
					catch
					{
					}
					if (ex.StackTrace != null)
					{
						try
						{
							string text2 = _exceptionForm.BuildAdvancedInfo(ex, AdvancedInfoType.StackTrace);
							if (!string.IsNullOrEmpty(text2))
							{
								TreeNode treeNode5 = new TreeNode(Resources.ExBoxCodeLocation);
								treeNode5.Tag = text2;
								treeNode2.Nodes.Add(treeNode5);
							}
						}
						catch
						{
						}
					}
				}
				catch
				{
				}
			}
		}
		if (treeNode.Nodes.Count == 0)
		{
			Close();
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		foreach (TreeNode node in treeNode.Nodes)
		{
			stringBuilder.Append(node.Tag.ToString());
		}
		treeNode.Tag = stringBuilder.ToString();
		treeMessages.Nodes.Add(treeNode);
		treeMessages.ExpandAll();
		treeMessages.SelectedNode = treeNode;
	}

	private void AdvancedInfoForm_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.Modifiers == Keys.Control && (e.KeyData & Keys.C) == Keys.C)
		{
			CopyToClipboard();
			e.Handled = true;
		}
	}

	private void treeMessages_AfterSelect(object sender, TreeViewEventArgs e)
	{
		txtDetails.Text = (string)e.Node.Tag;
	}

	private void btnCopy_Click(object sender, EventArgs e)
	{
		CopyToClipboard();
	}

	private void CopyToClipboard()
	{
		try
		{
			Clipboard.SetDataObject((txtDetails.SelectionLength == 0) ? txtDetails.Text : txtDetails.SelectedText, copy: true);
		}
		catch (Exception exError)
		{
			_exceptionForm.ShowError(Resources.ExBoxCopyToClipboardError, exError);
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Common.Controls.AdvancedInfoForm));
		this.lblMessageComponents = new System.Windows.Forms.Label();
		this.treeMessages = new System.Windows.Forms.TreeView();
		this.lblDetails = new System.Windows.Forms.Label();
		this.txtDetails = new System.Windows.Forms.TextBox();
		this.btnClose = new System.Windows.Forms.Button();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnCopy = new System.Windows.Forms.ToolStripButton();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		resources.ApplyResources(this.lblMessageComponents, "lblMessageComponents");
		this.lblMessageComponents.Name = "lblMessageComponents";
		resources.ApplyResources(this.treeMessages, "treeMessages");
		this.treeMessages.Name = "treeMessages";
		this.treeMessages.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(treeMessages_AfterSelect);
		resources.ApplyResources(this.lblDetails, "lblDetails");
		this.lblDetails.Name = "lblDetails";
		resources.ApplyResources(this.txtDetails, "txtDetails");
		this.txtDetails.Name = "txtDetails";
		this.txtDetails.ReadOnly = true;
		resources.ApplyResources(this.btnClose, "btnClose");
		this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.btnClose.Name = "btnClose";
		this.toolStrip1.AllowMerge = false;
		resources.ApplyResources(this.toolStrip1, "toolStrip1");
		this.toolStrip1.CanOverflow = false;
		this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnCopy });
		this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
		this.toolStrip1.TabStop = true;
		this.btnCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		resources.ApplyResources(this.btnCopy, "btnCopy");
		this.btnCopy.Name = "btnCopy";
		this.btnCopy.Click += new System.EventHandler(btnCopy_Click);
		base.AcceptButton = this.btnClose;
		resources.ApplyResources(this, "$this");
		base.CancelButton = this.btnClose;
		base.Controls.Add(this.toolStrip1);
		base.Controls.Add(this.btnClose);
		base.Controls.Add(this.txtDetails);
		base.Controls.Add(this.lblDetails);
		base.Controls.Add(this.treeMessages);
		base.Controls.Add(this.lblMessageComponents);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "AdvancedInfoForm";
		base.ShowInTaskbar = false;
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(AdvancedInfoForm_KeyDown);
		base.Load += new System.EventHandler(AdvancedInfoForm_Load);
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
