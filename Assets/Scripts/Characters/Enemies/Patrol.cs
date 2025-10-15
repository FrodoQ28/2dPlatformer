using System;
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
    private bool _isRight = true;

    private void Awake()
    {
        if (_targetPoints == null)
            throw new NullReferenceException("Список точек пуст");

        _mover = GetComponent<Mover2D>();
        _rotator = GetComponent<Rotator2D>();
        _animation = GetComponent<AnimationSwitch>();

        _direction = _targetPoints[_currentTarget].position;
    }

    private void Update()
    {
        Vector2 differencePosition = _targetPoints[_currentTarget].position - transform.position;

        if (differencePosition.sqrMagnitude <= _minDictanceSquared)
        {
            _currentTarget = ++_currentTarget % _targetPoints.Length;
            _direction = _targetPoints[_currentTarget].position;
            DefineTurn();
        }
    }

    private void FixedUpdate()
    {
        _rotator.Turn(_isRight);

        Move();
    }

    private void Move()
    {
        if (_direction != Vector3.zero)
        {
            _mover.Move(_direction);
            _animation.OnMove();
        }
        else
        {
            _animation.OffMove();
        }
    }

    private void DefineTurn()
    {
        if (_direction.x > transform.position.x)
        {
            _isRight = true;
        }
        else if (_direction.x < transform.position.x)
        {
            _isRight = false;
            _direction.x = -_direction.x;
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