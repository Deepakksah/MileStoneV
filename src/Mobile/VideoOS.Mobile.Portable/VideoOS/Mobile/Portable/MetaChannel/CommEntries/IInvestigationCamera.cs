using System;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public interface IInvestigationCamera : IInvestigationExports
{
	Guid CameraId { get; }

	string CameraName { get; }

	Guid RelatedMicrophone { get; }

	Guid RelatedSpeaker { get; }
}
