using System;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.OnlineService;

public class InvitationLinks : IInvitationLinks
{
	private readonly TimeoutUpdateChecker _timeoutUpdateChecker = new TimeoutUpdateChecker(TimeSpan.FromHours(12.0));

	public string AppLink { get; private set; }

	public string StoreLink { get; private set; }

	public InvitationLinks()
	{
		AppLink = InvitationLinksDefaultValues.Instance.AppLink;
		StoreLink = InvitationLinksDefaultValues.Instance.StoreLink;
	}

	public void Update(IAddressBookServiceAsync addressBookService)
	{
		_timeoutUpdateChecker.Update(delegate
		{
			addressBookService.GetInvitationLinks(OnGetInvitationLinksResponse, OnGetInvitationLinksResponse);
		});
	}

	private void OnGetInvitationLinksResponse(GetInvitationLinksResponse getInvitationLinksResponse)
	{
		try
		{
			if (getInvitationLinksResponse != null)
			{
				InvitationParams invitationParams = getInvitationLinksResponse.InvitationParams;
				if (invitationParams.CheckForMask(InvitationParamsHelper.ParamMask.AppLink))
				{
					AppLink = invitationParams.AppLink;
				}
				if (invitationParams.CheckForMask(InvitationParamsHelper.ParamMask.StoreLink))
				{
					StoreLink = invitationParams.StoreLink;
				}
			}
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "InvitationLinks.OnGetInvitationLinksResponse", ex.Message);
		}
	}
}
