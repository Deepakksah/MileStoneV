using System;
using System.Xml.XPath;

namespace VideoOS.Platform.Data;

public class MetadataQueueResult
{
	public bool FailedToRetrieveMetadata { get; }

	public bool NoMetadataAvailable { get; }

	public Exception Exception { get; }

	public IXPathNavigable MetadataXmlDocument { get; }

	public MetadataQueueResult(IXPathNavigable xmlDocument)
	{
		MetadataXmlDocument = xmlDocument;
	}

	public MetadataQueueResult(bool failedToRetrieveMetadata, bool noMetadataAvailable)
	{
		FailedToRetrieveMetadata = failedToRetrieveMetadata;
		NoMetadataAvailable = noMetadataAvailable;
	}

	public MetadataQueueResult(bool failedToRetrieveMetadata, bool noMetadataAvailable, Exception exception)
		: this(failedToRetrieveMetadata, noMetadataAvailable)
	{
		Exception = exception;
	}
}
