using System.Collections.ObjectModel;
using System.Diagnostics;
using test_maui.Model;
using test_maui.Services;
using CommunityToolkit.Mvvm.Input;
using test_maui.View;

namespace test_maui.ViewModel;

public partial class MonkeyViewModel : BaseViewModel
{
	MonkeyService monkeyService;

	IConnectivity connectivity;
	IGeolocation geolocation;
	public ObservableCollection<Monkey> Monkeys { get; } = new ();
	public MonkeyViewModel(MonkeyService monkeyService, IConnectivity connectivity, IGeolocation geolocation)
	{
        Title = "Home page";
        this.monkeyService = monkeyService;		
		this.connectivity = connectivity;
		this.geolocation = geolocation;
	}
	[RelayCommand]
	async Task GetClosestMonkeyAsync()
	{
		if (IsBusy || Monkeys.Count == 0)
			return;

		try
		{
			var permissionStatus = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
			if (permissionStatus != PermissionStatus.Granted)
			{
				permissionStatus = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

				if (permissionStatus != PermissionStatus.Granted)
				{
					await Shell.Current.DisplayAlert(
					"Location permission",
					$"Location permission isn`t granted😒",
					"Nice!");
					return;
				}
			}


			var location = await geolocation.GetLastKnownLocationAsync();
			if (location is null) location = await geolocation.GetLocationAsync(
				new GeolocationRequest()
				{
					DesiredAccuracy = GeolocationAccuracy.Medium,
					Timeout = TimeSpan.FromSeconds(30)
				}) ;
			if (location is null) return;

			var nearest = Monkeys
				.OrderBy(x => 
				location.CalculateDistance(x.Latitude, x.Longitude, DistanceUnits.Kilometers)
				).FirstOrDefault();
			if (nearest is null) return;


			await Shell.Current.DisplayAlert(
				"Closest monkey",
				$"{nearest.Name} in {nearest.Location} is the nearest monkey to you",
				"Nice!");
		}
		catch (Exception ex)
		{
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error!",
                $"Unable to get closest monkey: {ex.Message}", "OK");
            throw;
		}
	}

	[RelayCommand]
	async Task GoToDetailsAsync(Monkey monkey)
	{
		if(monkey is null)
			return;

		await Shell.Current.GoToAsync($"{nameof(DetailsPage)}", true,
			new Dictionary<string, object>()
			{
				{"Monkey", monkey }
			});
	}
	
	[RelayCommand]	
	async Task GetMonkeysAsync()
	{
		if (IsBusy)
			return;

		try
		{
			if(connectivity.NetworkAccess != NetworkAccess.Internet)
			{                
                await Shell.Current.DisplayAlert("Internet issue",
                    $"Check your network connection and try again", "OK");
				return;
            }

			IsBusy = true;
			var monkeys = await monkeyService.GetMonkeys();

			if(Monkeys.Count != 0)
				Monkeys.Clear();
					
			foreach (var monkey in monkeys)			
				Monkeys.Add(monkey);
		}	
		catch (Exception ex)
		{
			Debug.WriteLine(ex);
			await Shell.Current.DisplayAlert("Error!",
				$"Unable to get monkeys: {ex.Message}","OK");			
		}
		finally 
		{
			IsBusy = false;
		}
	}
}