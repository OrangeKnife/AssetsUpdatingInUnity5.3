using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
namespace LOM
{
    public class MyDebugger : MonoBehaviour
    {


        public Text DebugText;
        public InputField CommandBox;

        private List<string> lastCommands = new List<string>();
        private int lastCommandIdx = -1;

        public static void Log(string message)
        {
            EventManager.TriggerEvent(new EventObj("DebugAddText", message));
        }

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        // Use this for initialization
        void Start()
        {
            Application.logMessageReceived += DebugLogFromUnity;
            EventManager.RegisterEvent("DebugAddText", DebugAddText);
            DebugText.resizeTextMaxSize = 1;
        }


        public void ToggleCommandBox()
        {
            if (CommandBox.isActiveAndEnabled)
            {
                CommandBox.text = "";
                CommandBox.DeactivateInputField();
                CommandBox.gameObject.SetActive(false);
            }
            else
            {
                CommandBox.gameObject.SetActive(true);
                CommandBox.ActivateInputField();
            }
        }
        private void ExecuteCommand()
        {
            if (CommandBox.isActiveAndEnabled)
            {
                CommandEntered(CommandBox.text);
                CommandBox.text = "";
                CommandBox.ActivateInputField();
            }
        }
        void Update()
        {
            //for input field only
            //TODO make it a separate file
            if (Input.GetKeyUp(KeyCode.BackQuote))
            {
                ToggleCommandBox();
            }

            if (Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter))
            {
                ExecuteCommand();
            }
            if (Input.GetKeyUp(KeyCode.UpArrow) && CommandBox.isActiveAndEnabled)
            {
                if (lastCommandIdx >= 0 && lastCommandIdx < lastCommands.Count)
                {
                    CommandBox.text = lastCommands[lastCommandIdx];
                    lastCommandIdx = lastCommandIdx - 1 < 0 ? lastCommands.Count - 1 : lastCommandIdx - 1;
                }
            }

        }

        void OnDestroy()
        {
            Application.logMessageReceived -= DebugLogFromUnity;
            EventManager.RemoveEvent("DebugAddText", DebugAddText);
        }

        private void DebugLogFromUnity(string logString, string stackTrace, LogType type)
        {
            DebugText.text +=  logString + "\n";
        }

        public void DebugAddText(EventObj eo)
        {
            DebugText.text += eo.paramString + "\n";
        }

        public void WipePCData()
        {
#if UNITY_EDITOR
            PlayerPrefs.DeleteAll();
            
            DebugText.text +=  "clean PlayerPrefs, clean cache: " + (Caching.CleanCache()?"true":"false");
            

#endif
        }

        public void QuickTest()
        {
            
            GlobalBehaviors.instance.AddAMessageBox("This is test Title", "Hello, I am calling you from a debugger: yes"
                , MessageBoxType.YES, "Yes", "No!", "Forget it", ()=> { Debug.Log("here we are yes"); }, null, null);

            GlobalBehaviors.instance.AddAMessageBox("This is test Title", "Hello, I am calling you from a debugger:no"
                , MessageBoxType.YESNO, "Yes", "No!", "Forget it", () => { Debug.Log("here we are yes"); }, () => { Debug.Log("here we are no"); }, null);


            GlobalBehaviors.instance.AddAMessageBox("This is test Title", "Hello, I am calling you from a debugger:cancel"
                            , MessageBoxType.YESNOCANCEL, "Yes", "No!", "Forget it", () => { Debug.Log("here we are yes"); }, () => { Debug.Log("here we are no"); }, () => { Debug.Log("here we are cancel"); });
                            
        }

        private void CommandEntered(string cmd)
        {
            if (!string.IsNullOrEmpty(cmd))
            {
                if(lastCommands.Count == 0 || cmd != lastCommands[lastCommands.Count-1])
                    lastCommands.Add(CommandBox.text);
                lastCommandIdx = lastCommands.Count - 1;

                var cmdidx = cmd.IndexOf('(');
                if (cmdidx < 0 ||cmd.LastIndexOf(')') != cmd.Length -1)
                {
                    Debug.Log("Need '(' and ')' to call functions");
                    return;
                }
                              
                var funcName = cmd.Substring(0, cmdidx).ToLower();
                cmd = cmd.Substring(cmdidx);
                cmd = cmd.TrimStart('(');
                cmd = cmd.TrimEnd(')');
                string[] strparams = cmd.Split(',');
                Command.Instance.Exe(funcName, strparams);
            }
           
        }
    }

}