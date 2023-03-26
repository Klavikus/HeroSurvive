using UnityEngine;

namespace CodeBase.Infrastructure
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