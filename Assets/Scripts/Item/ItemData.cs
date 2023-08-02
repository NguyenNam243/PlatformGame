[System.Serializable]
public class ItemData
{
    public string itemName;
    public Rarity rarity;

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
