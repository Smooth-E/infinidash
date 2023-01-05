using UnityEngine;

namespace Generator
{
    [RequireComponent(typeof(PathMaker))]
    [RequireComponent(typeof(Collider2D))]
    public class GhostEnabler : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                GetComponent<PathMaker>().enabled = true;
                GetComponent<Collider2D>().enabled = false;
            }
        }
    }
}
