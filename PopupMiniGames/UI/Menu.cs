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
        public Menu(Form parentForm)
        {
            menuButtons = new List<MenuButton>();
            CurrentSelection = -1;
            this.parentForm = parentForm;
        }


        public void AddMenuButton(MenuButton button)
        {
            menuButtons.Add(button);
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
