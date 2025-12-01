# PublicHolidayTracker 🇹🇷

Bu proje, Türkiye'deki resmi tatil günlerini **Nager.Date API** üzerinden çekerek kullanıcıya sunan, filtreleme ve arama işlemleri yapabilen bir **C# Konsol Uygulamasıdır**.

## 🚀 Özellikler

Uygulama, 2023, 2024 ve 2025 yıllarına ait resmi tatil verilerini işler ve aşağıdaki işlevleri sunar:

* **Veri Çekme:** API üzerinden JSON formatındaki verileri asenkron olarak (`async/await`) çeker.
* **JSON İşleme:** Gelen verileri C# nesnelerine (`Class`) dönüştürür (Deserialization).
* **Yıl Bazlı Listeleme:** Kullanıcının seçtiği yıla göre tatilleri listeler.
* **Tarih Arama:** `gg-aa` (Gün-Ay) formatında girilen tarihe göre tatil sorgular (Örn: 29-10).
* **İsimle Arama:** Tatil ismine göre (Örn: "Ramazan") arama yapar.
* **Geniş Liste:** Hafızaya alınan 3 yıllık tüm veri setini tablo formatında gösterir.

## 🛠️ Kullanılan Teknolojiler

* **Dil:** C#
* **Platform:** .NET Core / .NET 6+
* **Kütüphaneler:**
    * `System.Net.Http`: API istekleri için.
    * `System.Text.Json`: JSON verisini işlemek için.
    * `System.Linq`: Veri filtreleme ve sorgulama işlemleri için.

## ⚙️ Kurulum ve Çalıştırma

Projeyi bilgisayarınızda çalıştırmak için aşağıdaki adımları izleyebilirsiniz:

1.  **Projeyi Klonlayın:**
    ```bash
    git clone [https://github.com/EmrGencer/PublicHolidayTracker.git](https://github.com/EmrGencer/PublicHolidayTracker.git)
    ```

2.  **Proje Klasörüne Girin:**
    İç içe klasör yapısı nedeniyle proje dizinine gitmek için şu komutları sırasıyla uygulayın:
    ```bash
    cd PublicHolidayTracker
    cd PublicHolidayTracker
    ```

3.  **Projeyi Çalıştırın:**
    ```bash
    dotnet run
    ```

## 📸 Ekran Görüntüsü (Örnek Çıktı)

Uygulama çalıştığında aşağıdaki gibi bir menü sunar:

```text
===== PublicHolidayTracker =====
1. Tatil listesini göster (yıl seçmeli)
2. Tarihe göre tatil ara (gg-aa formatı)
3. İsme göre tatil ara
4. Tüm tatilleri 3 yıl boyunca göster (2023–2025)
5. Çıkış
Seçiminiz:
