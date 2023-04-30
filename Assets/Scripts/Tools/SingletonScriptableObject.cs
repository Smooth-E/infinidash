using System;
using UnityEngine;

namespace Tools
{
    public abstract class SingletonScriptableObject<T> : ScriptableObject where T : SingletonScriptableObject<T>
    {

        private static T _instance;

        public static T Instance
        {
            get 
            {
                if (_instance == null)
                {
                    var assets = Resources.LoadAll<T>("");
                    if (assets == null || assets.Length < 1)
                        throw new ApplicationException("Cannot find any SingletonScriptableObject assets!");
                    else if (assets.Length > 1)
                        throw new ApplicationException("More than one SingletonScriptableObject asset created for SingletonSO!");
                    _instance = assets[0];
                }
                return _instance;                 
            }
        }
        
    }
}
