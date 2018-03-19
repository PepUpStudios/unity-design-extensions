
namespace Inventory {
    [System.Serializable]
    public struct ItemCapacity {
        [DropDownList("types", null, "InventoryManager", "GameController", null, true)]
        public int type;
        public int capacity;
    }
}