using CodeBase.MVVM.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardScoreView : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _position;
    [SerializeField] private TMP_Text _score;
    [SerializeField] private Image _positionIcon;

    public void Initialize(string userName, int score)
    {
        _positionIcon.gameObject.SetActive(false);
        _position.gameObject.SetActive(false);
        _name.text = userName;
        _score.text = score.ToString();
    }
}