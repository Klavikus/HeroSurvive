using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class TMPFontChangeHelper : MonoBehaviour
{
    [SerializeField] private TMP_FontAsset _font;

    [Button(nameof(ChangeAllFonts))]
    private void ChangeAllFonts()
    {
        TMP_Text[] texts = GetComponentsInChildren<TMP_Text>(true);

        foreach (TMP_Text tmpText in texts) 
            tmpText.font = _font;
    }
}