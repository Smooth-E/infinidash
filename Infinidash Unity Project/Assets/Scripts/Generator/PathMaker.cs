using System.Collections;
using Mono.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Tools;

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

        public delegate void OrbDelegate(OrbType orbType, Vector2 position);
        public delegate void PadDelegate(PadType padType, Vector2 position, bool onBlocks, int gravityDirection);
        public delegate void ColumnDelegate(Vector2Int[] positions, int gravityDirection);
        public delegate void BlockForPadDelegate(Vector2 position, int gravityDirection, bool wasSliding);
        
        public static event OrbDelegate OrbHere;
        public static event PadDelegate PadHere;
        public static event ColumnDelegate NewColumn;
        public static event BlockForPadDelegate BlockUnderPad;

        private void Jump(OrbType orbType)
        {
            _rigidbody.velocity = new Vector2(Constants.MoveVelocity, 0);
            if (orbType is OrbType.Blue or OrbType.Green)
                _gravityDirection = -_gravityDirection;
            _rigidbody.gravityScale = Constants.GravityScale * _gravityDirection;
            var force = new Vector2(0, Constants.JumpForce * Constants.JumpModifiers[(int)orbType] * _gravityDirection);
            _rigidbody.AddForce(force, ForceMode2D.Impulse);
        }

        private void Jump(PadType padType)
        {
            _rigidbody.velocity = new Vector2(Constants.MoveVelocity, 0);
            if (padType == PadType.Blue)
                _gravityDirection = -_gravityDirection;
            _rigidbody.gravityScale = Constants.GravityScale * _gravityDirection;
            var force = new Vector2(0, Constants.JumpForce * Constants.JumpModifiers[(int)padType] * _gravityDirection);
            _rigidbody.AddForce(force, ForceMode2D.Impulse);
        }
        
        private IEnumerator PlottingCoroutine()
        {
            while (true)
            {
                // There are a bunch of actions a plotter can make
                // 1. Start gliding
                // 2. Jump
                // 3. Jump off of an orb (1 out of 4)
                // 4. Jump off of a pad (1 of 4)
                var allowedActions = new Collection<int>();
                allowedActions.Add(1);
                if (_previousAction != -1)
                {
                    if (_previousAction == 1) allowedActions.Add(2);
                    else allowedActions.Add(3);
                    if (_previousAction != 3 && _previousAction != 4) allowedActions.Add(4);
                }
                var action = allowedActions[Random.Range(0, allowedActions.Count)];
                if (_previousAction != action)
                    yield return new WaitUntil(() => Mathf.Abs(transform.position.y - Mathf.RoundToInt(transform.position.y)) <= 0.1);
                var currentPosition = transform.position;
                currentPosition.y = Mathf.RoundToInt(currentPosition.y);
                transform.position = currentPosition;
                _onBlocks = action == 1;
                if (action == 4) BlockUnderPad?.Invoke(transform.position, _gravityDirection, _previousAction == 1);
                _previousAction = action;
                switch (action)
                {
                    case 1:
                        _rigidbody.gravityScale = 0;
                        _rigidbody.velocity = new Vector2(Constants.MoveVelocity, 0);
                        break;
                    case 2:
                        _rigidbody.gravityScale = Constants.GravityScale * _gravityDirection;
                        _rigidbody.AddForce(new Vector2(0, Constants.JumpForce * _gravityDirection), ForceMode2D.Impulse);
                        break;
                    case 3:
                        var orbType = (OrbType)Random.Range(0, 5);
                        OrbHere?.Invoke(orbType, transform.position);
                        Jump(orbType);
                        break;
                    case 4:
                        var padType = (PadType)Random.Range(0, 4);
                        PadHere?.Invoke(padType, transform.position, _previousAction == 1, _gravityDirection);
                        Jump(padType);
                        break;
                }
                yield return new WaitForSeconds(_stateChangeInterval);
                if (_stateChangeInterval >= .4f) _stateChangeInterval -= .0001f;
            }
        }

        private IEnumerator BlockingCoroutine()
        {
            while (true)
            {
                _touchedCells = new Collection<Vector2Int>();
                while (_onBlocks)
                {
                    var position = transform.position;
                    var currentX = Mathf.RoundToInt(position.x);
                    var currentY = Mathf.RoundToInt(position.y);
                    if (currentX != _lastFrameX)
                    {
                        if (_touchedCells.Count > 0) 
                            NewColumn?.Invoke(_touchedCells.ToArray(), _gravityDirection);
                        _touchedCells = new() { new Vector2Int(currentX, currentY) };
                    }
                    else if (currentY != _lastFrameY)
                    {
                        _touchedCells.Add(new Vector2Int(currentX, currentY));
                    }
                    _lastFrameX = currentX;
                    _lastFrameY = currentY;
                    yield return null;
                }
                yield return new WaitUntil(() => _onBlocks);
            }
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.gravityScale = 0;
            var position = transform.position;
            _touchedCells = new() { new Vector2Int((int)position.x, (int)position.y) };
            StartCoroutine(PlottingCoroutine());
            StartCoroutine(BlockingCoroutine());
        }
        
    }
}
