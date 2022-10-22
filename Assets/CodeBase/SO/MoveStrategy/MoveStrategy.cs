using UnityEngine;

public abstract class MoveStrategy : ScriptableObject
{
    public abstract Vector3 GetMoveVector(Transform origin, Vector3 target, float checkDistance);
}