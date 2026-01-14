using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Mover2D), typeof(Rotator2D), typeof(AnimationSwitch))]
public class Patrol : MonoBehaviour
{
    [SerializeField] private GameObject _targetPointsParent;
    [SerializeField] private Transform[] _targetPoints;

    private Mover2D _mover;
    private Rotator2D _rotator;
    private AnimationSwitch _animation;

    private Vector3 _direction;
    private int _currentTarget = 0;
    private float _minDictanceSquared = 0.25f;
    private Coroutine _bypassCoroutine;

    private void Awake()
    {
        if (_targetPoints == null)
            throw new NullReferenceException("Список точек пуст");

        _mover = GetComponent<Mover2D>();
        _rotator = GetComponent<Rotator2D>();
        _animation = GetComponent<AnimationSwitch>();
    }

    public void Enable()
    {
        if (_bypassCoroutine == null)
            _bypassCoroutine = StartCoroutine(Bypassing());
    }

    public void Disable()
    {
        if (_bypassCoroutine != null)
        {
            StopCoroutine(_bypassCoroutine);

            _bypassCoroutine = default;
        }
    }

    private IEnumerator Bypassing()
    {
        WaitForFixedUpdate wait = new WaitForFixedUpdate();

        SetDirection();

        while (enabled)
        {
            yield return wait;

            Vector2 differencePosition = _targetPoints[_currentTarget].position - transform.position;

            if (differencePosition.sqrMagnitude <= _minDictanceSquared)
            {
                _currentTarget = ++_currentTarget % _targetPoints.Length;

                SetDirection();
            }

            Move();
        }

    }

    private void SetDirection()
    {
        _direction = (_targetPoints[_currentTarget].position - transform.position).normalized;
        _rotator.Turn(_direction);
    }

    private void Move()
    {
        if (_direction != Vector3.zero)
        {
            _mover.Move(_direction);
            _animation.TurnOnMove();
        }
        else
        {
            _animation.TurnOffMove();
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Refresh Point List")]
    private void RefreshPointList()
    {
        int pointCount = _targetPointsParent.transform.childCount;
        _targetPoints = new Transform[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            _targetPoints[i] = _targetPointsParent.transform.GetChild(i).transform;
        }
    }
#endif
}