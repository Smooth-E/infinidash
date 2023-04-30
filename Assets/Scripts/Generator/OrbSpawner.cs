using UnityEngine;
using Game;

namespace Generator
{
    public class OrbSpawner : MonoBehaviour
    {

        [SerializeField] private PathMaker _pathMaker;
        [SerializeField] private GameObject _pinkOrb;
        [SerializeField] private GameObject _yellowOrb;
        [SerializeField] private GameObject _redOrb;
        [SerializeField] private GameObject _blueOrb;
        [SerializeField] private GameObject _greenOrb;

        private GameObject[] _orbs;

        private void CreateOrb(OrbType type, Vector2 position)
        {
            var orb = Instantiate(_orbs[(int)type]);
            orb.transform.position = position;
        }

        private void Awake()
        {
            _orbs = new[] { _pinkOrb, _yellowOrb, _redOrb, _blueOrb, _greenOrb };
            _pathMaker.OnCreateOrb += CreateOrb;
        }

        private void OnDestroy() => _pathMaker.OnCreateOrb -= CreateOrb;
        
    }
}
