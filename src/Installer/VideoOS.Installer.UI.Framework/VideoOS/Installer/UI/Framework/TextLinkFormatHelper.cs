using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using VideoOS.Installer.Interfaces.Content;
using VideoOS.Installer.UI.Framework.Properties;

namespace VideoOS.Installer.UI.Framework;

public static class TextLinkFormatHelper
{
	private class CopyCommand : ICommand
	{
		public event EventHandler CanExecuteChanged = delegate
		{
		};

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			Clipboard.SetText(parameter as string);
		}
	}

	public static readonly DependencyProperty ContentLinkTextProperty = DependencyProperty.RegisterAttached("ContentLinkText", typeof(ContentLinkText), typeof(TextLinkFormatHelper), new UIPropertyMetadata(null, ContentLinkTextChanged));

	public static ContentLinkText GetContentLinkText(DependencyObject obj)
	{
		return (ContentLinkText)(obj?.GetValue(ContentLinkTextProperty));
	}

	public static void SetContentLinkText(DependencyObject obj, IEnumerable<ContentLink> value)
	{
		obj?.SetValue(ContentLinkTextProperty, value);
	}

	private static void ContentLinkTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
	{
		ContentLinkText contentLinkText = e.NewValue as ContentLinkText;
		TextBlock textBlock = sender as TextBlock;
		if (contentLinkText != null && textBlock != null)
		{
			Format(textBlock, contentLinkText);
		}
	}

	private static void Format(TextBlock textBlock, ContentLinkText content)
	{
		textBlock.Inlines.Clear();
		if (content.ContentLinks == null || content.ContentLinks.Count() == 0)
		{
			if (!string.IsNullOrEmpty(content.Text))
			{
				textBlock.Inlines.Add(content.Text);
			}
			return;
		}
		IOrderedEnumerable<ContentLink> orderedEnumerable = content.ContentLinks.OrderBy((ContentLink l) => l.StartIndex);
		int num = 0;
		foreach (ContentLink contentLink in orderedEnumerable)
		{
			if (num < contentLink.StartIndex)
			{
				int length = contentLink.StartIndex - num;
				string text = content.Text.Substring(num, length);
				textBlock.Inlines.Add(text);
			}
			string text2 = content.Text.Substring(contentLink.StartIndex, contentLink.Length);
			Hyperlink hyperlink = new Hyperlink();
			hyperlink.Inlines.Add(text2);
			if (contentLink.ClickAction != null)
			{
				hyperlink.NavigateUri = new Uri("dummy://");
				hyperlink.RequestNavigate += delegate
				{
					contentLink.ClickAction();
				};
			}
			if (contentLink.Uri != null)
			{
				hyperlink.NavigateUri = contentLink.Uri;
				hyperlink.RequestNavigate += Hyperlink_RequestNavigate;
				hyperlink.ContextMenu = new ContextMenu();
				hyperlink.ContextMenu.Items.Add(new MenuItem
				{
					Header = Resources.CopyLink,
					Command = new CopyCommand(),
					CommandParameter = contentLink.Uri.ToString()
				});
			}
			textBlock.Inlines.Add(hyperlink);
			num = contentLink.StartIndex + contentLink.Length;
			if (contentLink.Equals(orderedEnumerable.Last()) && content.Text.Length > num)
			{
				int length2 = content.Text.Length - num;
				string text3 = content.Text.Substring(num, length2);
				textBlock.Inlines.Add(text3);
			}
		}
	}

	private static void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
	{
		if (e.Uri.IsAbsoluteUri)
		{
			Process.Start(e.Uri.ToString());
		}
	}
}
