using System;
using GameCore.Source.Presentation.Api;
using UnityEngine;

namespace GameCore.Source.Presentation.Core
{
    //TODO: Fix Nullref
    public class PoolableParticleSystem : MonoBehaviour, IPoolableParticleSystem
    {
        public event Action<IPoolableParticleSystem> Completed;
        public GameObject GameObject
        {
            get
            {
                try
                {
                    return gameObject;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);

                    throw;
                }
            }
        }

        private void OnDisable() =>
            Completed?.Invoke(this);
    }
}