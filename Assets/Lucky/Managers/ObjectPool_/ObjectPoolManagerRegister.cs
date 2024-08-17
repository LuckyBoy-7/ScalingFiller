using UnityEngine;
using UnityEngine.Pool;

namespace Lucky.Managers.ObjectPool_
{
    public partial class ObjectPoolManager
    {
        protected override void Awake()
        {
            base.Awake();
            typeToRecycles[typeof(Square)] = new(() => Instantiate(Resources.Load<Square>("Square")));
        }
    }
}