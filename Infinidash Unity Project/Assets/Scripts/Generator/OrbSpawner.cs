using UnityEngine;

namespace Generator
{
    public class OrbSpawner : MonoBehaviour
    {

        [SerializeField] private GameObject _pinkOrb;
        [SerializeField] private GameObject _yellowOrb;
        [SerializeField] private GameObject _redOrb;
        [SerializeField] private GameObject _blueOrb;
        [SerializeField] private GameObject _greenOrb;

        private void Awake()
        {
            var orbs = new[] { _pinkOrb, _yellowOrb, _redOrb, _blueOrb, _greenOrb };
            PathMaker.OrbHere += (type, position) =>
            {
                var orb = Instantiate(orbs[(int)type]);
                orb.transform.position = position;
            };
        }
        
    }
}
