using UnityEngine;
using Game;

namespace Generator
{
    public class PadSpawner : MonoBehaviour
    {
        
        [SerializeField] private PathMaker _pathMaker;
        [SerializeField] private GameObject _pinkPad;
        [SerializeField] private GameObject _yellowPad;
        [SerializeField] private GameObject _redPad;
        [SerializeField] private GameObject _bluePad;

        private GameObject[] _pads;

        private void CreatePad(PadType type, Vector2 position, bool onBlocks, int gravityDirection)
        {
            if (onBlocks && type is PadType.Pink) return;

            var pad = Instantiate(_pads[(int)type]);
            pad.transform.position = position;

            if (gravityDirection >= 0) return;

            var scale = pad.transform.localScale;
            scale.y *= -1;
            pad.transform.localScale = scale;
        }

        private void Awake()
        {
            _pads = new[] { _pinkPad, _yellowPad, _redPad, _bluePad };
            _pathMaker.OnCreatePad += CreatePad;
        }
        
    }
}
