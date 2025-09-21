using System;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] private Transform[] _targets;

    private int _currentTarget = 0;
    private float _speed = 1f;
    private float _minDistance = 0.5f;
    private Vector3 _direction;

    private void Awake()
    {
        if (_targets == null)
            throw new NullReferenceException("Список точек пуст!");

        _direction = _targets[_currentTarget].transform.position;
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, _targets[_currentTarget].position) < _minDistance)
        {
            _currentTarget = ++_currentTarget % _targets.Length;
            _direction = _targets[_currentTarget].transform.position;

            Rotate();

            if (Mathf.Approximately(transform.eulerAngles.y, 180f))
                _direction = -_direction;
        }

            transform.Translate(_direction * _speed * Time.deltaTime);
    }

    private void Rotate()
    {
        Vector2 targetLocalPosition = transform.InverseTransformPoint(_targets[_currentTarget].position);

        Vector2 rotate = new Vector2(0f, 180f);

        if (targetLocalPosition.x < 0f)
            transform.Rotate(rotate);
    }
}