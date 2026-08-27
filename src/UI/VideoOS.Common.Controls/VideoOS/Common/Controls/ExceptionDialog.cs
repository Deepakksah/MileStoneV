using System;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Microsoft.Win32;
using VideoOS.Common.Controls.Properties;

namespace VideoOS.Common.Controls;

public class ExceptionDialog
{
	private string[] _buttonTextList;

	private int _buttonCount;

	private Exception _exData;

	private bool _beep;

	private string _caption;

	private Exception _message;

	private Exception _innerException;

	private int _messageLevelCount;

	private RegistryKey _checkBoxRegistryKey;

	private string _checkBoxRegistryValue;

	private bool _checkBoxRegistryMeansDoNotShowDialog;

	private string _checkBoxText;

	private bool _showCheckBox;

	private bool _isCheckBoxChecked;

	private bool _showHelpButton;

	private ExceptionDialogButtons _buttons;

	private ExceptionDialogDefaultButton _defaultButton;

	private ExceptionDialogSymbol _symbol;

	private DialogResult _defaultDialogResult;

	private ExceptionDialogResult _customDialogResult;

	private Bitmap _customSymbol;

	private string _helpLink;

	private string _link;

	private int _linkPosition;

	private int _linkLength;

	private string _text;

	private bool _useOwnerFont;

	private Font _font;

	public static string AbortButtonText => Resources.ExBoxAbortButtonText;

	public static string CancelButtonText => Resources.ExBoxCancelButtonText;

	public static string FailButtonText => Resources.ExBoxFailButtonText;

	public static string IgnoreButtonText => Resources.ExBoxIgnoreButtonText;

	public static string NoButtonText => Resources.ExBoxNoButtonText;

	public static string OKButtonText => Resources.ExBoxOKButtonText;

	public static string RetryButtonText => Resources.ExBoxRetryButtonText;

	public static string UesButtonTest => Resources.ExBoxYesButtonText;

	public bool Beep
	{
		get
		{
			return _beep;
		}
		set
		{
			_beep = value;
		}
	}

	public string Caption
	{
		get
		{
			return _caption;
		}
		set
		{
			_caption = value;
		}
	}

	public Exception Message
	{
		get
		{
			return _message;
		}
		set
		{
			_message = value;
		}
	}

	public Exception InnerException
	{
		get
		{
			return _innerException;
		}
		set
		{
			_innerException = value;
		}
	}

	public int MessageLevelDefault
	{
		get
		{
			return _messageLevelCount;
		}
		set
		{
			_messageLevelCount = value;
		}
	}

	public RegistryKey CheckBoxRegistryKey
	{
		get
		{
			return _checkBoxRegistryKey;
		}
		set
		{
			_checkBoxRegistryKey = value;
		}
	}

	public string CheckBoxRegistryValue
	{
		get
		{
			return _checkBoxRegistryValue;
		}
		set
		{
			_checkBoxRegistryValue = value;
		}
	}

	public bool CheckBoxRegistryMeansDoNotShowDialog
	{
		get
		{
			return _checkBoxRegistryMeansDoNotShowDialog;
		}
		set
		{
			_checkBoxRegistryMeansDoNotShowDialog = value;
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

	public bool ShowCheckbox
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

	public bool IsCheckBoxChecked
	{
		get
		{
			return _isCheckBoxChecked;
		}
		set
		{
			_isCheckBoxChecked = value;
		}
	}

	public bool ShowToolBar
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

	public ExceptionDialogButtons Buttons
	{
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

	public DialogResult DefaultDialogResult
	{
		get
		{
			return _defaultDialogResult;
		}
		set
		{
			_defaultDialogResult = value;
		}
	}

	public ExceptionDialogResult CustomDialogResult => _customDialogResult;

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

	public IDictionary Data => _exData.Data;

	public string HelpLink
	{
		get
		{
			return _helpLink;
		}
		set
		{
			_helpLink = value;
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

	public string Text
	{
		get
		{
			return _text;
		}
		set
		{
			_text = value;
		}
	}

	public bool UseOwnerFont
	{
		get
		{
			return _useOwnerFont;
		}
		set
		{
			_useOwnerFont = value;
		}
	}

	public Font Font
	{
		get
		{
			return _font;
		}
		set
		{
			_font = value;
			_useOwnerFont = false;
		}
	}

	public event EventHandler<CopyToClipboardEventArgs> OnCopyToClipboard;

	public ExceptionDialog()
	{
		_buttonTextList = new string[5];
		_defaultDialogResult = DialogResult.OK;
		_checkBoxRegistryValue = string.Empty;
		_checkBoxRegistryMeansDoNotShowDialog = true;
		_text = string.Empty;
		_helpLink = string.Empty;
		_link = string.Empty;
		_linkLength = 0;
		_linkPosition = 0;
		_exData = new Exception();
		_beep = true;
		_showHelpButton = true;
		_caption = string.Empty;
		_checkBoxText = string.Empty;
		_messageLevelCount = -1;
		_symbol = ExceptionDialogSymbol.Warning;
	}

	public ExceptionDialog(string text)
		: this()
	{
		if (text == null)
		{
			throw new ArgumentNullException("text");
		}
		_text = text;
	}

	public ExceptionDialog(string text, string caption)
		: this()
	{
		if (text == null)
		{
			throw new ArgumentNullException("text");
		}
		_text = text;
		_caption = caption;
	}

	public ExceptionDialog(string text, string caption, ExceptionDialogButtons buttons)
		: this()
	{
		if (text == null)
		{
			throw new ArgumentNullException("text");
		}
		_text = text;
		_caption = caption;
		_buttons = buttons;
	}

	public ExceptionDialog(string text, string caption, ExceptionDialogButtons buttons, ExceptionDialogSymbol symbol)
		: this()
	{
		if (text == null)
		{
			throw new ArgumentNullException("text");
		}
		_text = text;
		_caption = caption;
		_buttons = buttons;
		_symbol = symbol;
	}

	public ExceptionDialog(string text, string caption, ExceptionDialogButtons buttons, ExceptionDialogSymbol symbol, ExceptionDialogDefaultButton defaultButton)
		: this()
	{
		if (text == null)
		{
			throw new ArgumentNullException("text");
		}
		_text = text;
		_caption = caption;
		_buttons = buttons;
		_symbol = symbol;
		_defaultButton = defaultButton;
	}

	public ExceptionDialog(Exception exception)
		: this()
	{
		if (exception == null)
		{
			throw new ArgumentNullException("exception");
		}
		_message = exception;
	}

	public ExceptionDialog(Exception exception, ExceptionDialogButtons buttons)
		: this()
	{
		if (exception == null)
		{
			throw new ArgumentNullException("exception");
		}
		_message = exception;
		_buttons = buttons;
	}

	public ExceptionDialog(Exception exception, ExceptionDialogButtons buttons, ExceptionDialogSymbol symbol)
		: this()
	{
		if (exception == null)
		{
			throw new ArgumentNullException("exception");
		}
		_message = exception;
		_buttons = buttons;
		_symbol = symbol;
	}

	public ExceptionDialog(Exception exception, ExceptionDialogButtons buttons, ExceptionDialogSymbol symbol, ExceptionDialogDefaultButton defaultButton)
		: this()
	{
		if (exception == null)
		{
			throw new ArgumentNullException("exception");
		}
		_message = exception;
		_buttons = buttons;
		_symbol = symbol;
		_defaultButton = defaultButton;
	}

	public static string GetMessageText(Exception exception)
	{
		using ExceptionDialogForm exceptionDialogForm = new ExceptionDialogForm();
		exceptionDialogForm.Message = exception;
		return exceptionDialogForm.BuildMessageText(isInternal: false);
	}

	public void SetButtonText(string button1Text)
	{
		SetButtonText(button1Text, null, null, null, null);
	}

	public void SetButtonText(string button1Text, string button2Text)
	{
		SetButtonText(button1Text, button2Text, null, null, null);
	}

	public void SetButtonText(string button1Text, string button2Text, string button3Text)
	{
		SetButtonText(button1Text, button2Text, button3Text, null, null);
	}

	public void SetButtonText(string button1Text, string button2Text, string button3Text, string button4Text)
	{
		SetButtonText(button1Text, button2Text, button3Text, button4Text, null);
	}

	public void SetButtonText(string button1Text, string button2Text, string button3Text, string button4Text, string button5Text)
	{
		_buttonTextList[0] = button1Text;
		_buttonTextList[1] = button2Text;
		_buttonTextList[2] = button3Text;
		_buttonTextList[3] = button4Text;
		_buttonTextList[4] = button5Text;
		if (string.IsNullOrEmpty(button1Text))
		{
			_buttonCount = 0;
		}
		else if (string.IsNullOrEmpty(button2Text))
		{
			_buttonCount = 1;
		}
		else if (string.IsNullOrEmpty(button3Text))
		{
			_buttonCount = 2;
		}
		else if (string.IsNullOrEmpty(button4Text))
		{
			_buttonCount = 3;
		}
		else if (string.IsNullOrEmpty(button5Text))
		{
			_buttonCount = 4;
		}
		else
		{
			_buttonCount = 5;
		}
	}

	public DialogResult Show(IWin32Window owner)
	{
		if (_message == null && string.IsNullOrEmpty(_text))
		{
			throw new ArgumentException("Message");
		}
		if (_buttons < ExceptionDialogButtons.OK || _buttons > ExceptionDialogButtons.Custom)
		{
			throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.InvalidEnumValue, "Buttons", (int)_buttons, typeof(ExceptionDialogButtons)));
		}
		if (_symbol < ExceptionDialogSymbol.None || _symbol > ExceptionDialogSymbol.Hand)
		{
			throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.InvalidEnumValue, "Symbol", (int)_symbol, typeof(ExceptionDialogSymbol)));
		}
		if (_defaultButton < ExceptionDialogDefaultButton.Button1 || _defaultButton > ExceptionDialogDefaultButton.Button5)
		{
			throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.InvalidEnumValue, "DefaultButton", (int)_defaultButton, typeof(ExceptionDialogDefaultButton)));
		}
		if (_buttons == ExceptionDialogButtons.Custom && _buttonCount == 0)
		{
			throw new InvalidOperationException(Resources.ExBoxCustomButtonTextError);
		}
		if (_messageLevelCount != -1 && _messageLevelCount < 1)
		{
			throw new InvalidOperationException(Resources.ExBoxMessageLevelCountError);
		}
		if (_message == null)
		{
			_message = new Exception(_text, _innerException);
			_message.HelpLink = _helpLink;
			foreach (DictionaryEntry datum in _exData.Data)
			{
				_message.Data.Add(datum.Key, datum.Value);
			}
		}
		if (_useOwnerFont)
		{
			try
			{
				if (owner is Form)
				{
					Font = ((Form)owner).Font;
				}
				else if (owner is UserControl)
				{
					Font = ((UserControl)owner).Font;
				}
				else if (owner is Control)
				{
					Font = ((Control)owner).Font;
				}
			}
			catch
			{
			}
		}
		if (_showCheckBox && _checkBoxRegistryKey != null)
		{
			try
			{
				_isCheckBoxChecked = (int)_checkBoxRegistryKey.GetValue(_checkBoxRegistryValue, 0) != 0;
				if (_checkBoxRegistryMeansDoNotShowDialog && _isCheckBoxChecked)
				{
					return _defaultDialogResult;
				}
			}
			catch
			{
			}
		}
		if (string.IsNullOrEmpty(_caption))
		{
			_caption = _message.Source;
		}
		if (string.IsNullOrEmpty(_caption) && owner is Form)
		{
			_caption = ((Form)owner).Text;
		}
		using ExceptionDialogForm exceptionDialogForm = new ExceptionDialogForm();
		exceptionDialogForm.SetButtonText(_buttonTextList);
		exceptionDialogForm.Buttons = _buttons;
		exceptionDialogForm.Caption = _caption;
		exceptionDialogForm.Message = _message;
		exceptionDialogForm.Link = _link;
		exceptionDialogForm.LinkPosition = _linkPosition;
		exceptionDialogForm.LinkLength = _linkLength;
		exceptionDialogForm.Symbol = _symbol;
		exceptionDialogForm.DefaultButton = _defaultButton;
		exceptionDialogForm.DoBeep = _beep;
		exceptionDialogForm.CheckBoxText = _checkBoxText;
		exceptionDialogForm.IsCheckBoxChecked = _isCheckBoxChecked;
		exceptionDialogForm.ShowCheckBox = _showCheckBox;
		exceptionDialogForm.MessageLevelCount = _messageLevelCount;
		exceptionDialogForm.ShowHelpButton = _showHelpButton;
		exceptionDialogForm.ShowInTaskbar = false;
		exceptionDialogForm.OnCopyToClipboardInternal += OnCopyToClipboardEventInternal;
		if (_customSymbol != null)
		{
			exceptionDialogForm.CustomSymbol = _customSymbol;
		}
		if (_font != null)
		{
			exceptionDialogForm.Font = _font;
		}
		if (owner == null)
		{
			exceptionDialogForm.StartPosition = FormStartPosition.CenterScreen;
			exceptionDialogForm.ShowInTaskbar = true;
		}
		else
		{
			exceptionDialogForm.StartPosition = FormStartPosition.CenterParent;
		}
		exceptionDialogForm.PrepareToShow();
		DialogResult result = exceptionDialogForm.ShowDialog(owner);
		if (exceptionDialogForm.ShowCheckBox && _checkBoxRegistryKey != null)
		{
			_checkBoxRegistryKey.SetValue(_checkBoxRegistryValue, exceptionDialogForm.IsCheckBoxChecked ? 1 : 0);
		}
		_isCheckBoxChecked = exceptionDialogForm.IsCheckBoxChecked;
		_customDialogResult = exceptionDialogForm.CustomDialogResult;
		return result;
	}

	public DialogResult Show(IntPtr hwnd, string message, string source, string sourceAppName, string sourceAppVersion, string sourceModule, string sourceMessageId, string sourceLanguage)
	{
		ExceptionMessageBoxParent owner = new ExceptionMessageBoxParent(hwnd);
		Message = new Exception(message);
		Message.Source = source;
		return Show(owner);
	}

	private void OnCopyToClipboardEventInternal(object sender, CopyToClipboardEventArgs e)
	{
		if (this.OnCopyToClipboard != null)
		{
			this.OnCopyToClipboard(this, e);
		}
	}
}
