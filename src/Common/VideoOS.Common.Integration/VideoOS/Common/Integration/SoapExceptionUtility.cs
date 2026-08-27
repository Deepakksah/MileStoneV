using System;
using System.Globalization;
using System.ServiceModel;
using System.Web.Services.Protocols;
using System.Xml;

namespace VideoOS.Common.Integration;

public static class SoapExceptionUtility
{
	public const string BaseExceptionName = "BaseExceptionName";

	public const string ExceptionMessage = "ExceptionMessage";

	public const string SqlNumber = "SqlNumber";

	public const string ErrorCode = "ErrorCode";

	public const string ErrorNumber = "ErrorNumber";

	public static SoapException CreateSoapException(Exception ex)
	{
		if (ex == null)
		{
			throw new ArgumentNullException("ex");
		}
		if (ex is SoapException)
		{
			return (SoapException)ex;
		}
		string attributeValue = null;
		string text = null;
		XmlNode detailNode = GetDetailNode();
		if (ex is FaultException<ExceptionDetail>)
		{
			FaultException<ExceptionDetail> ex2 = (FaultException<ExceptionDetail>)ex;
			text = ex2.Detail.Message;
			if (ex2.Code.Name == 50000.ToString(CultureInfo.InvariantCulture) || ex2.Code.Name == 51000.ToString(CultureInfo.InvariantCulture))
			{
				attributeValue = typeof(UnauthorizedAccessException).FullName;
				ex = new UnauthorizedAccessException(text);
				SetDetailAttribute(detailNode, "ErrorNumber", ex2.Code.Name);
				XmlNode xmlNode = detailNode.OwnerDocument.CreateNode(XmlNodeType.Element, "ErrorNumber", null);
				xmlNode.InnerText = ex2.Code.Name;
				detailNode.AppendChild(xmlNode);
			}
		}
		else if (ex is EndpointNotFoundException)
		{
			text = ex.Message;
			attributeValue = typeof(ServerServiceException).FullName;
			SetDetailAttribute(detailNode, "ErrorCode", 52000.ToString(CultureInfo.InvariantCulture));
			XmlNode xmlNode2 = detailNode.OwnerDocument.CreateNode(XmlNodeType.Element, "ErrorNumber", null);
			xmlNode2.InnerText = 52000.ToString(CultureInfo.InvariantCulture);
			detailNode.AppendChild(xmlNode2);
		}
		else
		{
			text = ex.Message;
			attributeValue = typeof(ServerServiceException).FullName;
		}
		SetDetailAttribute(detailNode, "BaseExceptionName", attributeValue);
		SetDetailAttribute(detailNode, "ExceptionMessage", text);
		return new SoapException(ex.Message, new XmlQualifiedName(ex.GetType().Name), string.Empty, string.Empty, detailNode, new SoapFaultSubCode(new XmlQualifiedName(ex.GetType().Name)), ex);
	}

	public static XmlNode GetDetailNode()
	{
		return new XmlDocument().CreateNode(XmlNodeType.Element, SoapException.DetailElementName.Name, SoapException.DetailElementName.Namespace);
	}

	public static void SetDetailAttribute(XmlNode detailNode, string attributeName, string attributeValue)
	{
		if (attributeValue != null)
		{
			XmlAttribute xmlAttribute = detailNode.OwnerDocument.CreateAttribute(attributeName);
			xmlAttribute.Value = attributeValue;
			detailNode.Attributes.Append(xmlAttribute);
		}
	}
}
