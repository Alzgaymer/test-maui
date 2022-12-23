using System.Collections.ObjectModel;
using System.Diagnostics;
using test_maui.Model;
using test_maui.Services;
using CommunityToolkit.Mvvm.Input;

namespace test_maui.ViewModel;

public partial class MonkeyViewModel : BaseViewModel
{
	MonkeyService monkeyService;
	public ObservableCollection<Monkey> Monkeys { get; } = new ();
	public MonkeyViewModel(MonkeyService monkeyService)
	{
        Title = "Home page";
        this.monkeyService = monkeyService;		
	}

	[RelayCommand]
	async Task GetMonkeysAsync()
	{
		if (IsBusy)
			return;

		try
		{
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