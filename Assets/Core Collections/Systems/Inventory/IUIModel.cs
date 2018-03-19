
namespace Inventory {
    public interface IUIModel {

        void AddItem(ItemID itemID);
        ItemTrack PopItem(ItemID itemID);
        ItemTrack PopItem(int type);
        ItemTrack[] GetItems(int type);
    }
}