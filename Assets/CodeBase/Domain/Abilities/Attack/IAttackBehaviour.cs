using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.Domain.Abilities.Attack
{
    public interface IAttackBehaviour
    {
        event Action PenetrationLimit;
        void Initialize(Rigidbody2D rigidbody2D);
        IEnumerator Run();
    }
}