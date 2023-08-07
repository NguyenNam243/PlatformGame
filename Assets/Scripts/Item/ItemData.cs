[System.Serializable]
public class ItemData
{
    public ItemType type;
    public Rarity rarity;
    public string itemName;
    public int HP = 50;
    public int ATK = 5;
    public int DFS = 3;
    public int SPD = 1;


    public ItemData()
    {

    }
}

public enum Rarity
{
    Common,
    Rare,
    Epic,
    Legendary
}
