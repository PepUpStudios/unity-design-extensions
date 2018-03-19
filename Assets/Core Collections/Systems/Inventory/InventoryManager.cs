using UnityEngine;
using System.Collections.Generic;
using State.Event;

public enum LogLevel { None, Debug }

namespace Inventory {
    public class InventoryManager : EventManager, IUIModel {

        public static InventoryManager instance {
            get {
                return m_instance;
            }
            set {
                if (m_instance == null) {
                    m_instance = value;
                }
            }
        }
        private static InventoryManager m_instance;

        const int POOL_TYPE = -1;
        const string LINK_MAP = "-1";
        const string COUNTER_START = "300";

        public LogLevel logLevel;

        [Header("Maps")]
        public List<string> types;
        public List<Item> items;
        public List<ItemCapacity> capacities;

        public Dictionary<int, Dictionary<string, ItemTrack>> inventory = new Dictionary<int, Dictionary<string, ItemTrack>>();        
        protected int counter = 300;

        public static event ObjectEventDelegator OnAdd;
        public static event ObjectEventDelegator OnRemove;
        public static event ObjectEventDelegator OnUpdate;

        public void Awake() {
            instance = this;
        }

        private void Start() {
            counter = int.Parse(COUNTER_START);
            ItemTrack newTrack = new ItemTrack();
            newTrack.capacity = -1;
            Dictionary<string, ItemTrack> dictionary = GetType(POOL_TYPE);
            dictionary.Add(LINK_MAP, newTrack);
            for (int i = 0; i < capacities.Count; i++) {
                newTrack = new ItemTrack();
                newTrack.capacity = capacities[i].capacity;
                dictionary = GetType(capacities[i].type);
                dictionary.Add(LINK_MAP, newTrack);
            }
        }

        public Dictionary<string, ItemTrack> GetType(int type) {
            if (!inventory.ContainsKey(type)) {
                Dictionary<string, ItemTrack> dictionary = new Dictionary<string, ItemTrack>();
                inventory.Add(type, dictionary);
                return dictionary;
            } else {
                return inventory[type];
            }
        }

        protected ItemTrack GetNewItem() {
            ItemTrack itemTrack = new ItemTrack();
            itemTrack.Reset();
            return itemTrack;
        }

        public void AddItem(ItemID itemID) {
            int type = GetItemType(itemID.classItem);
            itemID.type = type;
#if UNITY_EDITOR
            PreDebugLog("Adding Item", "Adding for type: ", type, itemID.item);
#endif
            Dictionary<string, ItemTrack> dictionary = GetType(type);
            ItemTrack newTrack = null;
            if (dictionary[LINK_MAP].capacity - dictionary[LINK_MAP].occupiedSpace == 0 && dictionary[LINK_MAP].capacity != -1) {
                newTrack = PopItem(type);
            }
            if (newTrack != null) {
                newTrack.Reset();
            } else {
                newTrack = GetNewItem();
            }
            newTrack.itemID = itemID.item;
            newTrack.item = itemID;
            dictionary.Add(this.counter.ToString(), newTrack);
            newTrack.localNext = this.counter;
            ItemTrack itemTrack = null;
            if (!dictionary.ContainsKey(itemID.item)) { //item index not found - New Item in TYPE
                itemTrack = GetNewItem();
                dictionary.Add(itemID.item, itemTrack);
                if(dictionary[LINK_MAP].globalNext == -99 && dictionary[LINK_MAP].globalPrevious == -99) { //first item in TYPE
                    dictionary[LINK_MAP].globalPrevious = this.counter;
                    dictionary[LINK_MAP].globalNext = this.counter;
                }
                itemTrack.item = itemID;
                itemTrack.localPrevious = this.counter;
                itemTrack.localNext = this.counter;
                itemTrack.occupiedSpace = 0;
                itemTrack.capacity = dictionary[LINK_MAP].capacity;
            } else {
                itemTrack = dictionary[itemID.item];
            }
            newTrack.localPrevious = itemTrack.localNext;
            newTrack.globalPrevious = dictionary[LINK_MAP].globalNext;
            newTrack.globalNext = this.counter;

            newTrack.occupiedSpace = 1; //occupied space is 1 for each item
            newTrack.capacity = 1; //capacity is 1 for each item

            dictionary[itemTrack.localNext.ToString()].localNext = this.counter;
            dictionary[dictionary[LINK_MAP].globalNext.ToString()].globalNext = this.counter;

            itemTrack.localNext = this.counter;
            dictionary[LINK_MAP].globalNext = this.counter;

            itemTrack.occupiedSpace++;
            dictionary[LINK_MAP].occupiedSpace++;
#if UNITY_EDITOR
            PostDebugLog("Added Item", type, itemID.item, dictionary);
#endif
            this.counter++;

            if (OnAdd != null) {
                OnAdd(itemID);
            }
            if (OnUpdate != null) {
                OnUpdate(GetItems(dictionary));
            }
        }

        public ItemTrack PopItem(int type) {
            return PopItem(GetType(type)[GetType(type)[LINK_MAP].globalPrevious.ToString()].item);
        }

        public ItemTrack PopItem(ItemID itemID) {
            return RemoveItem(itemID);
        }

        protected ItemTrack RemoveItem(ItemID itemID) {
            int type = GetItemType(itemID.classItem);
            itemID.type = type;
#if UNITY_EDITOR
            PreDebugLog("Removing Item", "Removing for type: ", type, itemID.item);
#endif
            Dictionary<string, ItemTrack> dictionary = GetType(type);
            ItemTrack itemTrack = null;
            if (dictionary.ContainsKey(itemID.item) && dictionary[LINK_MAP].globalPrevious != -99 && dictionary[LINK_MAP].globalNext != -99) { //NOT Empty List
                itemTrack = dictionary[dictionary[itemID.item].localPrevious.ToString()];
                dictionary.Remove(dictionary[itemID.item].localPrevious.ToString());
                dictionary[LINK_MAP].occupiedSpace--;
                dictionary[itemTrack.itemID].occupiedSpace--;
                if(dictionary[LINK_MAP].occupiedSpace == 0) { //last item in the list
                    dictionary[LINK_MAP].globalPrevious = -99;
                    dictionary[LINK_MAP].globalNext = -99;
                } else {
                    dictionary[itemID.item].localPrevious = itemTrack.localNext;
                    if (dictionary.ContainsKey(itemTrack.globalPrevious.ToString())) {
                        dictionary[itemTrack.globalPrevious.ToString()].globalNext = itemTrack.globalNext;
                    }
                    if (dictionary.ContainsKey(itemTrack.globalNext.ToString())) {
                        if(dictionary[itemTrack.globalNext.ToString()].globalPrevious == itemTrack.globalPrevious) { // IF ELEMENT PLACED AT LIST START GLOBALLY IS BEING REMOVED  
                            dictionary[itemTrack.globalNext.ToString()].globalPrevious = itemTrack.globalNext; // LET NEXT, POINT TO SELF
                        } else {
                            dictionary[itemTrack.globalNext.ToString()].globalPrevious = itemTrack.globalPrevious;
                        }
                    } else { // IF ELEMENT PLACED AT LIST END GLOBALLY IS BEING REMOVED 
                        dictionary[LINK_MAP].globalNext = itemTrack.globalPrevious;
                    }
                    if (dictionary[LINK_MAP].globalPrevious == itemTrack.localPrevious) { //Is First Item being removed
                        dictionary[LINK_MAP].globalPrevious = itemTrack.globalNext;
                    }
                }
                if (dictionary[itemTrack.itemID].occupiedSpace == 0) { //last item in the TYPE
                    dictionary.Remove(itemTrack.itemID);
                } else {
                    dictionary[itemTrack.localNext.ToString()].localPrevious = itemTrack.localNext;
                }
                if (OnRemove != null) {
                    OnRemove(itemID);
                }
                if (OnUpdate != null) {
                    OnUpdate(GetItems(dictionary));
                }
            }
#if UNITY_EDITOR
            PostDebugLog("Removed Item", type, itemID.item, dictionary);
#endif
            return itemTrack;
        }

        void PreDebugLog(string heading, string subMessage, int type, string item) {
            if (logLevel == LogLevel.Debug) {
                Debug.Log("----------- " + heading + " --------------");
                Debug.Log(subMessage + type + " item: " + item);
            }
        }

        void PostDebugLog(string heading, int type, string item, Dictionary<string, ItemTrack> dictionary) {
            if (logLevel == LogLevel.Debug) {
                Debug.Log("----------- " + heading + " --------------");
                Debug.Log("Added for type: " + type + " item: " + item);
                foreach (string i in dictionary.Keys) {
                    Debug.Log("[type: " + type + "][item: " + i + "] { " + dictionary[i].itemID + ", " + dictionary[i].globalPrevious + ", " + dictionary[i].globalNext + ", " + dictionary[i].localPrevious + ", " + dictionary[i].localNext + ", " + dictionary[i].occupiedSpace + "/" + dictionary[i].capacity + " }");
                }
                Debug.Log("--------------------------------");
            }
        }

        public int GetTypeCount(int type) {
            return GetItemCount(type, LINK_MAP);
        }

        public ItemTrack[] GetItems(int type) {
            Dictionary<string, ItemTrack> dictionary = GetType(type);
            return GetItems(dictionary);
        }

        protected ItemTrack[] GetItems(Dictionary<string, ItemTrack> dictionary) {
            List<ItemTrack> newItemTracks = new List<ItemTrack>();
            int counterStart = int.Parse(COUNTER_START);
            foreach (string i in dictionary.Keys) {
                int index;
                if (int.TryParse(i, out index) && index >= counterStart) {
                    newItemTracks.Add(dictionary[i]);
                }
            }
            return newItemTracks.ToArray();
        }

        public int GetItemCount(int type, string item) {
            Dictionary<string, ItemTrack> dictionary = GetType(type);
            if (dictionary.ContainsKey(item)) {
                return dictionary[item].occupiedSpace;
            }
            return 0;
        }

        public string GetTypeName(int type) {
            return types[type];
        }

        public string GetItemName(int item) {
            return items[item].item;
        }

        public string GetItemIndexInType(string item, int type) {
            Dictionary<string, ItemTrack> dictionary = GetType(type);
            if (dictionary.ContainsKey(item)) {
                return dictionary[item].itemID;
            }
            return null;
        }

        public int GetItemType(int item) {
            return items[item].type;
        }

        public int GetTypeCapacity(int type) {
            Dictionary<string, ItemTrack> dictionary = GetType(type);
            if (dictionary.ContainsKey(LINK_MAP)) {
                ItemTrack itemTrack = dictionary[LINK_MAP];
                return itemTrack.capacity;
            }
            return -1;
        }

        public bool IsCapacityFull(int type) {
            Dictionary<string, ItemTrack> dictionary = GetType(type);
            if (dictionary.ContainsKey(LINK_MAP)) {
                ItemTrack itemTrack = dictionary[LINK_MAP];
                return (itemTrack.occupiedSpace == itemTrack.capacity) ? true : false;
            }
            return false;
        }
    }
}