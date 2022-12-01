using TMPro;
using UnityEngine;

namespace CodeBase.Domain.Additional
{
    public class FPSShower : MonoBehaviour
    {
        public TMP_Text fpsText;
        public float deltaTime;

        private void Update()
        {
            if (Time.timeScale == 0)
                return;

            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
            float fps = 1.0f / deltaTime;
            fpsText.text = Mathf.Ceil(fps).ToString();
        }
    }
}