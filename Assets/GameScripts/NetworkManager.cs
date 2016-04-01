namespace LOM
{
    using LitJson;
    using UnityEngine;
    using System.Collections;
    using System.Text;
    using System.Collections.Generic;
    using UnityEngine.SceneManagement;


    public class NetworkManager : MonoBehaviour
    {
        private const string clientVerPropertyName = "clientVer";
        private static NetworkManager networkmanager;
        #region Callbacks
        public delegate void UpdateGameCallback(string error, AssetBundleUpdateInfo updateInfo);
        #endregion
        public static NetworkManager Instance
        {
            get
            {
                if (!networkmanager)
                {
                    networkmanager = FindObjectOfType(typeof(NetworkManager)) as NetworkManager;

                    if (!networkmanager)
                    {
                        Debug.LogError("There needs to be one active NetworkManager script on a GameObject in your scene.");
                    }
                }

                return networkmanager;
            }
        }

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
            SceneManager.LoadScene("UpdateGame");
        }
        #region update game
        public void RequestUpdateGameAsync(int verNum, UpdateGameCallback callback)
        {
            var payloadStringBuilder = new StringBuilder();
            var payloadJsonWriter = new JsonWriter(payloadStringBuilder);
            payloadJsonWriter.WriteObjectStart();
            payloadJsonWriter.WritePropertyName(clientVerPropertyName);
            payloadJsonWriter.Write(verNum);
            payloadJsonWriter.WriteObjectEnd();
#if UNITY_EDITOR
            if (AssetBundles.AssetBundleManager.SimulateAssetBundleInEditor)
            {
                callback("DUMMY", null);
            }
            else
#endif
            {
                string JsonArraystring = payloadStringBuilder.ToString();
                Dictionary<string, string> headers = new Dictionary<string, string>();
                headers.Add("Content-Type", "application/json");
                byte[] body = Encoding.UTF8.GetBytes(JsonArraystring);
                WWW www = new WWW("your service url", body, headers);
                StartCoroutine(CheckVersionEnumerator(www, callback));
            }

        }

        private IEnumerator CheckVersionEnumerator(WWW www, UpdateGameCallback callback)
        {
            //Wait for request to complete
            yield return www;
            if (www.error == null)
            {
                string serviceData = www.text;

                var jreader = new JsonReader(serviceData);
                var json = JsonMapper.ToObject(jreader);
                if (!(bool)json["error"])
                    callback(null, JsonUtility.FromJson<AssetBundleUpdateInfo>(json["updateInfo"].ToJson()));
                else
                    callback(ErrorCode.GetErrorString((int)json["errorCode"]), null);

            }
            else
            {
                callback(ErrorCode.GetErrorString((int)ErrorCodeEnum.UpdateGame_Client_CantReachServer), null);

                Debug.Log("www error:" + www.error);
            }
        }
        #endregion
    }
}
