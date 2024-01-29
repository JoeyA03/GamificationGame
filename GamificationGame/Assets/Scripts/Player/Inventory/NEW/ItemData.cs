using UnityEngine;

[CreateAssetMenu(menuName = "GamificationGame/ItemData")]
public class ItemData : ScriptableObject
{
    public int width = 1;
    public int height = 1;

    public Sprite itemIcon;

    public enum resourceType
    {
        Material,
        Medicine,
        Food,
        Scrap,
        None
    }
    public enum gridType
    {
        Dot,
        Square,
        IBlock,
        ThreeBlock,
        TBlock,
        None
    }


    public string id;
    public string displayName;
    public GameObject prefab;
    public resourceType type;
    // public SpriteIcon;

    public gridType gridSections;
}
