using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VideoOS.EventServer.Events.RestApi.Client;

[GeneratedCode("NSwag", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class Client : ClientWithBearerToken, IClient
{
	protected struct ObjectResponseResult<T>(T responseObject, string responseText)
	{
		public T Object { get; } = responseObject;

		public string Text { get; } = responseText;
	}

	private string _baseUrl;

	private HttpClient _httpClient;

	private static Lazy<JsonSerializerSettings> _settings = new Lazy<JsonSerializerSettings>(CreateSerializerSettings, isThreadSafe: true);

	public string BaseUrl
	{
		get
		{
			return _baseUrl;
		}
		set
		{
			_baseUrl = value;
			if (!string.IsNullOrEmpty(_baseUrl) && !_baseUrl.EndsWith("/"))
			{
				_baseUrl += "/";
			}
		}
	}

	protected JsonSerializerSettings JsonSerializerSettings => _settings.Value;

	public bool ReadResponseAsString { get; set; }

	public Client(string baseUrl, HttpClient httpClient)
	{
		BaseUrl = baseUrl;
		_httpClient = httpClient;
	}

	private static JsonSerializerSettings CreateSerializerSettings()
	{
		return new JsonSerializerSettings();
	}

	public virtual async Task<GetEventResponse> Events_GetByIdAsync(Guid id, string include = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		HttpClient client_ = _httpClient;
		bool disposeClient_ = false;
		try
		{
			HttpRequestMessage request_ = await CreateHttpRequestMessageAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				request_.Method = new HttpMethod("GET");
				request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
				StringBuilder stringBuilder = new StringBuilder();
				if (!string.IsNullOrEmpty(_baseUrl))
				{
					stringBuilder.Append(_baseUrl);
				}
				stringBuilder.Append("events/");
				stringBuilder.Append(Uri.EscapeDataString(ConvertToString(id, CultureInfo.InvariantCulture)));
				stringBuilder.Append('?');
				if (include != null)
				{
					stringBuilder.Append(Uri.EscapeDataString("include")).Append('=').Append(Uri.EscapeDataString(ConvertToString(include, CultureInfo.InvariantCulture)))
						.Append('&');
				}
				stringBuilder.Length--;
				string uriString = stringBuilder.ToString();
				request_.RequestUri = new Uri(uriString, UriKind.RelativeOrAbsolute);
				HttpResponseMessage response_ = await client_.SendAsync(request_, (HttpCompletionOption)1, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				bool disposeResponse_ = true;
				try
				{
					Dictionary<string, IEnumerable<string>> headers_ = new Dictionary<string, IEnumerable<string>>();
					foreach (KeyValuePair<string, IEnumerable<string>> item in (HttpHeaders)response_.Headers)
					{
						headers_[item.Key] = item.Value;
					}
					if (response_.Content != null && response_.Content.Headers != null)
					{
						foreach (KeyValuePair<string, IEnumerable<string>> item2 in (HttpHeaders)response_.Content.Headers)
						{
							headers_[item2.Key] = item2.Value;
						}
					}
					int status_ = (int)response_.StatusCode;
					switch (status_)
					{
					case 200:
					{
						ObjectResponseResult<GetEventResponse> objectResponseResult = await ReadObjectResponseAsync<GetEventResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						if (objectResponseResult.Object == null)
						{
							throw new ApiException("Response was null which was not expected.", status_, objectResponseResult.Text, headers_, null);
						}
						return objectResponseResult.Object;
					}
					case 404:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response2 = text;
						throw new ApiException("No event matches the given id, or it is not stored because of the event type retention policy.", status_, response2, headers_, null);
					}
					case 401:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response4 = text;
						throw new ApiException("Missing or invalid authentication token.", status_, response4, headers_, null);
					}
					case 403:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response3 = text;
						throw new ApiException("Calling user lacks required permissions or roles.", status_, response3, headers_, null);
					}
					case 400:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response5 = text;
						throw new ApiException("Bad request.", status_, response5, headers_, null);
					}
					default:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
						string response = text;
						throw new ApiException("The HTTP status code of the response was not expected (" + status_ + ").", status_, response, headers_, null);
					}
					}
				}
				finally
				{
					if (disposeResponse_)
					{
						response_.Dispose();
					}
				}
			}
			finally
			{
				((IDisposable)request_)?.Dispose();
			}
		}
		finally
		{
			if (disposeClient_)
			{
				((HttpMessageInvoker)client_).Dispose();
			}
		}
	}

	public virtual async Task<GetEventsResponse> Events_GetAsync(string orderBy = null, string time = null, string source_name = null, string message = null, string source_id = null, string type = null, string type_name = null, string typeGroup = null, string tag = null, string analyticData_object_data = null, string analyticData_object_value = null, string analyticData_rule_name = null, string analyticData_object_vendor = null, string localId = null, string include = null, int? page = null, int? size = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		HttpClient client_ = _httpClient;
		bool disposeClient_ = false;
		try
		{
			HttpRequestMessage request_ = await CreateHttpRequestMessageAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				request_.Method = new HttpMethod("GET");
				request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
				StringBuilder stringBuilder = new StringBuilder();
				if (!string.IsNullOrEmpty(_baseUrl))
				{
					stringBuilder.Append(_baseUrl);
				}
				stringBuilder.Append("events");
				stringBuilder.Append('?');
				if (orderBy != null)
				{
					stringBuilder.Append(Uri.EscapeDataString("orderBy")).Append('=').Append(Uri.EscapeDataString(ConvertToString(orderBy, CultureInfo.InvariantCulture)))
						.Append('&');
				}
				if (time != null)
				{
					stringBuilder.Append(Uri.EscapeDataString("time")).Append('=').Append(Uri.EscapeDataString(ConvertToString(time, CultureInfo.InvariantCulture)))
						.Append('&');
				}
				if (source_name != null)
				{
					stringBuilder.Append(Uri.EscapeDataString("source.name")).Append('=').Append(Uri.EscapeDataString(ConvertToString(source_name, CultureInfo.InvariantCulture)))
						.Append('&');
				}
				if (message != null)
				{
					stringBuilder.Append(Uri.EscapeDataString("message")).Append('=').Append(Uri.EscapeDataString(ConvertToString(message, CultureInfo.InvariantCulture)))
						.Append('&');
				}
				if (source_id != null)
				{
					stringBuilder.Append(Uri.EscapeDataString("source.id")).Append('=').Append(Uri.EscapeDataString(ConvertToString(source_id, CultureInfo.InvariantCulture)))
						.Append('&');
				}
				if (type != null)
				{
					stringBuilder.Append(Uri.EscapeDataString("type")).Append('=').Append(Uri.EscapeDataString(ConvertToString(type, CultureInfo.InvariantCulture)))
						.Append('&');
				}
				if (type_name != null)
				{
					stringBuilder.Append(Uri.EscapeDataString("type.name")).Append('=').Append(Uri.EscapeDataString(ConvertToString(type_name, CultureInfo.InvariantCulture)))
						.Append('&');
				}
				if (typeGroup != null)
				{
					stringBuilder.Append(Uri.EscapeDataString("typeGroup")).Append('=').Append(Uri.EscapeDataString(ConvertToString(typeGroup, CultureInfo.InvariantCulture)))
						.Append('&');
				}
				if (tag != null)
				{
					stringBuilder.Append(Uri.EscapeDataString("tag")).Append('=').Append(Uri.EscapeDataString(ConvertToString(tag, CultureInfo.InvariantCulture)))
						.Append('&');
				}
				if (analyticData_object_data != null)
				{
					stringBuilder.Append(Uri.EscapeDataString("analyticData.object.data")).Append('=').Append(Uri.EscapeDataString(ConvertToString(analyticData_object_data, CultureInfo.InvariantCulture)))
						.Append('&');
				}
				if (analyticData_object_value != null)
				{
					stringBuilder.Append(Uri.EscapeDataString("analyticData.object.value")).Append('=').Append(Uri.EscapeDataString(ConvertToString(analyticData_object_value, CultureInfo.InvariantCulture)))
						.Append('&');
				}
				if (analyticData_rule_name != null)
				{
					stringBuilder.Append(Uri.EscapeDataString("analyticData.rule.name")).Append('=').Append(Uri.EscapeDataString(ConvertToString(analyticData_rule_name, CultureInfo.InvariantCulture)))
						.Append('&');
				}
				if (analyticData_object_vendor != null)
				{
					stringBuilder.Append(Uri.EscapeDataString("analyticData.object.vendor")).Append('=').Append(Uri.EscapeDataString(ConvertToString(analyticData_object_vendor, CultureInfo.InvariantCulture)))
						.Append('&');
				}
				if (localId != null)
				{
					stringBuilder.Append(Uri.EscapeDataString("localId")).Append('=').Append(Uri.EscapeDataString(ConvertToString(localId, CultureInfo.InvariantCulture)))
						.Append('&');
				}
				if (include != null)
				{
					stringBuilder.Append(Uri.EscapeDataString("include")).Append('=').Append(Uri.EscapeDataString(ConvertToString(include, CultureInfo.InvariantCulture)))
						.Append('&');
				}
				if (page.HasValue)
				{
					stringBuilder.Append(Uri.EscapeDataString("page")).Append('=').Append(Uri.EscapeDataString(ConvertToString(page, CultureInfo.InvariantCulture)))
						.Append('&');
				}
				if (size.HasValue)
				{
					stringBuilder.Append(Uri.EscapeDataString("size")).Append('=').Append(Uri.EscapeDataString(ConvertToString(size, CultureInfo.InvariantCulture)))
						.Append('&');
				}
				stringBuilder.Length--;
				string uriString = stringBuilder.ToString();
				request_.RequestUri = new Uri(uriString, UriKind.RelativeOrAbsolute);
				HttpResponseMessage response_ = await client_.SendAsync(request_, (HttpCompletionOption)1, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				bool disposeResponse_ = true;
				try
				{
					Dictionary<string, IEnumerable<string>> headers_ = new Dictionary<string, IEnumerable<string>>();
					foreach (KeyValuePair<string, IEnumerable<string>> item in (HttpHeaders)response_.Headers)
					{
						headers_[item.Key] = item.Value;
					}
					if (response_.Content != null && response_.Content.Headers != null)
					{
						foreach (KeyValuePair<string, IEnumerable<string>> item2 in (HttpHeaders)response_.Content.Headers)
						{
							headers_[item2.Key] = item2.Value;
						}
					}
					int status_ = (int)response_.StatusCode;
					switch (status_)
					{
					case 200:
					{
						ObjectResponseResult<GetEventsResponse> objectResponseResult = await ReadObjectResponseAsync<GetEventsResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						if (objectResponseResult.Object == null)
						{
							throw new ApiException("Response was null which was not expected.", status_, objectResponseResult.Text, headers_, null);
						}
						return objectResponseResult.Object;
					}
					case 401:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response3 = text;
						throw new ApiException("Missing or invalid authentication token.", status_, response3, headers_, null);
					}
					case 400:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response2 = text;
						throw new ApiException("Bad request.", status_, response2, headers_, null);
					}
					case 403:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response4 = text;
						throw new ApiException("Calling user lacks required permissions or roles.", status_, response4, headers_, null);
					}
					default:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
						string response = text;
						throw new ApiException("The HTTP status code of the response was not expected (" + status_ + ").", status_, response, headers_, null);
					}
					}
				}
				finally
				{
					if (disposeResponse_)
					{
						response_.Dispose();
					}
				}
			}
			finally
			{
				((IDisposable)request_)?.Dispose();
			}
		}
		finally
		{
			if (disposeClient_)
			{
				((HttpMessageInvoker)client_).Dispose();
			}
		}
	}

	public virtual async Task<PostEventResponse> Events_PostAsync(PostEventBody body, bool? triggerRules = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (body == null)
		{
			throw new ArgumentNullException("body");
		}
		HttpClient client_ = _httpClient;
		bool disposeClient_ = false;
		try
		{
			HttpRequestMessage request_ = await CreateHttpRequestMessageAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				StringContent val = new StringContent(JsonConvert.SerializeObject(body, _settings.Value));
				((HttpContent)val).Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
				request_.Content = (HttpContent)(object)val;
				request_.Method = new HttpMethod("POST");
				request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
				StringBuilder stringBuilder = new StringBuilder();
				if (!string.IsNullOrEmpty(_baseUrl))
				{
					stringBuilder.Append(_baseUrl);
				}
				stringBuilder.Append("events");
				stringBuilder.Append('?');
				if (triggerRules.HasValue)
				{
					stringBuilder.Append(Uri.EscapeDataString("triggerRules")).Append('=').Append(Uri.EscapeDataString(ConvertToString(triggerRules, CultureInfo.InvariantCulture)))
						.Append('&');
				}
				stringBuilder.Length--;
				string uriString = stringBuilder.ToString();
				request_.RequestUri = new Uri(uriString, UriKind.RelativeOrAbsolute);
				HttpResponseMessage response_ = await client_.SendAsync(request_, (HttpCompletionOption)1, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				bool disposeResponse_ = true;
				try
				{
					Dictionary<string, IEnumerable<string>> headers_ = new Dictionary<string, IEnumerable<string>>();
					foreach (KeyValuePair<string, IEnumerable<string>> item in (HttpHeaders)response_.Headers)
					{
						headers_[item.Key] = item.Value;
					}
					if (response_.Content != null && response_.Content.Headers != null)
					{
						foreach (KeyValuePair<string, IEnumerable<string>> item2 in (HttpHeaders)response_.Content.Headers)
						{
							headers_[item2.Key] = item2.Value;
						}
					}
					int status_ = (int)response_.StatusCode;
					switch (status_)
					{
					case 202:
					{
						ObjectResponseResult<PostEventResponse> objectResponseResult = await ReadObjectResponseAsync<PostEventResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						if (objectResponseResult.Object == null)
						{
							throw new ApiException("Response was null which was not expected.", status_, objectResponseResult.Text, headers_, null);
						}
						return objectResponseResult.Object;
					}
					case 401:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response3 = text;
						throw new ApiException("Missing or invalid authentication token.", status_, response3, headers_, null);
					}
					case 400:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response2 = text;
						throw new ApiException("Bad request.", status_, response2, headers_, null);
					}
					case 403:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response4 = text;
						throw new ApiException("Unauthorized to trigger the event type or unauthorized to access the source.", status_, response4, headers_, null);
					}
					default:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
						string response = text;
						throw new ApiException("The HTTP status code of the response was not expected (" + status_ + ").", status_, response, headers_, null);
					}
					}
				}
				finally
				{
					if (disposeResponse_)
					{
						response_.Dispose();
					}
				}
			}
			finally
			{
				((IDisposable)request_)?.Dispose();
			}
		}
		finally
		{
			if (disposeClient_)
			{
				((HttpMessageInvoker)client_).Dispose();
			}
		}
	}

	public virtual async Task<PostBulkEventsResponse> Events_PostBulkAsync(IEnumerable<PostEventBody> body, bool? triggerRules = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (body == null)
		{
			throw new ArgumentNullException("body");
		}
		HttpClient client_ = _httpClient;
		bool disposeClient_ = false;
		try
		{
			HttpRequestMessage request_ = await CreateHttpRequestMessageAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				StringContent val = new StringContent(JsonConvert.SerializeObject(body, _settings.Value));
				((HttpContent)val).Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
				request_.Content = (HttpContent)(object)val;
				request_.Method = new HttpMethod("POST");
				request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
				StringBuilder stringBuilder = new StringBuilder();
				if (!string.IsNullOrEmpty(_baseUrl))
				{
					stringBuilder.Append(_baseUrl);
				}
				stringBuilder.Append("events/bulk");
				stringBuilder.Append('?');
				if (triggerRules.HasValue)
				{
					stringBuilder.Append(Uri.EscapeDataString("triggerRules")).Append('=').Append(Uri.EscapeDataString(ConvertToString(triggerRules, CultureInfo.InvariantCulture)))
						.Append('&');
				}
				stringBuilder.Length--;
				string uriString = stringBuilder.ToString();
				request_.RequestUri = new Uri(uriString, UriKind.RelativeOrAbsolute);
				HttpResponseMessage response_ = await client_.SendAsync(request_, (HttpCompletionOption)1, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				bool disposeResponse_ = true;
				try
				{
					Dictionary<string, IEnumerable<string>> headers_ = new Dictionary<string, IEnumerable<string>>();
					foreach (KeyValuePair<string, IEnumerable<string>> item in (HttpHeaders)response_.Headers)
					{
						headers_[item.Key] = item.Value;
					}
					if (response_.Content != null && response_.Content.Headers != null)
					{
						foreach (KeyValuePair<string, IEnumerable<string>> item2 in (HttpHeaders)response_.Content.Headers)
						{
							headers_[item2.Key] = item2.Value;
						}
					}
					int status_ = (int)response_.StatusCode;
					switch (status_)
					{
					case 207:
					{
						ObjectResponseResult<PostBulkEventsResponse> objectResponseResult = await ReadObjectResponseAsync<PostBulkEventsResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						if (objectResponseResult.Object == null)
						{
							throw new ApiException("Response was null which was not expected.", status_, objectResponseResult.Text, headers_, null);
						}
						return objectResponseResult.Object;
					}
					case 401:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response2 = text;
						throw new ApiException("Missing or invalid authentication token.", status_, response2, headers_, null);
					}
					case 400:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response3 = text;
						throw new ApiException("Bad request.", status_, response3, headers_, null);
					}
					default:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
						string response = text;
						throw new ApiException("The HTTP status code of the response was not expected (" + status_ + ").", status_, response, headers_, null);
					}
					}
				}
				finally
				{
					if (disposeResponse_)
					{
						response_.Dispose();
					}
				}
			}
			finally
			{
				((IDisposable)request_)?.Dispose();
			}
		}
		finally
		{
			if (disposeClient_)
			{
				((HttpMessageInvoker)client_).Dispose();
			}
		}
	}

	public virtual async Task<GetEventsSessionResponse> EventSessions_GetByIdAsync(Guid id, int? maxSize = null, string include = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		HttpClient client_ = _httpClient;
		bool disposeClient_ = false;
		try
		{
			HttpRequestMessage request_ = await CreateHttpRequestMessageAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				request_.Method = new HttpMethod("GET");
				request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
				StringBuilder stringBuilder = new StringBuilder();
				if (!string.IsNullOrEmpty(_baseUrl))
				{
					stringBuilder.Append(_baseUrl);
				}
				stringBuilder.Append("eventSessions/");
				stringBuilder.Append(Uri.EscapeDataString(ConvertToString(id, CultureInfo.InvariantCulture)));
				stringBuilder.Append("/events");
				stringBuilder.Append('?');
				if (maxSize.HasValue)
				{
					stringBuilder.Append(Uri.EscapeDataString("maxSize")).Append('=').Append(Uri.EscapeDataString(ConvertToString(maxSize, CultureInfo.InvariantCulture)))
						.Append('&');
				}
				if (include != null)
				{
					stringBuilder.Append(Uri.EscapeDataString("include")).Append('=').Append(Uri.EscapeDataString(ConvertToString(include, CultureInfo.InvariantCulture)))
						.Append('&');
				}
				stringBuilder.Length--;
				string uriString = stringBuilder.ToString();
				request_.RequestUri = new Uri(uriString, UriKind.RelativeOrAbsolute);
				HttpResponseMessage response_ = await client_.SendAsync(request_, (HttpCompletionOption)1, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				bool disposeResponse_ = true;
				try
				{
					Dictionary<string, IEnumerable<string>> headers_ = new Dictionary<string, IEnumerable<string>>();
					foreach (KeyValuePair<string, IEnumerable<string>> item in (HttpHeaders)response_.Headers)
					{
						headers_[item.Key] = item.Value;
					}
					if (response_.Content != null && response_.Content.Headers != null)
					{
						foreach (KeyValuePair<string, IEnumerable<string>> item2 in (HttpHeaders)response_.Content.Headers)
						{
							headers_[item2.Key] = item2.Value;
						}
					}
					int status_ = (int)response_.StatusCode;
					switch (status_)
					{
					case 200:
					{
						ObjectResponseResult<GetEventsSessionResponse> objectResponseResult = await ReadObjectResponseAsync<GetEventsSessionResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						if (objectResponseResult.Object == null)
						{
							throw new ApiException("Response was null which was not expected.", status_, objectResponseResult.Text, headers_, null);
						}
						return objectResponseResult.Object;
					}
					case 401:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response2 = text;
						throw new ApiException("Missing or invalid authentication token.", status_, response2, headers_, null);
					}
					case 404:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response4 = text;
						throw new ApiException("Session not found.", status_, response4, headers_, null);
					}
					case 400:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response3 = text;
						throw new ApiException("Bad request.", status_, response3, headers_, null);
					}
					case 403:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response5 = text;
						throw new ApiException("The user has no permission over the session.", status_, response5, headers_, null);
					}
					default:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
						string response = text;
						throw new ApiException("The HTTP status code of the response was not expected (" + status_ + ").", status_, response, headers_, null);
					}
					}
				}
				finally
				{
					if (disposeResponse_)
					{
						response_.Dispose();
					}
				}
			}
			finally
			{
				((IDisposable)request_)?.Dispose();
			}
		}
		finally
		{
			if (disposeClient_)
			{
				((HttpMessageInvoker)client_).Dispose();
			}
		}
	}

	public virtual async Task<PostEventSessionResponse> EventSessions_PostAsync(PostEventSessionBody body, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (body == null)
		{
			throw new ArgumentNullException("body");
		}
		HttpClient client_ = _httpClient;
		bool disposeClient_ = false;
		try
		{
			HttpRequestMessage request_ = await CreateHttpRequestMessageAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				StringContent val = new StringContent(JsonConvert.SerializeObject(body, _settings.Value));
				((HttpContent)val).Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
				request_.Content = (HttpContent)(object)val;
				request_.Method = new HttpMethod("POST");
				request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
				StringBuilder stringBuilder = new StringBuilder();
				if (!string.IsNullOrEmpty(_baseUrl))
				{
					stringBuilder.Append(_baseUrl);
				}
				stringBuilder.Append("eventSessions");
				string uriString = stringBuilder.ToString();
				request_.RequestUri = new Uri(uriString, UriKind.RelativeOrAbsolute);
				HttpResponseMessage response_ = await client_.SendAsync(request_, (HttpCompletionOption)1, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				bool disposeResponse_ = true;
				try
				{
					Dictionary<string, IEnumerable<string>> headers_ = new Dictionary<string, IEnumerable<string>>();
					foreach (KeyValuePair<string, IEnumerable<string>> item in (HttpHeaders)response_.Headers)
					{
						headers_[item.Key] = item.Value;
					}
					if (response_.Content != null && response_.Content.Headers != null)
					{
						foreach (KeyValuePair<string, IEnumerable<string>> item2 in (HttpHeaders)response_.Content.Headers)
						{
							headers_[item2.Key] = item2.Value;
						}
					}
					int status_ = (int)response_.StatusCode;
					switch (status_)
					{
					case 200:
					{
						ObjectResponseResult<PostEventSessionResponse> objectResponseResult = await ReadObjectResponseAsync<PostEventSessionResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						if (objectResponseResult.Object == null)
						{
							throw new ApiException("Response was null which was not expected.", status_, objectResponseResult.Text, headers_, null);
						}
						return objectResponseResult.Object;
					}
					case 401:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response3 = text;
						throw new ApiException("Missing or invalid authentication token.", status_, response3, headers_, null);
					}
					case 400:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response2 = text;
						throw new ApiException("Bad request.", status_, response2, headers_, null);
					}
					case 403:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response4 = text;
						throw new ApiException("The user has no permission over events.", status_, response4, headers_, null);
					}
					default:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
						string response = text;
						throw new ApiException("The HTTP status code of the response was not expected (" + status_ + ").", status_, response, headers_, null);
					}
					}
				}
				finally
				{
					if (disposeResponse_)
					{
						response_.Dispose();
					}
				}
			}
			finally
			{
				((IDisposable)request_)?.Dispose();
			}
		}
		finally
		{
			if (disposeClient_)
			{
				((HttpMessageInvoker)client_).Dispose();
			}
		}
	}

	public virtual async Task<DeleteEventSessionResponse> EventSessions_DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
	{
		HttpClient client_ = _httpClient;
		bool disposeClient_ = false;
		try
		{
			HttpRequestMessage request_ = await CreateHttpRequestMessageAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				request_.Method = new HttpMethod("DELETE");
				request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
				StringBuilder stringBuilder = new StringBuilder();
				if (!string.IsNullOrEmpty(_baseUrl))
				{
					stringBuilder.Append(_baseUrl);
				}
				stringBuilder.Append("eventSessions/");
				stringBuilder.Append(Uri.EscapeDataString(ConvertToString(id, CultureInfo.InvariantCulture)));
				string uriString = stringBuilder.ToString();
				request_.RequestUri = new Uri(uriString, UriKind.RelativeOrAbsolute);
				HttpResponseMessage response_ = await client_.SendAsync(request_, (HttpCompletionOption)1, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				bool disposeResponse_ = true;
				try
				{
					Dictionary<string, IEnumerable<string>> headers_ = new Dictionary<string, IEnumerable<string>>();
					foreach (KeyValuePair<string, IEnumerable<string>> item in (HttpHeaders)response_.Headers)
					{
						headers_[item.Key] = item.Value;
					}
					if (response_.Content != null && response_.Content.Headers != null)
					{
						foreach (KeyValuePair<string, IEnumerable<string>> item2 in (HttpHeaders)response_.Content.Headers)
						{
							headers_[item2.Key] = item2.Value;
						}
					}
					int status_ = (int)response_.StatusCode;
					switch (status_)
					{
					case 200:
					{
						ObjectResponseResult<DeleteEventSessionResponse> objectResponseResult = await ReadObjectResponseAsync<DeleteEventSessionResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						if (objectResponseResult.Object == null)
						{
							throw new ApiException("Response was null which was not expected.", status_, objectResponseResult.Text, headers_, null);
						}
						return objectResponseResult.Object;
					}
					case 401:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response2 = text;
						throw new ApiException("Missing or invalid authentication token.", status_, response2, headers_, null);
					}
					case 404:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response4 = text;
						throw new ApiException("Session not found.", status_, response4, headers_, null);
					}
					case 400:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response3 = text;
						throw new ApiException("Bad request.", status_, response3, headers_, null);
					}
					case 403:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response5 = text;
						throw new ApiException("The user has no permission to execute this action.", status_, response5, headers_, null);
					}
					default:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
						string response = text;
						throw new ApiException("The HTTP status code of the response was not expected (" + status_ + ").", status_, response, headers_, null);
					}
					}
				}
				finally
				{
					if (disposeResponse_)
					{
						response_.Dispose();
					}
				}
			}
			finally
			{
				((IDisposable)request_)?.Dispose();
			}
		}
		finally
		{
			if (disposeClient_)
			{
				((HttpMessageInvoker)client_).Dispose();
			}
		}
	}

	protected virtual async Task<ObjectResponseResult<T>> ReadObjectResponseAsync<T>(HttpResponseMessage response, IReadOnlyDictionary<string, IEnumerable<string>> headers, CancellationToken cancellationToken)
	{
		if (response == null || response.Content == null)
		{
			return new ObjectResponseResult<T>(default(T), string.Empty);
		}
		if (ReadResponseAsString)
		{
			string text = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				T responseObject = JsonConvert.DeserializeObject<T>(text, JsonSerializerSettings);
				return new ObjectResponseResult<T>(responseObject, text);
			}
			catch (JsonException innerException)
			{
				throw new ApiException("Could not deserialize the response body string as " + typeof(T).FullName + ".", (int)response.StatusCode, text, headers, innerException);
			}
		}
		try
		{
			using Stream stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(continueOnCapturedContext: false);
			using StreamReader reader = new StreamReader(stream);
			using JsonTextReader reader2 = new JsonTextReader(reader);
			T responseObject2 = JsonSerializer.Create(JsonSerializerSettings).Deserialize<T>(reader2);
			return new ObjectResponseResult<T>(responseObject2, string.Empty);
		}
		catch (JsonException innerException2)
		{
			throw new ApiException("Could not deserialize the response body stream as " + typeof(T).FullName + ".", (int)response.StatusCode, string.Empty, headers, innerException2);
		}
	}

	private string ConvertToString(object value, CultureInfo cultureInfo)
	{
		if (value == null)
		{
			return "";
		}
		if (value is Enum)
		{
			string name = Enum.GetName(value.GetType(), value);
			if (name != null)
			{
				FieldInfo declaredField = value.GetType().GetTypeInfo().GetDeclaredField(name);
				if (declaredField != null && declaredField.GetCustomAttribute(typeof(EnumMemberAttribute)) is EnumMemberAttribute enumMemberAttribute)
				{
					if (enumMemberAttribute.Value == null)
					{
						return name;
					}
					return enumMemberAttribute.Value;
				}
				string text = Convert.ToString(Convert.ChangeType(value, Enum.GetUnderlyingType(value.GetType()), cultureInfo));
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
		}
		else
		{
			if (value is bool)
			{
				return Convert.ToString((bool)value, cultureInfo).ToLowerInvariant();
			}
			if (value is byte[])
			{
				return Convert.ToBase64String((byte[])value);
			}
			if (value is string[])
			{
				return string.Join(",", (string[])value);
			}
			if (value.GetType().IsArray)
			{
				Array array = (Array)value;
				string[] array2 = new string[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array2[i] = ConvertToString(array.GetValue(i), cultureInfo);
				}
				return string.Join(",", array2);
			}
		}
		string text2 = Convert.ToString(value, cultureInfo);
		if (text2 != null)
		{
			return text2;
		}
		return "";
	}
}
