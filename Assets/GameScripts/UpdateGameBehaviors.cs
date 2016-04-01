namespace LOM
{
    using UnityEngine;
    using System.Collections;
    using AssetBundles;

    public class UpdateGameBehaviors : MonoBehaviour
    {
        private AssetBundleUpdateInfo savedupdateInfo = null;

        void Start()
        {
            EventManager.RegisterEvent("UpdateGame", UpdateGameHandler);
            EventManager.RegisterEvent("OfflineGame", OfflineGameHandler);
            GameManager.Instance.RequestUpdateGame();
        }

        void OnDestroy()
        {
            EventManager.RemoveEvent("UpdateGame", UpdateGameHandler);
            EventManager.RemoveEvent("OfflineGame", OfflineGameHandler);

        }

        private void UpdateGameHandler(EventObj eo)
        {
            savedupdateInfo = new AssetBundleUpdateInfo(eo.paramInt, eo.paramString);
            StartCoroutine(DownLoadManifestAndInitAssetBundleManager(savedupdateInfo));

        }

        private void OfflineGameHandler(EventObj eo)
        {
            Debug.Log("Offline mode: ver " + eo.paramInt);
            if (eo.paramInt > 0)
            {
                savedupdateInfo = new AssetBundleUpdateInfo(eo.paramInt, eo.paramString);
                StartCoroutine(DownLoadManifestAndInitAssetBundleManager(savedupdateInfo));
            }
            else
                EventManager.TriggerEvent(new EventObj("AssetBundlesDownloaded"));
        }

        private IEnumerator DownLoadManifestAndInitAssetBundleManager(AssetBundleUpdateInfo updateInfo)
        {
            yield return StartCoroutine(AssetsLoader.DownloadManifest(updateInfo));


            if (AssetBundleManager.AssetBundleManifestObject != null)
            {
                yield return StartCoroutine(AssetsLoader.Instance.DownLoadAllAssetBundlesAsync());
            }
            else
            {
                GlobalBehaviors.Instance.AddAMessageBox("Error", "You can't load game content index correctly :( Please try restart the game!", MessageBoxType.YES,
                    "OK", null, null, null, null, null); //no way ...
                yield break;
            }


            //load assets table first
            AssetsTableScriptableObject assetsTableObj;

            AssetsTableScriptableObject.AssetEntry assetsTableEntry = new AssetsTableScriptableObject.AssetEntry();//fake entry :D
            assetsTableEntry.AssetBundleName = "assetstable";
            assetsTableEntry.AssetName = "assetsTable";
            yield return StartCoroutine(AssetsLoader.Instance.InstantiateObjectAsync(assetsTableEntry, (newAssetstable) =>
            {
                if (newAssetstable != null)
                {
                    assetsTableObj = newAssetstable as AssetsTableScriptableObject;

                    if (assetsTableObj != null)
                        AssetsLoader.Instance.SetDownloadedAssetsTableObj(assetsTableObj);
                }
            }));


            EventManager.TriggerEvent(new EventObj("AssetBundlesDownloaded"));

        }
    }
}
