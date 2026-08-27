namespace VideoOS.Platform.Messaging;

public class MultiWindowCommand
{
	public const string CloseAllWindows = "CloseAllWindows";

	public const string CloseSelectedWindow = "CloseSelectedWindow";

	public const string MaximizeWindow = "MaximizeWindow";

	public const string MinimizeWindow = "MinimizeWindow";

	public const string OpenFloatingWindow = "OpenFloatingWindow";

	public const string OpenFullScreenWindow = "OpenFullScreenWindow";

	public const string RestoreWindow = "RestoreWindow";

	public const string SetViewInWindow = "SetViewInWindow";

	public const string SetWindowLocation = "SetWindowLocation";

	public const string SetWindowSize = "SetWindowSize";

	public const string SetNextViewInWindow = "SetNextViewInWindow";

	public const string SetPreviousViewInWindow = "SetPreviousViewInWindow";

	public const string SelectWindow = "SelectWindow";

	public const string LinkWindow = "LinkWindow";

	public const string UnlinkWindow = "UnlinkWindow";
}
