using System;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Domain.Models;
using GameCore.Source.Infrastructure.Api.GameFsm;
using GameCore.Source.Presentation.Api.GameLoop;
using Modules.MVPPassiveView.Runtime;

namespace GameCore.Source.Controllers.Core.Presenters
{
    public class LevelUpSystemPresenter : IPresenter
    {
        private readonly ILevelUpSystemView _view;
        private readonly LevelUpModel _levelUpModel;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IGameLoopService _gameLoopService;

        private int _respawnsCount;

        public LevelUpSystemPresenter(
            ILevelUpSystemView view,
            LevelUpModel levelUpModel)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _levelUpModel = levelUpModel ?? throw new ArgumentNullException(nameof(levelUpModel));
        }

        public void Enable()
        {
            FillCompletionProgressBar(_levelUpModel.CurrentCompletionProgress);
            _levelUpModel.LevelProgressChanged += FillCompletionProgressBar;
        }

        public void Disable()
        {
            _levelUpModel.LevelProgressChanged -= FillCompletionProgressBar;
        }

        private void FillCompletionProgressBar(float completePercentage) =>
            _view.LevelCompletionImage.fillAmount = completePercentage;
    }
}