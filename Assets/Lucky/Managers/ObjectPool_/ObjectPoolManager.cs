using System;
using System.Collections.Generic;
using System.Reflection;
using Lucky.Managers;
using UnityEngine;
using UnityEngine.Pool;

namespace Lucky.Managers.ObjectPool_
{
    public partial class ObjectPoolManager : Singleton<ObjectPoolManager>
    {
        private Dictionary<Type, ObjectPool<IRecycle>> typeToRecycles = new();

        public T Get<T>() where T : IRecycle
        {
            IRecycle retval = typeToRecycles[typeof(T)].Get();
            retval.OnGet();
            return (T)retval;
        }

        public void Release<T>(T value) where T : IRecycle
        {
            if (value == null)
                return;
            typeToRecycles[typeof(T)].Release(value);
            value.OnRelease();
        }
    }
}