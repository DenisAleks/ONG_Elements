using _Game._Scripts.Model.Data.Save;

namespace _Game._Scripts.Save
{
    public interface ISaveService
    {
        void Save(GameSaveData data);
        bool TryLoad(out GameSaveData data);
        void Clear();
    }
}