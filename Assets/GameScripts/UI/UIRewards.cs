namespace LOM
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;

    public class UIRewards : MonoBehaviour
    {

        public DataListPopulator rewardsDataList;
        public Button backButton;

        void Start()
        {
            backButton.onClick.AddListener(OnBackClicked);
        }

        public void DisplayRewards()
        {
            rewardsDataList.LoadData(GameManager.Instance, 0);
        }

        public void OnBackClicked()
        {
            SceneManager.LoadScene("MainMenu");
        }

    }
}