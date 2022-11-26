using CodeBase.MVVM.ViewModels;
using UnityEngine;

namespace CodeBase.MVVM.Views
{
    public class UserNameView : MonoBehaviour
    {
        private UserNameViewModel _userNameViewModel;

        public void Initialize(UserNameViewModel userNameViewModel)
        {
            _userNameViewModel = userNameViewModel;
        }
    }
}