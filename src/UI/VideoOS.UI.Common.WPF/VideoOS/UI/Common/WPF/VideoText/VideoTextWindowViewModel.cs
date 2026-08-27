using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

namespace VideoOS.UI.Common.WPF.VideoText;

public class VideoTextWindowViewModel : ViewModelBase
{
	private SlideViewModel _slide;

	private ObservableCollection<SlideViewModel> _preSlides;

	private ObservableCollection<SlideViewModel> _postSlides;

	private bool _slidesAreReadOnly;

	public SlideViewModel Slide
	{
		get
		{
			return _slide;
		}
		set
		{
			SetProperty(ref _slide, value, "Slide");
		}
	}

	public ObservableCollection<SlideViewModel> PreSlides
	{
		get
		{
			return _preSlides;
		}
		set
		{
			SetProperty(ref _preSlides, value, "PreSlides");
		}
	}

	public IList<SlideViewModel> ValidPreSlides => _preSlides.Where((SlideViewModel x) => x.IsValid).ToList();

	public ObservableCollection<SlideViewModel> PostSlides
	{
		get
		{
			return _postSlides;
		}
		set
		{
			SetProperty(ref _postSlides, value, "PostSlides");
		}
	}

	public IList<SlideViewModel> ValidPostSlides => _postSlides.Where((SlideViewModel x) => x.IsValid).ToList();

	public ICommand FontUpCommand { get; set; }

	public ICommand FontDownCommand { get; set; }

	public bool SlidesAreReadOnly
	{
		get
		{
			return _slidesAreReadOnly;
		}
		set
		{
			if (SetProperty(ref _slidesAreReadOnly, value, "SlidesAreReadOnly"))
			{
				OnPropertyChanged("FontDownCommand");
				OnPropertyChanged("FontUpCommand");
			}
		}
	}

	public string SerializedSlides
	{
		get
		{
			IEnumerable<SlideData> source = PreSlides.Select((SlideViewModel slideViewModel) => slideViewModel.SlideData);
			return SlideSerializer.SerializeSlides(postSlides: PostSlides.Select((SlideViewModel slideViewModel) => slideViewModel.SlideData).ToList(), preSlides: source.ToList());
		}
		set
		{
			PreSlides = new ObservableCollection<SlideViewModel>();
			PostSlides = new ObservableCollection<SlideViewModel>();
			List<SlideData> list = new List<SlideData>();
			List<SlideData> list2 = new List<SlideData>();
			SlideSerializer.DeserializeSlides(value, list, list2);
			foreach (SlideData item in list)
			{
				PreSlides.Add(new SlideViewModel(item));
			}
			foreach (SlideData item2 in list2)
			{
				PostSlides.Add(new SlideViewModel(item2));
			}
		}
	}

	public VideoTextWindowViewModel()
		: this(new List<SlideViewModel>(), new List<SlideViewModel>(), slidesAreReadOnly: false)
	{
	}

	public VideoTextWindowViewModel(IList<SlideViewModel> preSlides, IList<SlideViewModel> postSlides, bool slidesAreReadOnly)
	{
		SlidesAreReadOnly = slidesAreReadOnly;
		PreSlides = new ObservableCollection<SlideViewModel>(preSlides);
		PostSlides = new ObservableCollection<SlideViewModel>(postSlides);
		AddDefaultSlideIfNeeded(PreSlides);
		AddDefaultSlideIfNeeded(PostSlides);
		if (PreSlides.Count > 0)
		{
			Slide = PreSlides.FirstOrDefault();
		}
		FontUpCommand = new DelegateCommand(FontUpCommandExecute, FontUpCommandCanExecute);
		FontDownCommand = new DelegateCommand(FontDownCommandExecute, FontDownCommandCanExecute);
	}

	private void AddDefaultSlideIfNeeded(ObservableCollection<SlideViewModel> slideViewModels)
	{
		if (slideViewModels.Count == 0)
		{
			SlideViewModel slideViewModel = new SlideViewModel();
			slideViewModel.IsReadOnly = SlidesAreReadOnly;
			slideViewModels.Add(slideViewModel);
		}
	}

	private void FontDownCommandExecute(object obj)
	{
		Slide.FontSize -= 3.0;
		if (Slide.FontSize <= 1.0)
		{
			Slide.FontSize = 1.0;
			OnPropertyChanged("FontDownCommand");
		}
	}

	private bool FontDownCommandCanExecute(object arg)
	{
		if (!SlidesAreReadOnly)
		{
			SlideViewModel slide = Slide;
			if (slide == null)
			{
				return false;
			}
			return slide.FontSize > 1.0;
		}
		return false;
	}

	private void FontUpCommandExecute(object obj)
	{
		Slide.FontSize += 3.0;
		if (Slide.FontSize >= 1.0)
		{
			OnPropertyChanged("FontDownCommand");
		}
	}

	private bool FontUpCommandCanExecute(object arg)
	{
		return !SlidesAreReadOnly;
	}
}
