using System;
using UnityEngine;

namespace Assets.Scripts
{
    internal class Wall : MonoBehaviour
    {
        [SerializeField]
        private Obstacle[] _pieces = null;

        public void Disable(int[] indexes)
        {
            foreach (var index in indexes)
            {
                _pieces[index].gameObject.SetActive(false);
            }
        }

        public void Enable()
        {
            foreach (var piece in _pieces)
            {
                piece.gameObject.SetActive(true);
            }
        }

        public void SubscribeObstacles(Action action)
        {
            foreach (var piece in _pieces)
            {
                piece.OnPlayerEncountered += action;
            }
        }
    }
}
