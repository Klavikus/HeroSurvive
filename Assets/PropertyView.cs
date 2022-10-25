using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PropertyView : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _shortName;
    [SerializeField] private TMP_Text _value;

    public void Render(MainPropertyViewData viewData)
    {
        _image.sprite = viewData.Icon;
        _shortName.text = viewData.ShortName;
        _value.text = $"{viewData.Prefix} {viewData.Value} {viewData.Suffix}";
    }
}