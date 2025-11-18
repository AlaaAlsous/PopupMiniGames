using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MiniGames.MiniGames
{
    public class CatchTapGame : IMiniGameWithCleanup
    {
        public event EventHandler<GameResult>? GameEnded;
        private List<System.Windows.Forms.Timer> activeTimers = new List<System.Windows.Forms.Timer>();

        private Control parentContainer = null!;
        private List<PictureBox> objects = new List<PictureBox>();
        private Random rand = new Random();
        private Difficulty difficulty;

        private int score = 0;
        private int mistakes = 0;
        private int totalPopups = 0;

        private const int maxPopups = 14;
        private const int targetScore = 10;
        private const int maxMistakes = 4;

        private Label? scoreLabel;

        private string assetsPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            @"..\..\..\GameAssets"
        );

        private bool gameOver = false;
        public void StartGame(Difficulty difficulty)
        {
            if (parentContainer == null)
            {
                if (Application.OpenForms.Count == 0)
                    throw new InvalidOperationException("No open forms found.");
                parentContainer = Application.OpenForms[0]!;
            }
            MessageBox.Show("CatchTapGame starting! Be ready to tap the popups!", "Get Ready", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.difficulty = difficulty;

            SetupScoreLabel();
            ShowNextObject();
        }

        private void SetupScoreLabel()
        {
            if (scoreLabel == null)
            {
                scoreLabel = new Label
                {
                    Text = $"Score: {score}",
                    AutoSize = false,
                    Size = new Size(200, 50),
                    Location = new Point(10, 10),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Font = new Font("Arial", 14, FontStyle.Bold),
                    BackColor = Color.Transparent
                };
                parentContainer.Controls.Add(scoreLabel);
                scoreLabel.BringToFront();
            }
        }
        private void ShowNextObject()
        {
            if (gameOver) return;
            if (totalPopups >= maxPopups)
            {
                EndGame(score >= targetScore);
                return;
            }

            totalPopups++;

            PictureBox obj = new PictureBox
            {
                Size = new Size(100, 100),
                Location = new Point(Math.Max(0, rand.Next(Math.Max(1, parentContainer.Width - 100))),
                                    Math.Max(0, rand.Next(100, Math.Max(101, parentContainer.Height - 100)))),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
                Tag = false
            };

            string fadeGif = Path.Combine(assetsPath, "BopCat.gif");
            if (File.Exists(fadeGif))
                obj.Image = Image.FromFile(fadeGif);

            objects.Add(obj);
            parentContainer.Controls.Add(obj);
            obj.BringToFront();
        
        // --- Miss-timer ---
            int interval = GetPopupInterval(difficulty); // livstid för objekt
            System.Windows.Forms.Timer missTimer = new System.Windows.Forms.Timer { Interval = interval };
            activeTimers.Add(missTimer);

            missTimer.Tick += (s, e) =>
            {
                if (gameOver)
                {
                    missTimer.Stop();
                    missTimer.Dispose();
                    lock (activeTimers) { activeTimers.Remove(missTimer); }
                    return;
                }

                missTimer.Stop();
                missTimer.Dispose();
                lock (activeTimers) { activeTimers.Remove(missTimer); }

                
                if ((bool)obj.Tag == true) return;

                if (parentContainer.InvokeRequired)
                {
                    parentContainer.BeginInvoke(new Action(() =>
                    {
                        if (objects.Contains(obj))
                        {
                            parentContainer.Controls.Remove(obj);
                            objects.Remove(obj);
                        }
                        ProcessMissOrEnd();
                    }));
                }
                else
                {
                    if (objects.Contains(obj))
                    {
                        parentContainer.Controls.Remove(obj);
                        objects.Remove(obj);
                    }
                    ProcessMissOrEnd();
                }

                void ProcessMissOrEnd()
                {
                    mistakes++;
                    if (mistakes >= maxMistakes)
                    {
                        EndGame(false);
                    }
                    else if (totalPopups >= maxPopups)
                    {
                        EndGame(score >= targetScore);
                    }
                    else
                    {
                        ShowNextObject();
                    }
                }
            };
            missTimer.Start();

            // --- Klick-event ---
            obj.Click += (s, e) =>
            {
                if (gameOver) return;
                if ((bool)obj.Tag == true) return;

                obj.Tag = true;

                try
                {
                    missTimer.Stop();
                    missTimer.Dispose();
                    lock (activeTimers) { activeTimers.Remove(missTimer); }
                }
                catch { }

                if (parentContainer.InvokeRequired)
                {
                    parentContainer.BeginInvoke(new Action(() => HandleClick(obj)));
                }
                else
                {
                    HandleClick(obj);
                }
            };
        }
        private void HandleClick(PictureBox obj)
        {
            if (gameOver) return;

            if (objects.Contains(obj))
            {
                parentContainer.Controls.Remove(obj);
                objects.Remove(obj);
            }

            score++;
            scoreLabel!.Text = $"Score: {score}";

            if (score >= targetScore)
            {
                EndGame(true);
                return;
            }

            ShowNextObject();
        }
        private void HandleMiss(PictureBox obj)
        {
            if (gameOver) return;

            if (objects.Contains(obj))
            {
                parentContainer.Controls.Remove(obj);
                objects.Remove(obj);
            }

            mistakes++;

            if (mistakes >= maxMistakes)
            {
                EndGame(false);
                return;
            }

            if (totalPopups >= maxPopups)
            {
                EndGame(score >= targetScore);
                return;
            }

            ShowNextObject();
        }  
        private void EndGame(bool won)
        {
            if (gameOver) return;
            gameOver = true;

            // STOPPA ALLA TIMERS
            lock (activeTimers)
            {
                foreach (var t in activeTimers)
                {
                    try
                    {
                        t.Stop();
                        t.Dispose();
                    }
                    catch { }
                }
                activeTimers.Clear();
            }
            string resultMessage = won 
                ? $"Congratulations! You won with {score} points!" 
                : $"Game Over! You scored {score} points with {mistakes} mistakes.";
            MessageBox.Show(resultMessage, "Game Result", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Cleanup();
            GameEnded?.Invoke(this, new GameResult
            {
                Won = won,
                Points = score,
                Mistakes = mistakes
            });
        }            
    }
}