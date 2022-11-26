using System;
using CodeBase.Configs;
using CodeBase.MVVM.Models;

namespace CodeBase.MVVM.ViewModels
{
    public class UserNameViewModel
    {
        private readonly UserModel _userModel;
        private readonly GameLoopModel _gameLoopModel;

        public event Action<string> UserNameChanged;
        public event Action ShowInvoked;

        public bool IsDefaultName => _userModel.Name == GameConstants.BaseUserName;

        public UserNameViewModel(UserModel userModel, GameLoopModel gameLoopModel)
        {
            _userModel = userModel;
            _gameLoopModel = gameLoopModel;
            _userModel.NameChanged += name => UserNameChanged?.Invoke(name);
            _gameLoopModel.UserNameShowInvoked += () => ShowInvoked?.Invoke();
        }

        public void SetUserName(string name) => _userModel.SetName(name);
        private void Show() => ShowInvoked?.Invoke();
    }
}