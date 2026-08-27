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
using VideoOS.EventServer.Communication.MapsRestApiClient;

namespace VideoOS.EventServer.Maps.RestApi.Client;

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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		return new JsonSerializerSettings();
	}

	public virtual async Task<GetMapResponse> Maps_GetByIdAsync(Guid id, string include = null, CancellationToken cancellationToken = default(CancellationToken))
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
				stringBuilder.Append("maps/");
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
						ObjectResponseResult<GetMapResponse> objectResponseResult = await ReadObjectResponseAsync<GetMapResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						if (objectResponseResult.Object == null)
						{
							throw new ApiException("Response was null which was not expected.", status_, objectResponseResult.Text, headers_, null);
						}
						return objectResponseResult.Object;
					}
					case 404:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response3 = text;
						throw new ApiException("No map matches the given id.", status_, response3, headers_, null);
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

	public virtual async Task<GetMapsResponse> Maps_GetAsync(string include = null, int? page = null, int? size = null, CancellationToken cancellationToken = default(CancellationToken))
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
				stringBuilder.Append("maps");
				stringBuilder.Append('?');
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
						ObjectResponseResult<GetMapsResponse> objectResponseResult = await ReadObjectResponseAsync<GetMapsResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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

	public virtual async Task<GetVisualItemResponse> Maps_GetVisualItemOnMapByIdAsync(Guid mapId, Guid itemId, string include = null, CancellationToken cancellationToken = default(CancellationToken))
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
				stringBuilder.Append("maps/");
				stringBuilder.Append(Uri.EscapeDataString(ConvertToString(mapId, CultureInfo.InvariantCulture)));
				stringBuilder.Append("/visualItems/");
				stringBuilder.Append(Uri.EscapeDataString(ConvertToString(itemId, CultureInfo.InvariantCulture)));
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
						ObjectResponseResult<GetVisualItemResponse> objectResponseResult = await ReadObjectResponseAsync<GetVisualItemResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						if (objectResponseResult.Object == null)
						{
							throw new ApiException("Response was null which was not expected.", status_, objectResponseResult.Text, headers_, null);
						}
						return objectResponseResult.Object;
					}
					case 404:
					{
						string text = ((response_.Content != null) ? (await response_.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : string.Empty);
						string response3 = text;
						throw new ApiException("No map of visual item matches the given id.", status_, response3, headers_, null);
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

	public virtual async Task<GetVisualItemsResponse> Maps_GetVisualItemsOnMapAsync(Guid mapId, string include = null, int? page = null, int? size = null, CancellationToken cancellationToken = default(CancellationToken))
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
				stringBuilder.Append("maps/");
				stringBuilder.Append(Uri.EscapeDataString(ConvertToString(mapId, CultureInfo.InvariantCulture)));
				stringBuilder.Append("/visualItems");
				stringBuilder.Append('?');
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
						ObjectResponseResult<GetVisualItemsResponse> objectResponseResult = await ReadObjectResponseAsync<GetVisualItemsResponse>(response_, headers_, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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
			catch (JsonException ex)
			{
				JsonException innerException = ex;
				throw new ApiException("Could not deserialize the response body string as " + typeof(T).FullName + ".", (int)response.StatusCode, text, headers, (Exception)(object)innerException);
			}
		}
		try
		{
			using Stream stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(continueOnCapturedContext: false);
			using StreamReader streamReader = new StreamReader(stream);
			JsonTextReader val = new JsonTextReader((TextReader)streamReader);
			try
			{
				T responseObject2 = JsonSerializer.Create(JsonSerializerSettings).Deserialize<T>((JsonReader)(object)val);
				return new ObjectResponseResult<T>(responseObject2, string.Empty);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch (JsonException ex2)
		{
			JsonException innerException2 = ex2;
			throw new ApiException("Could not deserialize the response body stream as " + typeof(T).FullName + ".", (int)response.StatusCode, string.Empty, headers, (Exception)(object)innerException2);
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
