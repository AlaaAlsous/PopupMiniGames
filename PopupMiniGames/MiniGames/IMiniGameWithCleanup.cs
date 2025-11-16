using System;
using MiniGames.MiniGames;

namespace MiniGames.MiniGames
{
    
    public interface IMiniGameWithCleanup : IMiniGame
    {
        void Cleanup();
    }
}
/*   --- för att använda denna lägg till i spelen ---

public class (spelnamn) : IMiniGameWithCleanup

     --- sen copy pastea detta i till era spel ---

public void Cleanup()
{
    if (background != null)
    {
        parentContainer.Controls.Remove(background);  // (ta bort bakgrund) mot era egna variabler
        background.Dispose();
        background = null;
    }

    foreach (var c in cups) // (ta bort cups) mot era egna variabler
    {
        parentContainer.Controls.Remove(c);
        c.Dispose();
    }
    cups.Clear();

    if (instructionLabel != null)
    {
        parentContainer.Controls.Remove(instructionLabel); // (ta bort instruktioner) mot era egna variabler
        instructionLabel.Dispose();
        instructionLabel = null;
    }

    var resultBox = parentContainer.Controls["ResultBox"];
    if (resultBox != null)
    {
        parentContainer.Controls.Remove(resultBox);
        resultBox.Dispose();
    }
}
Obs! jag har inte något med timers eller andra asynkrona saker att göra,
så om ni har sånt i era spel så får ni själva se till att de också städas upp i Cleanup-metoden.
--------------------------------------------------------------------------------------------------------------------
*/