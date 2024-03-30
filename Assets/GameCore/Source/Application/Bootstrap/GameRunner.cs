using UnityEngine;

namespace GameCore.Source.Application.Bootstrap
{
    public class  GameRunner : MonoBehaviour
    {
        public Bootstrap _bootstrap;
        
        private void Awake()
        {
            Bootstrap bootstrapper = FindObjectOfType<Bootstrap>();
            
            if (bootstrapper == null) 
                Instantiate(_bootstrap);
        }
    }
}