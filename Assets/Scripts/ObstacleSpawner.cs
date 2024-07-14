using Assets.Scripts.Util;
using System;
using UnityEngine;

namespace Assets.Scripts
{
    internal class ObstacleSpawner : MonoBehaviour, ISpawner, IPauseable, IResumable
    {
        [SerializeField]
        private Wall _wall = null;
        [SerializeField]
        private GameObject _poolObject = null;

        [SerializeField]
        private int _maximumGapsPerWave = 0;

        private System.Random _rng = null;
        private int _positionCount = 0;
        private ObjectPool<Wall> _pool = null;

        private bool _isPaused = false;

        public Action OnObstacleTriggered = null;

        public System.Random RNG
        {
            set => _rng = value;
        }
        public int PositionCount
        {
            set
            {
                Debug.Log($"{gameObject.name} PositionCount assigned value: {value}");
                _positionCount = value;
            }
        }

        private void Awake()
        {
            if (_maximumGapsPerWave <= 0) Debug.LogWarning($"Maximum gap count per wave is lower or equals zero");
            _pool = new ObjectPool<Wall>();
        }

        [ContextMenu("Spawn")]
        public GameObject Spawn()
        {
            if (_rng == null) return null;
            if (_isPaused) return null;
            return SpawnWall().gameObject;
        }

        private Wall SpawnWall()
        {
            var gapsPositions = GenerateGapsPositions();

            Wall wall = _pool.Get();
            wall?.Enable();

            if (wall == null)
            {
                wall = Instantiate(_wall, _poolObject.transform);
                wall.SubscribeObstacles(OnObstacleTriggered);
                _pool.Add(wall);
            }

            wall.transform.position = transform.position;
            wall.transform.rotation = transform.rotation;
            wall.Disable(gapsPositions);
            wall.gameObject.SetActive(true);

            return wall;
        }

        private int[] GenerateGapsPositions()
        {
            int[] positions = new int[_maximumGapsPerWave];

            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = _rng.Next(_positionCount);
            }

            return positions;
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
