using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using QRCoder;
using QRCoder.Xaml;

namespace VideoOS.Common.Help.UI;

public class HelpNotAvailableDialogViewModel
{
	public string HelpUrl { get; }

	public ImageSource QrCodeImage { get; }

	public ICommand CopyToClipboardCommand { get; }

	public HelpNotAvailableDialogViewModel(string helpUrl)
	{
		if (string.IsNullOrEmpty(helpUrl))
		{
			throw new ArgumentNullException("helpUrl");
		}
		HelpUrl = ModifyUrl(helpUrl);
		QrCodeImage = GenerateQrCode(helpUrl);
		CopyToClipboardCommand = new RelayCommand(CopyToClipboard);
	}

	private void CopyToClipboard()
	{
		Clipboard.SetData(DataFormats.Text, HelpUrl);
	}

	private static string ModifyUrl(string helpUrl)
	{
		UriBuilder uriBuilder = new UriBuilder(helpUrl);
		if (string.IsNullOrWhiteSpace(uriBuilder.Query))
		{
			uriBuilder.Query = "utm_medium=Offline";
		}
		else
		{
			string text = uriBuilder.Query;
			if (text.StartsWith("?"))
			{
				text = text.Substring(1);
			}
			uriBuilder.Query = text + "&utm_medium=Offline";
		}
		return uriBuilder.Uri.ToString();
	}

	private static ImageSource GenerateQrCode(string url)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		QRCodeGenerator val = new QRCodeGenerator();
		try
		{
			QRCodeData val2 = val.CreateQrCode(url, (ECCLevel)2, false, false, (EciMode)0, -1);
			try
			{
				XamlQRCode val3 = new XamlQRCode(val2);
				try
				{
					return val3.GetGraphic(new Size(148.0, 148.0), (Brush)Brushes.Black, (Brush)Brushes.White, true);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}
}
