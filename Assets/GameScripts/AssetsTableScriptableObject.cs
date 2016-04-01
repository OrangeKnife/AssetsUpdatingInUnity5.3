namespace LOM
{
    using UnityEngine;
    using System.Collections.Generic;

    [CreateAssetMenu(fileName = "assetsTable", menuName = "assetsTable", order = 1)]
    public class AssetsTableScriptableObject : ScriptableObject
    {
        //assets table
        [System.Serializable]
        public class AssetEntry
        {
            public string AssetBundleName, AssetName; //for patch overrides
            public Object DefaultAssetRef; //for current ref
            public string DefaultAssetText; //for scene names
        }

        [System.Serializable]
        public class KVP_Asset
        {
            public string Key;
            public AssetEntry Value;
        }

        [SerializeField]
        private List<KVP_Asset> assetsTableKVP = null;

        public Dictionary<string, AssetEntry> assetsTable;

        public void Init()
        {
            if (assetsTableKVP != null && assetsTable == null)
            {
                assetsTable = new Dictionary<string, AssetEntry>(assetsTableKVP.Count);
                foreach (var entry in assetsTableKVP)
                {
                    assetsTable[entry.Key] = entry.Value;
                }
            }
        }
    }
}