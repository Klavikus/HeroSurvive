using TMPro;
using UnityEngine;

namespace CodeBase.GameCore.Presentation.Views
{
    public class CounterView : MonoBehaviour
    {
        [SerializeField] private string _name;
        [SerializeField] private string _initialValue;
        [SerializeField] private TMP_Text[] _textOutputs;

        public void Initialize() => SetOutputsText(_initialValue);

        public void OnCounterChanged(int currentReward) => SetOutputsText(currentReward.ToString());

        private void SetOutputsText(string newValue)
        {
            foreach (TMP_Text textOutput in _textOutputs)
                textOutput.text = newValue;
        }
    }
}