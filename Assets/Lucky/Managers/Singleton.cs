using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Lucky.Managers
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                    if (instance)
                        DontDestroyOnLoad(instance);
                }

                return instance;
            }
            set => instance = value;
        }

        protected virtual void Awake()
        {
            if (instance == this)
                return;
            if (instance == null)
            {
                instance = (T)this;
                DontDestroyOnLoad(instance.gameObject);
            }
            else
                Destroy(gameObject);
        }
    }
}