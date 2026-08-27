using System;

namespace VideoOS.Platform.Client;

public enum ImageViewerType
{
	Unspecified,
	CameraViewItem,
	HotspotViewItem,
	CarouselViewItem,
	MatrixViewItem,
	MIP,
	BookmarkEditDialog,
	RecordingPreview,
	SequenceExplorerPreview,
	[Obsolete("This enumeration value is obsolete and will no longer be supported from Smart Client 2021 R2", false)]
	CameraNavigationViewItem,
	[Obsolete("This enumeration value is obsolete and will no longer be supported from Smart Client 2021 R2", false)]
	CameraNavigationViewItemPreview,
	Export,
	ManagePresets,
	SearchResultPreview,
	SmartSearchSelection
}
