using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PopupMiniGames.UI
{
    internal class Menu
    {
        private List<MenuButton> menuButtons;
        public int CurrentSelection { get; private set; }
        private Form parentForm;
        public Action onBack;

        public Menu(Form parentForm)
        {
            menuButtons = new List<MenuButton>();
            CurrentSelection = 0;
            this.parentForm = parentForm;
            onBack = () => { };
        }

        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (menuButtons.Count == 0) return;

            switch (e.KeyCode)
            {
                case Keys.S:
                case Keys.Down:
                    CurrentSelection = (CurrentSelection + 1) % menuButtons.Count;
                    UpdateButtonStates();
                    break;

                case Keys.W:
                case Keys.Up:
                    CurrentSelection = (CurrentSelection - 1 + menuButtons.Count) % menuButtons.Count;
                    UpdateButtonStates();
                    break;

                case Keys.Enter:
                case Keys.Space:
                    if (CurrentSelection >= 0 && CurrentSelection < menuButtons.Count)
                        menuButtons[CurrentSelection].PerformClick();
                    break;

                case Keys.Escape:
                    onBack.Invoke();
                    break;
            }
        }

        public void AddMenuButton(MenuButton button)
        {
            menuButtons.Add(button);
            parentForm.Controls.Add(button);

            if (menuButtons.Count == 1)
            {
                CurrentSelection = 0;
                UpdateButtonStates();
            }
            button.MouseEnter += (s, e) =>
            {
                CurrentSelection = menuButtons.IndexOf(button);
                UpdateButtonStates();
            };
        }

        private void UpdateButtonStates()
        {
            for (int i = 0; i < menuButtons.Count; i++)
            {
                if (i == CurrentSelection)
                    menuButtons[i].SetSelectedState();
                else
                    menuButtons[i].SetNormalState();
            }
        }
    }
}
