using System;

namespace VideoOS.Platform.Messaging;

public class PlaybackTimeInformationData
{
	public FQID Item;

	public DateTime PreviousTime;

	public DateTime CurrentTime;

	public DateTime NextTime;
}
