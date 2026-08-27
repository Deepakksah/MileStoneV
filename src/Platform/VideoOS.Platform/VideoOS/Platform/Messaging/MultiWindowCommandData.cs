namespace VideoOS.Platform.Messaging;

public struct MultiWindowCommandData
{
	public FQID Screen;

	public FQID Window;

	public FQID View;

	public int X;

	public int Y;

	public int Width;

	public int Height;

	public string MultiWindowCommand;

	public bool PlaybackSupportedInFloatingWindow;
}
