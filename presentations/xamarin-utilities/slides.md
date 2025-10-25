---
theme: ktym4a
background: https://cover.sli.dev
title: Peryferia w Xamarinie
class: text-center
drawings:
  persist: false
transition: slide-left
mdc: true
---

# Peryferia w Xamarinie

---

# Kompas - teoria działania

- Kompas używa magnetometru do określania kierunku.
- Przydaje się w nawigacji i AR.
- Często łączony z akcelerometrem dla stabilności.

````md magic-move {class:'!children:text-xl'}
```csharp
using Xamarin.Essentials;

void StartCompass()
{
    try {
        Compass.ReadingChanged += Compass_ReadingChanged;
        Compass.Start(SensorSpeed.UI);
    } catch (FeatureNotSupportedException) {
        // brak magnetometru
    }
}

```
```csharp
void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
{
    var heading = e.Reading.HeadingMagneticNorth;
    // Aktualizuj UI na wątku głównym
    Device.BeginInvokeOnMainThread(() => 
        label.Text = $"Kąt: {Math.Round(heading)}°");
}
```
````

<!-- 
Kompas w kontekście Xamarin (i urządzeń mobilnych) opiera się na odczycie magnetometru dostarczanego przez sprzęt telefonu lub tabletu. W praktycznych aplikacjach używamy Xamarin.Essentials.Compass, który udostępnia gotowe API do subskrybowania odczytów i ustawiania częstotliwości. Surowe wartości mogą być zakłócane przez pola magnetyczne i metalowe obiekty w otoczeniu, dlatego warto filtrować dane, agregować pomiary i kalibrować urządzenie, prosząc użytkownika o ruch „ósemkę”. Kompas najlepiej działa razem z akcelerometrem i żyroskopem; fuzja sensorów (filtry komplementarne lub Kalman) poprawia stabilność i eliminuje dryf. Programista powinien dodatkowo sprawdzać uprawnienia i dostępność sensora, a także pamiętać o usuwaniu nasłuchów w metodach cyklu życia (np. OnSleep / OnResume). W aplikacjach Xamarin.Forms interfejs użytkownika często aktualizuje się poprzez Binding lub Device.BeginInvokeOnMainThread, by bezpiecznie modyfikować UI z wątków sensora. Dobrą praktyką jest oferowanie prostego trybu kalibracji oraz wskazówek dla użytkownika, jak trzymać urządzenie, by zminimalizować błędy.
-->

---

# Kompas - przykład (filtracja i kalibracja)

- Używamy średniej kroczącej do wygładzenia.
- Dodajemy próg zmiany by zredukować jitter.
- Informujemy użytkownika o kalibracji.


````md magic-move {class:'!children:text-xl'}
```csharp
using System.Collections.Generic;
using Xamarin.Essentials;

Queue<double> window = new Queue<double>();
int windowSize = 5;
```
```csharp
void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
{
    var heading = e.Reading.HeadingMagneticNorth;
    window.Enqueue(heading);
    if (window.Count > windowSize) window.Dequeue();

    double avg = 0;
    foreach (var v in window) avg += v;
    avg /= window.Count;
```
```csharp
    // Próg zmiany
    if (Math.Abs(avg - lastHeading) > 2) {
        lastHeading = avg;
        Device.BeginInvokeOnMainThread(() => 
            label.Text = $"Kąt: {Math.Round(avg)}°");
    }
}
```
````

<!-- 
W praktyce kompas wymaga wygładzania — wartości mogą skakać przy małych ruchach. Implementacja średniej kroczącej (rolling average) lub filtru wykładniczego pozwala uzyskać płynniejsze odczyty. Dodatkowo warto wyznaczyć minimalny próg zmiany kąta (np. 2–5 stopni), poniżej którego nie aktualizujemy UI, co zmniejsza migotanie elementów interfejsu. Jeśli aplikacja wymaga wysokiej dokładności, zaproponuj procedurę kalibracji: instrukcja „porusz urządzeniem w kształcie ósemki” i monitoring stabilności pomiarów. W przypadku nagłych odchyłów (np. duże zaburzenia magnetyczne) warto wyświetlić komunikat i tymczasowo zawiesić korzystanie z kompasu lub skorzystać z fuzji sensorów (akcelerometr + magnetometr) dla poprawy azymutu. W Xamarin można implementować te mechanizmy w kodzie obsługi Compass.ReadingChanged — zbierać kilka pomiarów, obliczać średnią i porównywać do poprzedniego wyjścia. Pamiętaj również o oszczędzaniu energii: nie wyłączaj kompasu dłużej niż trzeba oraz zatrzymuj go przy zamknięciu widoku.
-->

---

# Kontakty - teoria

- Kontakty są chronione — wymagane pozwolenia.
- Android: ContentResolver; iOS: Contacts framework.
- Xamarin pozwala na natywne wywołania.

````md magic-move {class:'!children:text-lg'}
```csharp
using Android.Content;
using Android.Database;
using Android.Provider;
```
```csharp
void ReadContacts()
{
    var uri = ContactsContract.CommonDataKinds.Phone.ContentUri;
    string[] projection = { 
        ContactsContract.Contacts.InterfaceConsts.DisplayName, 
        ContactsContract.CommonDataKinds.Phone.Number 
    };
    ICursor cursor = 
        ContentResolver.Query(uri, projection, null, null, null);
```
```csharp
    if (cursor != null) {
        while (cursor.MoveToNext()) {
            var name = cursor.GetString(cursor.GetColumnIndex(projection[0]));
            var number = cursor.GetString(cursor.GetColumnIndex(projection[1]));
            Android.Util.Log.Debug("Kontakt", $"{name}: {number}");
        }
        cursor.Close();
    }
}
```
````

<!-- 
Dostęp do książki kontaktów w aplikacjach mobilnych jest operacją prywatną i wymaga odpowiednich uprawnień przyznawanych przez użytkownika. W Xamarin można korzystać z natywnych API platform: na Androidzie przez ContentResolver i ContactsContract, na iOS przez Contacts framework albo użyć bibliotek trzecich/abstrakcji. Ważne jest sprawdzenie statusu uprawnień (Permissions in Xamarin.Essentials lub bezpośrednio przez Android/iOS) przed próbą odczytu. Wskazane jest pobieranie tylko niezbędnych pól (np. nazwa, numer), a także obsługa przypadków, gdy użytkownik odmówi dostępu. Dobre praktyki obejmują jasne komunikaty o tym, dlaczego aplikacja potrzebuje dostępu oraz zapewnienie opcji manualnego dodawania kontaktów, jeśli dostęp jest zablokowany. Ponadto należy pamiętać o ograniczeniach polityk sklepów (Google Play, App Store) dotyczących przetwarzania danych osobowych i przechowywania ich poza urządzeniem.
-->

---

# Kontakty - przykład (wybór kontaktu – cross-platform)

- Używamy pluginów lub natywnych intencji.  
- Weryfikujemy uprawnienia.  
- Udostępniamy tylko potrzebne pola.  

````md magic-move {class:'!children:text-lg'}
```csharp
public void PickContact()
{
    var intent = new Intent(
        Intent.ActionPick,
        ContactsContract.CommonDataKinds
            .Phone.ContentUri);
    StartActivityForResult(intent, PICK_CONTACT_REQUEST);
}
```
```csharp
protected override void OnActivityResult(
    int requestCode, Result resultCode, Intent data)
{
    base.OnActivityResult(requestCode, resultCode, data);
    if (requestCode == PICK_CONTACT_REQUEST 
        && resultCode == Result.Ok)
    {
```
```csharp
        var uri = data.Data;
        var cursor = ContentResolver.Query(uri, null, null, null, null);
```
```csharp
        if (cursor.MoveToFirst()) {
            var number = cursor.GetString(
                cursor.GetColumnIndex(
                    ContactsContract.CommonDataKinds
                        .Phone.Number));
        } 
        cursor.Close();
    }
}
```
````

<!-- 
W aplikacjach Xamarin.Forms korzystamy z gotowych bibliotek lub prostych wrapperów natywnych intencji do wyboru kontaktu.  
Na Androidzie uruchamiamy `Intent.ACTION_PICK` z `ContactsContract`, a na iOS używamy `CNContactPickerViewController`.  
Dzięki rozwiązaniom hybrydowym (np. community toolkit) możemy wywoływać jednolite API z poziomu projektu wspólnego.  
Kluczowe jest sprawdzenie i żądanie uprawnień przed otwarciem selektora, przetworzenie rezultatu zgodnie z platformą oraz przekazanie tylko potrzebnych pól (np. numer telefonu lub e-mail).  
Obsługujemy też sytuację, gdy użytkownik anuluje wybór.  
-->

---

# Wykryj wstrząs - teoria

- Wykorzystujemy akcelerometr do detekcji nagłego przyspieszenia.
- Porównujemy siłę wektorową z progiem.
- Dodajemy histerezę czasową, aby uniknąć false-positive.

````md magic-move {class:'!children:text-xl'}
```csharp
using Xamarin.Essentials;
int shakeCount = 0;
DateTime lastShake = DateTime.MinValue;

void StartAccelerometer()
{
    try {
        Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
        Accelerometer.Start(SensorSpeed.UI);
    } catch (FeatureNotSupportedException) {}
}
```
```csharp
void Accelerometer_ReadingChanged(
    object sender, AccelerometerChangedEventArgs e)
{
    var v = e.Reading;
    double force = Math.Sqrt(v.Acceleration.X*v.Acceleration.X +
                             v.Acceleration.Y*v.Acceleration.Y +
                             v.Acceleration.Z*v.Acceleration.Z);
```
```csharp
    if (force > 2.5) { // eksperymentalny próg
        if ((DateTime.Now - lastShake).TotalMilliseconds > 500) {
            lastShake = DateTime.Now;
            Device.BeginInvokeOnMainThread(() => 
                DisplayAlert("Wstrząs", "Wykryto wstrząs", "OK"));
        }
    }
}
```
````

<!-- 
Detekcja wstrząsu (shake) polega na monitorowaniu akcelerometru i wykrywaniu gwałtownej zmiany przyspieszenia. W Xamarin.Essentials dostępny jest Accelerometer, który zwraca wektor (X,Y,Z). Obliczamy normę wektora i porównujemy z ustalonym progiem – jeśli przekroczony, traktujemy to jako wstrząs. Trzeba jednak odfiltrować wpływ grawitacji i krótkotrwałe skoki przez zastosowanie dodatkowych reguł: minimalny odstęp czasowy między zarejestrowanymi wstrząsami, liczba próbek przekraczających próg w krótkim oknie czasowym, czy adaptacyjny próg w zależności od ruchu użytkownika. Dodatkowo należy zapewnić mechanizm włączania/wyłączania nasłuchu i dbać o zużycie baterii, ustawiając odpowiednią prędkość sensora (SensorSpeed.UI vs. Fast) oraz zatrzymując nasłuch gdy nie jest potrzebny.
-->

---

# Wykryj wstrząs - przykład (zaawansowany detektor)

- Używamy okna czasowego i liczników.
- Stosujemy filtr dolnoprzepustowy.
- Stosujemy debounce, aby uniknąć powtórzeń.

````md magic-move {class:'!children:text-xl'}
```csharp
using System.Collections.Generic;
using Xamarin.Essentials;

Queue<double> samples = new Queue<double>();
int sampleWindow = 10;
int thresholdCount = 3;
```
```csharp
void Accelerometer_ReadingChanged(
    object sender, AccelerometerChangedEventArgs e)
{
    var v = e.Reading;
    double force = Math.Sqrt(v.Acceleration.X*v.Acceleration.X +
                             v.Acceleration.Y*v.Acceleration.Y +
                             v.Acceleration.Z*v.Acceleration.Z);

    samples.Enqueue(force);
    if (samples.Count > sampleWindow) samples.Dequeue();
```
```csharp
    int over = 0;
    foreach (var s in samples) if (s > 2.2) over++;

    if (over >= thresholdCount && 
        (DateTime.Now - lastShake).TotalMilliseconds > 800) {
        lastShake = DateTime.Now;
        Device.BeginInvokeOnMainThread(() => 
            DisplayAlert(
                "Wstrząs", "Wykryto wstrząs (zaawansowany)", "OK"));
    }
}
```
````

<!-- 
Aby poprawić niezawodność detekcji wstrząsu, stosuje się techniki takie jak analiza szeregu próbek w krótkim oknie czasowym (np. 300–500 ms) i liczenie, ile próbek przekroczyło próg. Jeśli liczba przekroczeń przekroczy ustaloną wartość, uznajemy to za wstrząs. Przydatny jest też prosty filtr dolnoprzepustowy do eliminacji wysokoczęstotliwościowego szumu i filtru wykładniczego do wygładzania wartości. Debounce (blokada czasowa) zapobiega powtórnym detekcjom tuż po pierwszym wykryciu. W implementacji Xamarin warto też rozważyć różne tryby SensorSpeed (UI, Game, Fastest) – im szybszy, tym większe zużycie baterii, ale lepsza reakcja. Testy w realnych warunkach są konieczne, bo wartości progowe zależą od urządzenia i sposobu trzymania go przez użytkownika.
-->

---

# Informacje o wyświetlaniu urządzenia - teoria

- Screen size, DPI, orientacja i refresh rate.
- Kluczowe dla responsywnego UI.
- Xamarin.Essentials.DeviceDisplay daje dostęp.

```csharp {monaco}
// Odczyt informacji o wyświetlaczu
using Xamarin.Essentials;

var info = DeviceDisplay.MainDisplayInfo;
var szerokoscPx = info.Width;
var wysokoscPx = info.Height;
var dpi = info.Density;
var orientacja = info.Orientation;

System.Diagnostics.Debug.WriteLine($"Rozdzielczość: {szerokoscPx}x{wysokoscPx}, DPI: {dpi}, Orientacja: {orientacja}");
```

<!-- 
Informacje o wyświetlaczu pozwalają dostosować UI do ekranu urządzenia: rozdzielczość w pikselach, gęstość pikseli (DPI), orientacja ekranu i częstotliwość odświeżania. W Xamarin.Essentials dostępna jest klasa DeviceDisplay, która udostępnia DisplayInfo z polami Width, Height, Density i Orientation. Programista może użyć tych danych do wyboru zasobów graficznych (skalowanie) i reagowania na zmianę orientacji — nasłuchiwanie zdarzenia MainDisplayInfoChanged. W aplikacjach Xamarin.Forms warto użyć jednostek skalowanych (device independent) oraz układów responsywnych (Grid, FlexLayout), a przy renderowaniu grafik korzystać z wektorów lub kilku wersji bitmap o różnych gęstościach. Testy na realnych urządzeniach o różnych DPI są zalecane.
-->

---

# Informacje o wyświetlaniu urządzenia - przykład (nasłuchiwanie zmian)

- Nasłuchujemy MainDisplayInfoChanged.
- Aktualizujemy layout na wątku głównym.
- Reagujemy na zmianę orientacji.

```csharp {monaco}
// Nasłuchiwanie zmian ekranu
DeviceDisplay.MainDisplayInfoChanged += (s, e) =>
{
    var info = e.DisplayInfo;
    Device.BeginInvokeOnMainThread(() =>
    {
        // dopasuj layout, fonty, rozmiary graficzne
        System.Diagnostics.Debug.WriteLine($"Nowa orientacja: {info.Orientation}, DPI: {info.Density}");
    });
};
```

<!-- 
Zmiany parametrów ekranu (np. rotacja, zmiana DPI, tryb multi-window) mogą się zdarzyć w czasie działania aplikacji. Xamarin.Essentials udostępnia zdarzenie DeviceDisplay.MainDisplayInfoChanged, które pozwala reagować natychmiast po wystąpieniu zmian. W handlerze należy pobrać nowe DisplayInfo i zaktualizować UI bezpiecznie na wątku głównym. Dobrą praktyką jest minimalna ingerencja w obliczenia i delegowanie cięższych operacji poza główny wątek. Dodatkowo, warto uwzględnić skalowanie zasobów i testy layoutów przy orientacji pionowej i poziomej, a także w trybach podzielonego ekranu (tablet).
-->

---

# Informacje o urządzeniu - teoria

- Model, producent, system, wersja.
- Xamarin.Essentials.DeviceInfo udostępnia szczegóły.
- Używane do diagnostyki i telemetrii.

```csharp {monaco}
// Odczyt informacji o urządzeniu
using Xamarin.Essentials;

var model = DeviceInfo.Model;
var producent = DeviceInfo.Manufacturer;
var system = DeviceInfo.Platform;
var wersja = DeviceInfo.VersionString;

System.Diagnostics.Debug.WriteLine($"{producent} {model}, {system} {wersja}");
```

<!-- 
Informacje o urządzeniu (device info) pomagają w debugowaniu i raportowaniu błędów oraz doborze zachowań aplikacji wobec specyficznych modeli. Xamarin.Essentials.DeviceInfo dostarcza pola takie jak Model, Manufacturer, VersionString, Platform i Idiom. Należy jednak pamiętać o prywatności: nie przesyłaj identyfikatorów urządzenia bez zgody użytkownika. Informacje te są przydatne w logach, raportach crashów czy w testach warunkowanych specyficznymi modelami. Warto zbierać je anonimowo i ograniczać przechowywanie do niezbędnego okresu.
-->

---

# Informacje o urządzeniu - przykład (dodatkowe właściwości)

- DeviceInfo.Idiom i DeviceInfo.DeviceType.
- Używamy ich do dostosowania UI.
- Unikamy wycieków prywatnych danych.

```csharp {monaco}
// Przykład użycia Idiom i DeviceType
if (DeviceInfo.Idiom == DeviceIdiom.Tablet) {
    // większe marginesy, dwukolumnowy layout
}
if (DeviceInfo.DeviceType == DeviceType.Virtual) {
    // ograniczone testy specyficznych sensorów
}
```

<!-- 
DeviceInfo ma dodatkowe właściwości takie jak Idiom (Phone, Tablet, Desktop) i DeviceType (Physical, Virtual). Te wartości pozwalają decydować o układzie UI, funkcjach dostępnych tylko na tabletach czy o pominięciu niektórych integracji na emulatorach. W aplikacjach produkcyjnych warto używać tych danych do optymalizacji doświadczenia użytkownika, np. zmiany domyślnej skali elementów, czy ukrywania funkcji wymagających specyficznego sprzętu. Należy pamiętać, by nie używać danych identyfikujących urządzenie do śledzenia bez zgody użytkownika oraz by respektować polityki sklepów z aplikacjami. Testy na emulatorach nie zastąpią zawsze testów na realnych urządzeniach.
-->

---

# Poczta e-mail - teoria

- Xamarin.Essentials.Email umożliwia kompozycję e-mail.
- Otwarcie klienta, bez przechowywania haseł.
- Bezpieczeństwo: nie wysyłaj haseł z aplikacji.

```csharp {monaco}
// Kompozycja e-mail - Xamarin.Essentials
using Xamarin.Essentials;

async Task SendEmail()
{
    var message = new EmailMessage
    {
        Subject = "Pomoc techniczna",
        Body = "Opis problemu...",
        To = new List<string> { "support@firma.pl" }
    };
    await Email.ComposeAsync(message);
}
```

<!-- 
Wysyłanie e-maili z aplikacji mobilnej najlepiej realizować poprzez otwarcie domyślnego klienta poczty, pozostawiając uwierzytelnienie po stronie klienta. Xamarin.Essentials.Email.ComposeAsync pozwala wypełnić odbiorcę, temat i treść, a następnie otworzyć aplikację pocztową użytkownika. To bezpieczne podejście — nie musimy obsługiwać haseł ani konfiguracji SMTP. Jeśli aplikacja ma wysyłać e-maile automatycznie z własnego serwera, konieczne jest bezpieczne przechowywanie poświadczeń po stronie serwera, a nie w aplikacji. Zawsze proś użytkownika o zgodę, jeśli wysyłasz dane diagnostyczne i udostępnij możliwość ich wglądu przed wysłaniem.
-->

---

# Poczta e-mail - przykład (sprawdzanie czy e-mail dostępny)

- Sprawdzamy Email.IsComposeSupported.
- Zapewniamy fallback.
- Obsługujemy wyjątki.

```csharp
// Sprawdzenie wsparcia i fallback
if (Email.IsComposeSupported)
{
    await Email.ComposeAsync("Temat", "Treść", new[] { "support@firma.pl" });
}
else
{
    await DisplayAlert("Brak klienta e-mail", "Nie znaleziono klienta poczty na urządzeniu.", "OK");
    // alternatywa: otwórz stronę wsparcia
}
```

<!-- 
Przed wywołaniem ComposeAsync dobrze jest sprawdzić Email.IsComposeSupported, bo niektóre urządzenia (np. bez skonfigurowanego klienta pocztowego) mogą nie obsługiwać tej funkcji. W takim przypadku można zaproponować skopiowanie treści do schowka, otwarcie strony kontaktowej w przeglądarce lub wysłanie przez własny serwer (jeśli użytkownik się zgodzi). Obsługa wyjątków i komunikatów dla użytkownika poprawia UX. Zadbaj także o lokalizację komunikatów oraz informację, jakie dane zostaną dołączone (np. logi aplikacji), aby użytkownik wiedział, co wysyła.
-->

---

# Selektor plików - teoria

- Używamy FilePicker z Xamarin.Essentials.
- Działa cross-platform.
- Użytkownik wybiera plik; aplikacja dostaje URI.

```csharp
// FilePicker - Xamarin.Essentials (wybór pliku)
using Xamarin.Essentials;

async Task PickFile()
{
    var result = await FilePicker.PickAsync(new PickOptions
    {
        PickerTitle = "Wybierz plik",
        FileTypes = FilePickerFileType.All
    });

    if (result != null)
    {
        using var stream = await result.OpenReadAsync();
        // przetwarzaj strumień
    }
}
```

<!-- 
W Xamarin aplikacje mogą korzystać z Xamarin.Essentials.FilePicker do wyboru plików przez użytkownika. To bezpieczny sposób, ponieważ aplikacja otrzymuje dostęp tylko do wybranych plików, a system zarządza uprawnieniami. FilePicker pozwala filtrować typy MIME, umożliwia wielokrotny wybór i zwraca obiekty FileResult zawierające nazwę, typ i strumień do odczytu. Po zakończeniu pracy ze strumieniem należy go zamknąć. Na Androidzie i iOS działa to przez natywne UI selektora plików, co zwiększa spójność UX. Pamiętaj o obsłudze przypadków anulowania przez użytkownika.
-->

---

# Selektor plików - przykład (wiele plików i filtry)

- Ustawiamy FilePickerFileType.
- Obsługujemy wybór wielu plików.
- Czyścimy zasoby po użyciu.

```csharp
// Wybór wielu plików
var results = await FilePicker.PickMultipleAsync(new PickOptions { FileTypes = FilePickerFileType.Images });
if (results != null)
{
    foreach (var file in results)
    {
        using var stream = await file.OpenReadAsync();
        // przetwórz obraz
    }
}
```

<!-- 
FilePicker obsługuje także wybór wielu plików (PickMultipleAsync) oraz definiowanie własnych filtrów typów plików, co ułatwia użytkownikowi selekcję właściwych dokumentów. Po wybraniu plików aplikacja powinna sprawdzić rozmiar i typ, by uniknąć przeciążenia pamięci. Zwrócone FileResulty zawierają właściwość FullPath w niektórych platformach oraz metodę OpenReadAsync do bezpiecznego pobrania strumienia. Pamiętaj, by używać konstrukcji using lub ręcznie zamykać strumienie. W przypadku dużych plików rozważ kopiowanie na dysk aplikacji (FileSystem.AppDataDirectory) i pracę na lokalnej kopii.
-->

---

# Pomocnicy systemu plików - teoria

- FileSystem i File w .NET/Xamarin.
- AppDataDirectory dla plików prywatnych.
- Używamy strumieni i async I/O.

```csharp
// Zapis pliku w katalogu aplikacji
using System.IO;
using Xamarin.Essentials;

var path = Path.Combine(FileSystem.AppDataDirectory, "dane.txt");
await File.WriteAllTextAsync(path, "Przykładowa zawartość");
```

<!-- 
Zarządzanie plikami w aplikacji Xamarin wykorzystuje mechanizmy .NET: System.IO.File, Directory oraz Xamarin.Essentials.FileSystem dla lokalizacji specyficznych dla aplikacji (AppDataDirectory, CacheDirectory). Dane prywatne aplikacji powinny być zapisywane w katalogu prywatnym, aby inne aplikacje nie miały do nich dostępu. Dla dużych operacji plikowych korzystaj z asynchronicznego I/O (Stream, async/await) by nie blokować wątku UI. W przypadku zapisu danych w pamięci zewnętrznej (Android) pamiętaj o uprawnieniach i zasadach Scoped Storage. Dobrą praktyką jest implementacja helperów do zapisu/odczytu, obsługa wyjątków I/O i testy resetu/odtworzenia danych.
-->

---

# Pomocnicy systemu plików - przykład (odczyt i kopiowanie)

- Używamy async I/O.
- Kopiujemy duże pliki strumieniowo.
- Obsługujemy brak miejsca i wyjątki.

```csharp
// Odczyt i kopiowanie strumieniowe
using System.IO;
using Xamarin.Essentials;

async Task CopyFileAsync(string sourcePath, string destFileName)
{
    var destPath = Path.Combine(FileSystem.AppDataDirectory, destFileName);
    using var source = File.OpenRead(sourcePath);
    using var dest = File.Create(destPath);
    await source.CopyToAsync(dest);
}
```

<!-- 
Operacje na plikach powinny być odporne na błędy — sprawdzaj dostępność miejsca, uprawnienia i stany plików. Przy kopiowaniu dużych plików używaj strumieni (Stream.CopyToAsync) zamiast wczytywania całego pliku do pamięci. W przypadku pracy z plikami zewnętrznymi na Androidzie należy respektować Scoped Storage i prosić o odpowiednie uprawnienia lub użyć SAF (Storage Access Framework). Dodatkowo implementuj mechanizmy rollback przy przerwaniach operacji zapisu (dzięki tymczasowym plikom) i informuj użytkownika o postępie przy długotrwałych operacjach. Testy na urządzeniach z niską pamięcią i słabszym CPU są bardzo ważne.
-->
