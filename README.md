# 🎧 Amoboard

Soundboard zrobiony w **Unity**

## 🔥 Co to jest?

**amoboard** to apka typu **soundboard** – klikasz przyciski, leci dźwięk, zero filozofii.  
Można wrzucać **MP3** oraz pliki graficzne (**PNG, JPG, JPEG, BMP** itd.).

## 🛠️ Jak działa od strony użytkownika?

- Użytkownik instaluje aplikację (`aha.apk`) na telefonie.
- Uruchamia aplikację i daje wszystkie potrzebne zgody.
- Aplikacja tworzy folder, do którego należy dodać pliki **MP3** oraz **grafiki** o tym samym tytule (np. `smiech.mp3` + `smiech.png`).
- Ścieżka do folderu na telefonie:

```
Ten komputer\\POCO M6 Pro\\Wewnętrzna pamięć współdzielona\\Android\\data\\com.DefaultCompany.com.unity.template.mobile2D\\files\\SoundBoard
```

## MENU
![menu](screenshots/menu.jpg)

- W lewym górnym rogu jest przycisk **"earrape"**, który podnosi głośność aktualnie odtwarzanych dźwięków.
- W prawym górnym rogu są ustawienia do zmiany dźwięków.
- Poniżej w siatce znajdują się wszystkie dodane dźwięki, które aktywujemy wciśnięciem przycisku. Zjeżdżając w dół, widać więcej dźwięków.

## USTAWIENIA
![ustawienia](screenshots/ustawienia.jpg)

- Po wciśnięciu na nazwę dźwięku możemy ją zmieniać.
- Żółtym suwakiem można regulować głośność dźwięku.
- Strzałkami możemy przenosić dźwięki w menu w górę lub w dół.
- Po wciśnięciu kosza usuwa dźwięk z aplikacji oraz z folderu.

## 🛠️ Działanie od zaplecza

- Po uruchomieniu aplikacja tworzy folder `SoundBoard` w pamięci telefonu, jeśli jeszcze go nie ma.
- Aplikacja skanuje folder pod kątem plików **MP3** i dopasowanych grafik (**PNG, JPG, JPEG, BMP** itd.).
- Każdy plik MP3 jest automatycznie przypisywany do przycisku w menu.
- Nazwy plików są walidowane i w razie potrzeby automatycznie zmieniane na format:
  ``` XXX~NazwaPliku~Głośność.mp3 ```

  gdzie `XXX` to numer porządkowy, `NazwaPliku` to wyświetlana nazwa, a `Głośność` to procent głośności.
- Dla każdego pliku MP3, jeśli istnieje odpowiadająca grafika o tej samej nazwie, jest ona ładowana jako ikona przycisku.
- Aplikacja dynamicznie generuje przyciski w menu na podstawie plików w folderze i odświeża UI po każdej zmianie.
![folder](screenshots/folder.jpg)

## ‼️ Wady

- Aplikacja została stworzona w środowisku do tego nieprzystosowanym (**Unity**) i mimo prostoty posiada swoje mankamenty.
- Czasem pierwsze odtworzenie dźwięku po otworzeniu aplikacji nie działa.
- Dźwięki nie działają w tle – aplikacja musi być na pierwszym planie.
- Na niektórych urządzeniach nie można jej zainstalować.
- Może przycinać / ścinać.
- Możliwe jest wyjście poza grid w menu oraz ustawieniach – wtedy trzeba zrestartować aplikację.

> Wszystkie te wady wynikają z lenistwa oraz faktu, że projekt ten nie był poważnym przedsięwzięciem, a próbą stworzenia prostej aplikacji w silniku do gier.  
> Zostały użyte bardzo proste mechanizmy, aby aplikacja spełniała swoją rolę i działała.  
> Był to również mój pierwszy projekt na telefon oraz pierwszy skończony projekt w Unity.
