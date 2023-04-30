using System.Collections;
using UnityEngine;

namespace Tools
{
    [RequireComponent(typeof(LineRenderer))]
    public class TrajectoryPlotter : MonoBehaviour
    {

        private LineRenderer _lineRenderer;
        [SerializeField] private float _plotterInterval;

        private IEnumerator DrawingCoroutine()
        {
            while (true)
            {
                var positionCount = _lineRenderer.positionCount;
                _lineRenderer.positionCount = positionCount + 1;
                _lineRenderer.SetPosition(positionCount, transform.position);
                yield return new WaitForSeconds(_plotterInterval);
            }
        }

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.positionCount = 0;
            StartCoroutine(DrawingCoroutine());
        }

    }
}
