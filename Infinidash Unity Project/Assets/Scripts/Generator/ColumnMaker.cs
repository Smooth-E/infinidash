using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace Generator
{
    public class ColumnMaker : MonoBehaviour
    {

        private int _lastFrameX;
        private int _lastFrameY;
        private List<Vector2Int> _touchedCells;

        [SerializeField] private PathMaker _pathMaker;

        public delegate void ColumnDelegate(Vector2Int[] positions, int gravityDirection);

        public event ColumnDelegate OnCreateColumn;

        private IEnumerator BlockingCoroutine()
        {
            while (true)
            {
                _touchedCells = new List<Vector2Int>();

                while (_pathMaker.OnBlocks)
                {
                    var position = transform.position;
                    var currentX = Mathf.RoundToInt(position.x);
                    var currentY = Mathf.RoundToInt(position.y);

                    if (currentX != _lastFrameX)
                    {
                        if (_touchedCells.Count > 0) 
                            OnCreateColumn?.Invoke(_touchedCells.ToArray(), _pathMaker.GravityDirection);
                        
                        _touchedCells = new() { new Vector2Int(currentX, currentY) };
                    }
                    else if (currentY != _lastFrameY)
                        _touchedCells.Add(new Vector2Int(currentX, currentY));
                    
                    _lastFrameX = currentX;
                    _lastFrameY = currentY;

                    yield return null;
                }

                yield return new WaitUntil(() => _pathMaker.OnBlocks);
            }
        }

        private void Start()
        {
            var position = transform.position;
            _touchedCells = new() { new Vector2Int((int)position.x, (int)position.y) };
            StartCoroutine(BlockingCoroutine());
        }
        
    }
}
