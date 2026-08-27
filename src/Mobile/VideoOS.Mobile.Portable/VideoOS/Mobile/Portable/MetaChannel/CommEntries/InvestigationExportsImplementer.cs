using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Mobile.Portable.VideoChannel.Params;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class InvestigationExportsImplementer : IInvestigationExports
{
	private List<GetSetWrapper<Guid>> _exportIds;

	private Dictionary<ExportType, GetSetWrapper<Guid>> _exportTypeIds;

	public Guid OriginDbExportId { get; set; }

	public Guid DbExportId { get; set; }

	public Guid AviExportId { get; set; }

	public Guid MkvExportId { get; set; }

	public Guid BinaryExportId { get; set; }

	public IEnumerable<GetSetWrapper<Guid>> ExportIds => _exportIds;

	public IEnumerable<KeyValuePair<ExportType, GetSetWrapper<Guid>>> ExportTypeIds => _exportTypeIds;

	public GetSetWrapper<Guid> OriginDbExportIdGetSetWrapper { get; private set; }

	public GetSetWrapper<Guid> DbExportIdGetSetWrapper { get; private set; }

	public GetSetWrapper<Guid> AviExportIdGetSetWrapper { get; private set; }

	public GetSetWrapper<Guid> MkvExportIdGetSetWrapper { get; private set; }

	public GetSetWrapper<Guid> BinaryExportIdGetSetWrapper { get; private set; }

	private void InternalInit()
	{
		OriginDbExportIdGetSetWrapper = new GetSetWrapper<Guid>
		{
			Getter = () => OriginDbExportId,
			Setter = delegate(Guid id)
			{
				OriginDbExportId = id;
			}
		};
		DbExportIdGetSetWrapper = new GetSetWrapper<Guid>
		{
			Getter = () => DbExportId,
			Setter = delegate(Guid id)
			{
				DbExportId = id;
			}
		};
		AviExportIdGetSetWrapper = new GetSetWrapper<Guid>
		{
			Getter = () => AviExportId,
			Setter = delegate(Guid id)
			{
				AviExportId = id;
			}
		};
		MkvExportIdGetSetWrapper = new GetSetWrapper<Guid>
		{
			Getter = () => MkvExportId,
			Setter = delegate(Guid id)
			{
				MkvExportId = id;
			}
		};
		BinaryExportIdGetSetWrapper = new GetSetWrapper<Guid>
		{
			Getter = () => BinaryExportId,
			Setter = delegate(Guid id)
			{
				BinaryExportId = id;
			}
		};
		_exportIds = new List<GetSetWrapper<Guid>> { OriginDbExportIdGetSetWrapper, DbExportIdGetSetWrapper, AviExportIdGetSetWrapper, MkvExportIdGetSetWrapper, BinaryExportIdGetSetWrapper };
		_exportTypeIds = new Dictionary<ExportType, GetSetWrapper<Guid>>
		{
			{
				ExportType.OriginDb,
				OriginDbExportIdGetSetWrapper
			},
			{
				ExportType.Db,
				DbExportIdGetSetWrapper
			},
			{
				ExportType.Avi,
				AviExportIdGetSetWrapper
			},
			{
				ExportType.Mkv,
				MkvExportIdGetSetWrapper
			},
			{
				ExportType.Binary,
				BinaryExportIdGetSetWrapper
			}
		};
	}

	public InvestigationExportsImplementer()
	{
		InternalInit();
	}

	public InvestigationExportsImplementer(IInvestigationExports implementer)
	{
		InternalInit();
		InitFrom(implementer);
	}

	public Guid GetExportId(ExportType type)
	{
		return type switch
		{
			ExportType.Avi => AviExportId, 
			ExportType.OriginDb => OriginDbExportId, 
			ExportType.Db => DbExportId, 
			ExportType.Mkv => MkvExportId, 
			ExportType.Binary => BinaryExportId, 
			_ => default(Guid), 
		};
	}

	private void InitFrom(IInvestigationExports implementer)
	{
		if (implementer != null)
		{
			OriginDbExportId = implementer.OriginDbExportId;
			DbExportId = implementer.DbExportId;
			AviExportId = implementer.AviExportId;
			MkvExportId = implementer.MkvExportId;
			BinaryExportId = implementer.BinaryExportId;
		}
	}
}
