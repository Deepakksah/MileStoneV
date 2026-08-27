using System;

namespace VideoOS.Installer.UI.Framework;

public class UIFrameworkErrorException : Exception
{
	private bool _enablePreviousButton;

	public bool EnablePreviousButton => _enablePreviousButton;

	public UIFrameworkErrorException(string message)
		: base(message)
	{
	}

	public UIFrameworkErrorException(string message, bool enablePreviousButtong)
		: base(message)
	{
		_enablePreviousButton = enablePreviousButtong;
	}

	public UIFrameworkErrorException(Exception ex, bool enablePreviousButtong)
		: base(ex.Message, ex)
	{
		_enablePreviousButton = enablePreviousButtong;
	}

	public UIFrameworkErrorException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
