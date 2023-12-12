
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

    
    public string id;
    public string displayName;
    public resourceType type;
    // public SpriteIcon;
    public GameObject prefab;

    // public gridType;         //Make a grid type  
}
