using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using VideoOS.Platform.UI.Controls.MessageBoxes;
using VideoOS.Platform.UI.Extensions;
using VideoOS.Platform.UI.Resources;

namespace VideoOS.Platform.UI.Controls;

public static class VideoOSMessageBox
{
	public static class VideoOSMessageBoxInternals
	{
		public const double MessageBoxDesiredMaxWidth = 440.0;

		public const string HeaderAndMessageTemplateKey = "VideoOSMessageBoxHeaderAndMessageTemplateKey";

		public const string HeaderOnlyTemplateKey = "VideoOSMessageBoxHeaderOnlyTemplateKey";

		public const string MessageOnlyTemplateKey = "VideoOSMessageBoxMessageOnlyTemplateKey";
	}

	public enum Buttons
	{
		OK,
		OKCancel,
		YesNo,
		YesNoCancel,
		Close
	}

	public enum ResultButtons
	{
		None,
		OK,
		Cancel,
		Yes,
		No,
		Close
	}

	public class Result
	{
		public ResultButtons Button { get; }

		public bool RememberChoice { get; }

		public Result(ResultButtons button, bool rememberChoice)
		{
			Button = button;
			RememberChoice = rememberChoice;
		}
	}

	public enum ButtonAppearances
	{
		Normal,
		Destructive
	}

	public class Options
	{
		public string OKButtonText { get; set; }

		public ButtonAppearances OKButtonAppearance { get; set; }

		public string YesButtonText { get; set; }

		public ButtonAppearances YesButtonAppearance { get; set; }

		public string NoButtonText { get; set; }

		public ButtonAppearances NoButtonAppearance { get; set; }

		public string CancelButtonText { get; set; }

		public ButtonAppearances CancelButtonAppearance { get; set; }

		public string CloseButtonText { get; set; }

		public ButtonAppearances CloseButtonAppearance { get; set; }

		public string RememberChoiceCheckBoxText { get; set; }
	}

	private static class NativeMethods
	{
		public const int GWL_EXSTYLE = -20;

		public const int WS_EX_DLGMODALFRAME = 1;

		public const int SWP_NOSIZE = 1;

		public const int SWP_NOMOVE = 2;

		public const int SWP_NOZORDER = 4;

		public const int SWP_FRAMECHANGED = 32;

		[DllImport("user32.dll")]
		public static extern int GetWindowLong(IntPtr hWnd, int index);

		[DllImport("user32.dll")]
		public static extern int SetWindowLong(IntPtr hWnd, int index, int newStyle);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int width, int height, uint flags);
	}

	private static readonly DataTemplate HeaderAndMessageTemplate;

	private static readonly DataTemplate HeaderOnlyTemplate;

	private static readonly DataTemplate MessageOnlyTemplate;

	static VideoOSMessageBox()
	{
		HeaderAndMessageTemplate = (DataTemplate)ThemeManager.Instance.FindStaticResource("VideoOSMessageBoxHeaderAndMessageTemplateKey");
		HeaderOnlyTemplate = (DataTemplate)ThemeManager.Instance.FindStaticResource("VideoOSMessageBoxHeaderOnlyTemplateKey");
		MessageOnlyTemplate = (DataTemplate)ThemeManager.Instance.FindStaticResource("VideoOSMessageBoxMessageOnlyTemplateKey");
	}

	public static Result Show(Window owner, string title, string header, string message, Buttons buttons)
	{
		return Show(owner, title, header, message, buttons, ResultButtons.None, null, showRememberChoiceCheckBox: false, null);
	}

	public static Result Show(Window owner, string title, string header, string message, Buttons buttons, ResultButtons defaultButton)
	{
		return Show(owner, title, header, message, buttons, defaultButton, null, showRememberChoiceCheckBox: false, null);
	}

	public static Result Show(Window owner, string title, string header, string message, Buttons buttons, ResultButtons defaultButton, VideoOSIconSourceBase icon)
	{
		return Show(owner, title, header, message, buttons, defaultButton, icon, showRememberChoiceCheckBox: false, null);
	}

	public static Result Show(Window owner, string title, string header, string message, Buttons buttons, ResultButtons defaultButton, VideoOSIconSourceBase icon, bool showRememberChoiceCheckBox, Options options)
	{
		VideoOSDialog videoOSDialog = new VideoOSDialog
		{
			Title = title,
			ResizeMode = ResizeMode.NoResize,
			SizeToContent = SizeToContent.WidthAndHeight,
			Content = GetContent(header, message, icon),
			Owner = owner,
			WindowStartupLocation = ((owner == null) ? WindowStartupLocation.CenterScreen : WindowStartupLocation.CenterOwner),
			Icon = owner?.Icon,
			MinWidth = 440.0,
			DialogContentMaxWidth = 440.0
		};
		if (videoOSDialog.Icon == null)
		{
			RemoveIcon(videoOSDialog);
		}
		Options options2 = options ?? new Options();
		Collection<VideoOSDialogActionBase> collection = new Collection<VideoOSDialogActionBase>();
		switch (buttons)
		{
		case Buttons.OK:
			collection.Add(CreateDialogButtonAction(primaryButton: true, options2.OKButtonText.ValueOrDefault(Strings.VideoOSMessageBox_ButtonOK), options2.OKButtonAppearance, defaultButton == ResultButtons.OK, ResultButtons.OK, videoOSDialog));
			break;
		case Buttons.OKCancel:
			collection.Add(CreateDialogButtonAction(primaryButton: false, options2.CancelButtonText.ValueOrDefault(Strings.VideoOSMessageBox_ButtonCancel), options2.CancelButtonAppearance, defaultButton == ResultButtons.Cancel, ResultButtons.Cancel, videoOSDialog));
			collection.Add(CreateDialogButtonAction(primaryButton: true, options2.OKButtonText.ValueOrDefault(Strings.VideoOSMessageBox_ButtonOK), options2.OKButtonAppearance, defaultButton == ResultButtons.OK, ResultButtons.OK, videoOSDialog));
			break;
		case Buttons.Close:
			collection.Add(CreateDialogButtonAction(primaryButton: true, options2.CloseButtonText.ValueOrDefault(Strings.VideoOSMessageBox_ButtonClose), options2.CloseButtonAppearance, defaultButton == ResultButtons.Close, ResultButtons.Close, videoOSDialog));
			break;
		case Buttons.YesNo:
			collection.Add(CreateDialogButtonAction(primaryButton: false, options2.NoButtonText.ValueOrDefault(Strings.VideoOSMessageBox_ButtonNo), options2.NoButtonAppearance, defaultButton == ResultButtons.No, ResultButtons.No, videoOSDialog));
			collection.Add(CreateDialogButtonAction(primaryButton: true, options2.YesButtonText.ValueOrDefault(Strings.VideoOSMessageBox_ButtonYes), options2.YesButtonAppearance, defaultButton == ResultButtons.Yes, ResultButtons.Yes, videoOSDialog));
			break;
		case Buttons.YesNoCancel:
			collection.Add(CreateDialogButtonAction(primaryButton: false, options2.CancelButtonText.ValueOrDefault(Strings.VideoOSMessageBox_ButtonCancel), options2.CancelButtonAppearance, defaultButton == ResultButtons.Cancel, ResultButtons.Cancel, videoOSDialog));
			collection.Add(CreateDialogButtonAction(primaryButton: false, options2.NoButtonText.ValueOrDefault(Strings.VideoOSMessageBox_ButtonNo), options2.NoButtonAppearance, defaultButton == ResultButtons.No, ResultButtons.No, videoOSDialog));
			collection.Add(CreateDialogButtonAction(primaryButton: true, options2.YesButtonText.ValueOrDefault(Strings.VideoOSMessageBox_ButtonYes), options2.YesButtonAppearance, defaultButton == ResultButtons.Yes, ResultButtons.Yes, videoOSDialog));
			break;
		}
		VideoOSDialogActionCheckBox videoOSDialogActionCheckBox = null;
		if (showRememberChoiceCheckBox)
		{
			videoOSDialogActionCheckBox = new VideoOSDialogActionCheckBox
			{
				Placement = VideoOSDialogActionBase.Placements.Left,
				Text = options2.RememberChoiceCheckBoxText.ValueOrDefault(Strings.VideoOSMessageBox_CheckBoxRememberChoice)
			};
			collection.Add(videoOSDialogActionCheckBox);
		}
		videoOSDialog.DialogActions = collection;
		videoOSDialog.ShowDialog();
		return new Result((videoOSDialog.Tag != null) ? ((ResultButtons)videoOSDialog.Tag) : ResultButtons.None, videoOSDialogActionCheckBox?.IsChecked ?? false);
	}

	private static FrameworkElement GetContent(string header, string message, VideoOSIconSourceBase icon)
	{
		DataTemplate dataTemplate;
		if (!string.IsNullOrWhiteSpace(header) && !string.IsNullOrWhiteSpace(message))
		{
			dataTemplate = HeaderAndMessageTemplate;
		}
		else if (!string.IsNullOrWhiteSpace(header) && string.IsNullOrWhiteSpace(message))
		{
			dataTemplate = HeaderOnlyTemplate;
		}
		else
		{
			if (!string.IsNullOrWhiteSpace(header) || string.IsNullOrWhiteSpace(message))
			{
				return null;
			}
			dataTemplate = MessageOnlyTemplate;
		}
		FrameworkElement obj = (FrameworkElement)dataTemplate.LoadContent();
		obj.DataContext = new MessageBoxContentDataContext(header, message, icon);
		return obj;
	}

	private static VideoOSDialogActionBase CreateDialogButtonAction(bool primaryButton, string text, ButtonAppearances appearance, bool activateOnSubmit, ResultButtons resultButton, VideoOSDialog dialog)
	{
		VideoOSDialogActionButtonBase obj = (primaryButton ? ((VideoOSDialogActionButtonBase)new VideoOSDialogActionButtonPrimary
		{
			IsDestructive = (appearance == ButtonAppearances.Destructive)
		}) : ((VideoOSDialogActionButtonBase)new VideoOSDialogActionButtonSecondary
		{
			IsDestructive = (appearance == ButtonAppearances.Destructive)
		}));
		obj.Text = text;
		obj.AutomationId = string.Format(CultureInfo.InvariantCulture, "Button{0}", resultButton);
		obj.ActivateOnSubmit = activateOnSubmit;
		obj.Click += delegate
		{
			dialog.Tag = resultButton;
			dialog.Close();
		};
		return obj;
	}

	private static void RemoveIcon(VideoOSDialog dialog)
	{
		IntPtr hWnd = new WindowInteropHelper(dialog).EnsureHandle();
		int windowLong = NativeMethods.GetWindowLong(hWnd, -20);
		NativeMethods.SetWindowLong(hWnd, -20, windowLong | 1);
		NativeMethods.SetWindowPos(hWnd, IntPtr.Zero, 0, 0, 0, 0, 39u);
	}
}
