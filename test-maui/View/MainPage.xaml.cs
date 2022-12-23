using test_maui.ViewModel;

namespace test_maui;

public partial class MainPage : ContentPage
{
	public MainPage(MonkeyViewModel viewModel)
	{
		InitializeComponent();
		BindingContext= viewModel;
	}
}