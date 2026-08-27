using System.Collections.Generic;

namespace VideoOS.Mobile.Portable.MetaChannel;

public class CommandMetaData
{
	public IEnumerable<string> RequiredInputParams { get; set; }

	public bool CanSkipIfChannelIsClosed { get; set; }

	public ProcessingContext ProcessingContext { get; set; }

	public CommandPriority CommandPriority { get; set; }

	public IEnumerable<Command.CommandKinds> RelatedCommands { get; set; }

	public Dictionary<string, int> MaxParameterLengths { get; set; } = new Dictionary<string, int>();

	public IEnumerable<string> UnsanitizedParameters { get; set; } = new List<string>();
}
