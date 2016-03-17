using UnityEngine;
using UnityEngine.UI;
using System.Collections;
namespace LOM
{
    public class ButtonItemWidget : MonoBehaviour, IDataListWidget
    {
        public Text ItemNameText, ItemCountText;
        public Image ItemImg;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetData(MetaDataForWidget mdi)
        {
            ItemNameText.text = mdi.dataStr1;
            ItemCountText.text = mdi.dataInt1.ToString();
            if(mdi.dataImg != null)
                ItemImg = mdi.dataImg;
        }

    }
}
