using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.GameCore.Domain.Abilities.Attack
{
    public interface IAttackBehaviour
    {
        event Action PenetrationLimit;
        event Action<Transform> EnemyHitted;
        void Initialize(Rigidbody2D rigidbody2D);
        IEnumerator Run();
    }
}