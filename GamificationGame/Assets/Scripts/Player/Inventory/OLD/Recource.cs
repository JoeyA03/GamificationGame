
using UnityEngine;

[CreateAssetMenu(menuName = "GamificationGame/Recource")]
public class Recource : ScriptableObject
{
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
    public int width;
    public int height;
    // public SpriteIcon;

    public gridType gridSections;         //Make a grid type  
}
