using System;
using System.Windows.Input;
using VideoOS.Platform.UI.Commands;

namespace VideoOS.Platform.UI.Controls;

public abstract class VideoOSDialogActionButtonBase : VideoOSDialogActionBase
{
	private string __text;

	private VideoOSIconSourceBase __iconSource;

	public string Text
	{
		get
		{
			return __text;
		}
		set
		{
			if (__text != value)
			{
				__text = value;
				OnPropertyChanged("Text");
			}
		}
	}

	public VideoOSIconSourceBase IconSource
	{
		get
		{
			return __iconSource;
		}
		set
		{
			if (__iconSource != value)
			{
				__iconSource = value;
				OnPropertyChanged("IconSource");
			}
		}
	}

	public bool ActivateOnSubmit { get; set; }

	public ICommand ClickCommand { get; }

	public override bool CanResize => !string.IsNullOrWhiteSpace(Text);

	public event EventHandler Click;

	public VideoOSDialogActionButtonBase()
	{
		ClickCommand = new DelegateCommand(delegate
		{
			this.Click?.Invoke(this, EventArgs.Empty);
		});
	}
}
