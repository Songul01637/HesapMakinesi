// Hata ayıklama (Debug) kayıtlarını kullanabilmek için gerekli kütüphane.
using Microsoft.Extensions.Logging;

namespace HesapMakine
{
	// .NET MAUI uygulamasının başlangıç ayarlarını içeren sınıf.
	public static class MauiProgram
	{
		// Uygulamanın oluşturulmasını ve yapılandırılmasını sağlayan metot.
		public static MauiApp CreateMauiApp()
		{
			// MAUI uygulamasının yapılandırma nesnesini oluşturur.
			var builder = MauiApp.CreateBuilder();

			// Uygulamanın ana sınıfını App olarak belirler.
			builder
				.UseMauiApp<App>()

				// Uygulamada kullanılacak yazı tiplerini tanımlar.
				.ConfigureFonts(fonts =>
				{
					// Normal yazı tipi dosyasını OpenSansRegular adıyla kaydeder.
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");

					// Kalın yazı tipi dosyasını OpenSansSemibold adıyla kaydeder.
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				});

			// Kod yalnızca Debug modunda çalıştırıldığında hata ayıklama kayıtlarını etkinleştirir.
#if DEBUG
			builder.Logging.AddDebug();
#endif

			// Yapılandırılan MAUI uygulamasını oluşturur ve geri döndürür.
			return builder.Build();
		}
	}
}