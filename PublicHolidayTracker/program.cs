using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace PublicHolidayTracker
{
    // İstenilen veri modeli
    public class Holiday
    {
        public string date { get; set; }
        public string localName { get; set; }
        public string name { get; set; }
        public string countryCode { get; set; }
        
        // 'fixed' C# dilinde rezerve edilmiş bir kelime olduğu için başına @ koyuyoruz
        // veya JsonPropertyName attribute'u kullanabiliriz.
        public bool @fixed { get; set; } 
        public bool global { get; set; }
    }

    class Program
    {
        // Bellekte tutulacak tüm tatil listesi
        private static List<Holiday> _allHolidays = new List<Holiday>();
        private static readonly HttpClient _client = new HttpClient();

        static async Task Main(string[] args)
        {
            Console.Title = "Türkiye Resmi Tatil Takipçisi";
            
            Console.WriteLine("Veriler sunucudan çekiliyor, lütfen bekleyiniz...");
            await InitializeDataAsync(); // Uygulama başlarken verileri çek ve hafızaya al
            Console.Clear();

            bool exit = false;
            while (!exit)
            {
                ShowMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ListByYear();
                        break;
                    case "2":
                        SearchByDate();
                        break;
                    case "3":
                        SearchByName();
                        break;
                    case "4":
                        ListAllHolidays();
                        break;
                    case "5":
                        exit = true;
                        Console.WriteLine("Çıkış yapılıyor. İyi günler!");
                        break;
                    default:
                        Console.WriteLine("Geçersiz seçim! Lütfen tekrar deneyin.");
                        break;
                }

                if (!exit)
                {
                    Console.WriteLine("\nMenüye dönmek için bir tuşa basın...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        // --- Veri Çekme İşlemi ---
        static async Task InitializeDataAsync()
        {
            int[] years = { 2023, 2024, 2025 };
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            foreach (var year in years)
            {
                try
                {
                    string url = $"https://date.nager.at/api/v3/PublicHolidays/{year}/TR";
                    string response = await _client.GetStringAsync(url);
                    
                    var holidays = JsonSerializer.Deserialize<List<Holiday>>(response, options);
                    if (holidays != null)
                    {
                        _allHolidays.AddRange(holidays);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hata: {year} yılı verisi çekilemedi. Detay: {ex.Message}");
                }
            }
            Console.WriteLine($"Toplam {_allHolidays.Count} adet tatil kaydı hafızaya alındı.");
        }

        // --- Menü Ekranı ---
        static void ShowMenu()
        {
            Console.WriteLine("===== PublicHolidayTracker =====");
            Console.WriteLine("1. Tatil listesini göster (yıl seçmeli)");
            Console.WriteLine("2. Tarihe göre tatil ara (gg-aa formatı)");
            Console.WriteLine("3. İsme göre tatil ara");
            Console.WriteLine("4. Tüm tatilleri 3 yıl boyunca göster (2023–2025)");
            Console.WriteLine("5. Çıkış");
            Console.Write("Seçiminiz: ");
        }

        // --- Fonksiyonlar ---

        // 1. Yıla Göre Listeleme
        static void ListByYear()
        {
            Console.Write("Listelemek istediğiniz yılı girin (2023, 2024, 2025): ");
            string inputYear = Console.ReadLine();

            var results = _allHolidays
                .Where(h => h.date.StartsWith(inputYear))
                .ToList();

            PrintResults(results);
        }

        // 2. Tarihe Göre Arama (gg-aa)
        static void SearchByDate()
        {
            Console.Write("Aramak istediğiniz tarih (gg-aa formatında, örn: 29-10): ");
            string inputDate = Console.ReadLine(); // Örn: 01-01

            // API tarihi yyyy-MM-dd formatında veriyor. Sondaki MM-dd kısmını kontrol edeceğiz.
            // Kullanıcı gg-aa (dd-MM) girdiği için ters çevirip eşleştirmeliyiz ya da DateTime parse etmeliyiz.
            
            // Basit string manipülasyonu ile kontrol:
            // Input: 23-04 -> Hedef format: -04-23 (çünkü yyyy-MM-dd)
            
            if (inputDate.Length == 5 && inputDate.Contains("-"))
            {
                string[] parts = inputDate.Split('-');
                string day = parts[0];
                string month = parts[1];
                string searchSuffix = $"-{month}-{day}"; // -MM-dd formatı

                var results = _allHolidays
                    .Where(h => h.date.EndsWith(searchSuffix))
                    .ToList();

                PrintResults(results);
            }
            else
            {
                Console.WriteLine("Hatalı format! Lütfen gg-aa şeklinde giriniz.");
            }
        }

        // 3. İsme Göre Arama
        static void SearchByName()
        {
            Console.Write("Tatil isminde aranacak kelime: ");
            string keyword = Console.ReadLine().ToLower();

            var results = _allHolidays
                .Where(h => h.localName.ToLower().Contains(keyword) || h.name.ToLower().Contains(keyword))
                .ToList();

            PrintResults(results);
        }

        // 4. Tüm Listeyi Göster
        static void ListAllHolidays()
        {
            PrintResults(_allHolidays);
        }

        // Yardımcı Metot: Sonuçları Ekrana Yazdırma
        static void PrintResults(List<Holiday> holidays)
        {
            Console.WriteLine("\n--- Sonuçlar ---");
            if (holidays.Count == 0)
            {
                Console.WriteLine("Kayıt bulunamadı.");
                return;
            }

            // Tablo başlığı
            Console.WriteLine($"{"Tarih",-12} | {"Yerel İsim",-35} | {"Global İsim",-30}");
            Console.WriteLine(new string('-', 85));

            foreach (var h in holidays)
            {
                Console.WriteLine($"{h.date,-12} | {h.localName,-35} | {h.name,-30}");
            }
        }
    }
}
