using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Unity.Multiplayer.Widgets
{
    [RequireComponent(typeof(Button))]
    internal class SceneSwitch : MonoBehaviour
    {
        [Tooltip("The name of the scene to switch to.")]
        [SerializeField] private string sceneName;

        [Tooltip("Event invoked when the scene switch is triggered.")]
        public UnityEvent OnSceneSwitched = new();

        private Button m_Button;

        void Start()
        {
            m_Button = GetComponent<Button>();
            m_Button.onClick.AddListener(SwitchScene);
            SetButtonActive();
        }

        void SetButtonActive()
        {
            m_Button.interactable = !string.IsNullOrEmpty(sceneName);
        }

        void SwitchScene()
        {
            if (!string.IsNullOrEmpty(sceneName))
            {
                SceneManager.LoadScene(sceneName);
                OnSceneSwitched.Invoke();
            }
            else
            {
                Debug.LogWarning("Scene name is not set.");
            }
        }
    }
}