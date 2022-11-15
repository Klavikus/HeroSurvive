using UnityEngine;

public class FloatingDamageView : MonoBehaviour
{
    [SerializeField] private TextMesh _textMesh;
    [SerializeField] private float _dieDelay = 0.5f;

    public void Initialize(int damage, Color color)
    {
        _textMesh.color = color;
        _textMesh.text = $"-{damage}";
        Destroy(gameObject, _dieDelay);
    }
}