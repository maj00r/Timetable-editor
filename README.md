# Timetable-editor
Timetable editor for ISDR simulators in versions before November 22, 2020 (.roz files)

W pewnych miejscach kodu można było zastosować lepsze i czytelniejsze rozwiązania...

Opis ogólny
Program ma za zadanie tworzyć i modyfikować dane rozkładu jazdy w formacie *.roz dedykowanym symulatorom prowadzenia ruchu kolejowego ISDR (isdr.pl) w wersjach sprzed 22 listopada 2020 r. Edytor  powstał  na  podstawie  danych  zawartych  w  tych programach, ich  dokumentacjach oraz własnych badań. 


Licencja Autor zezwala na użycie programu w celach niekomercyjnych.

Wygląd
W  centralnej  części  znajduje  się  tabela  z  pociągami,  nad  którymi  jest  wykonywana  praca. Zaznaczenie wiersza powoduje wyświetlenie się własności pociągu poniżej tabeli.

Zakładka Info umożliwia edycję metadanych rozkładu: posterunek, autor, opis, data początkowa symulacji. W Narzędziach dostępne są opcje usunięcia i aktualizacji zaznaczonego pociągu, dodania pociągu o własnościach  podanych  po  lewej  od  przycisku.
Posortuj  czasowo porządkuje  pociągi  według chronologii  przyjazdu  na  posterunek,  bądź  odjazdu  jeśli  pociąg  jest  uruchamiany.  Sortowanie wymagane jest przez symulator.

Własności
Pola dotyczące relacji, przewoźnika, składu posiadają podstawowe wartości do wybrania i zezwalają na umieszczenie własnego tekstu.

Posterunek  wyjazdu/  przyjazdu,  typ  postoju  zamówionego,  priorytet,  postoje,  sygnał wjazdowy można jedynie wybrać z rozwijanej listy.

Pola czasowe należy wypełnić sześcioma cyframi, bez dwukropków, bądź pozostawić puste, jeśli pociąg jest:

	uruchamiany (Odj. z post., Odj. s., Przyjazd);
	
	kończy jazdę (Odjazd, Przyj s.).
	
Szlak oznacza, dla wyprawiających, numery torów po jakich będą wyprawiane pociągi (przy szlaku  dwutorowym  pierwsza  liczba  powinna  oznaczać  tor  prawy np. 2,1 dla  jazd  z Wilamowic, 1,2 ze Ślemienia).

Postój zamówiony w minutach będzie wyświetlany w SWDR.

Kursuje – wartość wyświetlana w Wyciągu rozkładu jazdy – powinna być pusta jeśli pociąg kursuje codziennie.

Wypr. – dni wyprawiania w formie zakresulub wypisane po przecinku

Semafory wyj. –nazwy semaforów, przed którymi pociąg powinien oczekiwać na godzinę odjazdu

Sygnał wj. –sygnał na semaforze wjazdowym posterunku przyjmującego pociąg (ma wpływ na prędkość, do jakiej powinien zwolnić dojeżdżający pociąg)

Zmiana nr –przyjeżdżający pociąg zmieni swój numer na stacji, można to zautomatyzować zaznaczając auto

Przejście z –pociąg wyprawiany będzie oczekiwał na przybycie pociągu o podanym numerze, dodatkowo można wpisać czas oczekiwania w minutach

Prawdopodobieństwa:

	Kursowania –prawdopodobieństwo oznaczenie pola K w SWDR
	
	TWR –prawdopodobieństwooznaczenia, że pociąg ma władunku towary wysokiego ryzyka
	
	Przekr. Skrajni–prawdopodobieństwo oznaczenia,  że  pociąg ma  przekroczoną skrajnię ładunku
	
	Przyspieszenia –wyprawienia przed planowanym czasem w polu Odj. Zpost.
	
	Opóźnienia -wyprawienia poplanowanym czasiew polu Odj. Z post.
	
	Maksymalne możliwe wartości w minutach są do wpisania w grupie Zakres

Skład – zestawienie wpisów pojazdów oddzielone przecinkami Wpispojazdu składa się z 3 wartości oddzielonych przecinkami:

	nazwa,długość,napęd
	
	wyświetlana nazwa
	
	długość w osiach obliczeniowych, przelicznik na metry *5
	
	napęd – 1-nieelektryczny, 2-elektryczny, 0-wagon
	
	Dodatkowo można podać parametry pojazdu w sekcji nazwa-masę w tonach,moc w kW, prędkość maksymalną w km/h, ilość kabin
	
	Przykłady:
	
	45WE[p=2000][m=202],18,2
	
	SM42[1k][m=70][p=588],3,1,wagony[m=200][vm=80],12,0

Informacje o rozkładzie
	Należy wybrać, dla jakiej stacji jest stworzony rozkład, oraz podać datę początkową wskazującą na poniedziałek, bądź wpisać sam rok.
	
	Narzędzia
	
	Dodaj (Alt D) dodaje pociąg o własnościach podanych po lewej
	
	Usuń (Alt U) usuwa zaznaczony pociąg
	
	Aktualizuj (Alt A) aktualizuje własności podane po lewej zaznaczonemu pociągowi
	
	Posortuj czasowo (Alt S) sortuje pociąg chronologicznie według czasuprzyjazdu na stację, lub odjazdu jeśli jest uruchamiany.
	
	Można wyszukać pociąg po numerze oraz przeliczyć pola czasowe o zadany interwał w minutach do przodu i w tył (odpowiednie strzałka w górę / dół). Powielanie nie jest przygotowane.
