using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

namespace LOM
{
    public class GlobalBehaviors : MonoBehaviour
    {
        


        public GameObject FullScreenOverlayTemplate, MessageBoxTemplate;

        private static GlobalBehaviors _GlobalBehaviors;

        public static GlobalBehaviors instance
        {
            get
            {
                if (!_GlobalBehaviors)
                {
                    _GlobalBehaviors = FindObjectOfType(typeof(GlobalBehaviors)) as GlobalBehaviors;

                    if (!_GlobalBehaviors)
                    {
                        Debug.LogError("There needs to be one active GlobalBehaviors script on a GameObject in your scene.");
                    }
                }

                return _GlobalBehaviors;
            }
        }
        private GlobalBehaviors()
        {}
        private Queue<MessageInfo> messageBoxInfoQueue;
        private GameObject currentMessageBoxGO;

        void Start()
        {
            messageBoxInfoQueue = new Queue<MessageInfo>();
        }

        // Update is called once per frame
        void Update()
        {
            if(messageBoxInfoQueue != null && messageBoxInfoQueue.Count > 0 && currentMessageBoxGO == null)
            {
                currentMessageBoxGO = Instantiate(MessageBoxTemplate);
                currentMessageBoxGO.GetComponent<MessageBox>().Initialize(messageBoxInfoQueue.Dequeue());
            }
        }

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void CreateAFullScreenOverlay(string displayTextOnOverlay, string eventNameToRemoveOverlay)
        {
            Instantiate(FullScreenOverlayTemplate).GetComponent<FullScreenOverlay>().Initialize(displayTextOnOverlay, eventNameToRemoveOverlay);
        }

        public void CreateAFullScreenOverlayWithDelayKill(string displayTextOnOverlay, float delayToKill)
        {
            Instantiate(FullScreenOverlayTemplate).GetComponent<FullScreenOverlay>().Initialize(displayTextOnOverlay, delayToKill);
        }

        public void AddAMessageBox(string title, string content, MessageBoxType tp, string YesStr, string NoStr, string Cancel, UnityAction YesCallback, UnityAction NoCallback, UnityAction CancelCallback)
        {
            MessageInfo mi;
            mi.type = tp;
            mi.TitleString = title; mi.ContentString = content;
            mi.YesString = YesStr; mi.NoString = NoStr; mi.CancelString = Cancel;
            mi.YesCallback = YesCallback; mi.NoCallback = NoCallback; mi.CancelCallback= CancelCallback;
            if (messageBoxInfoQueue != null)
                messageBoxInfoQueue.Enqueue(mi);
            
        }
    }
}