using Assets.Scripts.Location;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.MainScene
{
    internal class Director : MonoBehaviour
    {
        [SerializeField]
        private Button _startButton = null;
        [SerializeField]
        private GeoRequester _geoRequester = null;

        private async void Start()
        {
            _startButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("GameloopScene");
            });

            var geoData = await _geoRequester.GetAsync();
            if (geoData.country != "Ukraine")
            {
                Application.OpenURL("https://uk.wikipedia.org/");
            }
            else
            {
                _startButton.interactable = true;
            }
        }
    }
}
