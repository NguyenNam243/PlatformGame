using System.Collections.Generic;

[System.Serializable]
public class HeroData
{
    [System.Serializable]
    public class Costume
    {
        public ItemType type;
        public ItemData itemData;
    }

    public List<Costume> costumes = new List<Costume>();
    public int HP = 50;
    public int ATK = 5;
    public int DFS = 3;
    public int SPD = 1;

    public HeroData() { }
}
