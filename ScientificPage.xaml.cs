
using HesapMakine.Services;

namespace HesapMakine.Pages;

// Bilimsel hesap makinesi sayfasının kod tarafı.
public partial class ScientificPage : ContentPage
{
	private readonly CalculatorEngine calculator = new();

	public ScientificPage()
	{
		InitializeComponent();

		// Trigonometri seçenekleri.
		TrigonometryPicker.Items.Add("sin");
		TrigonometryPicker.Items.Add("cos");
		TrigonometryPicker.Items.Add("tan");
		TrigonometryPicker.Items.Add("cot");
		TrigonometryPicker.Items.Add("sec");
		TrigonometryPicker.Items.Add("csc");
		TrigonometryPicker.Items.Add("asin");
		TrigonometryPicker.Items.Add("acos");
		TrigonometryPicker.Items.Add("atan");

		// Matematiksel fonksiyon seçenekleri.
		FunctionPicker.Items.Add("√x");
		FunctionPicker.Items.Add("∛x");
		FunctionPicker.Items.Add("log");
		FunctionPicker.Items.Add("ln");
		FunctionPicker.Items.Add("|x|");
		FunctionPicker.Items.Add("1/x");

		UpdateDisplay();
	}

	// Ekranı ve işlem geçmişini günceller.
	private void UpdateDisplay()
	{
		DisplayLabel.Text = calculator.Display;
		HistoryLabel.Text = calculator.HistoryText;
	}

	// Sayı butonları.
	private void OnNumberClicked(object sender, EventArgs e)
	{
		if (sender is Button button)
		{
			calculator.Number(button.Text);
		}

		UpdateDisplay();
	}

	// İşlem butonları.
	private void OnOperatorClicked(object sender, EventArgs e)
	{
		if (sender is Button button)
		{
			calculator.SetOperation(button.Text);
		}

		UpdateDisplay();
	}

	// Trigonometri menüsü.
	private void OnTrigonometrySelected(object sender, EventArgs e)
	{
		if (TrigonometryPicker.SelectedIndex < 0)
			return;

		string function = TrigonometryPicker.SelectedItem?.ToString() ?? "";

		calculator.InsertFunction(function);

		TrigonometryPicker.SelectedIndex = -1;

		UpdateDisplay();
	}

	// Fonksiyon menüsü.
	private void OnFunctionSelected(object sender, EventArgs e)
	{
		if (FunctionPicker.SelectedIndex < 0)
			return;

		string selected = FunctionPicker.SelectedItem?.ToString() ?? "";

		string function = selected switch
		{
			"√x" => "sqrt",
			"∛x" => "cbrt",
			"log" => "log",
			"ln" => "ln",
			"|x|" => "abs",
			"1/x" => "inv",
			_ => ""
		};

		calculator.InsertFunction(function);

		FunctionPicker.SelectedIndex = -1;

		UpdateDisplay();
	}

	// Diğer bilimsel tuşlar.
	private void OnFunctionClicked(object sender, EventArgs e)
	{
		if (sender is not Button button)
			return;

		string function = button.Text switch
		{
			"x²" => "square",
			"x³" => "cube",
			"π" => "pi",
			"e" => "e",
			"%" => "percent",
			"+/-" => "sign",
			"," => "decimal",
			_ => ""
		};

		calculator.Function(function);

		UpdateDisplay();
	}

	// Açık parantez.
	private void OnOpenParenthesisClicked(object sender, EventArgs e)
	{
		calculator.OpenParenthesis();
		UpdateDisplay();
	}

	// Kapalı parantez.
	private void OnCloseParenthesisClicked(object sender, EventArgs e)
	{
		calculator.CloseParenthesis();
		UpdateDisplay();
	}

	// Ekranı temizleme.
	private void OnClearClicked(object sender, EventArgs e)
	{
		calculator.Clear();
		UpdateDisplay();
	}

	// Son karakteri silme.
	private void OnBackspaceClicked(object sender, EventArgs e)
	{
		calculator.Backspace();
		UpdateDisplay();
	}

	// İşlemi hesaplama.
	private void OnEqualClicked(object sender, EventArgs e)
	{
		calculator.Equal();
		UpdateDisplay();
	}

	// Standart hesap makinesine geçiş.
	private async void OnStandardClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("//standard");
	}

	// Ayarlar sayfasına geçiş.
	private async void OnSettingsClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("//settings");
	}
}
