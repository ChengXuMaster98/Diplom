public interface ISaveService
{
    bool HasSave();
    void Save();
    void Load();
    void DeleteSave();
    string GetSavePath();

    void NewGame();
}