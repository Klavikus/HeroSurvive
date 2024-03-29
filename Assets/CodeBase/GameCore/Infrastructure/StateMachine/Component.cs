using UnityEngine;

namespace CodeBase.GameCore.Infrastructure.StateMachine
{
    public class Component : MonoBehaviour
    {
        private Animator _animator;


        public void Initialize(Animator animator)
        {
            _animator = animator;
        }
    }
}