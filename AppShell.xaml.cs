// Uygulamanın HesapMakine isim alanında yer aldığını belirtir.
namespace HesapMakine;

// AppShell sınıfı, Shell sınıfından miras alır.
// Uygulamanın sayfa gezinme (navigasyon) yapısını yönetir.
public partial class AppShell : Shell
{
	// AppShell sınıfının yapıcı metodudur.
	// AppShell nesnesi oluşturulduğunda otomatik olarak çalışır.
	public AppShell()
	{
		// XAML dosyasında tanımlanan arayüz bileşenlerini yükler.
		InitializeComponent();
	}
}