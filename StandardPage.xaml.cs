
using HesapMakine.Services;

namespace HesapMakine.Pages;

public partial class StandardPage : ContentPage
{
	private readonly CalculatorEngine calculator;

	public StandardPage()
	{
		InitializeComponent();

		calculator = new CalculatorEngine();

		UpdateDisplay();
	}

	// Ekranı ve işlem geçmişini günceller.
	private void UpdateDisplay()
	{
		DisplayLabel.Text = calculator.Display;

		if (HistoryLabel != null)
			HistoryLabel.Text = calculator.HistoryText;
	}

	// Sayı butonları.
	private void OnNumberClicked(object sender, EventArgs e)
	{
		if (sender is Button button)
		{
			calculator.Number(button.Text);
			UpdateDisplay();
		}
	}

	// Dört işlem ve üs alma butonları.
	private void OnOperatorClicked(object sender, EventArgs e)
	{
		if (sender is Button button)
		{
			calculator.SetOperation(button.Text);
			UpdateDisplay();
		}
	}

	// Parantez butonları.
	private void OnParenthesisClicked(object sender, EventArgs e)
	{
		if (sender is Button button)
		{
			if (button.Text == "(")
				calculator.OpenParenthesis();
			else if (button.Text == ")")
				calculator.CloseParenthesis();

			UpdateDisplay();
		}
	}

	// Ekranı temizler.
	private void OnClearClicked(object sender, EventArgs e)
	{
		calculator.Clear();
		UpdateDisplay();
	}

	// Son karakteri siler.
	private void OnBackspaceClicked(object sender, EventArgs e)
	{
		calculator.Backspace();
		UpdateDisplay();
	}

	// Yüzde işlemi.
	private void OnPercentClicked(object sender, EventArgs e)
	{
		calculator.InsertFunction("percent");
		UpdateDisplay();
	}

	// Sayının işaretini değiştirir.
	private void OnSignClicked(object sender, EventArgs e)
	{
		calculator.ChangeSign();
		UpdateDisplay();
	}

	// Ondalık ayırıcı.
	private void OnDecimalClicked(object sender, EventArgs e)
	{
		calculator.Decimal();
		UpdateDisplay();
	}

	// Sonucu hesaplar.
	private void OnEqualClicked(object sender, EventArgs e)
	{
		calculator.Equal();
		UpdateDisplay();
	}

	// Bilimsel hesap makinesine geçer.
	private async void OnScientificClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("//scientific");
	}

	// Ayarlar sayfasına geçer.
	private async void OnSettingsClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("//settings");
	}
}