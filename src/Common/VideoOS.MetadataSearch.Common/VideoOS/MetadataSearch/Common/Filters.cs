using System;
using System.Collections.Generic;
using VideoOS.MetadataSearch.Common.Resources;
using VideoOS.Platform.Resources;

namespace VideoOS.MetadataSearch.Common;

public static class Filters
{
	public static readonly Guid Age = new Guid("59FE8653-4335-497B-8BF2-490867DD6973");

	public static readonly Guid Height = new Guid("ECDCB7FF-0A4B-4B4C-9272-A2FD23639070");

	public static readonly Guid Gender = new Guid("C592F9F6-4CE7-4CEE-B7C3-FC18B58E8B36");

	public static readonly Guid ClothingTopCategory = new Guid("58687AEB-A7A3-4BE9-8D9B-59467E1B0543");

	public static readonly Guid ClothingTopGrain = new Guid("C558996C-BD40-4F7E-B22D-9067DB61AEAF");

	public static readonly Guid ClothingTopColor = new Guid("BD42A66B-FF5E-4951-AAEC-6299C1D6381A");

	public static readonly Guid ClothingTopStyle = new Guid("8D8404A4-4C7F-4C30-B52E-3C2A0194B3FE");

	public static readonly Guid ClothingBottomCategory = new Guid("844774F8-76BB-404F-A8B8-7A0EAFF51FB6");

	public static readonly Guid ClothingBottomGrain = new Guid("6BC1B79B-D799-4B71-9C23-B80F512875B5");

	public static readonly Guid ClothingBottomColor = new Guid("103A47C0-A405-4F5E-A1A0-CE50A59E35DB");

	public static readonly Guid ClothingBottomStyle = new Guid("1D7664E0-F67A-4DD5-A70F-62362896397B");

	public static readonly Guid Bag = new Guid("{135659D7-1934-4249-B6CA-D9AA2E3428C2}");

	public static readonly Guid BagCategory = new Guid("ECFA5207-34B1-4588-A037-4AA6885AFF2E");

	public static readonly Guid BagColor = new Guid("A1F57724-941A-46DD-8773-CD8B79B6254E");

	public static readonly Guid HairLength = new Guid("3367DAFC-A27E-4F73-BB51-B1E586A2FE9F");

	public static readonly Guid HairColor = new Guid("FC7FA276-DD38-4456-AD3B-B87F76356757");

	public static readonly Guid Face = new Guid("4C6AEECB-D705-44BF-8BEA-2D866FBD3BA8");

	public static readonly Guid Beard = new Guid("{5082F29A-92E0-4DA2-B2A8-70511A7F11D9}");

	public static readonly Guid Mustache = new Guid("{9A3D4D02-02B4-4DEB-9808-E0FE1D18D70F}");

	public static readonly Guid Sideburn = new Guid("{0780514A-C1AE-497F-B2BC-A54CE72FF067}");

	public static readonly Guid Glasses = new Guid("{E3F21D4C-84AE-44E1-80BE-0552FD254449}");

	public static readonly Guid Gloves = new Guid("{68A2F1BF-3954-4638-8A47-FA05B7A053D1}");

	public static readonly Guid GlovesColor = new Guid("{BF1F0B6C-CE06-428A-A9E7-69DBEBBF4F3A}");

	public static readonly Guid Hat = new Guid("{EA408F87-BE05-45E7-838D-1798C30352E2}");

	public static readonly Guid HatColor = new Guid("{4C345605-021F-47C6-AAF8-B76612541C9C}");

	public static readonly Guid Helmet = new Guid("{1503C856-3172-450B-AF16-D3A260ECE8E3}");

	public static readonly Guid HelmetColor = new Guid("{1A692927-1179-4213-8C2C-29BB9D66C1F3}");

	public static readonly Guid Mask = new Guid("{086DB922-ED67-4AEA-9F28-18EA90720AAB}");

	public static readonly Guid MaskColor = new Guid("{6091A365-B25D-4350-B8DE-88A2BF5AFD6C}");

	public static readonly Guid Scarf = new Guid("{1A78575F-639A-456D-A11E-84D1E8D33A24}");

	public static readonly Guid ScarfColor = new Guid("{F5E210A8-1CE3-4C79-A249-B1E131AB1A74}");

	public static readonly Guid ShoesCategory = new Guid("{0704C12E-74E7-48BF-8F1A-AE4B57B643C3}");

	public static readonly Guid ShoesColor = new Guid("{0BF6FCAD-60CC-41F8-9067-0D03DA84ACB7}");

	public static readonly Guid VehicleColor = new Guid("2D812F0D-ED3E-4553-B3FA-642BDC95D886");

	public static readonly Guid LicensePlate = new Guid("489A1205-5119-40B7-B1BA-C9A3F6DC2E92");

	public static readonly Guid VehicleType = new Guid("E07EF69F-F064-48FA-B2EA-2C0C6F1F19FC");

	public static readonly Guid VehicleModel = new Guid("39A4FE74-D2F7-4DA6-AEFC-68D4B3517A22");

	public static readonly Guid VehicleBrand = new Guid("{ABF9A511-DECA-44D8-BD1C-2C8ACA544B39}");

	public static readonly Guid LicensePlateCountryCode = new Guid("{8570EEBF-0E5D-483F-97AC-00E577A8F434}");

	public static readonly Guid LicensePlateIssuingEntity = new Guid("{61FBB9BF-A652-4E5B-B98D-201B7A1805A5}");

	public static readonly Guid LicensePlateType = new Guid("{CF04872F-24FC-46F8-A51F-64D5BB5C6557}");

	public static readonly Guid Speed = new Guid("621FEB37-B0A1-4C63-9F0A-E78B1AC1966D");

	public static readonly Guid GeoCoordinates = new Guid("97B0BD8A-719D-4235-959A-76B7CB8FFAE2");

	private static IReadOnlyDictionary<Guid, string> FilterStrings = new Dictionary<Guid, string>
	{
		{
			Age,
			VideoOS.Platform.Resources.Strings.PersonAgeSearchFilter_Name_WithoutUnit
		},
		{
			Height,
			VideoOS.MetadataSearch.Common.Resources.Strings.HeightSearchFilterName
		},
		{
			Gender,
			VideoOS.Platform.Resources.Strings.PersonGenderSearchFilter_Name
		},
		{
			ClothingTopCategory,
			VideoOS.MetadataSearch.Common.Resources.Strings.TopTypeSearchFilterName
		},
		{
			ClothingTopGrain,
			VideoOS.MetadataSearch.Common.Resources.Strings.TopGrainSearchFilterName
		},
		{
			ClothingTopColor,
			VideoOS.MetadataSearch.Common.Resources.Strings.TopColorSearchFilterName
		},
		{
			ClothingTopStyle,
			VideoOS.MetadataSearch.Common.Resources.Strings.TopStyleSearchFilterName
		},
		{
			ClothingBottomCategory,
			VideoOS.MetadataSearch.Common.Resources.Strings.BottomTypeSearchFilterName
		},
		{
			ClothingBottomGrain,
			VideoOS.MetadataSearch.Common.Resources.Strings.BottomGrainSearchFilterName
		},
		{
			ClothingBottomColor,
			VideoOS.MetadataSearch.Common.Resources.Strings.BottomColorSearchFilterName
		},
		{
			ClothingBottomStyle,
			VideoOS.MetadataSearch.Common.Resources.Strings.BottomStyleSearchFilterName
		},
		{
			Bag,
			VideoOS.MetadataSearch.Common.Resources.Strings.BagFilterName
		},
		{
			BagCategory,
			VideoOS.MetadataSearch.Common.Resources.Strings.BagCategorySearchFilterName
		},
		{
			BagColor,
			VideoOS.MetadataSearch.Common.Resources.Strings.BagColorSearchFilterName
		},
		{
			HairLength,
			VideoOS.MetadataSearch.Common.Resources.Strings.HairLengthSearchFilterName
		},
		{
			HairColor,
			VideoOS.MetadataSearch.Common.Resources.Strings.HairColorSearchFilterName
		},
		{
			Face,
			VideoOS.MetadataSearch.Common.Resources.Strings.FaceSearchFilterName
		},
		{
			Beard,
			VideoOS.MetadataSearch.Common.Resources.Strings.BeardFilterName
		},
		{
			Mustache,
			VideoOS.MetadataSearch.Common.Resources.Strings.MustacheFilterName
		},
		{
			Sideburn,
			VideoOS.MetadataSearch.Common.Resources.Strings.SideburnFilterName
		},
		{
			Glasses,
			VideoOS.MetadataSearch.Common.Resources.Strings.GlassesFilterName
		},
		{
			Gloves,
			VideoOS.MetadataSearch.Common.Resources.Strings.GlovesFilterName
		},
		{
			GlovesColor,
			VideoOS.MetadataSearch.Common.Resources.Strings.GlovesColorFilterName
		},
		{
			Hat,
			VideoOS.MetadataSearch.Common.Resources.Strings.HatFilterName
		},
		{
			HatColor,
			VideoOS.MetadataSearch.Common.Resources.Strings.HatColorFilterName
		},
		{
			Helmet,
			VideoOS.MetadataSearch.Common.Resources.Strings.HelmetFilterName
		},
		{
			HelmetColor,
			VideoOS.MetadataSearch.Common.Resources.Strings.HelmetColorFilterName
		},
		{
			Mask,
			VideoOS.MetadataSearch.Common.Resources.Strings.MaskFilterName
		},
		{
			MaskColor,
			VideoOS.MetadataSearch.Common.Resources.Strings.MaskColorFilterName
		},
		{
			Scarf,
			VideoOS.MetadataSearch.Common.Resources.Strings.ScarfFilterName
		},
		{
			ScarfColor,
			VideoOS.MetadataSearch.Common.Resources.Strings.ScarfColorFilterName
		},
		{
			ShoesCategory,
			VideoOS.MetadataSearch.Common.Resources.Strings.ShoesCategoryFilterName
		},
		{
			ShoesColor,
			VideoOS.MetadataSearch.Common.Resources.Strings.ShoesColorFilterName
		},
		{
			VehicleColor,
			VideoOS.Platform.Resources.Strings.VehicleColorSearchFilter_Name
		},
		{
			LicensePlate,
			VideoOS.Platform.Resources.Strings.VehicleLicensePlateSearchFilter_Name
		},
		{
			VehicleType,
			VideoOS.MetadataSearch.Common.Resources.Strings.VehicleTypeSearchFilterName
		},
		{
			VehicleModel,
			VideoOS.MetadataSearch.Common.Resources.Strings.VehicleModelFilterName
		},
		{
			VehicleBrand,
			VideoOS.MetadataSearch.Common.Resources.Strings.VehicleBrandFilterName
		},
		{
			LicensePlateCountryCode,
			VideoOS.MetadataSearch.Common.Resources.Strings.LicensePlateCountryCodeFilterName
		},
		{
			LicensePlateIssuingEntity,
			VideoOS.MetadataSearch.Common.Resources.Strings.LicensePlateIssuingEntityFilterName
		},
		{
			LicensePlateType,
			VideoOS.MetadataSearch.Common.Resources.Strings.LicensePlateTypeFilterName
		},
		{
			Speed,
			VideoOS.MetadataSearch.Common.Resources.Strings.VehicleSpeedSearchFilterName
		},
		{
			GeoCoordinates,
			VideoOS.MetadataSearch.Common.Resources.Strings.GeoCoordinatesFilterName
		}
	};

	public static string GetValue(Guid filterId)
	{
		try
		{
			return FilterStrings[filterId];
		}
		catch (KeyNotFoundException innerException)
		{
			throw new InvalidOperationException("No filter found for the ID", innerException);
		}
	}
}
