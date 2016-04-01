namespace LOM
{
    using UnityEngine;

    // Some of the functionality of the Main Menu.
    public class MainMenuBehaviors : MonoBehaviour
    {
        void OnDestroy()
        {
            CleanUpEventCallbacks();
        }

        public void NewGameButtonBehavior()
        {
            GlobalBehaviors.Instance.CreateAFullScreenOverlay("Creating a game for you now...", "NewMatchCreated");

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
    }
}
