[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-22041afd0340ce965d47ae6ef1cefeee28c7c493a6346c4f15d667ab976d596c.svg)](https://classroom.github.com/a/TyBvFPPq)
# .net25-oop-group
# Grupparbete för första kursen
# Beskrivning
PopupMiniGames är en Windows Forms-baserad applikation som samlar flera minispel i ett gemensamt gränssnitt. Programmet är uppbyggt kring en huvudmeny som laddar grafiska resurser från projektets UI-mapp och visar dem med pixelinspirerad design. Menyn fungerar som navet för applikationen och ger användaren möjlighet att starta spel, ändra svårighetsnivå och avsluta programmet.

Hela systemet är byggt modulärt, vilket innebär att varje minispel är en separat klass som implementerar ett gemensamt interface (IMiniGame). Tack vare detta kan applikationen automatiskt hitta och ladda alla minispel som finns i projektet utan att du behöver skriva extra kod varje gång ett nytt spel läggs till.

Programmet använder en central datastruktur (GameData) för att lagra spelrelaterad information såsom poäng, antal misstag och inställd svårighetsgrad. Dessa data följer med varje minispel och uppdateras automatiskt beroende på spelarens prestation.

När ett minispel körs visas det i ett eget fönster och kör sin egen logik – till exempel tidsbegränsade frågor, hinder som ska undvikas eller slumpmässiga utmaningar. När spelet avslutas (antingen genom vinst eller förlust) skickas resultatet tillbaka till huvudprogrammet via en GameEnded-händelse. På detta sätt hålls huvudprogrammet uppdaterat utan hård koppling mellan komponenterna.

Programmet har också stöd för resursstädning genom interfacet IMiniGameWithCleanup. Det gör att minispel som använder timers, event-händelser eller dynamiskt skapade resurser kan avslutas och rensas korrekt, vilket förhindrar frysningar och minnesläckor.

Applikationen är utvecklad för att köras i Visual Studio och använder standardfunktioner i .NET Windows Forms, kombinerat med egen UI-hantering genom klasser som UIManager, Menu och MenuButton. Detta ger en flexibel struktur där både layout och logik är tydligt uppdelade.


Hur man lägger till ett nytt minispel
Skapa en ny klass i MiniGames-namnrymden.
Implementera IMiniGame (och IMiniGameWithCleanup när resurser måste frigöras explicit).

Viktiga krav för ett minispel
Public konstruktor som tar GameData som parameter, t.ex.: public MyGame(GameData data)
Implementera void StartGame(Difficulty difficulty) för att starta spelet.
Raise eventet GameEnded när spelet är klart med relevant GameResult.
Om spelet skapar timers, bilder eller andra resurser, implementera Cleanup() i IMiniGameWithCleanup och ta bort events/timers/bilder där.