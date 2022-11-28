using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardScoreView : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _position;
    [SerializeField] private TMP_Text _score;
    [SerializeField] private Image _positionIcon;

    public void Initialize(string userName, int score, int rank)
    {
        _positionIcon.gameObject.SetActive(false);
        _position.text = rank.ToString();
        _name.text = userName;
        _score.text = score.ToString();
    }
}