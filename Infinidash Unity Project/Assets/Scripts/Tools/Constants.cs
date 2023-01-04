using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    [CreateAssetMenu(menuName = "Single Instance Scriptable Objects/Constants", fileName = "Constants", order = 1)]
    public class Constants : SingletonScriptableObject<Constants>
    {

        [SerializeField] private float _gravityScale = 4;
        [SerializeField] private float _jumpForce = 8.5f;
        [SerializeField] private float _moveVelocity = 7f;
        [SerializeField] private float _pinkOrbModifier = 1;
        [SerializeField] private float _yellowOrbModifier = 1.2f * 1.5f;
        [SerializeField] private float _redOrbModifier = 1.5f * 1.5f;
        [SerializeField] private float _blueOrbModifier = 0;
        [SerializeField] private float _greenOrbModifier = 1.5f * 1.5f;

        public static float[] JumpModifiers => new float[]
        {
            Instance._pinkOrbModifier,
            Instance._yellowOrbModifier,
            Instance._redOrbModifier,
            Instance._blueOrbModifier,
            Instance._greenOrbModifier
        };

        public static Dictionary<string, float> JumpModifiersDictionary => new()
        {
            { "Pink Orb", Instance._pinkOrbModifier },
            { "Yellow Orb", Instance._yellowOrbModifier },
            { "Red Orb", Instance._redOrbModifier },
            { "Blue Orb", Instance._blueOrbModifier },
            { "Green Orb", Instance._greenOrbModifier }
        };

        public static float GravityScale => Instance._gravityScale;
        public static float JumpForce => Instance._jumpForce;
        public static float MoveVelocity => Instance._moveVelocity;

    }
}