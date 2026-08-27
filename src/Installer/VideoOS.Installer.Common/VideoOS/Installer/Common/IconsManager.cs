using System.Drawing;
using System.Reflection;

namespace VideoOS.Installer.Common;

public class IconsManager
{
	private static Image _warningIcon;

	private static Image _questionIcon;

	private static Image _exclamationIcon;

	private static Image _errorIcon;

	public static Image WarningIcon
	{
		get
		{
			if (_warningIcon == null)
			{
				_warningIcon = GetImage("warning_icon.png");
			}
			return _warningIcon;
		}
	}

	public static Image QuestionIcon
	{
		get
		{
			if (_questionIcon == null)
			{
				_questionIcon = GetImage("question_icon.png");
			}
			return _questionIcon;
		}
	}

	public static Image ExclamationIcon
	{
		get
		{
			if (_exclamationIcon == null)
			{
				_exclamationIcon = GetImage("exclamation_icon.png");
			}
			return _exclamationIcon;
		}
	}

	public static Image ErrorIcon
	{
		get
		{
			if (_errorIcon == null)
			{
				_errorIcon = GetImage("error_icon.png");
			}
			return _errorIcon;
		}
	}

	public static Image GetImage(string name)
	{
		try
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string name2 = executingAssembly.GetName().Name;
			return new Bitmap(executingAssembly.GetManifestResourceStream(string.Format("{0}.{1}.{2}", name2, "Icons", name)));
		}
		catch
		{
			return null;
		}
	}
}
