using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using VideoOS.UI.Common.WPF.IO.FolderBrowser.resources;
using VideoOS.UI.Common.WPF.LoadAndGrowTreeView;
using VideoOS.UI.Common.WPF.Utils.Converters;
using VideoOS.UI.Common.WPF.Utils.Threading;

namespace VideoOS.UI.Common.WPF.IO;

public partial class FolderBrowserTreeView : UserControl, IComponentConnector
{
	private ImageSource _treeImageServer;

	private ImageSource _treeImageCddisk;

	private ImageSource _treeImageFixedDisk;

	private ImageSource _treeImageNetworkDisk;

	private List<DriveType> _supportedDriveTypes;

	private List<FileAttributes> _excludedFileAttributes;

	private bool _hasInitialPath;

	private string _initialPath;

	private string[] _initialPathArray;

	private bool _isInitializing = true;

	private CultureInfo _uiCulture;

	public static readonly DependencyProperty NodeImageProperty = DependencyProperty.RegisterAttached("NodeImage", typeof(ImageSource), typeof(FolderBrowserTreeView), new FrameworkPropertyMetadata(null));

	public bool ShowDesktop { get; set; }

	public bool ShowMyDocuments { get; set; }

	public ImageSource TreeImageServer
	{
		set
		{
			_treeImageServer = value;
		}
	}

	public ImageSource TreeImageCddisk
	{
		set
		{
			_treeImageCddisk = value;
		}
	}

	public ImageSource TreeImageFixedDisk
	{
		set
		{
			_treeImageFixedDisk = value;
		}
	}

	public ImageSource TreeImageNetworkDisk
	{
		set
		{
			_treeImageNetworkDisk = value;
		}
	}

	public event EventHandler<NodeExpandedEventArgs> NodeExpanding;

	public event EventHandler<FolderSelectionChangedEventArgs> FolderSelectionChanged;

	public static ImageSource GetNodeImage(DependencyObject depObj)
	{
		return (ImageSource)depObj.GetValue(NodeImageProperty);
	}

	public static void SetNodeImage(DependencyObject depObj, ImageSource value)
	{
		depObj.SetValue(NodeImageProperty, value);
	}

	public FolderBrowserTreeView()
	{
		InitializeComponent();
		string text = "pack://application:,,,/VideoOS.UI.Common.WPF;component/IO/FolderBrowser/resources/";
		TreeImageServer = BitmapConverter.ImageSourceFromUri(new Uri(text + "Folder Closed Flat 16 n p.png"));
		TreeImageCddisk = BitmapConverter.ImageSourceFromUri(new Uri(text + "CD_Drive.png"));
		TreeImageFixedDisk = BitmapConverter.ImageSourceFromUri(new Uri(text + "Hard_Drive.png"));
		TreeImageNetworkDisk = BitmapConverter.ImageSourceFromUri(new Uri(text + "Network_Drive.png"));
	}

	public void Initialize(List<DriveType> supportedDriveTypes, List<FileAttributes> excludedFileAttributes, string initialPath)
	{
		_supportedDriveTypes = supportedDriveTypes;
		_excludedFileAttributes = excludedFileAttributes;
		_initialPath = initialPath;
		if (_initialPath != null && _initialPath.EndsWith("\\"))
		{
			_initialPath = _initialPath.Remove(_initialPath.Length - 1);
		}
		_initialPathArray = _initialPath.Split(Path.DirectorySeparatorChar);
		_hasInitialPath = !string.IsNullOrEmpty(_initialPath);
		_isInitializing = true;
		_uiCulture = Thread.CurrentThread.CurrentUICulture;
		InitTree();
	}

	private void InitTree()
	{
		if (_lazyTreeFileStructure != null)
		{
			List<TreeViewItemData> list = new List<TreeViewItemData>();
			list.Add(new TreeViewItemData(null, Strings.LoadingDrives, hasPossibleSubItems: false, null));
			_lazyTreeFileStructure.ItemsSource = list;
			ThreadPool.QueueUserWorkItem(LoadDrivesThread, _lazyTreeFileStructure);
		}
	}

	private void AddNode(object identifier, bool hasPossibleSubitems, ImageSource nodeImage, TreeViewItemData parent, List<TreeViewItemData> addNodeToThisCollection, bool expand, bool select)
	{
		string header = "";
		if (identifier is DisplayInfo displayInfo)
		{
			header = displayInfo.DisplayText;
		}
		FileStructureTreeViewItemData fileStructureTreeViewItemData = new FileStructureTreeViewItemData(identifier, header, hasPossibleSubitems, parent);
		SetNodeImage(fileStructureTreeViewItemData, nodeImage);
		fileStructureTreeViewItemData.IsSelected = select;
		fileStructureTreeViewItemData.IsExpanded = expand;
		addNodeToThisCollection.Add(fileStructureTreeViewItemData);
	}

	private void LoadDrivesThread(object loadAndGrowTreeViewObj)
	{
		VideoOS.UI.Common.WPF.LoadAndGrowTreeView.LoadAndGrowTreeView loadAndGrowTreeView = loadAndGrowTreeViewObj as VideoOS.UI.Common.WPF.LoadAndGrowTreeView.LoadAndGrowTreeView;
		Thread.CurrentThread.CurrentUICulture = _uiCulture;
		if (loadAndGrowTreeView == null)
		{
			return;
		}
		List<DisplayInfo> drivesReadyToUse = new List<DisplayInfo>();
		List<TreeViewItemData> result = new List<TreeViewItemData>();
		try
		{
			AddSpecialFolders(drivesReadyToUse);
			DriveInfo[] drives = DriveInfo.GetDrives();
			if (drives != null)
			{
				DriveInfo[] array = drives;
				foreach (DriveInfo driveInfo in array)
				{
					try
					{
						if (_supportedDriveTypes.Contains(driveInfo.DriveType) && driveInfo.IsReady)
						{
							DriveDisplayInfo driveDisplayInfo = new DriveDisplayInfo(driveInfo);
							switch (driveInfo.DriveType)
							{
							case DriveType.CDRom:
								driveDisplayInfo.IconCollapsed = _treeImageCddisk;
								break;
							case DriveType.Fixed:
								driveDisplayInfo.IconCollapsed = _treeImageFixedDisk;
								break;
							case DriveType.Network:
								driveDisplayInfo.IconCollapsed = _treeImageNetworkDisk;
								break;
							default:
								driveDisplayInfo.IconCollapsed = _treeImageFixedDisk;
								break;
							}
							drivesReadyToUse.Add(driveDisplayInfo);
						}
					}
					catch (Exception)
					{
					}
				}
			}
		}
		catch (Exception)
		{
			drivesReadyToUse.Clear();
			CustomDisplayInfo item = new CustomDisplayInfo(Strings.ErrorUnknown);
			drivesReadyToUse.Add(item);
		}
		base.Dispatcher.InvokeOrCall(delegate
		{
			string text = "";
			if (_hasInitialPath)
			{
				string initPathWithRespectToSpecialFolders = GetInitPathWithRespectToSpecialFolders();
				text = Path.GetPathRoot(initPathWithRespectToSpecialFolders);
				if (text.EndsWith(":"))
				{
					text += "\\";
				}
				int num = initPathWithRespectToSpecialFolders.IndexOf("\\");
				if (string.IsNullOrWhiteSpace(text) && num > 0)
				{
					text = initPathWithRespectToSpecialFolders.Substring(0, num);
				}
			}
			foreach (DisplayInfo item2 in drivesReadyToUse)
			{
				bool expand = false;
				bool flag = false;
				if (_isInitializing && _hasInitialPath && (text == item2.DisplayText || text == item2.FullPath))
				{
					expand = true;
					if (_initialPath.Equals(item2.FullPath))
					{
						_isInitializing = false;
						flag = true;
					}
				}
				AddNode(item2, hasPossibleSubitems: true, item2.IconCollapsed, null, result, expand, flag);
			}
			loadAndGrowTreeView.ItemsSource = result;
		});
	}

	private void AddSpecialFolders(List<DisplayInfo> drivesReadyToUse)
	{
		if (ShowDesktop)
		{
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
			if (Directory.Exists(folderPath))
			{
				DirectoryDisplayInfo directoryDisplayInfo = new DirectoryDisplayInfo(new DirectoryInfo(folderPath), Strings.DesktopDisplayName);
				directoryDisplayInfo.IconCollapsed = _treeImageServer;
				drivesReadyToUse.Add(directoryDisplayInfo);
			}
		}
		if (ShowMyDocuments)
		{
			string folderPath2 = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			if (Directory.Exists(folderPath2))
			{
				DirectoryDisplayInfo directoryDisplayInfo2 = new DirectoryDisplayInfo(new DirectoryInfo(folderPath2), Strings.MyDocumentsDisplayName);
				directoryDisplayInfo2.IconCollapsed = _treeImageServer;
				drivesReadyToUse.Add(directoryDisplayInfo2);
			}
		}
	}

	private string GetInitPathWithRespectToSpecialFolders()
	{
		string text = _initialPath;
		if (ShowDesktop && text.StartsWith(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)))
		{
			text = text.Replace(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), Strings.DesktopDisplayName);
		}
		if (ShowMyDocuments && text.StartsWith(Environment.GetFolderPath(Environment.SpecialFolder.Personal)))
		{
			text = text.Replace(Environment.GetFolderPath(Environment.SpecialFolder.Personal), Strings.MyDocumentsDisplayName);
		}
		return text;
	}

	private TreeViewItem FindNextItem(TreeViewItem currentItem, bool walkIntoSubtree, TreeViewItem hasBeenAtTheBottom = null)
	{
		if (walkIntoSubtree && currentItem != null && currentItem.IsExpanded && currentItem.ItemContainerGenerator.ContainerFromIndex(0) is TreeViewItem result)
		{
			return result;
		}
		if (currentItem != null)
		{
			ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(currentItem);
			if (itemsControl != null)
			{
				int num = itemsControl.ItemContainerGenerator.IndexFromContainer(currentItem);
				int count = itemsControl.Items.Count;
				if (itemsControl is TreeView && num == count - 1)
				{
					if (hasBeenAtTheBottom != null)
					{
						return hasBeenAtTheBottom;
					}
					return currentItem;
				}
				while (num < count)
				{
					num++;
					if (itemsControl.ItemContainerGenerator.ContainerFromIndex(num) is TreeViewItem result2)
					{
						return result2;
					}
				}
				if (itemsControl is TreeViewItem currentItem2)
				{
					if (hasBeenAtTheBottom == null)
					{
						return FindNextItem(currentItem2, walkIntoSubtree: false, currentItem);
					}
					return FindNextItem(currentItem2, walkIntoSubtree: false, hasBeenAtTheBottom);
				}
				return null;
			}
		}
		return null;
	}

	private List<TreeViewItemData> LazyTreeFileStructure_ItemHasBeenExpandedSynchronousEvent(object sender, ItemHasBeenExpandedSynchronousArgs args)
	{
		if (args == null || args.Data == null)
		{
			return null;
		}
		return ExpandNode(args.Data);
	}

	private List<TreeViewItemData> ExpandNode(TreeViewItemData nodeData)
	{
		if (!(nodeData.Identifier is DisplayInfo info))
		{
			return new List<TreeViewItemData>();
		}
		NodeExpandedEventArgs e = new NodeExpandedEventArgs(info);
		EventHandler<NodeExpandedEventArgs> eventHandler = this.NodeExpanding;
		if (eventHandler != null)
		{
			eventHandler(this, e);
			if (e.Handled)
			{
				List<TreeViewItemData> list = new List<TreeViewItemData>();
				{
					foreach (DisplayInfo child in e.Children)
					{
						AddNode(child, child.MightHaveChildren, child.IconCollapsed, nodeData, list, expand: false, select: false);
					}
					return list;
				}
			}
		}
		if (nodeData.Identifier is DriveDisplayInfo)
		{
			return ExpandNode(nodeData.Identifier as DriveDisplayInfo, nodeData);
		}
		if (nodeData.Identifier is DirectoryDisplayInfo)
		{
			return ExpandNode(nodeData.Identifier as DirectoryDisplayInfo, nodeData);
		}
		return new List<TreeViewItemData>();
	}

	private List<TreeViewItemData> ExpandNode(DirectoryDisplayInfo currentDirectoryInfo, TreeViewItemData data)
	{
		List<TreeViewItemData> list = new List<TreeViewItemData>();
		if (currentDirectoryInfo == null)
		{
			return list;
		}
		try
		{
			if (currentDirectoryInfo.DirectoryInfo.Exists)
			{
				DirectoryInfo[] directories = currentDirectoryInfo.DirectoryInfo.GetDirectories();
				foreach (DirectoryInfo directoryInfo in directories)
				{
					bool flag = false;
					foreach (FileAttributes excludedFileAttribute in _excludedFileAttributes)
					{
						if (directoryInfo.Attributes.HasFlag(excludedFileAttribute))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						continue;
					}
					bool expand = false;
					bool flag2 = false;
					if (_isInitializing && _hasInitialPath && IsPartOfInitialPath(directoryInfo.FullName))
					{
						expand = true;
						if (_initialPath.Equals(directoryInfo.FullName))
						{
							_isInitializing = false;
							flag2 = true;
						}
					}
					string displayName = GetDisplayName(directoryInfo);
					AddNode(new DirectoryDisplayInfo(directoryInfo, displayName), hasPossibleSubitems: true, _treeImageServer, data, list, expand, flag2);
				}
			}
			else
			{
				list.Add(new TreeViewItemData(null, Strings.ErrorOpenFolder, hasPossibleSubItems: false, data));
			}
		}
		catch (Exception)
		{
			list.Add(new TreeViewItemData(null, Strings.ErrorOpenFolder, hasPossibleSubItems: false, data));
		}
		return list;
	}

	private static string GetDisplayName(DirectoryInfo directoryInfo)
	{
		string text = "LocalizedResourceName";
		string result = directoryInfo.Name;
		if ((directoryInfo.Attributes & FileAttributes.System) == FileAttributes.System)
		{
			string path = Path.Combine(directoryInfo.FullName, "desktop.ini");
			if (File.Exists(path))
			{
				foreach (string item in File.ReadLines(path))
				{
					if (item.StartsWith(text))
					{
						result = item.Substring(text.Length + 1, item.Length - text.Length - 1);
						break;
					}
				}
			}
		}
		return result;
	}

	private List<TreeViewItemData> ExpandNode(DriveDisplayInfo driveDisplayInfo, TreeViewItemData data)
	{
		List<TreeViewItemData> list = new List<TreeViewItemData>();
		if (driveDisplayInfo == null)
		{
			return list;
		}
		try
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(driveDisplayInfo.DriveInfo.Name);
			if (directoryInfo.Exists)
			{
				DirectoryInfo[] directories = directoryInfo.GetDirectories();
				foreach (DirectoryInfo directoryInfo2 in directories)
				{
					bool flag = false;
					foreach (FileAttributes excludedFileAttribute in _excludedFileAttributes)
					{
						if (directoryInfo2.Attributes.HasFlag(excludedFileAttribute))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						continue;
					}
					bool expand = false;
					bool flag2 = false;
					if (_isInitializing && _hasInitialPath && IsPartOfInitialPath(directoryInfo2.FullName))
					{
						expand = true;
						if (_initialPath.Equals(directoryInfo2.FullName))
						{
							_isInitializing = false;
							flag2 = true;
						}
					}
					AddNode(new DirectoryDisplayInfo(directoryInfo2, directoryInfo2.Name), hasPossibleSubitems: true, _treeImageServer, data, list, expand, flag2);
				}
			}
			else
			{
				list.Add(new TreeViewItemData(null, Strings.ErrorOpenDrive, hasPossibleSubItems: false, data));
			}
		}
		catch (Exception)
		{
			list.Add(new TreeViewItemData(null, Strings.ErrorOpenDrive, hasPossibleSubItems: false, data));
		}
		return list;
	}

	private bool IsPartOfInitialPath(string path)
	{
		string[] array = path.Split(Path.DirectorySeparatorChar);
		int num = array.Length;
		if (num > _initialPathArray.Length)
		{
			return false;
		}
		for (int i = 0; i < num; i++)
		{
			if (_initialPathArray[i] != array[i])
			{
				return false;
			}
		}
		return true;
	}

	private void TreeViewFileStructure_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
	{
		_lazyTreeFileStructure.SelectedTreeViewItem?.BringIntoView();
		if (_lazyTreeFileStructure == null || _lazyTreeFileStructure.SelectedItem == null || !(_lazyTreeFileStructure.SelectedItem is TreeViewItemData { Identifier: DisplayInfo identifier } treeViewItemData))
		{
			return;
		}
		EventHandler<FolderSelectionChangedEventArgs> eventHandler = this.FolderSelectionChanged;
		if (eventHandler == null)
		{
			return;
		}
		string text = null;
		if (string.IsNullOrWhiteSpace(identifier.FullPath) && treeViewItemData.Parent != null)
		{
			if (treeViewItemData.Parent.Identifier is DisplayInfo displayInfo)
			{
				text = displayInfo.FullPath;
			}
			if (string.IsNullOrWhiteSpace(text) && treeViewItemData.Parent.Parent != null && treeViewItemData.Parent.Parent.Identifier is DisplayInfo displayInfo2)
			{
				text = displayInfo2.FullPath;
			}
		}
		eventHandler(this, new FolderSelectionChangedEventArgs(identifier, text));
	}

	private void LazyTreeFileStructure_TextInput(object sender, TextCompositionEventArgs e)
	{
		string text = e.Text;
		if (string.IsNullOrWhiteSpace(text))
		{
			return;
		}
		text = text.ToLower();
		TreeViewItem selectedTreeViewItem = _lazyTreeFileStructure.SelectedTreeViewItem;
		if (selectedTreeViewItem == null)
		{
			return;
		}
		TreeViewItem treeViewItem = FindNextItem(selectedTreeViewItem, walkIntoSubtree: true);
		if (selectedTreeViewItem == treeViewItem)
		{
			treeViewItem = _lazyTreeFileStructure.ItemContainerGenerator.ContainerFromIndex(0) as TreeViewItem;
		}
		while (selectedTreeViewItem != treeViewItem && treeViewItem != null)
		{
			if (treeViewItem.DataContext is FileStructureTreeViewItemData fileStructureTreeViewItemData)
			{
				string text2 = fileStructureTreeViewItemData.Header as string;
				if (!string.IsNullOrEmpty(text2))
				{
					text2 = text2.ToLower();
					if (text2.StartsWith(text))
					{
						fileStructureTreeViewItemData.IsSelected = true;
						break;
					}
				}
			}
			TreeViewItem treeViewItem2 = treeViewItem;
			treeViewItem = FindNextItem(treeViewItem, walkIntoSubtree: true);
			if (treeViewItem2 == treeViewItem)
			{
				treeViewItem = _lazyTreeFileStructure.ItemContainerGenerator.ContainerFromIndex(0) as TreeViewItem;
			}
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "9.0.6.0")]
	public Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
