using CommunityToolkit.Mvvm.ComponentModel;


using System.ComponentModel;


namespace test_maui.ViewModel
{
    
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        bool isBusy;

        [ObservableProperty]
        string title;   
        
        public bool IsNotBusy => !isBusy;
    }
}