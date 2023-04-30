using UnityEngine;
using Game;

[RequireComponent(typeof(Collider2D))]
public class Pad : MonoBehaviour
{

    [SerializeField] private PadType _type;
    public PadType Type => _type;

    public void Deactivate() => GetComponent<Collider2D>();
    
}
