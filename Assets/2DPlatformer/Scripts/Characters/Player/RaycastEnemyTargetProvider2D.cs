using UnityEngine;

public class RaycastEnemyTargetProvider2D : ILifeStealTargetProvider
{
    private int _rayCount;
    private LayerMask _enemyMask;

    public RaycastEnemyTargetProvider2D(int rayCount, LayerMask enemyMask)
    {
        _rayCount = rayCount;
        _enemyMask = enemyMask;
    }

    public Health GetTarget(Vector2 origin, float radius)
    {
        Health bestTarget = null;
        float bestDistance = Mathf.Infinity;

        float angleStep = 360f / _rayCount;

        for (int i = 0; i < _rayCount; i++)
        {
            float angle = angleStep * i * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            RaycastHit2D hit = Physics2D.Raycast(origin, direction, radius, _enemyMask);
            if (hit.collider == null)
            {
                continue;
            }

            Health health = hit.collider.GetComponent<Health>();
            if (health == null)
            {
                continue;
            }

            float distance = Vector2.Distance(origin, hit.point);
            if (distance < bestDistance)
            {
                bestDistance = distance;
                bestTarget = health;
            }
        }

        return bestTarget;
    }
}