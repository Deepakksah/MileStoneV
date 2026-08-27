using System;
using System.Security;
using System.Windows;
using System.Windows.Controls;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public static class PasswordBoxAttachedProperties
{
	public static readonly DependencyProperty EncryptedPasswordProperty = DependencyProperty.RegisterAttached("EncryptedPassword", typeof(SecureString), typeof(PasswordBoxAttachedProperties), new PropertyMetadata(OnCurrentReadingChanged));

	public static SecureString GetEncryptedPassword(DependencyObject obj)
	{
		if (obj == null)
		{
			throw new ArgumentNullException("obj");
		}
		return (SecureString)obj.GetValue(EncryptedPasswordProperty);
	}

	public static void SetEncryptedPassword(DependencyObject obj, SecureString value)
	{
		if (obj == null)
		{
			throw new ArgumentNullException("obj");
		}
		obj.SetValue(EncryptedPasswordProperty, value);
	}

	private static void OnCurrentReadingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		SecureString secureString = e.NewValue as SecureString;
		if (!(e.OldValue is SecureString) && secureString != null)
		{
			(d as PasswordBox).Password = secureString.ConvertToUnsecureString();
		}
	}
}
