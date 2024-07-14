using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class ObjectPool<T> where T : MonoBehaviour
    {
        [SerializeField]
        private List<T> _pool = new List<T>();

        public T Get()
        {
            foreach (T obj in _pool)
            {
                if (obj.gameObject.activeInHierarchy is false) return obj;
            }
            return null;
        }

        public void Add(T obj)
        {
            _pool.Add(obj);
        }
    }
}
