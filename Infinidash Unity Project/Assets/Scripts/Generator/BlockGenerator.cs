using UnityEngine;

namespace Generator
{
    public class BlockGenerator : MonoBehaviour
    {

        [SerializeField] private PathMaker _pathMaker;
        [SerializeField] private ColumnMaker _columnMaker;
        [SerializeField] private GameObject _blockPrefab;
        [SerializeField] private GameObject _extendedBlockPrefab;

        private void SpawnBlock(Vector2 position, int gravityDirection, GameObject prefab)
        {
            var block = Instantiate(prefab);
            block.transform.position = new Vector2(position.x, position.y - gravityDirection);
            var scale = block.transform.localScale;
            block.transform.localScale = new Vector3(scale.x, scale.y * gravityDirection, scale.z);
        }

        private void CreateNewColumn(Vector2Int[] positions, int gravityDirection)
        {
            var position = positions[0];
            if (gravityDirection > 0)
            {
                foreach (var givenPosition in positions)
                    if (givenPosition.y < position.y)
                        position = givenPosition;
            }
            else
            {
                foreach (var givenPosition in positions)
                    if (givenPosition.y > position.y)
                        position = givenPosition;
            }
            SpawnBlock(position, gravityDirection, _blockPrefab);
        }

        private void Awake() 
        {
            _columnMaker.OnCreateColumn += CreateNewColumn;
            _pathMaker.OnCreateBlockUnderPad += (position, gravityDirection, wasSliding) =>
                SpawnBlock(position, gravityDirection, wasSliding ? _extendedBlockPrefab : _blockPrefab);
        }

    }
}
