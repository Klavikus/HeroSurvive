using System.Collections;
using UnityEngine;

namespace CodeBase.Presentation.Views
{
    public class SimpleSpriteAnimator : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private float _delay;

        private int _currentSpriteId;

        private IEnumerator Start()
        {
            var delay = new WaitForSeconds(_delay);
            while (true)
            {
                SetNextSprite();
                yield return delay;
            }
        }

        private void SetNextSprite()
        {
            if (++_currentSpriteId == _sprites.Length)
                _currentSpriteId = 0;
            _renderer.sprite = _sprites[_currentSpriteId];
        }
    }
}