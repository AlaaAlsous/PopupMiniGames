## 3. Objektorienterad Programmering (OOP)
```
public class Player
{
    public string Name { get; set; }
    public int CurrentScore { get; set; }
    public void PlayGame(MiniGame game)
    {
        game.Start(this);
    }
}

public abstract class MiniGame
{
    public string Name { get; set; }
    public abstract void Start(Player player);
}

public class MapGame : MiniGame
{
    public override void Start(Player player)
    {
        // logik för karta
    }
}

public class ButtonGame : MiniGame
{
    public override void Start(Player player)
    {
        // logik för knappspel
    }
}

public class MathGame : MiniGame
{
    public override void Start(Player player)
    {
        // logik för mattespel
    }
}

public class Settings
{
    public DifficultyLevel Difficulty { get; set; }
    public bool SoundEnabled { get; set; }
    public int MaxScore { get; set; }

    public Settings(DifficultyLevel difficulty, bool sound, int maxScore)
    {
        Difficulty = difficulty;
        SoundEnabled = sound;
        MaxScore = maxScore;
    }
}

public class UI
{
    public void DisplayMenu()
    {
        // visa meny
    }

    public void ShowScore(Player player)
    {
        // visa poäng
    }
}

public enum DifficultyLevel
{
    Easy,
    Medium,
    Hard,
    Debug // extra läge för testning
}

