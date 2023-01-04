using UnityEngine;

namespace Generator
{
    public class StructureGenerator : MonoBehaviour
    {
        
        [SerializeField] private GameObject _blockPrefab;

        private void SpawnBlock(Vector2 position, int gravityDirection)
        {
            var block = Instantiate(_blockPrefab);
            block.transform.position = new Vector2(position.x, position.y - gravityDirection);
            var scale = block.transform.localScale;
            block.transform.localScale = new Vector3(scale.x, scale.y * gravityDirection, scale.z);
        }

        private void Awake() 
        {
            PathMaker.NewColumn += (positions, gravityDirection) =>
            {
                var position = positions[0];
                if (gravityDirection > 0)
                {
                    foreach (var givenPosition in positions)
                        if (givenPosition.y < position.y) position = givenPosition;
                }
                else
                {
                    foreach (var givenPosition in positions)
                        if (givenPosition.y > position.y) position = givenPosition;
                }
                SpawnBlock(position, gravityDirection);
            };
            PathMaker.BlockHere += (position, gravityDirection) =>
                SpawnBlock(position, gravityDirection);
        }

    }
}
