using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class Chest_Tool : EditorWindow
{
    private GameObject selectedObject;
    private bool[] resourceSelections = new bool[4];
    private int[] minCounts = new int[4];
    private int[] maxCounts = new int[4];
    private int[] percentChance = new int[4];

    private string[] resourceTypes = { "Food", "Materials", "Medicine", "Scrap" };

    private const string ChestDataFilePath = "Assets/ChestAttributes.json";
    
    private List<ChestAttributes> chestDataList = new List<ChestAttributes>();
     private Dictionary<string, ChestAttributes> chestAttributesDict = new Dictionary<string, ChestAttributes>();
    public ChestAttributes attributes = new ChestAttributes();


    public ChestsForJson chests = new ChestsForJson();

    //public ChestList chests = new ChestList();

    [MenuItem("Window/Chest Tool")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(Chest_Tool));
    }
    private void OnEnable()
    {
        LoadChestAttributes();
    }
    private void OnSelectionChange()
    {
        selectedObject = Selection.activeGameObject;
        Repaint(); // Repaint the window to reflect the changes
        LoadChestAttributes();
         if (selectedObject != null && selectedObject.CompareTag("Chest"))
        {
            ChestAttributes attributes = chestDataList.Find(data => data.chestName == selectedObject.name);

            // Check if the attributes are found
            if (attributes != null)
            {
                resourceSelections = attributes.resourceSelections;
                minCounts = attributes.minCounts;
                maxCounts = attributes.maxCounts;
                percentChance = attributes.percentChance;
            }
            else
            {
                // If attributes are not found, reset the arrays to default values
                resourceSelections = new bool[resourceTypes.Length];
                minCounts = new int[resourceTypes.Length];
                maxCounts = new int[resourceTypes.Length];
                percentChance = new int[resourceTypes.Length];
            }
        }
        else
        {
            // Reset the arrays if no chest is selected
            resourceSelections = new bool[resourceTypes.Length];
            minCounts = new int[resourceTypes.Length];
            maxCounts = new int[resourceTypes.Length];
            percentChance = new int[resourceTypes.Length];
        }
        UpdateChestProperties();

    // Repaint the GUI to reflect the changes
    Repaint();
    }
    private void LoadChestAttributes()
    {
        try
        {
            if (File.Exists(ChestDataFilePath))
            {
                string json = File.ReadAllText(ChestDataFilePath);
                chests = JsonUtility.FromJson<ChestsForJson>(json);
                chestDataList = chests.chest_list;
            }
            else
            {
                Debug.Log("Chest data file does not exist: " + ChestDataFilePath);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error loading chest attributes: " + e.Message);
        }
        GameObject[] chests_In_Scene = GameObject.FindGameObjectsWithTag("Chest");
        int count = 0;
        int c = 0;
        foreach (ChestAttributes j in chestDataList)
        {   
            foreach(GameObject i in chests_In_Scene)
            {

                if(j.chestName != i.name)
                {
                    count += 1;
                }
                else
                {
                    break;
                }
                if(count >= chests_In_Scene.Length)
                {
                    chests.chest_list.RemoveAt(c);
                }
            }
            c += 1;
            count = 0;
            SaveChestAttributes();
        }
    }


    private void SaveChestAttributes()
    {
        try
        {
            ChestsForJson test = new ChestsForJson();
            test.chest_list = chestDataList;
            string json = JsonUtility.ToJson(test, true);
            File.WriteAllText(ChestDataFilePath, json);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error saving chest attributes: " + e.Message);
        }
    }

    private void OnGUI()
    {
        selectedObject = Selection.activeGameObject;

        if (selectedObject != null && selectedObject.CompareTag("Chest"))
        {
            ChestAttributes attributes = chestDataList.Find(data => data.chestName == selectedObject.name);
            EditorGUILayout.LabelField("Selected Object: ", selectedObject.name);

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Resource Type", EditorStyles.boldLabel, GUILayout.Width(100));
            EditorGUILayout.LabelField("Select", EditorStyles.boldLabel, GUILayout.Width(50));
            EditorGUILayout.LabelField("Min Count", EditorStyles.boldLabel, GUILayout.Width(60));
            EditorGUILayout.LabelField("Max Count", EditorStyles.boldLabel, GUILayout.Width(60));
            EditorGUILayout.LabelField("Percent to Spawn", EditorStyles.boldLabel, GUILayout.Width(120));
            GUILayout.EndHorizontal();

            for (int i = 0; i < resourceTypes.Length; i++)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(resourceTypes[i], GUILayout.Width(100));
                attributes.resourceSelections[i] = EditorGUILayout.Toggle(attributes.resourceSelections[i], GUILayout.Width(50));
                attributes.minCounts[i] = EditorGUILayout.IntField(attributes.minCounts[i], GUILayout.Width(60));
                attributes.maxCounts[i] = EditorGUILayout.IntField(attributes.maxCounts[i], GUILayout.Width(60));
                attributes.percentChance[i] = EditorGUILayout.IntField(attributes.percentChance[i], GUILayout.Width(60));
                GUILayout.EndHorizontal(); 
            }
            UpdateChestProperties();
        }
        else if (selectedObject != null && !selectedObject.CompareTag("Chest"))
        {
            EditorGUILayout.LabelField("Selected Object: ", selectedObject.name + " (not tagged as 'Chest')");
            EditorGUILayout.HelpBox("Please select an object tagged as 'Chest'.", MessageType.Warning);
        }
        else
        {
            EditorGUILayout.LabelField("No object selected.");
        }
    }
    private void UpdateGUI()
    {
        selectedObject = Selection.activeGameObject;
        if (selectedObject != null && selectedObject.CompareTag("Chest"))
        {
            ChestAttributes attributes = chestDataList.Find(data => data.chestName == selectedObject.name);

        }

        for (int i = 0; i < resourceTypes.Length; i++)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(resourceTypes[i], GUILayout.Width(100));
                resourceSelections[i] = EditorGUILayout.Toggle(resourceSelections[i], GUILayout.Width(50));
                minCounts[i] = EditorGUILayout.IntField(minCounts[i], GUILayout.Width(60));
                maxCounts[i] = EditorGUILayout.IntField(maxCounts[i], GUILayout.Width(60));
                percentChance[i] = EditorGUILayout.IntField(percentChance[i], GUILayout.Width(60));
                GUILayout.EndHorizontal(); 
            }
    }
    // reloading on assemblys
    public void OnAfterAssemblyReload()
    {
        UpdateChestProperties();
    }

     private void UpdateChestProperties()
    {   if (selectedObject != null && selectedObject.CompareTag("Chest"))
        {

            string currentChestName = selectedObject.name;

            ChestAttributes attributes = chestDataList.Find(data => data.chestName == currentChestName);

            if (attributes == null)
                {
                    // Create a new entry if the chest doesn't exist in the dictionary
                    attributes = new ChestAttributes();
                    attributes.chestName = currentChestName;
                    chestDataList.Add(attributes);
                }
                
            attributes.resourceSelections = resourceSelections;
            attributes.minCounts = minCounts;
            attributes.maxCounts = maxCounts;
            attributes.percentChance = percentChance;
            SaveChestAttributes();

        }
        
    }
}


[System.Serializable]
public class ChestAttributes
{
    public string chestName;
    public bool[] resourceSelections;
    public int[] minCounts;
    public int[] maxCounts;
    public int[] percentChance;
}
[System.Serializable]
public class ChestsForJson
{
    public List<ChestAttributes> chest_list = new List<ChestAttributes>();
}

