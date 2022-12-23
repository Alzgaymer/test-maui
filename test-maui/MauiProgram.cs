using test_maui.Services;
using test_maui.ViewModel;

namespace test_maui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");				
				fonts.AddFont("e-Ukraine-Medium.otf", "e-Ukraine");
			});
		builder.Services.AddSingleton<MonkeyService>();
		builder.Services.AddSingleton<MonkeyViewModel>();
		builder.Services.AddSingleton<MainPage>();

		return builder.Build();
	}
}
