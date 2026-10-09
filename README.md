# Hotel Search API

## Pregled

.NET Web API za pretraživanje hotela pomoću upita napisanih prirodnim jezikom. Projekt uključuje CRUD operacije i funkcionalnost pretraživanja na temelju korisničkog upita.

## Implementirane funkcionalnosti

* **CRUD operacije:** Osnovne funkcionalnosti za kreiranje, dohvat, ažuriranje i brisanje zapisa o hotelima.
* **Pretraživanje pomoću upita:** Endpoint koji prima tekstualni upit korisnika i obrađuje ga za potrebe pretraživanja hotela.
* **Obrada upita:** Uz pomoć AI-ja razvio sam i doradio regularne izraze (Regex) za prepoznavanje elemenata korisničkih upita.
* **Docker:** Pokušao sam konfigurirati Docker i povezati aplikaciju s njim, ali taj dio još nije dovršen.

## Daljnja poboljšanja

Da sam imao više vremena, napravio bih sljedeće:

* Popunio bazu podataka testnim podacima o hotelima.
* Testirao funkcionalnost pretraživanja s različitim upitima i rubnim slučajevima.
* Dovršio i provjerio Docker konfiguraciju.
* Napisao jedinične testove (unit testove) za glavnu poslovnu logiku i API endpointove.
* Dodatno poboljšao obradu pogrešaka i validaciju ulaznih podataka.

## Napomena

Tijekom razvoja koristio sam AI kao pomoć, prvenstveno pri izradi i doradi regularnih izraza za obradu korisničkih upita te pri pokušaju konfiguracije Dockera.
