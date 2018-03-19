
namespace Inventory {
    [System.Serializable]
    public struct Item {
        [UnityEngine.Tooltip("Item name has to be unique")]
        public string item;
        [DropDownList("types", null, "InventoryManager", "GameController", null, true)]
        public int type;
    }
}