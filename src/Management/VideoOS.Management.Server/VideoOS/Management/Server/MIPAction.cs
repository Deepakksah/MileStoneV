using System;
using System.Text.RegularExpressions;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public sealed class MIPAction
{
	public string Id { get; set; }

	public string Name { get; set; }

	public string DefiningPluginId { get; set; }

	public string DefiningPluginName { get; set; }

	public string SelectionText { get; set; }

	public string DescriptionText { get; set; }

	public string PickerDefaultText { get; set; }

	public string MIPKinds { get; set; }

	public string PickerDefaultText2 { get; set; }

	public string MIPKinds2 { get; set; }

	public DateTime CreatedDate { get; set; }

	public DateTime LastModified { get; set; }

	public static MIPAction[] QueryMIPActions(IGeminiRequestContext requestContext)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		using IMIPActionComponent iMIPActionComponent = requestContext.CreateMIPActionComponent();
		return iMIPActionComponent.QueryMIPActions();
	}

	public static MIPAction QueryMIPAction(IGeminiRequestContext requestContext, string mipActionId)
	{
		Validation.CheckObjectId(ref mipActionId, "mipActionId", allowNullOrEmpty: false);
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		using IMIPActionComponent iMIPActionComponent = requestContext.CreateMIPActionComponent();
		return iMIPActionComponent.QueryMIPAction(mipActionId);
	}

	public static MIPAction[] QueryMIPActionsByIds(IGeminiRequestContext requestContext, string[] mipActionIds)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		using IMIPActionComponent iMIPActionComponent = requestContext.CreateMIPActionComponent();
		return iMIPActionComponent.QueryMIPActionsByIds(mipActionIds);
	}

	public static MIPAction CreateMIPAction(IGeminiRequestContext requestContext, string id, string name, string definingPluginId, string definingPluginName, string selectionText, string descriptionText, string pickerText, string mipKinds, string pickerText2, string mipKinds2)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		Validation.CheckObjectId(id, "id", allowNullOrEmpty: false);
		Validation.CheckStringNotNullOrWhiteSpace(name, "name");
		Validation.CheckObjectId(definingPluginId, "definingPluginId", allowNullOrEmpty: false);
		Validation.CheckStringNotNullOrWhiteSpace(definingPluginName, "definingPluginName");
		Validation.CheckStringNotNullOrWhiteSpace(selectionText, "selectionText");
		Validation.CheckStringNotNullOrWhiteSpace(descriptionText, "descriptionText");
		id = GeminiResourceComponent.GetGuid(id);
		if (!string.IsNullOrEmpty(mipKinds))
		{
			MIPKinds mIPKinds = new MIPKinds(mipKinds);
			if (mIPKinds.MIPKindList != null && mIPKinds.MIPKindList.Length != 0)
			{
				Validation.CheckStringNotNullOrWhiteSpace(pickerText, "pickerText");
				if (!ContainsSelectionCurlyBrackets(descriptionText))
				{
					throw new MIPActionDescriptionTextNotValidException();
				}
			}
			else if (descriptionText.Contains("{0}"))
			{
				throw new MIPActionDescriptionTextNotValidException();
			}
		}
		else if (descriptionText.Contains("{0}"))
		{
			throw new MIPActionDescriptionTextNotValidException();
		}
		if (!string.IsNullOrEmpty(mipKinds2))
		{
			MIPKinds mIPKinds2 = new MIPKinds(mipKinds2);
			if (mIPKinds2.MIPKindList != null && mIPKinds2.MIPKindList.Length != 0)
			{
				Validation.CheckStringNotNullOrWhiteSpace(pickerText2, "pickerText2");
				if (!descriptionText.Contains("{1}"))
				{
					throw new MIPActionDescriptionTextNotValidException();
				}
			}
		}
		using IMIPActionComponent iMIPActionComponent = requestContext.CreateMIPActionComponent();
		iMIPActionComponent.CreateMIPAction(id, name, definingPluginId, definingPluginName, selectionText, descriptionText, pickerText, mipKinds, pickerText2, mipKinds2);
		return FindMIPAction(requestContext, iMIPActionComponent, id);
	}

	private static bool ContainsSelectionCurlyBrackets(string descriptionText)
	{
		if (!descriptionText.Contains("{0}"))
		{
			return false;
		}
		return new Regex("^[^{}]*(\\{0\\}([^{}]*\\{1\\})?|\\{1\\}[^{}]*\\{0\\})[^{}]*$").Match(descriptionText).Success;
	}

	public static MIPAction UpdateMIPAction(IGeminiRequestContext requestContext, string id, string name, string definingPluginId, string definingPluginName, string selectionText, string descriptionText, string pickerText, string mipKinds, string pickerText2, string mipKinds2)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		Validation.CheckName(name, "name");
		Validation.CheckObjectId(definingPluginId, "definingPluginId", allowNullOrEmpty: false);
		Validation.CheckName(definingPluginName, "definingPluginName");
		using IMIPActionComponent iMIPActionComponent = requestContext.CreateMIPActionComponent();
		iMIPActionComponent.UpdateMIPAction(id, name, definingPluginId, definingPluginName, selectionText, descriptionText, pickerText, mipKinds, pickerText2, mipKinds2);
		return FindMIPAction(requestContext, iMIPActionComponent, id);
	}

	public static void DeleteMIPAction(IGeminiRequestContext requestContext, string mipActionId)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		Validation.CheckObjectId(ref mipActionId, "mipActionId", allowNullOrEmpty: false);
		using IMIPActionComponent iMIPActionComponent = requestContext.CreateMIPActionComponent();
		iMIPActionComponent.DeleteMIPAction(mipActionId);
	}

	private static MIPAction FindMIPAction(IGeminiRequestContext requestContext, IMIPActionComponent mipComponent, string mipActionId)
	{
		bool flag = false;
		try
		{
			if (mipComponent == null)
			{
				mipComponent = requestContext.CreateMIPActionComponent();
				flag = true;
			}
			return mipComponent.QueryMIPAction(mipActionId);
		}
		finally
		{
			if (flag)
			{
				mipComponent.Dispose();
			}
		}
	}
}
