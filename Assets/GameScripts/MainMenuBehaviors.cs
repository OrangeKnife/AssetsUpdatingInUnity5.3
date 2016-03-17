using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace LOM
{
    // Some of the functionality of the Main Menu.
    public class MainMenuBehaviors : MonoBehaviour
    {
        void Start()
        {
            
        }

        void OnDestroy()
        {
            CleanUpEventCallbacks();
        }

        public void NewGameButtonBehavior()
        {
            GlobalBehaviors.instance.CreateAFullScreenOverlay("Creating a game for you now...", "NewMatchCreated");

            EventManager.RegisterEvent("NewMatchCreated", LoadToMatchScene, true);
            
        }

        private void LoadToMatchScene(EventObj ob)
        {
        }

        private void CleanUpEventCallbacks()
        {
            EventManager.RemoveEvent("NewMatchCreated", LoadToMatchScene);
        }

        public void SignInButtonBehavior()
        {
            GameManager.Instance.LogInToFacebook();
        }

      

        public void RefreshButtonBehavior()
        {
        }

        public void SettingsButtonBehavior()
        {
            SceneManager.LoadScene("SettingsMenu");
        }

        public void FBLoginCallback(EventObj eo)
        {
            
        }


    }
}
