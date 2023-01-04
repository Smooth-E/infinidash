using System;
using System.Collections;
using Mono.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Generator
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PathMaker : MonoBehaviour
    {

        private Rigidbody2D _rigidbody;
        private float _stateChangeInterval = .5f;
        private int _lastFrameX;
        private int _lastFrameY;
        private Collection<Vector2Int> _touchedCells;
        private int _gravityDirection = 1;
        private bool _onBlocks;
        private int _previousAction = -1;
        
        private readonly float _gravityScale = 4;
        private readonly float _jumpForce = 8.5f;
        private readonly float _moveVelocity = 7f;

        private readonly float[] _jumpModifiers = new[]
        {
            1,
            1.2f * 1.5f,
            1.5f * 1.5f,
            0,
            1.5f * 1.5f
        };

        public delegate void OrbDelegate(OrbType orbType, Vector2 position);
        public delegate void PadDelegate(PadType padType, Vector2 position, bool onBlocks);
        public delegate void ColumnDelegate(Vector2Int[] positions, int gravityDirection, bool onBlocks);
        
        public static event OrbDelegate OrbHere;
        public static event PadDelegate PadHere;
        public static event ColumnDelegate NewColumn;

        private void Jump(OrbType orbType)
        {
            _rigidbody.velocity = new Vector2(_moveVelocity, 0);
            if (orbType is OrbType.Blue or OrbType.Green)
            {
                _gravityDirection = -_gravityDirection;
                _rigidbody.gravityScale = _gravityScale * _gravityDirection;
            }
            var force = new Vector2(0, _jumpForce * _jumpModifiers[(int)orbType] * _gravityDirection);
            _rigidbody.AddForce(force, ForceMode2D.Impulse);
        }

        private void Jump(PadType padType)
        {
            _rigidbody.velocity = new Vector2(_moveVelocity, 0);
            if (padType == PadType.Blue)
            {
                _gravityDirection = -_gravityDirection;
                _rigidbody.gravityScale = _gravityScale * _gravityDirection;
            }
            var force = new Vector2(0, _jumpForce * _jumpModifiers[(int)padType] * _gravityDirection);
            _rigidbody.AddForce(force, ForceMode2D.Impulse);
        }
        
        private IEnumerator PlottingCoroutine()
        {
            while (true)
            {
                // There are a bunch of actions a plotter can make
                // 1. Stay as is and continue gliding
                // 2. Jump
                // 3. Jump off of an orb (1 out of 4)
                // 4. Jump off of a pad (1 of 4)
                yield return new WaitUntil(() => Mathf.Abs(transform.position.y - Mathf.RoundToInt(transform.position.y)) <= 0.1);
                var currentPosition = transform.position;
                currentPosition.y = Mathf.RoundToInt(currentPosition.y);
                transform.position = currentPosition;
                var action = Random.Range(1, 5);
                while (_previousAction is 4 or 3 && action is 4)
                    action = Random.Range(1, 3);
                _onBlocks = action == 1 || action == 4;
                _previousAction = action;
                switch (action)
                {
                    case 1:
                        _rigidbody.gravityScale = 0;
                        _rigidbody.velocity = new Vector2(_moveVelocity, 0);
                        break;
                    case 2:
                        _rigidbody.gravityScale = _gravityScale * _gravityDirection;
                        _rigidbody.AddForce(new Vector2(0, _jumpForce * _gravityDirection), ForceMode2D.Impulse);
                        break;
                    case 3:
                        var orbType = (OrbType)Random.Range(0, 5);
                        OrbHere?.Invoke(orbType, transform.position);
                        Jump(orbType);
                        break;
                    case 4:
                        var padType = (PadType)Random.Range(0, 4);
                        PadHere?.Invoke(padType, transform.position, _previousAction == 1);
                        Jump(padType);
                        break;
                }
                yield return new WaitForSeconds(_stateChangeInterval);
                if (_stateChangeInterval >= .2f) _stateChangeInterval -= .001f;
            }
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.gravityScale = 0;
            var position = transform.position;
            _touchedCells = new() { new Vector2Int((int)position.x, (int)position.y) };
            StartCoroutine(PlottingCoroutine());
        }

        private void Update()
        {
            var position = transform.position;
            var currentX = Mathf.RoundToInt(position.x);
            var currentY = Mathf.RoundToInt(position.y);
            if (currentX != _lastFrameX)
            {
                NewColumn?.Invoke(_touchedCells.ToArray(), _gravityDirection, _onBlocks);
                _touchedCells = new() { new Vector2Int(currentX, currentY) };
            }
            else if (currentY != _lastFrameY)
            {
                _touchedCells.Add(new Vector2Int(currentX, currentY));
            }
            _lastFrameX = currentX;
            _lastFrameY = currentY;
        }
        
    }
}
