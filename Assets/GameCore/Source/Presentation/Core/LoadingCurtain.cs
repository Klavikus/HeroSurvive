using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameCore.Source.Presentation.Core
{
    public class LoadingCurtain : MonoBehaviour
    {
        public CanvasGroup Curtain;
        private float _fadeDuration = 0.4f;

        public void Initialize() =>
            DontDestroyOnLoad(this);

        public async UniTask ShowAsync()
        {
            gameObject.SetActive(true);

            Curtain.alpha = 0f;

            while (Curtain.alpha < 1f)
            {
                Curtain.alpha += 1f / _fadeDuration * Time.unscaledDeltaTime;
                await UniTask.Yield();
            }

            Curtain.alpha = 1;
        }

        public async UniTask HideAsync()
        {
            Curtain.alpha = 1;

            while (Curtain.alpha > 0f)
            {
                Curtain.alpha -= 1f / (_fadeDuration * 3) * Time.unscaledDeltaTime;
                await UniTask.Yield();
            }

            Curtain.alpha = 0;

            gameObject.SetActive(false);
        }
    }
}