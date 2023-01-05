using System.Collections;
using Mono.Collections.Generic;
using UnityEngine;
using Tools;
using Random = System.Random;

namespace Generator
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PathMaker : MonoBehaviour
    {

        private Rigidbody2D _rigidbody;
        private float _stateChangeInterval = .5f;
        private int _previousAction = -1;
        private Random _random;

        protected bool OnBlocks { private set; get; }
        protected int GravityDirection { private set; get; } = 1;
        
        protected virtual void CreateOrb(OrbType orbType, Vector2 position) {  }

        protected virtual void CreatePad(PadType padType, Vector2 position, bool onBlocks, int gravityDirection) {  }

        protected virtual void CreateBlockUnderPad(Vector2 position, int gravityDirection, bool wasSliding) {  }

        private void Jump(OrbType orbType)
        {
            _rigidbody.velocity = new Vector2(Constants.MoveVelocity, 0);
            if (orbType is OrbType.Blue or OrbType.Green)
                GravityDirection = -GravityDirection;
            _rigidbody.gravityScale = Constants.GravityScale * GravityDirection;
            var force = new Vector2(0, Constants.JumpForce * Constants.JumpModifiers[(int)orbType] * GravityDirection);
            _rigidbody.AddForce(force, ForceMode2D.Impulse);
        }

        private void Jump(PadType padType)
        {
            _rigidbody.velocity = new Vector2(Constants.MoveVelocity, 0);
            if (padType == PadType.Blue)
                GravityDirection = -GravityDirection;
            _rigidbody.gravityScale = Constants.GravityScale * GravityDirection;
            var force = new Vector2(0, Constants.JumpForce * Constants.JumpModifiers[(int)padType] * GravityDirection);
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
                var action = allowedActions[_random.Next(0, allowedActions.Count)];
                if (_previousAction != action)
                    yield return new WaitUntil(() => Mathf.Abs(transform.position.y - Mathf.RoundToInt(transform.position.y)) <= 0.1);
                var currentPosition = transform.position;
                currentPosition.y = Mathf.RoundToInt(currentPosition.y);
                transform.position = currentPosition;
                OnBlocks = action == 1;
                if (action == 4) CreateBlockUnderPad(transform.position, GravityDirection, _previousAction == 1);
                _previousAction = action;
                switch (action)
                {
                    case 1:
                        _rigidbody.gravityScale = 0;
                        _rigidbody.velocity = new Vector2(Constants.MoveVelocity, 0);
                        break;
                    case 2:
                        _rigidbody.gravityScale = Constants.GravityScale * GravityDirection;
                        _rigidbody.AddForce(new Vector2(0, Constants.JumpForce * GravityDirection), ForceMode2D.Impulse);
                        break;
                    case 3:
                        var orbType = (OrbType)_random.Next(0, 5);
                        CreateOrb(orbType, transform.position);
                        Jump(orbType);
                        break;
                    case 4:
                        var padType = (PadType)_random.Next(0, 4);
                        CreatePad(padType, transform.position, _previousAction == 1, GravityDirection);
                        Jump(padType);
                        break;
                }
                yield return new WaitForSeconds(_stateChangeInterval);
                if (_stateChangeInterval >= .4f) _stateChangeInterval -= .0001f;
            }
        }

        protected virtual void PostStart() {  }

        private void Start()
        {
            _random = new Random(SeedGiver.Seed);
            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.gravityScale = 0;
            StartCoroutine(PlottingCoroutine());
            PostStart();
        }

    }
}
