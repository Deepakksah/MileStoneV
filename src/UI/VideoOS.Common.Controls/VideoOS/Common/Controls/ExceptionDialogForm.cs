using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using VideoOS.Common.Controls.Properties;

namespace VideoOS.Common.Controls;

public sealed class ExceptionDialogForm : Form
{
	public enum BeepType
	{
		Default = 0,
		Hand = 16,
		Question = 32,
		Exclamation = 48,
		Asterisk = 64,
		Standard = -1
	}

	private string[] _buttonTextArray;

	private int _buttonCount;

	private bool _isButtonPressed;

	private Icon _iconSymbol;

	private ExceptionDialogButtons _buttons;

	private ExceptionDialogDefaultButton _defaultButton;

	private ExceptionDialogResult _customDialogResult;

	private bool _doBeep;

	private Exception _exMessage;

	private string _link;

	private int _linkPosition;

	private int _linkLength;

	private Bitmap _customSymbol;

	private ExceptionDialogSymbol _symbol;

	private bool _showCheckBox;

	public string _checkBoxText;

	private int _messageLimitCount;

	private bool _showHelpButton;

	private IContainer components;

	private TableLayoutPanel pnlForm;

	private Panel pnlIcon;

	private TableLayoutPanel pnlMessage;

	private LinkLabel lblTopMessage;

	private Label lblAdditionalInfo;

	private TableLayoutPanel pnlAdditional;

	private GroupBox grpSeparator;

	private TableLayoutPanel pnlButtons;

	private ToolStrip toolStrip1;

	private Button button1;

	private Button button2;

	private Button button3;

	private Button button4;

	private Button button5;

	private ToolStripButton btnCopy;

	private ToolStripButton btnAdvanced;

	private ToolStripButton btnHelp;

	private ImageList imageList;

	private WrappingCheckBox chkDontShow;

	public ExceptionDialogButtons Buttons{
		get
		{
			return _buttons;
		}
		set
		{
			_buttons = value;
		}
	}

	public ExceptionDialogDefaultButton DefaultButton
	{
		get
		{
			return _defaultButton;
		}
		set
		{
			_defaultButton = value;
		}
	}

	public ExceptionDialogResult CustomDialogResult => _customDialogResult;

	public string Caption
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

	public bool DoBeep
	{
		get
		{
			return _doBeep;
		}
		set
		{
			_doBeep = value;
		}
	}

	public Exception Message
	{
		get
		{
			return _exMessage;
		}
		set
		{
			_exMessage = value;
		}
	}

	public string Link
	{
		get
		{
			return _link;
		}
		set
		{
			_link = value;
		}
	}

	public int LinkPosition
	{
		get
		{
			return _linkPosition;
		}
		set
		{
			_linkPosition = value;
		}
	}

	public int LinkLength
	{
		get
		{
			return _linkLength;
		}
		set
		{
			_linkLength = value;
		}
	}

	public Bitmap CustomSymbol
	{
		get
		{
			return _customSymbol;
		}
		set
		{
			_customSymbol = value;
		}
	}

	public ExceptionDialogSymbol Symbol
	{
		get
		{
			return _symbol;
		}
		set
		{
			_symbol = value;
		}
	}

	public bool ShowCheckBox
	{
		get
		{
			return _showCheckBox;
		}
		set
		{
			_showCheckBox = value;
		}
	}

	public string CheckBoxText
	{
		get
		{
			return _checkBoxText;
		}
		set
		{
			_checkBoxText = value;
		}
	}

	public bool IsCheckBoxChecked
	{
		get
		{
			return chkDontShow.Checked;
		}
		set
		{
			chkDontShow.Checked = value;
		}
	}

	public int MessageLevelCount
	{
		get
		{
			return _messageLimitCount;
		}
		set
		{
			_messageLimitCount = value;
		}
	}

	public bool ShowHelpButton
	{
		get
		{
			return _showHelpButton;
		}
		set
		{
			_showHelpButton = value;
		}
	}

	public event EventHandler<CopyToClipboardEventArgs> OnCopyToClipboardInternal;

	private static void DisableCloseButton(Form form)
	{
		ExceptionDialogNativeMethods.EnableMenuItem(ExceptionDialogNativeMethods.GetSystemMenu(form.Handle, bRevert: false), 61536u, 1u);
	}

	public ExceptionDialogForm()
	{
		_symbol = ExceptionDialogSymbol.Warning;
		_showHelpButton = true;
		_messageLimitCount = -1;
		_doBeep = true;
		InitializeComponent();
		toolStrip1.Renderer = new ToolStripRenderer();
	}

	private void InitializeButtons()
	{
		Button[] array = new Button[5] { button1, button2, button3, button4, button5 };
		switch (_buttons)
		{
		case ExceptionDialogButtons.OK:
			_buttonTextArray[0] = Resources.ExBoxOKButtonText;
			button1.DialogResult = DialogResult.OK;
			base.AcceptButton = button1;
			base.CancelButton = button1;
			_buttonCount = 1;
			break;
		case ExceptionDialogButtons.OKCancel:
			_buttonTextArray[0] = Resources.ExBoxOKButtonText;
			_buttonTextArray[1] = Resources.ExBoxCancelButtonText;
			button1.DialogResult = DialogResult.OK;
			button2.DialogResult = DialogResult.Cancel;
			base.AcceptButton = button1;
			base.CancelButton = button2;
			_buttonCount = 2;
			break;
		case ExceptionDialogButtons.YesNoCancel:
			_buttonTextArray[0] = Resources.ExBoxYesButtonText;
			_buttonTextArray[1] = Resources.ExBoxNoButtonText;
			_buttonTextArray[2] = Resources.ExBoxCancelButtonText;
			button1.DialogResult = DialogResult.Yes;
			button2.DialogResult = DialogResult.No;
			button3.DialogResult = DialogResult.Cancel;
			_buttonCount = 3;
			base.CancelButton = button3;
			break;
		case ExceptionDialogButtons.YesNo:
			_buttonTextArray[0] = Resources.ExBoxYesButtonText;
			_buttonTextArray[1] = Resources.ExBoxNoButtonText;
			button1.DialogResult = DialogResult.Yes;
			button2.DialogResult = DialogResult.No;
			_buttonCount = 2;
			DisableCloseButton(this);
			base.CancelButton = button2;
			break;
		case ExceptionDialogButtons.AbortRetryIgnore:
			_buttonTextArray[0] = Resources.ExBoxAbortButtonText;
			_buttonTextArray[1] = Resources.ExBoxRetryButtonText;
			_buttonTextArray[2] = Resources.ExBoxIgnoreButtonText;
			button1.DialogResult = DialogResult.Abort;
			button2.DialogResult = DialogResult.Retry;
			button3.DialogResult = DialogResult.Ignore;
			_buttonCount = 3;
			base.CancelButton = button3;
			break;
		case ExceptionDialogButtons.RetryCancel:
			_buttonTextArray[0] = Resources.ExBoxRetryButtonText;
			_buttonTextArray[1] = Resources.ExBoxCancelButtonText;
			button1.DialogResult = DialogResult.Retry;
			button2.DialogResult = DialogResult.Cancel;
			_buttonCount = 2;
			base.CancelButton = button2;
			break;
		case ExceptionDialogButtons.Custom:
			DisableCloseButton(this);
			break;
		}
		Size preferredSize = pnlButtons.GetPreferredSize(Size.Empty);
		int num = preferredSize.Width;
		for (int i = 0; i < _buttonCount; i++)
		{
			Button obj = array[i];
			obj.Text = _buttonTextArray[i];
			obj.Visible = true;
		}
		AdjustDialogWidth(preferredSize.Width - num, isAdjustingForButtons: true);
		if ((int)_defaultButton >= _buttonCount)
		{
			throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.InvalidEnumValue, "DefaultButton", (int)_defaultButton, typeof(ExceptionDialogDefaultButton)));
		}
		base.AcceptButton = array[(int)_defaultButton];
	}

	private void InitializeCheckbox()
	{
		if (!_showCheckBox)
		{
			chkDontShow.Visible = false;
			chkDontShow.Enabled = false;
		}
		else if (chkDontShow.Text.Length == 0)
		{
			chkDontShow.Text = Resources.ExBoxDefaultCheckBoxText;
		}
	}

	private void InitializeSymbol()
	{
		if (_customSymbol != null)
		{
			int num = _customSymbol.Width + 2 - pnlIcon.Width;
			pnlIcon.Width += num;
			AdjustDialogWidth(num, isAdjustingForButtons: false);
			pnlIcon.Height = _customSymbol.Height + 2;
			pnlIcon.MinimumSize = new Size(0, _customSymbol.Height + 2);
			return;
		}
		switch (_symbol)
		{
		case ExceptionDialogSymbol.None:
			pnlIcon.Visible = false;
			break;
		case ExceptionDialogSymbol.Warning:
		case ExceptionDialogSymbol.Exclamation:
			_iconSymbol = SystemIcons.Warning;
			break;
		case ExceptionDialogSymbol.Information:
		case ExceptionDialogSymbol.Asterisk:
			_iconSymbol = SystemIcons.Information;
			break;
		case ExceptionDialogSymbol.Stop:
		case ExceptionDialogSymbol.Hand:
			_iconSymbol = SystemIcons.Error;
			break;
		case ExceptionDialogSymbol.Question:
			_iconSymbol = SystemIcons.Question;
			break;
		}
	}

	private void InitializeMessage()
	{
		int num = 0;
		for (Exception innerException = _exMessage.InnerException; innerException != null; innerException = innerException.InnerException)
		{
			num++;
		}
		if (_messageLimitCount > 0 && num > _messageLimitCount - 1)
		{
			num = _messageLimitCount - 1;
		}
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				pnlAdditional.ColumnStyles.Insert(0, new ColumnStyle(SizeType.Absolute, 20f));
				pnlAdditional.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			}
			pnlAdditional.ColumnCount = num + 1;
			pnlAdditional.RowCount = num;
		}
		Label label = lblAdditionalInfo;
		bool visible = (pnlAdditional.Visible = num > 0);
		label.Visible = visible;
		Exception ex = _exMessage;
		int num2 = 0;
		while (ex != null && (_messageLimitCount < 0 || num2 < _messageLimitCount))
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (string.IsNullOrEmpty(ex.Message))
			{
				stringBuilder.Append(Resources.ExBoxCantComplete);
			}
			else
			{
				stringBuilder.Append(ex.Message);
			}
			if (num2 == 0)
			{
				lblTopMessage.Text = stringBuilder.ToString();
				lblTopMessage.LinkArea = new LinkArea(0, 0);
				if (!string.IsNullOrEmpty(_link) && lblTopMessage.Links.Count == 0)
				{
					lblTopMessage.LinkClicked += lblTopMessage_LinkClicked;
					lblTopMessage.Links.Add(_linkPosition, _linkLength, _link);
				}
				lblTopMessage.Tag = ex;
			}
			else
			{
				AddAdditionalInfoMessage(num2 - 1, stringBuilder.ToString(), ex);
			}
			ex = ex.InnerException;
			num2++;
		}
		Size preferredSize = GetPreferredSize(Size.Empty);
		if (base.Location.Y + preferredSize.Height > Screen.PrimaryScreen.WorkingArea.Bottom)
		{
			base.Location = new Point(base.Location.X, Screen.PrimaryScreen.WorkingArea.Bottom - base.Size.Height - 10);
		}
	}

	private void lblTopMessage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		string text = e.Link.LinkData as string;
		if (string.IsNullOrEmpty(text) || !Uri.TryCreate(text, UriKind.Absolute, out var result) || (result.Scheme != Uri.UriSchemeHttp && result.Scheme != Uri.UriSchemeHttps))
		{
			return;
		}
		try
		{
			Process.Start(text);
		}
		catch (Win32Exception)
		{
		}
	}

	private void AdjustDialogWidth(int offset, bool isAdjustingForButtons)
	{
		if (offset <= 0)
		{
			return;
		}
		lblTopMessage.MaximumSize = new Size(lblTopMessage.MaximumSize.Width + offset, lblTopMessage.MaximumSize.Height);
		lblTopMessage.MinimumSize = lblTopMessage.MinimumSize;
		pnlAdditional.MaximumSize = new Size(pnlAdditional.MaximumSize.Width + offset, pnlAdditional.MaximumSize.Height);
		foreach (Label control in pnlAdditional.Controls)
		{
			if (control.ImageIndex > 0)
			{
				control.MaximumSize = new Size(control.MaximumSize.Width + offset, control.MaximumSize.Height);
			}
		}
		if (!isAdjustingForButtons)
		{
			pnlButtons.MinimumSize = new Size(pnlButtons.MinimumSize.Width + offset, pnlButtons.MaximumSize.Height);
		}
	}

	private void Beep()
	{
		switch (_symbol)
		{
		case ExceptionDialogSymbol.None:
		case ExceptionDialogSymbol.Warning:
			ExceptionDialogNativeMethods.MessageBeep(BeepType.Asterisk);
			break;
		case ExceptionDialogSymbol.Information:
			ExceptionDialogNativeMethods.MessageBeep(BeepType.Exclamation);
			break;
		case ExceptionDialogSymbol.Stop:
			ExceptionDialogNativeMethods.MessageBeep(BeepType.Hand);
			break;
		}
	}

	private void OnLoad(object sender, EventArgs e)
	{
		if (base.Parent == null)
		{
			base.StartPosition = FormStartPosition.CenterScreen;
		}
		else
		{
			base.StartPosition = FormStartPosition.CenterParent;
			CenterToParent();
		}
		if (_doBeep)
		{
			Beep();
		}
		switch (_defaultButton)
		{
		case ExceptionDialogDefaultButton.Button1:
			button1.Focus();
			base.ActiveControl = button1;
			break;
		case ExceptionDialogDefaultButton.Button2:
			button2.Focus();
			base.ActiveControl = button2;
			break;
		case ExceptionDialogDefaultButton.Button3:
			button3.Focus();
			base.ActiveControl = button3;
			break;
		case ExceptionDialogDefaultButton.Button4:
			button4.Focus();
			base.ActiveControl = button4;
			break;
		case ExceptionDialogDefaultButton.Button5:
			button5.Focus();
			base.ActiveControl = button5;
			break;
		}
	}

	private void OnFormClosing(object sender, FormClosingEventArgs e)
	{
		switch (_buttons)
		{
		case ExceptionDialogButtons.OK:
			base.DialogResult = DialogResult.OK;
			break;
		case ExceptionDialogButtons.OKCancel:
		case ExceptionDialogButtons.YesNoCancel:
		case ExceptionDialogButtons.RetryCancel:
			break;
		case ExceptionDialogButtons.YesNo:
		case ExceptionDialogButtons.AbortRetryIgnore:
		case ExceptionDialogButtons.Custom:
			if (!_isButtonPressed)
			{
				e.Cancel = true;
				ExceptionDialogNativeMethods.MessageBeep(BeepType.Hand);
			}
			break;
		}
	}

	private void OnKeyDown(object sender, KeyEventArgs e)
	{
		if (e.Modifiers == Keys.Control && (e.KeyData & Keys.C) == Keys.C)
		{
			CopyToClipboard();
			e.Handled = true;
		}
	}

	private void btnHelp_Click(object sender, EventArgs e)
	{
	}

	private void btnCopy_Click(object sender, EventArgs e)
	{
		CopyToClipboard();
	}

	private void btnAdvanced_Click(object sender, EventArgs e)
	{
		ShowDetails();
	}

	private void button_Click(object sender, EventArgs e)
	{
		if (_buttons == ExceptionDialogButtons.Custom)
		{
			if (sender == button1)
			{
				_customDialogResult = ExceptionDialogResult.Button1;
			}
			else if (sender == button2)
			{
				_customDialogResult = ExceptionDialogResult.Button2;
			}
			else if (sender == button3)
			{
				_customDialogResult = ExceptionDialogResult.Button3;
			}
			else if (sender == button4)
			{
				_customDialogResult = ExceptionDialogResult.Button4;
			}
			else
			{
				_customDialogResult = ExceptionDialogResult.Button5;
			}
		}
		else
		{
			_customDialogResult = ExceptionDialogResult.None;
		}
		_isButtonPressed = true;
		Close();
	}

	private void pnlIcon_Paint(object sender, PaintEventArgs e)
	{
		if (_customSymbol != null)
		{
			e.Graphics.DrawImageUnscaled(_customSymbol, new Point(0, 0));
		}
		else if (_iconSymbol != null)
		{
			e.Graphics.DrawIcon(_iconSymbol, 0, 0);
		}
	}

	public void PrepareToShow()
	{
		if (_exMessage == null)
		{
			throw new Exception();
		}
		if (_buttonTextArray == null || _buttonTextArray.Length < 5)
		{
			throw new Exception(Resources.ExBoxCantComplete);
		}
		if (_isButtonPressed)
		{
			throw new Exception(Resources.ExBoxCantReuseObject);
		}
		if (string.IsNullOrEmpty(Caption))
		{
			Caption = _exMessage.Source;
		}
		SuspendLayout();
		toolStrip1.Visible = _showHelpButton;
		int num = Screen.FromControl(pnlMessage).WorkingArea.Height * 3 / 4;
		pnlMessage.MaximumSize = new Size(pnlMessage.MaximumSize.Width, num);
		pnlMessage.Padding = new Padding(pnlMessage.Padding.Left, pnlMessage.Padding.Top, pnlMessage.Padding.Right + SystemInformation.VerticalScrollBarWidth, pnlMessage.Padding.Bottom);
		InitializeCheckbox();
		InitializeMessage();
		InitializeSymbol();
		InitializeButtons();
		if (!HasTechnicalDetails())
		{
			btnAdvanced.Visible = false;
			btnAdvanced.Enabled = false;
		}
		if (_showHelpButton)
		{
			btnHelp.Visible = false;
		}
		ResumeLayout();
	}

	private void AddAdditionalInfoMessage(int messageCount, string strText, Exception ex)
	{
		int count = pnlAdditional.Controls.Count;
		pnlAdditional.SuspendLayout();
		try
		{
			Label label = new Label();
			label.Name = "lblArrow" + count.ToString(CultureInfo.InvariantCulture);
			label.TabIndex = count++;
			label.Visible = true;
			label.ImageList = imageList;
			label.ImageIndex = 0;
			label.Size = new Size(16, 16);
			label.Anchor = AnchorStyles.Top | AnchorStyles.Left;
			label.Margin = new Padding(0);
			label.AutoSize = false;
			pnlAdditional.Controls.Add(label, messageCount, messageCount);
			Label label2 = new Label();
			label2.Name = "lblMessage" + count.ToString(CultureInfo.InvariantCulture);
			label2.AutoSize = true;
			label2.TabIndex = count;
			label2.Text = strText;
			label2.Visible = true;
			label2.Margin = new Padding(0, 0, 0, 8);
			label2.MaximumSize = new Size(pnlAdditional.GetPreferredSize(Size.Empty).Width - (messageCount + 1) * 20, 0);
			label2.Anchor = AnchorStyles.Top | AnchorStyles.Left;
			label2.Tag = ex;
			pnlAdditional.Controls.Add(label2, messageCount + 1, messageCount);
			pnlAdditional.SetColumnSpan(label2, pnlAdditional.ColumnStyles.Count - (messageCount + 1));
		}
		finally
		{
			pnlAdditional.ResumeLayout();
		}
	}

	private bool HasTechnicalDetails()
	{
		for (Exception ex = _exMessage; ex != null; ex = ex.InnerException)
		{
			if (ex.StackTrace != null && ex.StackTrace.Length > 0)
			{
				return true;
			}
			foreach (DictionaryEntry datum in ex.Data)
			{
				if (string.Compare((string)datum.Key, 0, "AdvancedInformation", 0, "AdvancedInformation".Length, StringComparison.InvariantCulture) == 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	private void ShowDetails()
	{
		try
		{
			using AdvancedInfoForm advancedInfoForm = new AdvancedInfoForm();
			advancedInfoForm.ExceptionDialogForm = this;
			if (base.Parent == null)
			{
				advancedInfoForm.ShowInTaskbar = true;
				advancedInfoForm.StartPosition = FormStartPosition.CenterScreen;
				advancedInfoForm.ShowDialog(this);
			}
			else
			{
				advancedInfoForm.StartPosition = FormStartPosition.CenterParent;
				advancedInfoForm.ShowDialog(base.Parent);
			}
		}
		catch (Exception exError)
		{
			ShowError(Resources.ExBoxCantShowTechnicalDetailsError, exError);
		}
	}

	public string BuildTechnicalDetails(Exception ex)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (ex.StackTrace != null && ex.StackTrace.Length > 0)
		{
			stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0}------------------------------{0}", Environment.NewLine);
			stringBuilder.AppendFormat(Resources.ExBoxCodeLocation);
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append(ex.StackTrace);
			stringBuilder.Append(Environment.NewLine);
		}
		return stringBuilder.ToString();
	}

	public string BuildAdvancedInfo(Exception ex, AdvancedInfoType advancedInfoType)
	{
		if (ex == null)
		{
			throw new ArgumentNullException("ex");
		}
		StringBuilder stringBuilder = new StringBuilder();
		if (advancedInfoType == AdvancedInfoType.All)
		{
			stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "==================================={0}{0}", Environment.NewLine);
		}
		if (!string.IsNullOrEmpty(ex.Message) && (advancedInfoType == AdvancedInfoType.All || advancedInfoType == AdvancedInfoType.Message))
		{
			stringBuilder.Append(ex.Message);
			if (!string.IsNullOrEmpty(ex.Source))
			{
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, " ({0})", ex.Source);
			}
			if (advancedInfoType == AdvancedInfoType.All)
			{
				stringBuilder.Append(Environment.NewLine);
				stringBuilder.Append(Environment.NewLine);
			}
		}
		string value = BuildAdvancedInfoData(ex);
		if (!string.IsNullOrEmpty(value))
		{
			if (advancedInfoType == AdvancedInfoType.All)
			{
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "------------------------------{0}", Environment.NewLine);
			}
			if (advancedInfoType == AdvancedInfoType.All || advancedInfoType == AdvancedInfoType.Data)
			{
				stringBuilder.Append(value);
			}
			if (advancedInfoType == AdvancedInfoType.All)
			{
				stringBuilder.Append(Environment.NewLine);
				stringBuilder.Append(Environment.NewLine);
			}
		}
		if (!string.IsNullOrEmpty(ex.StackTrace))
		{
			if (advancedInfoType == AdvancedInfoType.All)
			{
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "------------------------------{0}", Environment.NewLine);
				stringBuilder.AppendFormat(CultureInfo.CurrentCulture, "{0}{1}{1}", Resources.ExBoxCodeLocation, Environment.NewLine);
			}
			if (advancedInfoType == AdvancedInfoType.All || advancedInfoType == AdvancedInfoType.StackTrace)
			{
				stringBuilder.Append(ex.StackTrace);
			}
			if (advancedInfoType == AdvancedInfoType.All)
			{
				stringBuilder.Append(Environment.NewLine);
				stringBuilder.Append(Environment.NewLine);
			}
		}
		return stringBuilder.ToString();
	}

	private string BuildAdvancedInfoData(Exception ex)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (DictionaryEntry datum in ex.Data)
		{
			if (datum.Key != null && datum.Key.GetType() == typeof(string) && datum.Value != null && string.Compare((string)datum.Key, 0, "AdvancedInformation.", 0, "AdvancedInformation.".Length, ignoreCase: false, CultureInfo.CurrentCulture) == 0)
			{
				stringBuilder.AppendFormat(CultureInfo.CurrentCulture, "{0} = {1}{2}", ((string)datum.Key).Substring("AdvancedInformation.".Length), datum.Value.ToString(), Environment.NewLine);
			}
		}
		return stringBuilder.ToString();
	}

	public string BuildMessageText(bool isInternal)
	{
		if (_exMessage == null)
		{
			return string.Empty;
		}
		string value = string.Format("{0}------------------------------{0}", Environment.NewLine);
		StringBuilder stringBuilder = new StringBuilder();
		if (isInternal)
		{
			stringBuilder.AppendFormat(CultureInfo.CurrentCulture, "{0} {1}", Resources.ExBoxMessageTitle, Caption);
		}
		int num = 1;
		for (Exception ex = _exMessage; ex != null; ex = ex.InnerException)
		{
			if (isInternal || num > 1)
			{
				stringBuilder.Append(value);
			}
			if (_exMessage.InnerException != null && num == 2)
			{
				stringBuilder.Append(Resources.ExBoxAdditionalInfo);
				stringBuilder.Append(Environment.NewLine);
			}
			if (isInternal || num > 1)
			{
				stringBuilder.Append(Environment.NewLine);
			}
			if (string.IsNullOrEmpty(ex.Message))
			{
				stringBuilder.Append(Resources.ExBoxCantComplete);
			}
			else
			{
				stringBuilder.Append(ex.Message);
			}
			if (!string.IsNullOrEmpty(ex.Source) && num != 1 && Caption != ex.Source)
			{
				stringBuilder.AppendFormat(CultureInfo.CurrentCulture, " ({0})", ex.Source);
			}
			stringBuilder.Append(Environment.NewLine);
			num++;
		}
		if (isInternal)
		{
			stringBuilder.Append(value);
			stringBuilder.Append(Resources.ExBoxButtons);
			stringBuilder.Append(Environment.NewLine);
			for (int i = 0; i < _buttonCount; i++)
			{
				stringBuilder.Append(Environment.NewLine);
				stringBuilder.Append(_buttonTextArray[i]);
			}
			stringBuilder.Append(value);
		}
		return stringBuilder.ToString();
	}

	private void CopyToClipboard()
	{
		string text = BuildMessageText(isInternal: true);
		if (this.OnCopyToClipboardInternal != null)
		{
			CopyToClipboardEventArgs e = new CopyToClipboardEventArgs(text);
			this.OnCopyToClipboardInternal(this, e);
			if (e.EventHandled)
			{
				return;
			}
		}
		try
		{
			Clipboard.SetDataObject(text, copy: true);
		}
		catch (Exception exError)
		{
			ShowError(Resources.ExBoxCopyToClipboardError, exError);
		}
	}

	public void SetButtonText(string[] value)
	{
		_buttonTextArray = value;
		for (int i = 0; i <= 5; i++)
		{
			if (i == 5)
			{
				_buttonCount = 5;
				break;
			}
			if (string.IsNullOrEmpty(_buttonTextArray[i]))
			{
				_buttonCount = i;
				break;
			}
		}
	}

	public void Show(Control owner)
	{
		if (owner == null)
		{
			base.StartPosition = FormStartPosition.CenterScreen;
			base.ShowInTaskbar = true;
		}
		else
		{
			base.StartPosition = FormStartPosition.CenterParent;
			base.Parent = owner;
			CenterToParent();
		}
		Show();
	}

	public void ShowError(string message, Exception exError)
	{
		Exception ex = new Exception(message, exError);
		ex.Source = Text;
		new ExceptionDialog(ex).Show(base.Parent);
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
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Common.Controls.ExceptionDialogForm));
		this.pnlForm = new System.Windows.Forms.TableLayoutPanel();
		this.pnlIcon = new System.Windows.Forms.Panel();
		this.pnlMessage = new System.Windows.Forms.TableLayoutPanel();
		this.lblTopMessage = new System.Windows.Forms.LinkLabel();
		this.lblAdditionalInfo = new System.Windows.Forms.Label();
		this.pnlAdditional = new System.Windows.Forms.TableLayoutPanel();
		this.chkDontShow = new VideoOS.Common.Controls.WrappingCheckBox();
		this.grpSeparator = new System.Windows.Forms.GroupBox();
		this.pnlButtons = new System.Windows.Forms.TableLayoutPanel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnHelp = new System.Windows.Forms.ToolStripButton();
		this.btnCopy = new System.Windows.Forms.ToolStripButton();
		this.btnAdvanced = new System.Windows.Forms.ToolStripButton();
		this.button1 = new System.Windows.Forms.Button();
		this.button2 = new System.Windows.Forms.Button();
		this.button3 = new System.Windows.Forms.Button();
		this.button4 = new System.Windows.Forms.Button();
		this.button5 = new System.Windows.Forms.Button();
		this.imageList = new System.Windows.Forms.ImageList(this.components);
		this.pnlForm.SuspendLayout();
		this.pnlMessage.SuspendLayout();
		this.pnlButtons.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		resources.ApplyResources(this.pnlForm, "pnlForm");
		this.pnlForm.Controls.Add(this.pnlIcon, 0, 0);
		this.pnlForm.Controls.Add(this.pnlMessage, 1, 0);
		this.pnlForm.Controls.Add(this.chkDontShow, 1, 1);
		this.pnlForm.Controls.Add(this.grpSeparator, 0, 2);
		this.pnlForm.Controls.Add(this.pnlButtons, 0, 3);
		this.pnlForm.Name = "pnlForm";
		resources.ApplyResources(this.pnlIcon, "pnlIcon");
		this.pnlIcon.Name = "pnlIcon";
		this.pnlIcon.Paint += new System.Windows.Forms.PaintEventHandler(pnlIcon_Paint);
		resources.ApplyResources(this.pnlMessage, "pnlMessage");
		this.pnlMessage.Controls.Add(this.lblTopMessage, 0, 0);
		this.pnlMessage.Controls.Add(this.lblAdditionalInfo, 0, 1);
		this.pnlMessage.Controls.Add(this.pnlAdditional, 0, 2);
		this.pnlMessage.Name = "pnlMessage";
		resources.ApplyResources(this.lblTopMessage, "lblTopMessage");
		this.lblTopMessage.Name = "lblTopMessage";
		resources.ApplyResources(this.lblAdditionalInfo, "lblAdditionalInfo");
		this.lblAdditionalInfo.Name = "lblAdditionalInfo";
		this.pnlMessage.SetRowSpan(this.lblAdditionalInfo, 2);
		resources.ApplyResources(this.pnlAdditional, "pnlAdditional");
		this.pnlAdditional.Name = "pnlAdditional";
		resources.ApplyResources(this.chkDontShow, "chkDontShow");
		this.chkDontShow.Name = "chkDontShow";
		resources.ApplyResources(this.grpSeparator, "grpSeparator");
		this.pnlForm.SetColumnSpan(this.grpSeparator, 2);
		this.grpSeparator.Name = "grpSeparator";
		this.grpSeparator.TabStop = false;
		resources.ApplyResources(this.pnlButtons, "pnlButtons");
		this.pnlForm.SetColumnSpan(this.pnlButtons, 2);
		this.pnlButtons.Controls.Add(this.toolStrip1, 0, 0);
		this.pnlButtons.Controls.Add(this.button1, 1, 0);
		this.pnlButtons.Controls.Add(this.button2, 2, 0);
		this.pnlButtons.Controls.Add(this.button3, 3, 0);
		this.pnlButtons.Controls.Add(this.button4, 4, 0);
		this.pnlButtons.Controls.Add(this.button5, 5, 0);
		this.pnlButtons.Name = "pnlButtons";
		this.toolStrip1.AllowMerge = false;
		this.toolStrip1.CanOverflow = false;
		resources.ApplyResources(this.toolStrip1, "toolStrip1");
		this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnHelp, this.btnCopy, this.btnAdvanced });
		this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
		this.toolStrip1.TabStop = true;
		this.btnHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		resources.ApplyResources(this.btnHelp, "btnHelp");
		this.btnHelp.Name = "btnHelp";
		this.btnHelp.Click += new System.EventHandler(btnHelp_Click);
		this.btnCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		resources.ApplyResources(this.btnCopy, "btnCopy");
		this.btnCopy.Name = "btnCopy";
		this.btnCopy.Click += new System.EventHandler(btnCopy_Click);
		this.btnAdvanced.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		resources.ApplyResources(this.btnAdvanced, "btnAdvanced");
		this.btnAdvanced.Name = "btnAdvanced";
		this.btnAdvanced.Click += new System.EventHandler(btnAdvanced_Click);
		resources.ApplyResources(this.button1, "button1");
		this.button1.Name = "button1";
		this.button1.Click += new System.EventHandler(button_Click);
		resources.ApplyResources(this.button2, "button2");
		this.button2.Name = "button2";
		this.button2.Click += new System.EventHandler(button_Click);
		resources.ApplyResources(this.button3, "button3");
		this.button3.Name = "button3";
		this.button3.Click += new System.EventHandler(button_Click);
		resources.ApplyResources(this.button4, "button4");
		this.button4.Name = "button4";
		this.button4.Tag = "z";
		this.button4.Click += new System.EventHandler(button_Click);
		resources.ApplyResources(this.button5, "button5");
		this.button5.Name = "button5";
		this.button5.Click += new System.EventHandler(button_Click);
		this.imageList.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageList.ImageStream");
		this.imageList.TransparentColor = System.Drawing.Color.Transparent;
		this.imageList.Images.SetKeyName(0, "Arrow.bmp");
		this.imageList.Images.SetKeyName(1, "Copy 16 n i8.ico");
		this.imageList.Images.SetKeyName(2, "Document Settings 16 n i8.ico");
		this.imageList.Images.SetKeyName(3, "Help Blue 16 n i8.ico");
		resources.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.pnlForm);
		this.DoubleBuffered = true;
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
		base.KeyPreview = true;
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "ExceptionDialogForm";
		base.ShowInTaskbar = false;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(OnFormClosing);
		base.Load += new System.EventHandler(OnLoad);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(OnKeyDown);
		this.pnlForm.ResumeLayout(false);
		this.pnlForm.PerformLayout();
		this.pnlMessage.ResumeLayout(false);
		this.pnlMessage.PerformLayout();
		this.pnlButtons.ResumeLayout(false);
		this.pnlButtons.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
