using UnityEngine;

namespace Generator
{
    public class PadSpawner : MonoBehaviour
    {
        
        [SerializeField] private GameObject _pinkPad;
        [SerializeField] private GameObject _yellowPad;
        [SerializeField] private GameObject _redPad;
        [SerializeField] private GameObject _bluePad;

        private void Awake()
        {
            var pads = new[] { _pinkPad, _yellowPad, _redPad, _bluePad };
            LevelMaker.PadHere += (type, position, onBlocks, gravityDirection) =>
            {
                if (onBlocks && type is PadType.Pink) return;
                var pad = Instantiate(pads[(int)type]);
                pad.transform.position = position;
                if (gravityDirection < 0)
                {
                    var scale = pad.transform.localScale;
                    scale.y *= -1;
                    pad.transform.localScale = scale;
                }
            };
        }
        
    }
}
