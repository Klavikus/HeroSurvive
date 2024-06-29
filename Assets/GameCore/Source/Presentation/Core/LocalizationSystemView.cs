using System.Linq;
using GameCore.Source.Common.Localization;
using GameCore.Source.Presentation.Api.GameLoop;
using Modules.MVPPassiveView.Runtime;
using UnityEngine;

namespace GameCore.Source.Presentation.Core
{
    public class LocalizationSystemView : ViewBase, ILocalizationSystemView
    {
        [SerializeField] private LocalizableTMPText[] _localizableTMPTexts;

        public ILocalizable[] Localizables => _localizableTMPTexts.Cast<ILocalizable>().ToArray();

        [ContextMenu(nameof(CollectAllLocalizable))]
        public void CollectAllLocalizable()
        {
            _localizableTMPTexts = FindObjectsOfType<LocalizableTMPText>(true);
        }
    }
}