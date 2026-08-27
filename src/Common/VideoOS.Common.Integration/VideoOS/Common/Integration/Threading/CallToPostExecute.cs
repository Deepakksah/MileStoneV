using System;

namespace VideoOS.Common.Integration.Threading;

[Flags]
public enum CallToPostExecute
{
	Never = 0,
	WhenWorkItemCanceled = 1,
	WhenWorkItemNotCanceled = 2,
	Always = 3
}
