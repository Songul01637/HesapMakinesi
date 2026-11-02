using Microsoft.Maui.Storage;

namespace HesapMakine;

public partial class App : Application // Uygulamanın ana sınıfı
{
	public App() // App sınıfının yapıcı metodu
	{
		InitializeComponent(); // XAML dosyasındaki bileşenleri yükler

		// Daha önce kaydedilmiş tema tercihini getirir.
		// Daha önce seçim yapılmamışsa Sistem teması kullanılır.
		string savedTheme = Preferences.Get("AppTheme", "System");

		// Kaydedilen tercihe göre uygulama temasını ayarlar.
		if (savedTheme == "Light")
		{
			UserAppTheme = AppTheme.Light;
		}
		else if (savedTheme == "Dark")
		{
			UserAppTheme = AppTheme.Dark;
		}
		else
		{
			UserAppTheme = AppTheme.Unspecified;
		}

		// Uygulamanın başlangıç sayfası olarak AppShell'i belirler.
		MainPage = new AppShell();
	}

	// Uygulama penceresi oluşturulurken çağrılan metot.
	protected override Window CreateWindow(IActivationState? activationState)
	{
		// Varsayılan pencereyi oluşturur.
		var window = base.CreateWindow(activationState);

		// Pencerenin başlangıç genişliğini ayarlar.
		window.Width = 420;

		// Pencerenin başlangıç yüksekliğini ayarlar.
		window.Height = 700;

		// Boyutları ayarlanmış pencereyi döndürür.
		return window;
	}
}