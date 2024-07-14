using System;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Collider))]
    internal class Obstacle : MonoBehaviour
    {
        public event Action OnPlayerEncountered = null;
        private void OnTriggerEnter(Collider other)
        {
            switch (other.tag)
            {
                case "ObjectDestroyer":
                    transform.parent.gameObject.SetActive(false);
                    break;
                case "Player":
                    Debug.Log("Obstacle triggered!");
                    OnPlayerEncountered?.Invoke();
                    break;
            }
        }
    }
}
