using GameCore.Source.Presentation.Api.MainMenu;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Source.Presentation.Core.MainMenu
{
    public class MVPLeaderBoardScoreView : MonoBehaviour, ILeaderBoardScoreView
    {
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _position;
        [SerializeField] private TMP_Text _score;
        [SerializeField] private Image _positionIcon;

        // private ITranslationService _translationService;

        // public void Initialize(ITranslationService translationService) =>
            // _translationService = translationService;

        public void Render(string userName, int score, int rank)
        {
            // _name.text = string.IsNullOrEmpty(userName) ? _translationService.GetLocalizedHiddenUser() : userName;

            _positionIcon.gameObject.SetActive(false);
            _position.text = rank.ToString();
            _score.text = score.ToString();
        }
    }
}