using System.Collections;
using Mono.Collections.Generic;
using UnityEngine;

namespace Generator
{
    public class LevelMaker : PathMaker
    {

        private int _lastFrameX;
        private int _lastFrameY;
        private Collection<Vector2Int> _touchedCells;

        public delegate void OrbDelegate(OrbType orbType, Vector2 position);
        public delegate void PadDelegate(PadType padType, Vector2 position, bool onBlocks, int gravityDirection);
        public delegate void ColumnDelegate(Vector2Int[] positions, int gravityDirection);
        public delegate void BlockForPadDelegate(Vector2 position, int gravityDirection, bool wasSliding);
        
        public static event OrbDelegate OrbHere;
        public static event PadDelegate PadHere;
        public static event ColumnDelegate NewColumn;
        public static event BlockForPadDelegate BlockUnderPad;
        
        protected override void CreateOrb(OrbType orbType, Vector2 position) => OrbHere?.Invoke(orbType, position);

        protected override void CreatePad(PadType padType, Vector2 position, bool onBlocks, int gravityDirection) =>
            PadHere?.Invoke(padType, position, onBlocks, gravityDirection);
        
        protected override void CreateBlockUnderPad(Vector2 position, int gravityDirection, bool wasSliding) =>
            BlockUnderPad?.Invoke(position, gravityDirection, wasSliding);

        private IEnumerator BlockingCoroutine()
        {
            while (true)
            {
                _touchedCells = new Collection<Vector2Int>();
                while (OnBlocks)
                {
                    var position = transform.position;
                    var currentX = Mathf.RoundToInt(position.x);
                    var currentY = Mathf.RoundToInt(position.y);
                    if (currentX != _lastFrameX)
                    {
                        if (_touchedCells.Count > 0) 
                            NewColumn?.Invoke(_touchedCells.ToArray(), GravityDirection);
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
                yield return new WaitUntil(() => OnBlocks);
            }
        }

        protected override void PostStart()
        {
            var position = transform.position;
            _touchedCells = new() { new Vector2Int((int)position.x, (int)position.y) };
            StartCoroutine(BlockingCoroutine());
        }
        
    }
}
