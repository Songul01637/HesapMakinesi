using System.Globalization;
using System.Text.Json;

namespace HesapMakine.Services;

// Standart ve bilimsel hesaplamaları ortak olarak yönetir.
public class CalculatorEngine
{
	private readonly CultureInfo culture =
		CultureInfo.GetCultureInfo("tr-TR");

	private const string HistoryFile = "history.json";

	private List<string> history = new();

	private string expression = "";
	private bool resultShown = false;

	public string Display =>
		string.IsNullOrEmpty(expression) ? "0" : expression;

	public string HistoryText =>
		string.Join("\n", history.TakeLast(5).Reverse());

	public CalculatorEngine()
	{
		LoadHistory();
	}

	private string HistoryPath =>
		Path.Combine(FileSystem.AppDataDirectory, HistoryFile);

	// İşlem geçmişini yükler.
	private void LoadHistory()
	{
		try
		{
			if (File.Exists(HistoryPath))
			{
				string json = File.ReadAllText(HistoryPath);

				history =
					JsonSerializer.Deserialize<List<string>>(json)
					?? new List<string>();
			}
		}
		catch
		{
			history = new List<string>();
		}
	}

	// İşlem geçmişini kaydeder.
	private void SaveHistory()
	{
		try
		{
			string json = JsonSerializer.Serialize(history);
			File.WriteAllText(HistoryPath, json);
		}
		catch
		{
			// Kayıt hatası uygulamanın çalışmasını engellemez.
		}
	}

	private void AddHistory(string item)
	{
		history.Add(item);
		SaveHistory();
	}

	// Sayı ekler.
	public void Number(string number)
	{
		if (resultShown)
		{
			expression = "";
			resultShown = false;
		}

		if (expression.EndsWith(")") ||
			expression.EndsWith("π") ||
			expression.EndsWith("e"))
		{
			expression += "×";
		}

		expression += number;
	}

	// Ondalık ayırıcı ekler.
	public void Decimal()
	{
		if (resultShown)
		{
			expression = "";
			resultShown = false;
		}

		string current = GetCurrentNumber();

		if (!current.Contains(","))
		{
			if (string.IsNullOrEmpty(current))
				expression += "0";

			expression += ",";
		}
	}

	private string GetCurrentNumber()
	{
		int i = expression.Length - 1;

		while (i >= 0 &&
			   (char.IsDigit(expression[i]) || expression[i] == ','))
		{
			i--;
		}

		return expression[(i + 1)..];
	}

	// Matematiksel işlem ekler.
	public void SetOperation(string op)
	{
		if (resultShown)
			resultShown = false;

		if (string.IsNullOrEmpty(expression))
		{
			if (op == "-")
				expression = "-";

			return;
		}

		char last = expression[^1];

		if ("+-×÷^".Contains(last))
		{
			if (op == "-" && last != '-')
			{
				expression += op;
				return;
			}

			expression = expression[..^1];
		}

		expression += op;
	}

	// Açma parantezi ekler.
	public void OpenParenthesis()
	{
		if (resultShown)
		{
			expression = "";
			resultShown = false;
		}

		if (expression.Length > 0)
		{
			char last = expression[^1];

			if (char.IsDigit(last) ||
				last == ')' ||
				last == 'π' ||
				last == 'e')
			{
				expression += "×";
			}
		}

		expression += "(";
	}

	// Kapatma parantezi ekler.
	public void CloseParenthesis()
	{
		int open = expression.Count(c => c == '(');
		int close = expression.Count(c => c == ')');

		if (open > close &&
			expression.Length > 0 &&
			!"+-×÷^(".Contains(expression[^1]))
		{
			expression += ")";
		}
	}

	// Bilimsel fonksiyon ekler.
	public void InsertFunction(string function)
	{
		if (resultShown)
		{
			resultShown = false;
		}

		// Sabitler.
		if (function == "pi" || function == "e")
		{
			if (expression.Length > 0)
			{
				char last = expression[^1];

				if (char.IsDigit(last) ||
					last == ')' ||
					last == 'π' ||
					last == 'e')
				{
					expression += "×";
				}
			}

			expression += function == "pi" ? "π" : "e";
			return;
		}

		// Üs işlemleri.
		if (function == "square" || function == "cube")
		{
			if (expression.Length == 0)
				return;

			expression += function == "square" ? "^2" : "^3";
			return;
		}

		// İşaret değiştirme.
		if (function == "sign")
		{
			if (expression.Length == 0)
				expression = "-";
			else if (expression.StartsWith("-"))
				expression = expression[1..];
			else
				expression = "-" + expression;

			return;
		}

		// Yüzde.
		if (function == "percent")
		{
			if (expression.Length > 0)
				expression += "÷100";

			return;
		}

		// Fonksiyon isimleri.
		string name = function switch
		{
			"sqrt" => "sqrt",
			"cbrt" => "cbrt",

			"sin" => "sin",
			"cos" => "cos",
			"tan" => "tan",
			"cot" => "cot",
			"sec" => "sec",
			"csc" => "csc",

			"asin" => "asin",
			"acos" => "acos",
			"atan" => "atan",

			"log" => "log",
			"ln" => "ln",
			"abs" => "abs",
			"1/x" => "inv",

			_ => ""
		};

		if (name == "")
			return;

		if (expression.Length > 0)
		{
			char last = expression[^1];

			if (char.IsDigit(last) ||
				last == ')' ||
				last == 'π' ||
				last == 'e')
			{
				expression += "×";
			}
		}

		expression += name + "(";
	}

	// Eski kodla uyumluluk.
	public void Function(string function)
	{
		if (function == "decimal")
		{
			Decimal();
			return;
		}

		InsertFunction(function);
	}

	public void ChangeSign()
	{
		InsertFunction("sign");
	}

	// Son karakteri siler.
	public void Backspace()
	{
		if (expression.Length > 0)
		{
			expression = expression[..^1];
			resultShown = false;
		}
	}

	// Ekranı temizler.
	public void Clear()
	{
		expression = "";
		resultShown = false;
	}

	// İşlemi hesaplar.
	public void Equal()
	{
		if (string.IsNullOrWhiteSpace(expression))
			return;

		try
		{
			string original = expression;

			int open = expression.Count(c => c == '(');
			int close = expression.Count(c => c == ')');

			expression += new string(')', Math.Max(0, open - close));

			double result = new ExpressionParser(expression).Parse();

			if (double.IsNaN(result) || double.IsInfinity(result))
				throw new ArithmeticException();

			string formatted = result.ToString("G15", culture);

			AddHistory($"{original} = {formatted}");

			expression = formatted;
			resultShown = true;
		}
		catch
		{
			expression = "Hata";
			resultShown = true;
		}
	}

	// İşlem geçmişini temizler.
	public void ClearHistory()
	{
		history.Clear();
		SaveHistory();
	}

	// Matematiksel ifade çözümleyicisi.
	private class ExpressionParser
	{
		private readonly string input;
		private int position;

		public ExpressionParser(string input)
		{
			this.input = input.Replace(" ", "");
		}

		public double Parse()
		{
			double value = ParseExpression();

			if (position < input.Length)
				throw new FormatException();

			return value;
		}

		private bool Match(char c)
		{
			if (position < input.Length &&
				input[position] == c)
			{
				position++;
				return true;
			}

			return false;
		}

		// Toplama ve çıkarma.
		private double ParseExpression()
		{
			double value = ParseTerm();

			while (true)
			{
				if (Match('+'))
					value += ParseTerm();

				else if (Match('-'))
					value -= ParseTerm();

				else
					return value;
			}
		}

		// Çarpma ve bölme.
		private double ParseTerm()
		{
			double value = ParsePower();

			while (true)
			{
				if (Match('×'))
					value *= ParsePower();

				else if (Match('÷'))
				{
					double divisor = ParsePower();

					if (divisor == 0)
						throw new DivideByZeroException();

					value /= divisor;
				}
				else
					return value;
			}
		}

		// Üs alma.
		private double ParsePower()
		{
			double value = ParseUnary();

			if (Match('^'))
				value = Math.Pow(value, ParsePower());

			return value;
		}

		// Negatif ve pozitif sayılar.
		private double ParseUnary()
		{
			if (Match('+'))
				return ParseUnary();

			if (Match('-'))
				return -ParseUnary();

			return ParsePrimary();
		}

		// Sayı, parantez, sabit ve fonksiyonlar.
		private double ParsePrimary()
		{
			if (Match('('))
			{
				double value = ParseExpression();

				if (!Match(')'))
					throw new FormatException();

				return value;
			}

			// Pi sabiti.
			if (Match('π'))
				return Math.PI;

			// e sabiti.
			if (Match('e'))
				return Math.E;

			// Fonksiyonlar.
			if (position < input.Length &&
				char.IsLetter(input[position]))
			{
				string name = "";

				while (position < input.Length &&
					   char.IsLetter(input[position]))
				{
					name += input[position++];
				}

				if (!Match('('))
					throw new FormatException();

				double argument = ParseExpression();

				if (!Match(')'))
					throw new FormatException();

				double radians = argument * Math.PI / 180;

				return name switch
				{
					"sqrt" when argument >= 0 =>
						Math.Sqrt(argument),

					"cbrt" =>
						Math.Cbrt(argument),

					// Trigonometri.
					"sin" =>
						Math.Sin(radians),

					"cos" =>
						Math.Cos(radians),

					"tan" =>
						Math.Tan(radians),

					"cot" when Math.Sin(radians) != 0 =>
						1 / Math.Tan(radians),

					"sec" when Math.Cos(radians) != 0 =>
						1 / Math.Cos(radians),

					"csc" when Math.Sin(radians) != 0 =>
						1 / Math.Sin(radians),

					// Ters trigonometrik fonksiyonlar.
					"asin" when argument >= -1 && argument <= 1 =>
						Math.Asin(argument) * 180 / Math.PI,

					"acos" when argument >= -1 && argument <= 1 =>
						Math.Acos(argument) * 180 / Math.PI,

					"atan" =>
						Math.Atan(argument) * 180 / Math.PI,

					// Logaritma.
					"log" when argument > 0 =>
						Math.Log10(argument),

					"ln" when argument > 0 =>
						Math.Log(argument),

					// Diğer fonksiyonlar.
					"abs" =>
						Math.Abs(argument),

					"inv" when argument != 0 =>
						1 / argument,

					_ => throw new ArithmeticException()
				};
			}

			return ParseNumber();
		}

		// Sayı okuma.
		private double ParseNumber()
		{
			int start = position;

			while (position < input.Length &&
				   (char.IsDigit(input[position]) ||
					input[position] == ','))
			{
				position++;
			}

			if (start == position)
				throw new FormatException();

			string number = input[start..position].Replace(',', '.');

			return double.Parse(
				number,
				CultureInfo.InvariantCulture);
		}
	}
}
