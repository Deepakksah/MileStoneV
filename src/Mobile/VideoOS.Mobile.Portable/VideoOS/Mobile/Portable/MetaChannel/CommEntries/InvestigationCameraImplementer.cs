using System;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class InvestigationCameraImplementer : InvestigationExportsImplementer, IInvestigationCamera, IInvestigationExports
{
	public Guid CameraId { get; set; }

	public Guid RelatedMicrophone { get; set; }

	public Guid RelatedSpeaker { get; set; }

	public string CameraName { get; set; }

	public InvestigationCameraImplementer()
	{
	}

	public InvestigationCameraImplementer(IInvestigationCamera implementer)
		: base(implementer)
	{
		CameraId = implementer.CameraId;
		CameraName = implementer.CameraName;
		RelatedMicrophone = implementer.RelatedMicrophone;
		RelatedSpeaker = implementer.RelatedSpeaker;
	}
}
