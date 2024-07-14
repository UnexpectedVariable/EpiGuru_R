using System;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Collider))]
    internal class Coin : MonoBehaviour
    {
        public event Action OnPlayerEncountered = null;
        [SerializeField]
        private int _points = 0;
        public int Points => _points;

        private void Start()
        {
            OnPlayerEncountered += () =>
            {
                gameObject.SetActive(false);
            };
        }

        private void OnTriggerEnter(Collider other)
        {
            switch (other.tag)
            {
                case "ObjectDestroyer":
                    gameObject.SetActive(false);
                    break;
                case "Player":
                    Debug.Log("Coin triggered!");
                    OnPlayerEncountered?.Invoke();
                    break;
            }
        }
    }
}
