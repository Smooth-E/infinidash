using System.Collections;
using Mono.Collections.Generic;
using UnityEngine;
using Game;
using Random = System.Random;

namespace Generator
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PathMaker : MonoBehaviour
    {

        private enum PlotterAction
        {
            Slide,
            Jump,
            Orb,
            Pad
        }

        private Random _random;
        private Rigidbody2D _rigidbody;
        private PlotterAction _previousAction = PlotterAction.Slide;
        
        [SerializeField] private float _stateChangeInterval = .5f;
        
        public bool OnBlocks { private set; get; }
        public int GravityDirection { private set; get; } = 1;

        public delegate void OrbDelegate(OrbType orbType, Vector2 position);
        public delegate void PadDelegate(PadType padType, Vector2 position, bool onBlocks, int gravityDirection);
        public delegate void BlockUnderPadDelegate(Vector2 position, int gravityDirection, bool wasSliding);
        
        public event OrbDelegate OnCreateOrb;
        public event PadDelegate OnCreatePad;
        public event BlockUnderPadDelegate OnCreateBlockUnderPad;

        private void PerformJump(int modifierIndex)
        {
            _rigidbody.velocity = new Vector2(Constants.MoveVelocity, 0);
            _rigidbody.gravityScale = Constants.GravityScale * GravityDirection;
            var force = Constants.CalculateJumpForce(Constants.JumpModifiers[modifierIndex], GravityDirection);
            _rigidbody.AddForce(force, ForceMode2D.Impulse);
        }

        private void Jump(OrbType orbType)
        {
            if (orbType is OrbType.Blue or OrbType.Green)
                GravityDirection = -GravityDirection;
            PerformJump((int)orbType);
        }

        private void Jump(PadType padType)
        {
            if (padType == PadType.Blue)
                GravityDirection = -GravityDirection;
            PerformJump((int)padType);
        }

        private bool PositionCloseToInt() => 
            Mathf.Abs(transform.position.y - Mathf.RoundToInt(transform.position.y)) <= 0.1;

        private IEnumerator PlottingCoroutine()
        {
            while (true)
            {
                var allowedActions = new Collection<PlotterAction>();
                allowedActions.Add(PlotterAction.Slide);

                if (_previousAction == PlotterAction.Slide)
                    allowedActions.Add(PlotterAction.Jump);
                else 
                    allowedActions.Add(PlotterAction.Orb);

                if (_previousAction != PlotterAction.Orb && _previousAction != PlotterAction.Pad) 
                    allowedActions.Add(PlotterAction.Pad);

                var action = allowedActions[_random.Next(0, allowedActions.Count)];

                if (_previousAction != action)
                    yield return new WaitUntil(PositionCloseToInt);
                
                var currentPosition = transform.position;
                currentPosition.y = Mathf.RoundToInt(currentPosition.y);
                transform.position = currentPosition;

                OnBlocks = action == PlotterAction.Slide;

                if (action == PlotterAction.Pad) 
                    OnCreateBlockUnderPad?.Invoke(transform.position, GravityDirection, _previousAction == PlotterAction.Slide);

                switch (action)
                {
                    case PlotterAction.Slide:
                        _rigidbody.gravityScale = 0;
                        _rigidbody.velocity = new Vector2(Constants.MoveVelocity, 0);
                        break;
                    case PlotterAction.Jump:
                        _rigidbody.gravityScale = Constants.GravityScale * GravityDirection;
                        _rigidbody.AddForce(new Vector2(0, Constants.JumpForce * GravityDirection), ForceMode2D.Impulse);
                        break;
                    case PlotterAction.Orb:
                        var orbType = (OrbType)_random.Next(0, 5);
                        OnCreateOrb?.Invoke(orbType, transform.position);
                        Jump(orbType);
                        break;
                    case PlotterAction.Pad:
                        var padType = (PadType)_random.Next(0, 4);
                        OnCreatePad?.Invoke(padType, transform.position, _previousAction == PlotterAction.Slide, GravityDirection);
                        Jump(padType);
                        break;
                }

                yield return new WaitForSeconds(_stateChangeInterval);

                _previousAction = action;

                if (_stateChangeInterval >= .4f) _stateChangeInterval -= .0001f;
            }
        }

        private void Start()
        {
            _random = new Random(SeedGiver.Seed);
            _rigidbody = GetComponent<Rigidbody2D>();
            StartCoroutine(PlottingCoroutine());
        }

    }
}
