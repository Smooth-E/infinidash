using System;
using Random = UnityEngine.Random;

namespace Generator
{
    public static class SeedGiver
    {
        
        private static int _seed = 0;

        public static int Seed
        {
            get
            {
                while (_seed == 0) _seed = Random.Range(Int32.MinValue, Int32.MaxValue);
                return _seed;
            }
        }

    }
}
