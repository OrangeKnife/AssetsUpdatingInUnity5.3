using UnityEngine;
using System.Collections.Generic;


namespace LOM
{

    [CreateAssetMenu(fileName = "AssetsTable", menuName = "AssetsTable", order = 1)]
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
        private List<KVP_Asset> AssetsTableKVP = null;

        public Dictionary<string, AssetEntry> AssetsTable;

        public void Init()
        {
            if (AssetsTableKVP != null && AssetsTable == null)
            {
                AssetsTable = new Dictionary<string, AssetEntry>(AssetsTableKVP.Count);
                foreach (var entry in AssetsTableKVP)
                {
                    AssetsTable[entry.Key] = entry.Value;
                }
            }
        }
    }
}