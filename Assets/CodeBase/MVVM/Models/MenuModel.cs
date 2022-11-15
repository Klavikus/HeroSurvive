using System;

namespace CodeBase.MVVM.Models
{
    public class MenuModel
    {
        public event Action OpenedHeroSelection;
        public event Action OpenedUpgradeSelection;

        public event Action ClosedHeroSelection;
        public event Action ClosedUpgradeSelection;

        
        public void EnableHeroSelector() => OpenedHeroSelection?.Invoke();
        public void DisableHeroSelector() => ClosedHeroSelection?.Invoke();
        public void EnableUpgradeSelection() => OpenedUpgradeSelection?.Invoke();

        public void DisableUpgradeSelection() => ClosedUpgradeSelection?.Invoke();
    }
}