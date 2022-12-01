using UnityEngine;
using UnityEngine.UI;

namespace NeoludicGames.PopInput
{
    [RequireComponent(typeof(InputField))]
    public class WebGLInputFieldHelper : BaseWebGLInputFieldHelper
    {
        private InputField inputField;
        private void Start() => inputField = gameObject.GetComponent<InputField>();

        internal override string GetCurrentTextInputText() => inputField.text;

        internal override void SetCurrentTextInputText(string result)
        {
            inputField.text = result;
            inputField.onEndEdit.Invoke(result);
        }
    }
}