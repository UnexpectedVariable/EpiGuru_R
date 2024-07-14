using AppsFlyerSDK;
using OneSignalSDK;
using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    internal class Director : MonoBehaviour
    {
        [Header("Actors")]
        [SerializeField]
        private ObstacleSpawner _obstacleSpawner = null;
        [SerializeField]
        private CoinSpawner _coinSpawner = null;
        [SerializeField]
        private ObjectMovementManager _movementManager = null;
        [SerializeField]
        private Player _player = null;

        [Header("Geometry")]
        [SerializeField]
        private GameObject _plane = null;
        [SerializeField]
        private GameObject _obstacle = null;

        [Header("Random")]
        [SerializeField]
        private int _seed = 0;
        [SerializeField]
        private bool _useSeed = false;
        [SerializeField]
        private System.Random _rng = null;

        [Header("Game")]
        [SerializeField]
        private float _spawnInterval = 0f;
        [SerializeField]
        private float _moveInterval = 0f;
        [SerializeField]
        private bool _isWorking = false;
        [SerializeField]
        private bool _isRunning = true;

        [Header("UI")]
        [SerializeField]
        private TextMeshProUGUI _pointsLabel = null;
        [SerializeField]
        private TextMeshProUGUI _finalPointsLabel = null;
        [SerializeField]
        private Button _pauseButton = null;
        [SerializeField]
        private Button _exitButton = null;
        [SerializeField]
        private Button _closeButton = null;

        [SerializeField]
        private GameObject _gameScreen = null;
        [SerializeField]
        private GameObject _endgameScreen = null;

        private void Awake()
        {
            AppsFlyer.initSDK("ytPuQc6oHMvGHLh83FVpdd", null);
            //OneSignal.Initialize("635c5f12-bb1c-4e8a-92a5-65636c604328");
        }

        private void Start()
        {
            if (_useSeed) _rng = new System.Random(_seed);
            else _rng = new System.Random();

            InitializeObstacleSpawner();
            InitializeCoinSpawner();
            InitializePlayer();
            InitializeUI();

            Begin();
        }

        private void InitializeUI()
        {
            _pauseButton.onClick.AddListener(() =>
            {
                if (_isRunning)
                {
                    Pause();
                }
                else
                {
                    Resume();
                }
            });
            _exitButton.onClick.AddListener(() =>
            {
                Pause();
                SceneManager.LoadScene("MainScene");
            });
            _closeButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("MainScene");
            });
        }

        private void Pause()
        {
            _coinSpawner.Pause();
            _obstacleSpawner.Pause();
            _movementManager.Pause();
            _player.Pause();
            _isRunning = false;
        }

        private void Resume()
        {
            _coinSpawner.Resume();
            _obstacleSpawner.Resume();
            _movementManager.Resume();
            _player.Resume();
            _isRunning = true;
        }

        private void InitializePlayer()
        {
            _player.MaximumX = _plane.transform.lossyScale.x * 5 - _player.transform.lossyScale.x;
        }

        private void InitializeCoinSpawner()
        {
            _coinSpawner.RNG = _rng;
            _coinSpawner.MinimalX = _plane.transform.lossyScale.x * -5;
            _coinSpawner.MaximumX = _plane.transform.lossyScale.x * 5;
            _coinSpawner.PointsLabel = _pointsLabel;
        }

        private void InitializeObstacleSpawner()
        {
            _obstacleSpawner.RNG = _rng;
            _obstacleSpawner.PositionCount = (int)(_plane.transform.lossyScale.x * 10 / _obstacle.transform.lossyScale.x);
            _obstacleSpawner.OnObstacleTriggered += () =>
            {
                Pause();
                _finalPointsLabel.text = $"Final points: {_pointsLabel.text}";
                _gameScreen.SetActive(false);
                _endgameScreen.SetActive(true);
            };
        }

        private void Spawn(ISpawner spawner)
        {
            var gObject = spawner?.Spawn();
            if (gObject == null) return;
            _movementManager?.Add(gObject);
        }

        [ContextMenu("Move")]
        public void Move()
        {
            _movementManager?.Move();
        }

        private async void SpawnLoopAsync(ISpawner spawner)
        {
            while (_isWorking && spawner != null)
            {
                Spawn(spawner);
                await Task.Delay(TimeSpan.FromSeconds(_spawnInterval));
            }
        }

        private async void MoveLoopAsync()
        {
            while (_isWorking && _movementManager != null)
            {
                Move();
                await Task.Delay(TimeSpan.FromSeconds(_moveInterval));
            }
        }

        [ContextMenu("Begin")]
        public async void Begin()
        {
            _isWorking = true;
            SpawnLoopAsync(_obstacleSpawner);
            //MoveLoopAsync();
            await Task.Delay(TimeSpan.FromSeconds(_spawnInterval * 0.5));
            SpawnLoopAsync(_coinSpawner);
        }

        private void Update()
        {
            Debug.Log($"Framerate: {(int)(1.0f / Time.deltaTime)}");
        }
    }
}
