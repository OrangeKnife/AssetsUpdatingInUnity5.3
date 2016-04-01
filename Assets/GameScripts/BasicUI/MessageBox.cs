namespace LOM
{
    using UnityEngine.Events;
    using UnityEngine.UI;
    using UnityEngine;

    public enum MessageBoxType
    {
        YES,
        YESNO,
        YESNOCANCEL,
        NOBUTTON
    }

    public struct MessageInfo
    {
        public MessageBoxType type;
        public string YesString, NoString, CancelString, TitleString, ContentString;
        public UnityAction YesCallback, NoCallback, CancelCallback;
    }

    public class MessageBox : MonoBehaviour
    {
        public Text TitleText, ContentText;
        public Text YesText, NoText, CancelText;
        public Button YesButton, NoButton, CancelButton;

        public void DestroyBox()
        {
            //Debug.Log("DestroyBox called");
            Destroy(gameObject);
        }
        public void Initialize(MessageInfo mi)
        {
            TitleText.text = mi.TitleString;
            ContentText.text = mi.ContentString;
            YesText.text = mi.YesString;
            NoText.text = mi.NoString;
            CancelText.text = mi.CancelString;


            switch (mi.type)
            {
                case MessageBoxType.YES:
                    YesButton.gameObject.SetActive(true);
                    if (mi.YesCallback != null)
                        YesButton.onClick.AddListener(mi.YesCallback);
                    YesButton.onClick.AddListener(DestroyBox);
                    break;
                case MessageBoxType.YESNO:
                    YesButton.gameObject.SetActive(true);
                    NoButton.gameObject.SetActive(true);
                    if (mi.YesCallback != null)
                        YesButton.onClick.AddListener(mi.YesCallback);
                    YesButton.onClick.AddListener(DestroyBox);
                    if (mi.NoCallback != null)
                        NoButton.onClick.AddListener(mi.NoCallback);
                    NoButton.onClick.AddListener(DestroyBox);
                    break;

                case MessageBoxType.YESNOCANCEL:
                    YesButton.gameObject.SetActive(true);
                    NoButton.gameObject.SetActive(true);
                    CancelButton.gameObject.SetActive(true);
                    if (mi.YesCallback != null)
                        YesButton.onClick.AddListener(mi.YesCallback);
                    YesButton.onClick.AddListener(DestroyBox);
                    if (mi.NoCallback != null)
                        NoButton.onClick.AddListener(mi.NoCallback);
                    NoButton.onClick.AddListener(DestroyBox);
                    if (mi.CancelCallback != null)
                        CancelButton.onClick.AddListener(mi.CancelCallback);
                    CancelButton.onClick.AddListener(DestroyBox);
                    break;
                default:
                    YesButton.gameObject.SetActive(true);
                    YesButton.onClick.AddListener(mi.YesCallback);
                    YesButton.onClick.AddListener(DestroyBox);
                    break;

            }
        }
    }
}
