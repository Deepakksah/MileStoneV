using System;
using System.Runtime.Serialization;
using VideoOS.Installer.Interfaces.Content;

namespace VideoOS.Installer.UI.Framework;

[Serializable]
public class DetailedErrorException : Exception
{
	private Func<string> _lookupErrorPageTitle;

	private Func<ContentLinkText> _lookupErrorPageBody;

	public bool EnablePreviousButton { get; }

	public string ErrorTitle => _lookupErrorPageTitle?.Invoke();

	public ContentLinkText ErrorBody => _lookupErrorPageBody?.Invoke();

	public DetailedErrorException(Func<string> lookupErrorPageTitle, Func<ContentLinkText> lookupErrorPageBody, bool enablePreviousButton)
		: this(lookupErrorPageTitle, lookupErrorPageBody, enablePreviousButton, null)
	{
	}

	public DetailedErrorException(Func<string> lookupErrorPageTitle, Func<ContentLinkText> lookupErrorPageBody, bool enablePreviousButton, Exception innerException)
		: base(lookupErrorPageTitle?.Invoke() + Environment.NewLine + lookupErrorPageBody?.Invoke().Text, innerException)
	{
		_lookupErrorPageTitle = lookupErrorPageTitle;
		_lookupErrorPageBody = lookupErrorPageBody;
		EnablePreviousButton = enablePreviousButton;
	}

	public DetailedErrorException()
	{
	}

	public DetailedErrorException(string message)
		: base(message)
	{
		_lookupErrorPageTitle = () => "Error";
		_lookupErrorPageBody = () => new ContentLinkText(message);
	}

	public DetailedErrorException(string message, bool enablePreviousButton)
		: base(message)
	{
		_lookupErrorPageTitle = () => "Error";
		_lookupErrorPageBody = () => new ContentLinkText(message);
		EnablePreviousButton = enablePreviousButton;
	}

	public DetailedErrorException(string message, Exception innerException)
		: base(message, innerException)
	{
		_lookupErrorPageTitle = () => "Error";
		_lookupErrorPageBody = () => new ContentLinkText(message);
	}

	protected DetailedErrorException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		base.GetObjectData(info, context);
	}
}
