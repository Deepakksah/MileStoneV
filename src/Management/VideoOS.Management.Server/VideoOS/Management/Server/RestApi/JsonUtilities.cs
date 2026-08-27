using System;
using System.Globalization;
using System.Net;
using System.Text;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.ConfigurationApi;
using VideoOS.Platform.JsonHandling;
using VideoOS.Platform.Proxy.RestApi;

namespace VideoOS.Management.Server.RestApi;

public static class JsonUtilities
{
	public static IRestSerializerFactory SerializerFactory { get; set; } = new RestSerializerFactory();

	public static INameConvert NameConvert { get; set; } = new NameConvert();

	public static string JsonEscape(string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return "";
		}
		StringBuilder stringBuilder = new StringBuilder();
		foreach (char c in value)
		{
			if (c == '"' || c == '\\')
			{
				stringBuilder.Append('\\');
				stringBuilder.Append(c);
			}
			else if (c < ' ')
			{
				stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "\\u{0:x4}", (int)c));
			}
			else
			{
				stringBuilder.Append(c);
			}
		}
		return stringBuilder.ToString();
	}

	public static HttpStatusCode ConvertValidateResult(ValidateResult result, string itemType, JsonBuilder jsonBuilder, IRestApiRequestParameters restApiRequestParameters)
	{
		if (result == null)
		{
			throw new ArgumentNullException("result");
		}
		if (jsonBuilder == null)
		{
			throw new ArgumentNullException("jsonBuilder");
		}
		HttpStatusCode result2 = HttpStatusCode.OK;
		if (result.ErrorResults != null && result.ErrorResults.Length != 0)
		{
			ErrorModel errorModel = new ErrorModel();
			errorModel.HttpCode = HttpStatusCode.BadRequest;
			ErrorResult[] errorResults = result.ErrorResults;
			foreach (ErrorResult errorResult in errorResults)
			{
				string key = (string.IsNullOrEmpty(errorResult.ErrorProperty) ? "" : NameConvert.PropertyKeyToField(itemType, errorResult.ErrorProperty));
				if (!string.IsNullOrEmpty(errorResult.ErrorTextId))
				{
					errorModel.AddKeyTextAndTextId(key, errorResult.ErrorText, errorResult.ErrorTextId);
				}
				else
				{
					errorModel.AddKeyText(key, errorResult.ErrorText);
				}
			}
			ConvertErrorModel(errorModel, jsonBuilder);
			result2 = HttpStatusCode.BadRequest;
		}
		if (result.ResultItem != null)
		{
			IConfigurationItemSerializer restSerializer = SerializerFactory.GetRestSerializer(result.ResultItem);
			jsonBuilder.AppendElement("data", restSerializer.Serialize(restApiRequestParameters));
		}
		return result2;
	}

	public static void ConvertErrorModel(ErrorModel errorModel, JsonBuilder jsonBuilder)
	{
		if (errorModel == null)
		{
			throw new ArgumentNullException("errorModel");
		}
		if (jsonBuilder == null)
		{
			throw new ArgumentNullException("jsonBuilder");
		}
		JsonBuilder jsonBuilder2 = new JsonBuilder(array: false);
		if (errorModel.HttpCode != 0)
		{
			jsonBuilder2.AppendKeyValue("httpCode", ((int)errorModel.HttpCode).ToString(CultureInfo.InvariantCulture));
		}
		JsonBuilder jsonBuilder3 = new JsonBuilder(array: true);
		foreach (ErrorModelDetail error in errorModel.Errors)
		{
			JsonBuilder jsonBuilder4 = new JsonBuilder(array: false);
			if (!string.IsNullOrEmpty(error.Key))
			{
				jsonBuilder4.AppendKeyString("property", NameConvert.PropertyKeyToField("", error.Key));
			}
			if (!string.IsNullOrEmpty(error.Text))
			{
				jsonBuilder4.AppendKeyString("errorText", JsonEscape(error.Text));
			}
			if (!string.IsNullOrEmpty(error.TextId))
			{
				jsonBuilder4.AppendKeyString("errorTextId", error.TextId);
			}
			jsonBuilder3.AppendElement(jsonBuilder4.ToJson());
		}
		jsonBuilder2.AppendElement("details", jsonBuilder3.ToJson());
		jsonBuilder.AppendElement("error", jsonBuilder2.ToJson());
	}

	public static void ConvertError(string errorTextId, string errorText, JsonBuilder jsonBuilder)
	{
		ErrorModel errorModel = new ErrorModel();
		errorModel.HttpCode = HttpStatusCode.BadRequest;
		if (!string.IsNullOrEmpty(errorTextId))
		{
			errorModel.AddTextAndTextId(errorText, errorTextId);
		}
		else
		{
			errorModel.AddText(errorText);
		}
		ConvertErrorModel(errorModel, jsonBuilder);
	}

	public static string CheckConvertPath(string text)
	{
		if (text == null)
		{
			return text;
		}
		int num = text.IndexOf('[');
		if (num > 0)
		{
			int num2 = text.IndexOf(']', num + 1);
			if (num2 - num == 37)
			{
				num--;
				while (num > 0 && char.IsLetter(text[num]))
				{
					num--;
				}
				if (num != 0)
				{
					num++;
				}
				num2++;
				ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreatePath(text.Substring(num, num2 - num));
				string text2;
				try
				{
					text2 = NameConvert.ItemTypeToResourcePlural(configurationItemPath.ItemType) + "/" + configurationItemPath.Id.ToLowerInvariant();
				}
				catch (ResourceNotFoundException)
				{
					return text;
				}
				string text3 = text.Substring(0, num);
				string text4 = text.Substring(num2);
				text = text3 + text2 + text4;
			}
		}
		return text;
	}
}
