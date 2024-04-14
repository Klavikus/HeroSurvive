using GameCore.Source.Presentation.Api.MainMenu;
using Modules.MVPPassiveView.Runtime;
using Modules.UIComponents.Runtime.Implementations.Buttons;
using UnityEngine;

namespace GameCore.Source.Presentation.Core.MainMenu
{
    public class MainMenuView : ViewBase, IMainMenuView
    {
        [field: SerializeField] public Canvas[] Canvases { get; private set; }
        [field: SerializeField] public ActionButton StartButton { get; private set; }
        [field: SerializeField] public ActionButton PersistentUpgradesButton { get; private set; }

        [field: SerializeField] public ActionButton LeaderBoardButton { get; private set; }

        public void Show()
        {
            foreach (Canvas canvas in Canvases)
                canvas.enabled = true;
        }

        public void Hide()
        {
            foreach (Canvas canvas in Canvases)
                canvas.enabled = false;
        }
    }
}