using UnityEngine;

namespace Generator
{
    public class StructureGenerator : MonoBehaviour
    {
        
        [SerializeField] private GameObject _blockPrefab;
        [SerializeField] private GameObject _spikePrefab;

        private void Awake() => PathMaker.NewColumn += (positions, gravityDirection, onBlocks) =>
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
            GameObject obj;
            if (onBlocks) obj = Instantiate(_blockPrefab);
            else obj = Instantiate(_spikePrefab);
            obj.transform.position = new Vector2(position.x, position.y - gravityDirection);
            var scale = obj.transform.localScale;
            obj.transform.localScale = new Vector3(scale.x, scale.y * gravityDirection, scale.z);
        };

    }
}
