using UnityEngine;

public interface ILifeStealTargetProvider
{
    public Health GetTarget(Vector2 origin, float radius);
}