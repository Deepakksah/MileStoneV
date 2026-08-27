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

namespace VideoOS.EventServer.Alarms.RestApi.Client;

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

	public virtual async Task<GetAlarmAuthorizedRolesResponse> AlarmAuthorizedRoles_GetByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
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
				stringBuilder.Append("alarms/");
				stringBuilder.Append(Uri.EscapeDataString(ConvertToString(id, CultureInfo.InvariantCulture)));
				stringBuilder.Append("/authorizedRoles");
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
						ObjectResponseResult<GetAlarmAuthorizedRolesResponse> objectResponseResult = await ReadObjectResponseAsync<GetAlarmAuthorizedRolesResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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
						throw new ApiException("No alarm matches the given id.", status_, response2, headers_, null);
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

	public virtual async Task<GetAlarmCategoriesResponse> AlarmCategories_GetAsync(int? page = null, int? size = null, CancellationToken cancellationToken = default(CancellationToken))
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
				stringBuilder.Append("alarmCategories");
				stringBuilder.Append('?');
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
						ObjectResponseResult<GetAlarmCategoriesResponse> objectResponseResult = await ReadObjectResponseAsync<GetAlarmCategoriesResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						if (objectResponseResult.Object == null)
						{
							throw new ApiException("Response was null which was not expected.", status_, objectResponseResult.Text, headers_, null);
						}
						return objectResponseResult.Object;
					}
					case 400:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response3 = text;
						throw new ApiException("Bad request.", status_, response3, headers_, null);
					}
					case 401:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response2 = text;
						throw new ApiException("Missing or invalid authentication token.", status_, response2, headers_, null);
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

	public virtual async Task<PostAlarmCategoryResponse> AlarmCategories_PostAsync(PostAlarmCategoryDto body, CancellationToken cancellationToken = default(CancellationToken))
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
				stringBuilder.Append("alarmCategories");
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
						ObjectResponseResult<PostAlarmCategoryResponse> objectResponseResult = await ReadObjectResponseAsync<PostAlarmCategoryResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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
					case 403:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response2 = text;
						throw new ApiException("Calling user lacks required permissions or roles.", status_, response2, headers_, null);
					}
					case 400:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response4 = text;
						throw new ApiException("Bad request.", status_, response4, headers_, null);
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

	public virtual async Task<PatchAlarmCategoryResponse> AlarmCategories_PatchByIdAsync(Guid id, PatchAlarmCategoryBody body, CancellationToken cancellationToken = default(CancellationToken))
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
				request_.Method = new HttpMethod("PATCH");
				request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
				StringBuilder stringBuilder = new StringBuilder();
				if (!string.IsNullOrEmpty(_baseUrl))
				{
					stringBuilder.Append(_baseUrl);
				}
				stringBuilder.Append("alarmCategories/");
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
						ObjectResponseResult<PatchAlarmCategoryResponse> objectResponseResult = await ReadObjectResponseAsync<PatchAlarmCategoryResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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
						throw new ApiException("No alarm category matches the given id.", status_, response2, headers_, null);
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

	public virtual async Task<DeleteAlarmCategoryResponse> AlarmCategories_DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
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
				stringBuilder.Append("alarmCategories/");
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
						ObjectResponseResult<DeleteAlarmCategoryResponse> objectResponseResult = await ReadObjectResponseAsync<DeleteAlarmCategoryResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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
						throw new ApiException("No alarm category matches the given id.", status_, response2, headers_, null);
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

	public virtual async Task<GetAlarmCloseReasonsResponse> AlarmCloseReasons_GetAsync(CancellationToken cancellationToken = default(CancellationToken))
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
				stringBuilder.Append("alarmCloseReasons");
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
						ObjectResponseResult<GetAlarmCloseReasonsResponse> objectResponseResult = await ReadObjectResponseAsync<GetAlarmCloseReasonsResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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
					case 403:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response3 = text;
						throw new ApiException("Calling user lacks required permissions or roles.", status_, response3, headers_, null);
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

	public virtual async Task<PutAlarmCloseReasonsResponse> AlarmCloseReasons_PutAsync(AlarmCloseReasons body, CancellationToken cancellationToken = default(CancellationToken))
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
				request_.Method = new HttpMethod("PUT");
				request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
				StringBuilder stringBuilder = new StringBuilder();
				if (!string.IsNullOrEmpty(_baseUrl))
				{
					stringBuilder.Append(_baseUrl);
				}
				stringBuilder.Append("alarmCloseReasons");
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
						ObjectResponseResult<PutAlarmCloseReasonsResponse> objectResponseResult = await ReadObjectResponseAsync<PutAlarmCloseReasonsResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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
					case 403:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response2 = text;
						throw new ApiException("Calling user lacks required permissions or roles.", status_, response2, headers_, null);
					}
					case 400:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response4 = text;
						throw new ApiException("Bad request.", status_, response4, headers_, null);
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

	public virtual async Task<GetAlarmDisablesResponse> AlarmDisables_GetAsync(string source_id = null, int? page = null, int? size = null, CancellationToken cancellationToken = default(CancellationToken))
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
				stringBuilder.Append("alarmDisables");
				stringBuilder.Append('?');
				if (source_id != null)
				{
					stringBuilder.Append(Uri.EscapeDataString("source.id")).Append('=').Append(Uri.EscapeDataString(ConvertToString(source_id, CultureInfo.InvariantCulture)))
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
						ObjectResponseResult<GetAlarmDisablesResponse> objectResponseResult = await ReadObjectResponseAsync<GetAlarmDisablesResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						if (objectResponseResult.Object == null)
						{
							throw new ApiException("Response was null which was not expected.", status_, objectResponseResult.Text, headers_, null);
						}
						return objectResponseResult.Object;
					}
					case 400:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response3 = text;
						throw new ApiException("Bad request.", status_, response3, headers_, null);
					}
					case 401:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response2 = text;
						throw new ApiException("Missing or invalid authentication token.", status_, response2, headers_, null);
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

	public virtual async Task<PostPutAlarmDisableResponse> AlarmDisables_PostAsync(SimpleAlarmDisable body, CancellationToken cancellationToken = default(CancellationToken))
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
				stringBuilder.Append("alarmDisables");
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
						ObjectResponseResult<PostPutAlarmDisableResponse> objectResponseResult = await ReadObjectResponseAsync<PostPutAlarmDisableResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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
					case 403:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response2 = text;
						throw new ApiException("Calling user lacks required permissions or roles.", status_, response2, headers_, null);
					}
					case 400:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response4 = text;
						throw new ApiException("Bad request.", status_, response4, headers_, null);
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

	public virtual async Task<GetAlarmDisableResponse> AlarmDisables_Get2Async(Guid id, CancellationToken cancellationToken = default(CancellationToken))
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
				stringBuilder.Append("alarmDisables/");
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
						ObjectResponseResult<GetAlarmDisableResponse> objectResponseResult = await ReadObjectResponseAsync<GetAlarmDisableResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						if (objectResponseResult.Object == null)
						{
							throw new ApiException("Response was null which was not expected.", status_, objectResponseResult.Text, headers_, null);
						}
						return objectResponseResult.Object;
					}
					case 400:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response2 = text;
						throw new ApiException("Bad request.", status_, response2, headers_, null);
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
					case 404:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response5 = text;
						throw new ApiException("A server side error occurred.", status_, response5, headers_, null);
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

	public virtual async Task<PostPutAlarmDisableResponse> AlarmDisables_PutByIdAsync(Guid id, SimpleAlarmDisable body, CancellationToken cancellationToken = default(CancellationToken))
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
				request_.Method = new HttpMethod("PUT");
				request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
				StringBuilder stringBuilder = new StringBuilder();
				if (!string.IsNullOrEmpty(_baseUrl))
				{
					stringBuilder.Append(_baseUrl);
				}
				stringBuilder.Append("alarmDisables/");
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
						ObjectResponseResult<PostPutAlarmDisableResponse> objectResponseResult = await ReadObjectResponseAsync<PostPutAlarmDisableResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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
						throw new ApiException("No alarm disable matches the given id.", status_, response2, headers_, null);
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
						throw new ApiException("Not authorized.", status_, response3, headers_, null);
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

	public virtual async Task<DeleteAlarmDisableResponse> AlarmDisables_DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
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
				stringBuilder.Append("alarmDisables/");
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
						ObjectResponseResult<DeleteAlarmDisableResponse> objectResponseResult = await ReadObjectResponseAsync<DeleteAlarmDisableResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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
						throw new ApiException("No alarm disable matches the given id.", status_, response2, headers_, null);
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

	public virtual async Task<GetAlarmManagerColumnsResponse> AlarmManagerColumns_GetAsync(CancellationToken cancellationToken = default(CancellationToken))
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
				stringBuilder.Append("alarmManagerColumns");
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
						ObjectResponseResult<GetAlarmManagerColumnsResponse> objectResponseResult = await ReadObjectResponseAsync<GetAlarmManagerColumnsResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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
					case 403:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response3 = text;
						throw new ApiException("Calling user lacks required permissions or roles.", status_, response3, headers_, null);
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

	public virtual async Task<PutAlarmManagerColumnsResponse> AlarmManagerColumns_PutAsync(AlarmManagerColumns body, CancellationToken cancellationToken = default(CancellationToken))
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
				request_.Method = new HttpMethod("PUT");
				request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
				StringBuilder stringBuilder = new StringBuilder();
				if (!string.IsNullOrEmpty(_baseUrl))
				{
					stringBuilder.Append(_baseUrl);
				}
				stringBuilder.Append("alarmManagerColumns");
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
						ObjectResponseResult<PutAlarmManagerColumnsResponse> objectResponseResult = await ReadObjectResponseAsync<PutAlarmManagerColumnsResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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
					case 403:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response2 = text;
						throw new ApiException("Calling user lacks required permissions or roles.", status_, response2, headers_, null);
					}
					case 400:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response4 = text;
						throw new ApiException("Bad request.", status_, response4, headers_, null);
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

	public virtual async Task<GetAlarmPrioritiesResponse> AlarmPriorities_GetAsync(int? page = null, int? size = null, CancellationToken cancellationToken = default(CancellationToken))
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
				stringBuilder.Append("alarmPriorities");
				stringBuilder.Append('?');
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
						ObjectResponseResult<GetAlarmPrioritiesResponse> objectResponseResult = await ReadObjectResponseAsync<GetAlarmPrioritiesResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						if (objectResponseResult.Object == null)
						{
							throw new ApiException("Response was null which was not expected.", status_, objectResponseResult.Text, headers_, null);
						}
						return objectResponseResult.Object;
					}
					case 400:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response3 = text;
						throw new ApiException("Bad request.", status_, response3, headers_, null);
					}
					case 401:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response2 = text;
						throw new ApiException("Missing or invalid authentication token.", status_, response2, headers_, null);
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

	public virtual async Task<PostAlarmPriorityResponse> AlarmPriorities_PostAsync(PostAlarmPriorityDto body, CancellationToken cancellationToken = default(CancellationToken))
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
				stringBuilder.Append("alarmPriorities");
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
						ObjectResponseResult<PostAlarmPriorityResponse> objectResponseResult = await ReadObjectResponseAsync<PostAlarmPriorityResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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
					case 403:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response2 = text;
						throw new ApiException("Calling user lacks required permissions or roles.", status_, response2, headers_, null);
					}
					case 400:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response4 = text;
						throw new ApiException("Bad request.", status_, response4, headers_, null);
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

	public virtual async Task<PatchAlarmPriorityResponse> AlarmPriorities_PatchByIdAsync(Guid id, PatchAlarmPriorityBody body, CancellationToken cancellationToken = default(CancellationToken))
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
				request_.Method = new HttpMethod("PATCH");
				request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
				StringBuilder stringBuilder = new StringBuilder();
				if (!string.IsNullOrEmpty(_baseUrl))
				{
					stringBuilder.Append(_baseUrl);
				}
				stringBuilder.Append("alarmPriorities/");
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
						ObjectResponseResult<PatchAlarmPriorityResponse> objectResponseResult = await ReadObjectResponseAsync<PatchAlarmPriorityResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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
						throw new ApiException("No alarm priority matches the given id.", status_, response2, headers_, null);
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

	public virtual async Task<DeleteAlarmPriorityResponse> AlarmPriorities_DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
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
				stringBuilder.Append("alarmPriorities/");
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
						ObjectResponseResult<DeleteAlarmPriorityResponse> objectResponseResult = await ReadObjectResponseAsync<DeleteAlarmPriorityResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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
						throw new ApiException("No alarm priority matches the given id.", status_, response2, headers_, null);
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

	public virtual async Task<GetAlarmResponse> Alarms_GetByIdAsync(Guid id, string include = null, CancellationToken cancellationToken = default(CancellationToken))
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
				stringBuilder.Append("alarms/");
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
						ObjectResponseResult<GetAlarmResponse> objectResponseResult = await ReadObjectResponseAsync<GetAlarmResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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
						throw new ApiException("No alarm matches the given id.", status_, response2, headers_, null);
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

	public virtual async Task<PatchAlarmResponse> Alarms_PatchAsync(Guid id, PatchAlarmBody body, CancellationToken cancellationToken = default(CancellationToken))
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
				request_.Method = new HttpMethod("PATCH");
				request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
				StringBuilder stringBuilder = new StringBuilder();
				if (!string.IsNullOrEmpty(_baseUrl))
				{
					stringBuilder.Append(_baseUrl);
				}
				stringBuilder.Append("alarms/");
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
					case 202:
					{
						ObjectResponseResult<PatchAlarmResponse> objectResponseResult = await ReadObjectResponseAsync<PatchAlarmResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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
					case 403:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response2 = text;
						throw new ApiException("Calling user lacks required permissions or roles.", status_, response2, headers_, null);
					}
					case 400:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response4 = text;
						throw new ApiException("Bad request.", status_, response4, headers_, null);
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

	public virtual async Task<PostAlarmResponse> Alarms_PostAsync(PostAlarmBody body, CancellationToken cancellationToken = default(CancellationToken))
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
				stringBuilder.Append("alarms");
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
						ObjectResponseResult<PostAlarmResponse> objectResponseResult = await ReadObjectResponseAsync<PostAlarmResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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
					case 403:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response2 = text;
						throw new ApiException("Calling user lacks required permissions or roles.", status_, response2, headers_, null);
					}
					case 400:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response4 = text;
						throw new ApiException("Bad request.", status_, response4, headers_, null);
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

	public virtual async Task<GetAlarmsResponse> Alarms_GetAsync(string orderBy = null, string localId = null, string message = null, string assignedTo_displayName = null, string time = null, string lastUpdatedTime = null, string state_level = null, string state_name = null, string priority_level = null, string priority_name = null, string category_level = null, string category_name = null, string source_name = null, string source_id = null, string include = null, int? page = null, int? size = null, CancellationToken cancellationToken = default(CancellationToken))
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
				stringBuilder.Append("alarms");
				stringBuilder.Append('?');
				if (orderBy != null)
				{
					stringBuilder.Append(Uri.EscapeDataString("orderBy")).Append('=').Append(Uri.EscapeDataString(ConvertToString(orderBy, CultureInfo.InvariantCulture)))
						.Append('&');
				}
				if (localId != null)
				{
					stringBuilder.Append(Uri.EscapeDataString("localId")).Append('=').Append(Uri.EscapeDataString(ConvertToString(localId, CultureInfo.InvariantCulture)))
						.Append('&');
				}
				if (message != null)
				{
					stringBuilder.Append(Uri.EscapeDataString("message")).Append('=').Append(Uri.EscapeDataString(ConvertToString(message, CultureInfo.InvariantCulture)))
						.Append('&');
				}
				if (assignedTo_displayName != null)
				{
					stringBuilder.Append(Uri.EscapeDataString("assignedTo.displayName")).Append('=').Append(Uri.EscapeDataString(ConvertToString(assignedTo_displayName, CultureInfo.InvariantCulture)))
						.Append('&');
				}
				if (time != null)
				{
					stringBuilder.Append(Uri.EscapeDataString("time")).Append('=').Append(Uri.EscapeDataString(ConvertToString(time, CultureInfo.InvariantCulture)))
						.Append('&');
				}
				if (lastUpdatedTime != null)
				{
					stringBuilder.Append(Uri.EscapeDataString("lastUpdatedTime")).Append('=').Append(Uri.EscapeDataString(ConvertToString(lastUpdatedTime, CultureInfo.InvariantCulture)))
						.Append('&');
				}
				if (state_level != null)
				{
					stringBuilder.Append(Uri.EscapeDataString("state.level")).Append('=').Append(Uri.EscapeDataString(ConvertToString(state_level, CultureInfo.InvariantCulture)))
						.Append('&');
				}
				if (state_name != null)
				{
					stringBuilder.Append(Uri.EscapeDataString("state.name")).Append('=').Append(Uri.EscapeDataString(ConvertToString(state_name, CultureInfo.InvariantCulture)))
						.Append('&');
				}
				if (priority_level != null)
				{
					stringBuilder.Append(Uri.EscapeDataString("priority.level")).Append('=').Append(Uri.EscapeDataString(ConvertToString(priority_level, CultureInfo.InvariantCulture)))
						.Append('&');
				}
				if (priority_name != null)
				{
					stringBuilder.Append(Uri.EscapeDataString("priority.name")).Append('=').Append(Uri.EscapeDataString(ConvertToString(priority_name, CultureInfo.InvariantCulture)))
						.Append('&');
				}
				if (category_level != null)
				{
					stringBuilder.Append(Uri.EscapeDataString("category.level")).Append('=').Append(Uri.EscapeDataString(ConvertToString(category_level, CultureInfo.InvariantCulture)))
						.Append('&');
				}
				if (category_name != null)
				{
					stringBuilder.Append(Uri.EscapeDataString("category.name")).Append('=').Append(Uri.EscapeDataString(ConvertToString(category_name, CultureInfo.InvariantCulture)))
						.Append('&');
				}
				if (source_name != null)
				{
					stringBuilder.Append(Uri.EscapeDataString("source.name")).Append('=').Append(Uri.EscapeDataString(ConvertToString(source_name, CultureInfo.InvariantCulture)))
						.Append('&');
				}
				if (source_id != null)
				{
					stringBuilder.Append(Uri.EscapeDataString("source.id")).Append('=').Append(Uri.EscapeDataString(ConvertToString(source_id, CultureInfo.InvariantCulture)))
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
						ObjectResponseResult<GetAlarmsResponse> objectResponseResult = await ReadObjectResponseAsync<GetAlarmsResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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

	public virtual async Task<PatchAlarmResponse> Alarms_PatchAlarmsBulkAsync(BulkPatchAlarmBody body, string source_id = null, int? state_level = null, CancellationToken cancellationToken = default(CancellationToken))
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
				request_.Method = new HttpMethod("PATCH");
				request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
				StringBuilder stringBuilder = new StringBuilder();
				if (!string.IsNullOrEmpty(_baseUrl))
				{
					stringBuilder.Append(_baseUrl);
				}
				stringBuilder.Append("alarms/Bulk");
				stringBuilder.Append('?');
				if (source_id != null)
				{
					stringBuilder.Append(Uri.EscapeDataString("source.id")).Append('=').Append(Uri.EscapeDataString(ConvertToString(source_id, CultureInfo.InvariantCulture)))
						.Append('&');
				}
				if (state_level.HasValue)
				{
					stringBuilder.Append(Uri.EscapeDataString("state.level")).Append('=').Append(Uri.EscapeDataString(ConvertToString(state_level, CultureInfo.InvariantCulture)))
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
						ObjectResponseResult<PatchAlarmResponse> objectResponseResult = await ReadObjectResponseAsync<PatchAlarmResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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
					case 403:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response2 = text;
						throw new ApiException("Calling user lacks required permissions or roles.", status_, response2, headers_, null);
					}
					case 400:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response4 = text;
						throw new ApiException("Bad request.", status_, response4, headers_, null);
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

	public virtual async Task<GetAlarmHistoryResponse> Alarms_GetHistoryAsync(Guid id, int? page = null, int? size = null, CancellationToken cancellationToken = default(CancellationToken))
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
				stringBuilder.Append("alarms/");
				stringBuilder.Append(Uri.EscapeDataString(ConvertToString(id, CultureInfo.InvariantCulture)));
				stringBuilder.Append("/history");
				stringBuilder.Append('?');
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
						ObjectResponseResult<GetAlarmHistoryResponse> objectResponseResult = await ReadObjectResponseAsync<GetAlarmHistoryResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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
						throw new ApiException("No alarm matches the given id.", status_, response2, headers_, null);
					}
					case 401:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response4 = text;
						throw new ApiException("Missing or invalid authentication token.", status_, response4, headers_, null);
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
						throw new ApiException("Calling user lacks required permissions or roles.", status_, response5, headers_, null);
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

	public virtual async Task<GetAlarmSnapshotsResponse> Alarms_GetSnapshotsAsync(Guid id, int? page = null, int? size = null, CancellationToken cancellationToken = default(CancellationToken))
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
				stringBuilder.Append("alarms/");
				stringBuilder.Append(Uri.EscapeDataString(ConvertToString(id, CultureInfo.InvariantCulture)));
				stringBuilder.Append("/snapshots");
				stringBuilder.Append('?');
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
						ObjectResponseResult<GetAlarmSnapshotsResponse> objectResponseResult = await ReadObjectResponseAsync<GetAlarmSnapshotsResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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
						throw new ApiException("No alarm matches the given id.", status_, response2, headers_, null);
					}
					case 401:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response4 = text;
						throw new ApiException("Missing or invalid authentication token.", status_, response4, headers_, null);
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
						throw new ApiException("Calling user lacks required permissions or roles.", status_, response5, headers_, null);
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

	public virtual async Task<PostAlarmSnapshotResponse> Alarms_PostSnapshotAsync(Guid id, PostAlarmSnapshotBody body, CancellationToken cancellationToken = default(CancellationToken))
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
				stringBuilder.Append("alarms/");
				stringBuilder.Append(Uri.EscapeDataString(ConvertToString(id, CultureInfo.InvariantCulture)));
				stringBuilder.Append("/snapshots");
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
						ObjectResponseResult<PostAlarmSnapshotResponse> objectResponseResult = await ReadObjectResponseAsync<PostAlarmSnapshotResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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
						throw new ApiException("No alarm matches the given filter.", status_, response2, headers_, null);
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

	public virtual async Task<GetAlarmsSessionResponse> AlarmSessions_GetByIdAsync(Guid id, int? maxSize = null, CancellationToken cancellationToken = default(CancellationToken))
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
				stringBuilder.Append("alarmSessions/");
				stringBuilder.Append(Uri.EscapeDataString(ConvertToString(id, CultureInfo.InvariantCulture)));
				stringBuilder.Append("/alarms");
				stringBuilder.Append('?');
				if (maxSize.HasValue)
				{
					stringBuilder.Append(Uri.EscapeDataString("maxSize")).Append('=').Append(Uri.EscapeDataString(ConvertToString(maxSize, CultureInfo.InvariantCulture)))
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
						ObjectResponseResult<GetAlarmsSessionResponse> objectResponseResult = await ReadObjectResponseAsync<GetAlarmsSessionResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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

	public virtual async Task<PostAlarmSessionResponse> AlarmSessions_PostAsync(PostAlarmSessionBody body, CancellationToken cancellationToken = default(CancellationToken))
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
				stringBuilder.Append("alarmSessions");
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
						ObjectResponseResult<PostAlarmSessionResponse> objectResponseResult = await ReadObjectResponseAsync<PostAlarmSessionResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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

	public virtual async Task<DeleteSessionResponse> AlarmSessions_DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
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
				stringBuilder.Append("alarmSessions/");
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
						ObjectResponseResult<DeleteSessionResponse> objectResponseResult = await ReadObjectResponseAsync<DeleteSessionResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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

	public virtual async Task<GetAlarmSettingsResponse> AlarmSettings_GetAsync(CancellationToken cancellationToken = default(CancellationToken))
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
				stringBuilder.Append("alarmSettings");
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
						ObjectResponseResult<GetAlarmSettingsResponse> objectResponseResult = await ReadObjectResponseAsync<GetAlarmSettingsResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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
					case 403:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response3 = text;
						throw new ApiException("Calling user lacks required permissions or roles.", status_, response3, headers_, null);
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

	public virtual async Task<PatchAlarmSettingsResponse> AlarmSettings_PatchAsync(PatchAlarmSettingsBody body, CancellationToken cancellationToken = default(CancellationToken))
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
				request_.Method = new HttpMethod("PATCH");
				request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
				StringBuilder stringBuilder = new StringBuilder();
				if (!string.IsNullOrEmpty(_baseUrl))
				{
					stringBuilder.Append(_baseUrl);
				}
				stringBuilder.Append("alarmSettings");
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
						ObjectResponseResult<PatchAlarmSettingsResponse> objectResponseResult = await ReadObjectResponseAsync<PatchAlarmSettingsResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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
					case 403:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response2 = text;
						throw new ApiException("Calling user lacks required permissions or roles.", status_, response2, headers_, null);
					}
					case 400:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response4 = text;
						throw new ApiException("Bad request.", status_, response4, headers_, null);
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

	public virtual async Task<GetAlarmMessagesResponse> AlarmsMessages_GetMessagesAsync(int? page = null, int? size = null, CancellationToken cancellationToken = default(CancellationToken))
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
				stringBuilder.Append("alarmMessages");
				stringBuilder.Append('?');
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
						ObjectResponseResult<GetAlarmMessagesResponse> objectResponseResult = await ReadObjectResponseAsync<GetAlarmMessagesResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						if (objectResponseResult.Object == null)
						{
							throw new ApiException("Response was null which was not expected.", status_, objectResponseResult.Text, headers_, null);
						}
						return objectResponseResult.Object;
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
						string response2 = text;
						throw new ApiException("Calling user lacks required permissions or roles.", status_, response2, headers_, null);
					}
					case 401:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response4 = text;
						throw new ApiException("Missing or invalid authentication token.", status_, response4, headers_, null);
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

	public virtual async Task<GetAlarmSoundsResponse> AlarmSounds_GetAsync(int? page = null, int? size = null, CancellationToken cancellationToken = default(CancellationToken))
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
				stringBuilder.Append("alarmSounds");
				stringBuilder.Append('?');
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
						ObjectResponseResult<GetAlarmSoundsResponse> objectResponseResult = await ReadObjectResponseAsync<GetAlarmSoundsResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						if (objectResponseResult.Object == null)
						{
							throw new ApiException("Response was null which was not expected.", status_, objectResponseResult.Text, headers_, null);
						}
						return objectResponseResult.Object;
					}
					case 400:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response3 = text;
						throw new ApiException("Bad request.", status_, response3, headers_, null);
					}
					case 401:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response2 = text;
						throw new ApiException("Missing or invalid authentication token.", status_, response2, headers_, null);
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

	public virtual async Task<PostAlarmSoundResponse> AlarmSounds_PostAsync(PostAlarmSoundDto body, CancellationToken cancellationToken = default(CancellationToken))
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
				stringBuilder.Append("alarmSounds");
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
						ObjectResponseResult<PostAlarmSoundResponse> objectResponseResult = await ReadObjectResponseAsync<PostAlarmSoundResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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
					case 403:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response2 = text;
						throw new ApiException("Calling user lacks required permissions or roles.", status_, response2, headers_, null);
					}
					case 400:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response4 = text;
						throw new ApiException("Bad request.", status_, response4, headers_, null);
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

	public virtual async Task<GetAlarmSoundResponse> AlarmSounds_GetByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
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
				stringBuilder.Append("alarmSounds/");
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
						ObjectResponseResult<GetAlarmSoundResponse> objectResponseResult = await ReadObjectResponseAsync<GetAlarmSoundResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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
						throw new ApiException("No alarm sound matches the given id.", status_, response2, headers_, null);
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

	public virtual async Task<DeleteAlarmSoundResponse> AlarmSounds_DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
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
				stringBuilder.Append("alarmSounds/");
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
						ObjectResponseResult<DeleteAlarmSoundResponse> objectResponseResult = await ReadObjectResponseAsync<DeleteAlarmSoundResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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
						throw new ApiException("No alarm matches the given id.", status_, response2, headers_, null);
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

	public virtual async Task<GetAlarmStatesResponse> AlarmStates_GetAsync(int? page = null, int? size = null, CancellationToken cancellationToken = default(CancellationToken))
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
				stringBuilder.Append("alarmStates");
				stringBuilder.Append('?');
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
						ObjectResponseResult<GetAlarmStatesResponse> objectResponseResult = await ReadObjectResponseAsync<GetAlarmStatesResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						if (objectResponseResult.Object == null)
						{
							throw new ApiException("Response was null which was not expected.", status_, objectResponseResult.Text, headers_, null);
						}
						return objectResponseResult.Object;
					}
					case 400:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response3 = text;
						throw new ApiException("Bad request.", status_, response3, headers_, null);
					}
					case 401:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response2 = text;
						throw new ApiException("Missing or invalid authentication token.", status_, response2, headers_, null);
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

	public virtual async Task<PostAlarmStateResponse> AlarmStates_PostAsync(PostAlarmStateDto body, CancellationToken cancellationToken = default(CancellationToken))
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
				stringBuilder.Append("alarmStates");
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
						ObjectResponseResult<PostAlarmStateResponse> objectResponseResult = await ReadObjectResponseAsync<PostAlarmStateResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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
					case 403:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response2 = text;
						throw new ApiException("Calling user lacks required permissions or roles.", status_, response2, headers_, null);
					}
					case 400:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response4 = text;
						throw new ApiException("Bad request.", status_, response4, headers_, null);
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

	public virtual async Task<PatchAlarmStateResponse> AlarmStates_PatchByIdAsync(Guid id, PatchAlarmStateBody body, CancellationToken cancellationToken = default(CancellationToken))
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
				request_.Method = new HttpMethod("PATCH");
				request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
				StringBuilder stringBuilder = new StringBuilder();
				if (!string.IsNullOrEmpty(_baseUrl))
				{
					stringBuilder.Append(_baseUrl);
				}
				stringBuilder.Append("alarmStates/");
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
						ObjectResponseResult<PatchAlarmStateResponse> objectResponseResult = await ReadObjectResponseAsync<PatchAlarmStateResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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
						throw new ApiException("No alarm state matches the given id.", status_, response2, headers_, null);
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
						throw new ApiException("Not authorized.", status_, response3, headers_, null);
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

	public virtual async Task<DeleteAlarmStateResponse> AlarmStates_DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
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
				stringBuilder.Append("alarmStates/");
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
						ObjectResponseResult<DeleteAlarmStateResponse> objectResponseResult = await ReadObjectResponseAsync<DeleteAlarmStateResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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
						throw new ApiException("No alarm state matches the given id.", status_, response2, headers_, null);
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

	public virtual async Task<GetAlarmStatisticsResponse> AlarmsStatistics_GetAlarmStatisticsAsync(string include = null, CancellationToken cancellationToken = default(CancellationToken))
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
				stringBuilder.Append("alarmStatistics");
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
						ObjectResponseResult<GetAlarmStatisticsResponse> objectResponseResult = await ReadObjectResponseAsync<GetAlarmStatisticsResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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
					case 403:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response2 = text;
						throw new ApiException("Calling user lacks required permissions or roles.", status_, response2, headers_, null);
					}
					case 400:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response4 = text;
						throw new ApiException("Bad request.", status_, response4, headers_, null);
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

	public virtual async Task<PostAlarmSuppressionResponse> AlarmSuppression_PostAsync(PostAlarmSuppressionBody body, CancellationToken cancellationToken = default(CancellationToken))
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
				stringBuilder.Append("alarmSuppression");
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
					case 201:
					{
						ObjectResponseResult<PostAlarmSuppressionResponse> objectResponseResult = await ReadObjectResponseAsync<PostAlarmSuppressionResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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
					case 403:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response2 = text;
						throw new ApiException("Calling user lacks required permissions or roles.", status_, response2, headers_, null);
					}
					case 404:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response4 = text;
						throw new ApiException("A server side error occurred.", status_, response4, headers_, null);
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

	public virtual async Task<GetAlarmSuppressionResponse> AlarmSuppression_GetAsync(string source_id = null, int? page = null, int? size = null, CancellationToken cancellationToken = default(CancellationToken))
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
				stringBuilder.Append("alarmSuppression");
				stringBuilder.Append('?');
				if (source_id != null)
				{
					stringBuilder.Append(Uri.EscapeDataString("source.id")).Append('=').Append(Uri.EscapeDataString(ConvertToString(source_id, CultureInfo.InvariantCulture)))
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
						ObjectResponseResult<GetAlarmSuppressionResponse> objectResponseResult = await ReadObjectResponseAsync<GetAlarmSuppressionResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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

	public virtual async Task<PatchAlarmSuppressionResponse> AlarmSuppression_PatchAsync(Guid id, PatchAlarmSuppressionBody body, CancellationToken cancellationToken = default(CancellationToken))
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
				request_.Method = new HttpMethod("PATCH");
				request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
				StringBuilder stringBuilder = new StringBuilder();
				if (!string.IsNullOrEmpty(_baseUrl))
				{
					stringBuilder.Append(_baseUrl);
				}
				stringBuilder.Append("alarmSuppression/");
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
						ObjectResponseResult<PatchAlarmSuppressionResponse> objectResponseResult = await ReadObjectResponseAsync<PatchAlarmSuppressionResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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
					case 403:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response2 = text;
						throw new ApiException("Calling user lacks required permissions or roles.", status_, response2, headers_, null);
					}
					case 404:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response4 = text;
						throw new ApiException("A server side error occurred.", status_, response4, headers_, null);
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

	public virtual async Task<DeleteAlarmSuppressionResponse> AlarmSuppression_DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
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
				stringBuilder.Append("alarmSuppression/");
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
						ObjectResponseResult<DeleteAlarmSuppressionResponse> objectResponseResult = await ReadObjectResponseAsync<DeleteAlarmSuppressionResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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
					case 403:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response2 = text;
						throw new ApiException("Calling user lacks required permissions or roles.", status_, response2, headers_, null);
					}
					case 404:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response4 = text;
						throw new ApiException("No alarm suppression matches the given id.", status_, response4, headers_, null);
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
