using System.Collections.Generic;
using Source.Common.Components.Interfaces;
using UnityEngine;

namespace Source.Common.Components.Implementations.SelectablesGroup
{
    public class SelectablesUiGroup : MonoBehaviour
    {
        private IEnumerable<ISelectable> _selectables;

        public void Initialize(IEnumerable<ISelectable> selectables)
        {
            _selectables = selectables;
        }

        public void Select(ISelectable targetSelectable)
        {
            foreach (ISelectable selectable in _selectables)
            {
                if (selectable == targetSelectable)
                    selectable.Focus();
                else
                    selectable.Unfocus();
            }
        }
    }
}