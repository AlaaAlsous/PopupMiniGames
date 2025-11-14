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
        public Menu(Form parentForm)
        {
            menuButtons = new List<MenuButton>();
            CurrentSelection = -1;
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
