using UnityEngine;
using Game;

[RequireComponent(typeof(Collider2D))]
public class Orb : MonoBehaviour
{

    [SerializeField] private OrbType _type;
    public OrbType Type => _type;

    private Collider2D _collider;

    private void Awake() => _collider = GetComponent<Collider2D>();    

    public void Deactivate() => _collider.enabled = false;

}
