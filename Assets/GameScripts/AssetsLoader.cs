using UnityEngine;
using System.Collections;
using AssetBundles;
using UnityEngine.SceneManagement;

namespace LOM
{
    [System.Serializable]
    public class AssetBundleUpdateInfo
    {
        public int ver;
        public string url;
        public AssetBundleUpdateInfo(int verIn, string urlIn)
        {
            ver = verIn;
            url = urlIn;
        }
    };

    public class AssetsLoader : MonoBehaviour
    {
        //attach to a game object and place it in the first scene
        #region StaticFunctions
        static public AssetBundleManifest DownloadedAssetBundleManifestObject { get; private set; }
        static public bool bIsDownloading { get; private set; }
        static private int count;
        public static IEnumerator downloadManifest(AssetBundleUpdateInfo updateInfo, bool bAutoDownloadAllAssetBundles = false)
        {
#if UNITY_EDITOR
            // If we're in Editor simulation mode, we don't need the manifest assetBundle.
            if (AssetBundleManager.SimulateAssetBundleInEditor)
                yield break;
#endif
            //beginning of patch downloading
            while (!Caching.ready)
                yield return null;

            bIsDownloading = true;
            count = 0;
            while (DownloadedAssetBundleManifestObject == null)
            {
                count++;
                using (WWW www = WWW.LoadFromCacheOrDownload(updateInfo.url + Utility.GetPlatformName() + "/" + Utility.GetPlatformName(), updateInfo.ver))
                {
                    yield return www;
                    if (www.error == null)
                    {
                        DownloadedAssetBundleManifestObject = www.assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                        www.Dispose();
                        break;
                    }
                    else if (DownloadedAssetBundleManifestObject == null && count == 6)
                    {
                        EventManager.TriggerEvent(new EventObj("Error", (int)ErrorCodeEnum.UpdateGame_Client_CantDownloadData));
                        count = 0;
                    }
                    www.Dispose();
                    yield return new WaitForSeconds(1);
                }
            }
            //ready to use the asset bundle manager
            AssetBundleManager.SetSourceAssetBundleURL(updateInfo.url);
            AssetBundleManager.InitializeWithManifest(DownloadedAssetBundleManifestObject,new string[2]{ "hd","sd"});
            bIsDownloading = false;

        }

        #endregion


        public AssetsTableScriptableObject DefaultAssetsTableObj;
        private AssetsTableScriptableObject DownloadedAssetsTableObj = null;

        private static AssetsLoader assetsloader;
        public static AssetsLoader Instance
        {
            get
            {
                if (!assetsloader)
                {
                    assetsloader = FindObjectOfType(typeof(AssetsLoader)) as AssetsLoader;

                    if (!assetsloader)
                    {
                        Debug.LogError("There needs to be one active AssetsLoader script on a GameObject in your scene.");
                    }
                }

                return assetsloader;
            }
        }
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
        private AssetsLoader()
        {}

        public void SetDownloadedAssetsTableObj(AssetsTableScriptableObject objIn)
        {
            DownloadedAssetsTableObj = objIn;
            DownloadedAssetsTableObj.Init();
        }
        public AssetsTableScriptableObject.AssetEntry GetAssetEntry(string assetId)
        {
            if (DownloadedAssetsTableObj != null && DownloadedAssetsTableObj.AssetsTable.ContainsKey(assetId))
                return DownloadedAssetsTableObj.AssetsTable[assetId];
            else if (DefaultAssetsTableObj.AssetsTable.ContainsKey(assetId))
                return DefaultAssetsTableObj.AssetsTable[assetId];

            return null;
        }

        void Start()
        {
            DefaultAssetsTableObj.Init();

#if UNITY_EDITOR
            foreach (var entry in DefaultAssetsTableObj.AssetsTable)
                if (!string.IsNullOrEmpty(entry.Value.AssetBundleName) || !string.IsNullOrEmpty(entry.Value.AssetName))
                    Debug.LogError("Default AssetsTable Shall not have Asset Bundle / Asset names --> Key: " + entry.Key);
#endif
        }
        public void LoadLevel(ref AssetsTableScriptableObject.AssetEntry assetEntry)
        {
            if (string.IsNullOrEmpty(assetEntry.AssetBundleName) || string.IsNullOrEmpty(assetEntry.AssetName))
            {
                if (assetEntry.DefaultAssetText != null)
                    SceneManager.LoadSceneAsync(assetEntry.DefaultAssetText);
            }
            else
            {
                StartCoroutine(InitializeLevelAsync(assetEntry));
            }
        }

        protected IEnumerator InitializeLevelAsync(AssetsTableScriptableObject.AssetEntry assetEntry, bool isAdditive = false)
        {
            //float startTime = Time.realtimeSinceStartup;

            AssetBundleLoadOperation request = AssetBundleManager.LoadLevelAsync(assetEntry.AssetBundleName, assetEntry.AssetName, isAdditive);
            if (request == null)
                yield break;

            yield return StartCoroutine(request);

            //Calculate and display the elapsed time.
            //float elapsedTime = Time.realtimeSinceStartup - startTime;
            //Debug.Log("Finished loading scene " + levelName + " in " + elapsedTime + " seconds");
        }

        public IEnumerator InstantiateObjectAsync(AssetsTableScriptableObject.AssetEntry assetEntry, System.Action<Object> callback)
        {
            if (string.IsNullOrEmpty(assetEntry.AssetBundleName) || string.IsNullOrEmpty(assetEntry.AssetName))
            {

                if (assetEntry.DefaultAssetRef != null)
                    callback(Instantiate(assetEntry.DefaultAssetRef));

                yield break;

            }

            AssetBundleLoadAssetOperation request = AssetBundleManager.LoadAssetAsync(assetEntry.AssetBundleName, assetEntry.AssetName, typeof(Object));
            if (request == null)
                yield break;
            yield return StartCoroutine(request);

            // Get the asset.
            Object prefab = request.GetAsset<Object>();

            if (prefab != null)
                callback(Instantiate(prefab));
            else
                callback(null);
        }


        public IEnumerator DownLoadAllAssetBundlesAsync(bool bUnloadDownloadedAssetBundle = true)
        {
            if (AssetBundleManager.AssetBundleManifestObject != null)
            {
                while (!Caching.ready)
                    yield return null;

                var bundleNames = AssetBundleManager.AssetBundleManifestObject.GetAllAssetBundles();
                foreach (var bundleName in bundleNames)
                {
                    var count = 0;
                    while (!string.IsNullOrEmpty(bundleName))
                    {
                        using (WWW www = WWW.LoadFromCacheOrDownload(AssetBundleManager.BaseDownloadingURL + bundleName, AssetBundleManager.AssetBundleManifestObject.GetAssetBundleHash(bundleName), 0))
                        {
                            yield return www;
                            if (www.error == null)
                            {
                                Debug.Log(bundleName + " cached");
                                if (www.assetBundle != null && bUnloadDownloadedAssetBundle)
                                {
                                    www.assetBundle.Unload(false);
                                    Debug.Log(bundleName + " unloaded");
                                }
                                www.Dispose();
                                break;
                            }
                            else if (count == 5)
                            {
                                EventManager.TriggerEvent(new EventObj("Error", (int)ErrorCodeEnum.UpdateGame_Client_CantDownloadData));
                            }
                            www.Dispose();
                            yield return new WaitForSeconds(1);
                        }
                    }
                }
            }
        }
    }
}