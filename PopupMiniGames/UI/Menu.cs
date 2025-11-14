using MiniGames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopupMiniGames.UI
{
    internal class Menu
    {
        List<MenuButton> menuButtons;
        public int CurrentSelection { get; private set; }
        Form parentForm;
        public Action onBack;
        public Menu(Form parentForm)
        {
            menuButtons = new List<MenuButton>();
            CurrentSelection = -1;
            this.parentForm = parentForm;
            onBack = () => { };
        }
        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S)
            {
                SelectNext();
            }
            else if (e.KeyCode == Keys.W)
            {
                SelectPrevious();
            }
            else if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space)
            {
                if (menuButtons.Count > 0 && CurrentSelection >= 0)
                {
                    menuButtons[CurrentSelection].PerformClick();
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                onBack.Invoke();
            }
        }
        public void AddMenuButton(MenuButton button)
        {
            menuButtons.Add(button);
            menuButtons[menuButtons.IndexOf(button)].MouseEnter += (s, e) =>
            {
                SetSelection(menuButtons.IndexOf(button));
            };
            menuButtons[menuButtons.IndexOf(button)].MouseLeave += (s, e) =>
            {
                Deselect(menuButtons.IndexOf(button));
            };
            parentForm.Controls.Add(button);
        }
        public void SelectNext()
        {
            if (menuButtons.Count == 0) return;
            CurrentSelection = Math.Min(menuButtons.Count - 1, CurrentSelection + 1);
            UpdateButtonStates();
        }
        public void SelectPrevious()
        {
            if (menuButtons.Count == 0) return;
            CurrentSelection = Math.Max(0, CurrentSelection - 1);
            UpdateButtonStates();
        }
        public void SetSelection(int index)
        {
            if (index < 0 || index >= menuButtons.Count) return;
            CurrentSelection = index;
            UpdateButtonStates();
        }
        public void Deselect(int index)
        {
            if (CurrentSelection == index)
            {
                CurrentSelection = -1;
            }
            UpdateButtonStates();
        }
        private void UpdateButtonStates()
        {
            for (int i = 0; i < menuButtons.Count; i++)
            {
                if (i == CurrentSelection)
                {
                    menuButtons[i].SetSelectedState();
                }
                else
                {
                    menuButtons[i].SetNormalState();
                }
            }
        }
    }
}