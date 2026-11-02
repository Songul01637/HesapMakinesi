using Microsoft.Maui.Storage;

namespace HesapMakine.Pages;

// Ayarlar sayfasının kod tarafını tanımlar.
public partial class SettingsPage : ContentPage
{
	// Sayfa oluşturulduğunda çalışan yapıcı metot.
	public SettingsPage()
	{
		// XAML dosyasındaki arayüz elemanlarını yükler.
		InitializeComponent();

		// Daha önce kaydedilmiş tema tercihini getirir.
		string savedTheme = Preferences.Get("AppTheme", "System");

		// Kaydedilen temaya göre seçim listesini ayarlar.
		if (savedTheme == "Light")
			ThemePicker.SelectedIndex = 1;

		else if (savedTheme == "Dark")
			ThemePicker.SelectedIndex = 2;

		else
			ThemePicker.SelectedIndex = 0;
	}

	// Temayı kaydet butonuna basıldığında çalışır.
	private async void OnSaveThemeClicked(object sender, EventArgs e)
	{
		// Kullanıcının seçtiği temayı belirler.
		string selectedTheme;

		if (ThemePicker.SelectedIndex == 0)
			selectedTheme = "System";

		else if (ThemePicker.SelectedIndex == 1)
			selectedTheme = "Light";

		else if (ThemePicker.SelectedIndex == 2)
			selectedTheme = "Dark";

		else
		{
			await DisplayAlert("Uyarı", "Lütfen bir tema seçin.", "Tamam");
			return;
		}

		// Tema tercihini kalıcı olarak kaydeder.
		Preferences.Set("AppTheme", selectedTheme);

		// Seçilen temayı uygulamaya anında uygular.
		if (selectedTheme == "System")
			Application.Current!.UserAppTheme = AppTheme.Unspecified;

		else if (selectedTheme == "Light")
			Application.Current!.UserAppTheme = AppTheme.Light;

		else if (selectedTheme == "Dark")
			Application.Current!.UserAppTheme = AppTheme.Dark;

		// Kullanıcıya bilgi verir.
		await DisplayAlert("Bilgi", "Tema ayarı kaydedildi.", "Tamam");
	}

	// Standart hesap makinesi sayfasına geçiş yapar.
	private async void OnStandardClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("//standard");
	}

	// Bilimsel hesap makinesi sayfasına geçiş yapar.
	private async void OnScientificClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("//scientific");
	}
}