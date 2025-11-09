# Objektorienterad Design (OOD)

### Designbeslut:

* Player kan ha flera Score-objekt.
* MiniGame är en basklass för olika minispel (MapGame, ButtonGame, MathGame).
* Settings används av Program och MiniGame för att bestämma svårighetsgrad.
* UI ansvarar för att visa menyn, starta spelet och visa poäng.

### Relationer:

* Program äger UI och Settings.
* Player interagerar med MiniGame.
* MiniGame uppdaterar Score.
