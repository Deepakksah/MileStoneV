using System.Collections.ObjectModel;
using System.Net;
using VideoOS.Platform.JsonHandling;

namespace VideoOS.Management.Server.RestApi;

public class ErrorModel
{
	public HttpStatusCode HttpCode { get; set; } = HttpStatusCode.OK;

	public Collection<ErrorModelDetail> Errors { get; } = new Collection<ErrorModelDetail>();

	public void AddKeyTextAndTextId(string key, string text, string textId)
	{
		Errors.Add(new ErrorModelDetail
		{
			Key = key,
			Text = text,
			TextId = textId
		});
	}

	public void AddTextAndTextId(string text, string textId)
	{
		Errors.Add(new ErrorModelDetail
		{
			Key = "",
			Text = text,
			TextId = textId
		});
	}

	public void AddKeyText(string key, string text)
	{
		Errors.Add(new ErrorModelDetail
		{
			Key = key,
			Text = text
		});
	}

	public void AddText(string text)
	{
		Errors.Add(new ErrorModelDetail
		{
			Text = text
		});
	}

	public string Serialize()
	{
		JsonBuilder jsonBuilder = new JsonBuilder(array: false);
		JsonUtilities.ConvertErrorModel(this, jsonBuilder);
		return jsonBuilder.ToJson();
	}

	public override string ToString()
	{
		JsonBuilder jsonBuilder = new JsonBuilder(array: false);
		JsonUtilities.ConvertErrorModel(this, jsonBuilder);
		return jsonBuilder.ToJson();
	}
}
