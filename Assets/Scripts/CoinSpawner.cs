using Assets.Scripts.Util;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    internal class CoinSpawner : MonoBehaviour, ISpawner, IPauseable, IResumable
    {
        [SerializeField]
        private Coin _coin = null;
        [SerializeField]
        private GameObject _poolObject = null;

        private System.Random _rng = null;

        public float MinimalX = 0;
        public float MaximumX = 0;

        private ObjectPool<Coin> _pool = null;

        public TextMeshProUGUI PointsLabel = null;

        private bool _isPaused = false;

        public System.Random RNG
        {
            set => _rng = value;
        }

        private void Awake()
        {
            _pool = new ObjectPool<Coin>();
        }

        [ContextMenu("Spawn")]
        public GameObject Spawn()
        {
            if (_rng == null) return null;
            if (_isPaused) return null;
            return SpawnCoin().gameObject;
        }

        private GameObject SpawnCoin()
        {
            var x = MaximumX * (_rng.NextDouble() * 2 - 1);

            Vector3 position = transform.position;
            position.x = (float)x;
            position.y = _coin.gameObject.transform.lossyScale.x;

            Coin coin = _pool.Get();

            if (coin == null)
            {
                coin ??= Instantiate(_coin, _poolObject.transform);
                coin.OnPlayerEncountered += () =>
                {
                    var points = int.Parse(PointsLabel.text);
                    points += coin.Points;
                    PointsLabel.text = $"{points}";
                };
                _pool.Add(coin);
            }
            coin.transform.position = position;
            coin.gameObject.SetActive(true);

            return coin.gameObject;
        }

        public void Pause()
        {
            _isPaused = true;
        }

        public void Resume()
        {
            _isPaused = false;
        }
    }
}
