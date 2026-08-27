namespace VideoOS.Platform.Client;

public sealed class ShapesOverlayRenderParameters
{
	public bool FollowDigitalZoom { get; set; }

	public int ZOrder { get; set; }

	public ShapesOverlayRenderParameters()
	{
		FollowDigitalZoom = true;
		ZOrder = 0;
	}
}
