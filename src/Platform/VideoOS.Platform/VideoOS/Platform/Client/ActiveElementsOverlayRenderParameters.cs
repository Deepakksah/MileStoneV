using System;

namespace VideoOS.Platform.Client;

public sealed class ActiveElementsOverlayRenderParameters
{
	[Obsolete("Please use Placement instead")]
	public bool FollowDigitalZoom
	{
		get
		{
			return Placement == OverlayPlacement.FollowDigitalZoom;
		}
		set
		{
			Placement = (value ? OverlayPlacement.FollowDigitalZoom : OverlayPlacement.FollowImageViewport);
		}
	}

	public int ZOrder { get; set; }

	public bool ShowAlways { get; set; }

	public OverlayPlacement Placement { get; set; }

	public ActiveElementsOverlayRenderParameters()
	{
		Placement = OverlayPlacement.FollowDigitalZoom;
		ZOrder = 0;
		ShowAlways = false;
	}
}
