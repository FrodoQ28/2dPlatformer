using System;
using UnityEngine;

[RequireComponent(typeof(Rotator))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] private Transform[] _targets;

    private Rotator _rotator;

    private int _currentTarget = 0;
    private float _speed = 1f;
    private float _minDistance = 0.5f;
    private Vector2 _direction;

    private void Awake()
    {
        _rotator = GetComponent<Rotator>();

        if (_targets == null)
            throw new NullReferenceException("Список точек пуст!");

        _direction = _targets[_currentTarget].position;
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, _targets[_currentTarget].position) < _minDistance)
        {
            _currentTarget = ++_currentTarget % _targets.Length;
            _direction = _targets[_currentTarget].position;

            bool isRight = _targets[_currentTarget].position.x > transform.position.x;

            _rotator.Turn(isRight);

            if (Mathf.Approximately(transform.eulerAngles.y, 180f))
                _direction = -_direction;
        }

        transform.Translate(_direction * _speed * Time.deltaTime);
    }
}