// Pop-Input Version 1.1
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Scripting;

namespace NeoludicGames.PopInput
{
    public abstract class BaseWebGLInputFieldHelper : MonoBehaviour, IPointerClickHandler
    {
        //This pragma disables the value assigned but unused warning.
#pragma warning disable 414
        [Tooltip("Determines when native pop-ups should be invoked upon clicking this object.")]
        [SerializeField] private WebGlInputHelperType helperType = WebGlInputHelperType.TouchDevicesOnly;
#pragma warning restore 414
        
        [Tooltip("Appears in the native pop-up as a description of the type of input required by the user.")]
        [SerializeField] private string inputDescription = "";
        
        /// <summary>
        /// The salt is used to ensure the JS Callback method is called on the correct game object
        /// without you having to ensure every object has a different name.
        /// </summary>
        private const string SALT = "(Selected for Input)";

#if UNITY_WEBGL &&! UNITY_EDITOR
    [DllImport("__Internal")] private static extern bool IsTouchDevice();
    [DllImport("__Internal")] private static extern void RequestNativePopUp(string objectID, string description, string currentText);
#else
        private static bool IsTouchDevice() => Input.touchSupported;
        private static void RequestNativePopUp(string objectID, string description, string currentText) {}
#endif

        public void OnPointerClick(PointerEventData eventData) => PromptPopUp();

        
        /// <summary> Prompts a native pop-up with a predefined input description if applicable. </summary>
        [Preserve] 
        public void PromptPopUp() => PromptPopUp(inputDescription);
        
        
        /// <summary> Prompts a native pop-up with a given input description if applicable. </summary>
        [Preserve]
        public void PromptPopUp(string descriptionOfInput)
        {
#if UNITY_WEBGL &&! UNITY_EDITOR
            if (helperType == WebGlInputHelperType.Never) return;
            if (helperType == WebGlInputHelperType.TouchDevicesOnly & !IsTouchDevice()) return;

            SaltName();
            RequestNativePopUp(gameObject.name, descriptionOfInput, GetCurrentTextInputText());
#endif
        }
        
        /// <summary> Called from the native JS code, this function handles the native pop-up result. </summary>
        [Preserve]
        public void ReceiveInputFieldText(string result)
        {
            UnSaltName();
            SetCurrentTextInputText(result);
        }
        
        /// <summary> Returns the current input field value. </summary>
        internal virtual string GetCurrentTextInputText() => string.Empty;
        
        /// <summary> Sets the current input field value and invokes onEndEdit. </summary>
        internal virtual void SetCurrentTextInputText(string result) { }

        /// <summary>
        /// The salt is used to ensure the JS Callback method is called on the correct game object
        /// without you having to ensure every object has a different name.
        /// </summary>
        [Preserve]
        private void SaltName()
        {
            if (!gameObject.name.Contains(SALT)) gameObject.name += SALT;
        }

        /// <summary> Removes the salt from the game objects name. </summary>
        private void UnSaltName()
        {
            if (gameObject.name.Contains(SALT)) gameObject.name = gameObject.name.Replace(SALT, "");
        }
        
        public enum WebGlInputHelperType
        {
            Always,
            TouchDevicesOnly,
            Never
        }
    }
}