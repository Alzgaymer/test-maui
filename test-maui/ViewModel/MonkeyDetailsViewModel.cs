using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using test_maui.Model;

namespace test_maui.ViewModel;

[QueryProperty("Monkey", "Monkey")]
public partial class MonkeyDetailsViewModel : BaseViewModel
{

	IMap map;
	public MonkeyDetailsViewModel(IMap map)
	{
		this.map = map;
	}
	[ObservableProperty]
	Monkey monkey;

	[RelayCommand]
	async Task OpenMapAsync()
	{
		try
		{
			await map.OpenAsync(Monkey.Latitude, Monkey.Longitude,
				new MapLaunchOptions()
				{
					Name= Monkey.Name,
					NavigationMode = NavigationMode.Driving
				});	
		}
		catch (Exception ex)
		{
			Debug.WriteLine(ex);
			await Shell.Current.DisplayAlert(
				"Map error",
				$"Can`t open map. {ex.Message}",
				"Ok");
			
		}
	}

}