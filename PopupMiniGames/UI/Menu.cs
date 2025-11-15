using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PopupMiniGames.UI
{
    internal class Menu
    {
        private readonly List<MenuButton> menuButtons = new ();
        public int CurrentSelection { get; private set; } = 0;
        private readonly Form parentForm;
        public Action onBack = () => { };

        public Menu(Form parentForm) => this.parentForm = parentForm;

        public void AddMenuButton(MenuButton button)
        {
            menuButtons.Add(button);
            parentForm.Controls.Add(button);

            if (menuButtons.Count == 1)
                UpdateButtonStates();

            button.MouseEnter += (s, e) =>
            {
                CurrentSelection = menuButtons.IndexOf(button);
                UpdateButtonStates();
            };
        }

        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (menuButtons.Count == 0) return;

            switch (e.KeyCode)
            {
                case Keys.S:
                case Keys.Down:
                    CurrentSelection = (CurrentSelection + 1) % menuButtons.Count;
                    UpdateButtonStates(); // <?
                    break;

                case Keys.W:
                case Keys.Up:
                    CurrentSelection = (CurrentSelection - 1 + menuButtons.Count) % menuButtons.Count;
                    UpdateButtonStates(); //<?
                    break;

                case Keys.Enter:
                case Keys.Space:
                    menuButtons[CurrentSelection].PerformClick();
                    break;

                case Keys.Escape:
                    onBack.Invoke();
                    break;
            }
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
        public void ShowMenu() => menuButtons.ForEach(b => b.Visible = true);
        public void HideMenu() => menuButtons.ForEach(b => b.Visible = false);
    }
}
