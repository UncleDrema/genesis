using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Geneses.ArtLife.UI
{
    public class MenuUI : MonoBehaviour
    {
        [SerializeField] Button StartButton;
        [SerializeField] Button ExitButton;

        void Start()
        {
            StartButton.onClick.AddListener(OnStartClick);
            ExitButton.onClick.AddListener(OnExitClick);
        }

        void OnDestroy()
        {
            StartButton.onClick.RemoveListener(OnStartClick);
            ExitButton.onClick.RemoveListener(OnExitClick);
        }

        private void OnStartClick()
        {
            Debug.Log("Start Button Clicked");

            StartGame();
        }

        private void OnExitClick()
        {
            Debug.Log("Exit Button Clicked");

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }

        private void StartGame()
        {
            // TODO: �������� �� �������� ����
            Hide();
        }

        public void Hide()
        {
            this.GetComponent<Canvas>().enabled = false;
        }

        public void Show()
        { 
            this.GetComponent<Canvas>().enabled = true;
        }
    }
}
