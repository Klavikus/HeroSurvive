using System.Collections;
using CodeBase.Domain.Enemies;
using UnityEngine;

public class DamageEffectView : MonoBehaviour
{
    [SerializeField] private Damageable _damageable;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _changeColorTime;
    [SerializeField] private Color _colorOnDamage;
    [SerializeField] private bool _isEnabledFloatingDamageView;
    [SerializeField] private Color _floatingDamageColor;
    [SerializeField] private FloatingDamageView _floatingDamageView;

    private Color _initialColor;

    private void OnEnable()
    {
        _initialColor = _spriteRenderer.color;
        _damageable.DamageTaken += OnDamageTaken;
    }

    private void OnDisable()
    {
        _initialColor = _spriteRenderer.color;
        _damageable.DamageTaken -= OnDamageTaken;
    }

    private void OnDamageTaken(int damage)
    {
        if (_isEnabledFloatingDamageView)
            Instantiate(_floatingDamageView, transform.position, Quaternion.identity)
                .Initialize(damage, _floatingDamageColor);
        StartCoroutine(ChangeColor());
    }

    private IEnumerator ChangeColor()
    {
        _spriteRenderer.color = _colorOnDamage;
        yield return new WaitForSeconds(_changeColorTime);
        _spriteRenderer.color = _initialColor;
    }
}