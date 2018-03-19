
namespace Inventory {
    [System.Serializable]
    public class ItemTrack {
        public int localPrevious; //Also means first item for item row
        public int localNext; //Also means last item for item row
        public int globalPrevious; //Also means first item for item row
        public int globalNext; //Also means last item for item row
        public int occupiedSpace; //Amount of space occupied
        public int capacity;
        public string itemID;
        public ItemID item;

        public ItemTrack() {
            Reset();
        }

        public void Reset() {
            localPrevious = -99;
            localNext = -99;
            globalPrevious = -99;
            globalNext = -99;
            occupiedSpace = 0;
            capacity = 0;
            itemID = null;
            item = null;
        }
    }
}