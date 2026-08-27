using System;

namespace VideoOS.Platform.Messaging;

public class PlaybackData
{
	public const string Begin = "Begin";

	public const string End = "End";

	public const string Goto = "Goto";

	public const string Next = "Next";

	public const string NextSequence = "NextSequence";

	public const string Play = "Play";

	public const string PlayForward = "PlayForward";

	public const string PlayReverse = "PlayReverse";

	public const string PlayStop = "PlayStop";

	public const string Previous = "Previous";

	public const string PreviousSequence = "PreviousSequence";

	[Obsolete("Use the PlaybackPlayModeData instead")]
	public const string PlayMode = "PlayMode";

	public const string Speed = "Speed";
}
