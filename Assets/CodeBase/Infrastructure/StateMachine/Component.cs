using UnityEngine;

namespace CodeBase.Infrastructure.StateMachine
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