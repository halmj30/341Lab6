namespace Lab6Starter;

/**
 * 
 * Name: Jack Halm and Nick Miller
 * Date: 11/7/2022
 * Description: A TicTacToe Game
 * Bugs: None
 * Reflection: The Git part was easy, Features 2 and 3 were 30 seconds of work, 5 and 6 took a couple minutes of work. 1 and 4 were the hardest.
 * 
 */


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
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		return builder.Build();
	}
}

