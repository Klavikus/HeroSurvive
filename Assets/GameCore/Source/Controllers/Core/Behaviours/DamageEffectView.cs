using System.Collections;
using GameCore.Source.Domain.EntityComponents;
using UnityEngine;

namespace GameCore.Source.Controllers.Core.Behaviours
{
    public class DamageEffectView : MonoBehaviour
    {
        [SerializeField] private Damageable _damageable;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _changeColorTime;
        [SerializeField] private Color _colorOnDamage;

        private Color _initialColor;

        private void OnEnable()
        {
            _initialColor = _spriteRenderer.color;
            _damageable.DamageTaken += OnDamageTaken;
        }

        private void OnDisable()
        {
            _damageable.DamageTaken -= OnDamageTaken;
            _spriteRenderer.color = _initialColor;
        }

        private void OnDamageTaken(int damage, float stagger)
        {
            StartCoroutine(ChangeColor());
        }

        private IEnumerator ChangeColor()
        {
            _spriteRenderer.color = _colorOnDamage;
            yield return new WaitForSeconds(_changeColorTime);
            _spriteRenderer.color = _initialColor;
        }
    }
}