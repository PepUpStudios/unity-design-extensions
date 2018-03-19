using UnityEngine;
using State.Event;

namespace Inventory {
    public class ItemID : EventManager, IDisplay {

        protected InventoryManager inventory;

        [Disable]
        public string item;

        public bool isDynamic = false;

        [DropDownList("items", "item", "InventoryManager", "GameController", null, true)]
        public int classItem;

        [Header("Map")]
        public Sprite[] images;

        public int type { get; set; }
        public int ID { get; set; } //Currently item counter

        public event EventDelegator OnPlay;
        public event EventDelegator OnRemove;

        public static int itemCounter = 0;

        private void Start() {
            inventory = InventoryManager.instance;
        }

        public override void OnEnable() {
            base.OnEnable();
            InitNewClassID();
        }

        public void InitNewClassID() {
            ID = itemCounter++;
            item = classItem + "-" + ID;
        }

        public void AddToInventory() {
            inventory.AddItem(this);
        }

        public void RemoveFromInventory() {
            inventory.PopItem(this);
        }

        public string GetItemName() {
            return inventory.GetItemName(classItem);
        }

        public string GetItemType() {
            return inventory.GetTypeName(type);
        }

        public void Play() {
            if (OnPlay != null) {
                OnPlay();
            }
        }

        public void Removed() {
            if (OnRemove != null) {
                OnRemove();
            }
        }

        public Sprite[] GetImages() {
            return images;
        }
    }
}