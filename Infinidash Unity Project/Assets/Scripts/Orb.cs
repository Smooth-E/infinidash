using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Orb : MonoBehaviour
{

    private Collider2D _collider;

    private void Awake() => _collider = GetComponent<Collider2D>();    

    public void Deactivate() => _collider.enabled = false;

}
