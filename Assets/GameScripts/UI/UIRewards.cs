using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace LOM
{
    public class UIRewards : MonoBehaviour {

        public DataListPopulator rewardsDataList;
        public Button BackButton;

        void Start() {
            BackButton.onClick.AddListener(OnBackClicked);
        }

        // Update is called once per frame
        void Update() {

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