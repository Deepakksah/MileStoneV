using System;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
public enum ServerTaskDiffGramTypeDto
{
	DoesNotApply,
	Insert,
	Update,
	Delete
}
