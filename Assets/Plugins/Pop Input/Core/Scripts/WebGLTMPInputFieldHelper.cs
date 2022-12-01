using TMPro;
using UnityEngine;

namespace NeoludicGames.PopInput
{
    [RequireComponent(typeof(TMP_InputField))]
    public class WebGLTMPInputFieldHelper : BaseWebGLInputFieldHelper
    {
        private TMP_InputField inputField;
        private void Start() => inputField = gameObject.GetComponent<TMP_InputField>();

        internal override string GetCurrentTextInputText() => inputField.text;

        internal override void SetCurrentTextInputText(string result)
        {
            inputField.text = result;
            inputField.onEndEdit.Invoke(result);
        }
    }
}