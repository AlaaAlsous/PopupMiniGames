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
    }
}
