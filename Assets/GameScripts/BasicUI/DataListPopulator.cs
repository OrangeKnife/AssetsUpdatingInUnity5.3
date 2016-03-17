using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace LOM
{
    public struct MetaDataForWidget
    {
        public string dataStr1,dataStr2;
        public int dataInt1,dataInt2;
        public float dataFloat1,dataFloat2;
        public Image dataImg;
    };

    //attach this script to a gameObject considered as a container
    public class DataListPopulator : MonoBehaviour
    {
        public GameObject widgetTemplate;
        public float widgetWidth = 0, widgetHeight = 0; 
        public int paddingX = 10, paddingY = 10;
        public int maxNumX = 3, maxNumY = 3;//means x*y per page
        public int rowSpace = 10, columnSpace = 10;
        public bool populateXfirst = true;//otherwise populateY

        private int currentIdx;
        void Start()
        {
            //test
            //LoadData(GameManager.Instance, 0);
        }

        public void LoadData(IDataListDataProvider dp, int dataCategory)
        {
            currentIdx = 0;
            if (widgetTemplate != null)
            {
                var mdis = dp.GetData(dataCategory);
                foreach (var mdi in mdis)
                {
                    var globalScale = gameObject.transform.lossyScale;
                    GameObject oneWidgetGO = GameObject.Instantiate(widgetTemplate);
                    widgetWidth = widgetWidth == 0 ? ((RectTransform)oneWidgetGO.transform).sizeDelta.x : widgetWidth;
                    widgetHeight = widgetHeight == 0 ? ((RectTransform)oneWidgetGO.transform).sizeDelta.y : widgetHeight;

                    var offset = new Vector3(paddingX + (currentIdx % maxNumX) * (widgetWidth + columnSpace), -paddingY - (currentIdx / maxNumX) * (widgetHeight + rowSpace), 0);
                    offset = new Vector3(globalScale.x * offset.x, globalScale.y * offset.y, globalScale.z * offset.z);
                    oneWidgetGO.transform.SetParent(gameObject.transform, false);
                    oneWidgetGO.transform.position = gameObject.transform.position + offset;


                    var widget = oneWidgetGO.GetComponent<IDataListWidget>() as IDataListWidget;
                    if(widget != null)
                        widget.SetData(mdi);

                    currentIdx++;
                }
            }
            var rect = (RectTransform)gameObject.transform;
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, paddingY * 2 + ( currentIdx / maxNumX ) * widgetHeight  + Mathf.Max(0, currentIdx / maxNumX - 1) * rowSpace);
          
        }
    }
}