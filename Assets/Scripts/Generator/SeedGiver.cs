using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Generator
{
    public class SeedGiver : MonoBehaviour
    {

        public static int Seed { private set; get; } = 0;

        private void Awake()
        {
            Seed = 0;
            while (Seed == 0) Seed = Random.Range(Int32.MinValue, Int32.MaxValue);
        }

    }
}
