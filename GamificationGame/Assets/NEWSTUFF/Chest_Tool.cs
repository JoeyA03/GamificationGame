using UnityEngine;
using UnityEditor;

public class Chest_Tool : EditorWindow
{
    private GameObject selectedObject;
    private bool[] resourceSelections = new bool[4];
    private int[] minCounts = new int[4];
    private int[] maxCounts = new int[4];
    private int[] percentChance = new int[4];

    private string[] resourceTypes = { "Food", "Materials", "Medicine", "Scrap" };

    [MenuItem("Window/Chest Tool")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(Chest_Tool));
    }

    private void OnSelectionChange()
    {
        Repaint();
    }

    private void OnGUI()
    {
        selectedObject = Selection.activeGameObject;

        if (selectedObject != null && selectedObject.CompareTag("Chest"))
        {
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
                resourceSelections[i] = EditorGUILayout.Toggle(resourceSelections[i], GUILayout.Width(50));
                minCounts[i] = EditorGUILayout.IntField(minCounts[i], GUILayout.Width(60));
                maxCounts[i] = EditorGUILayout.IntField(maxCounts[i], GUILayout.Width(60));
                percentChance[i] = EditorGUILayout.IntField(percentChance[i], GUILayout.Width(60));
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

     private void UpdateChestProperties()
    {
        // Make sure the selectedObject is not null and tagged as 'Chest'
        if (selectedObject != null && selectedObject.CompareTag("Chest"))
        {
            ChestSystem chest = selectedObject.GetComponent<ChestSystem>();

            // Update the chest properties based on the values in the window
            if (chest != null)
            {
                chest.SetResourceSelections(resourceSelections);
                chest.SetMinCounts(minCounts);
                chest.SetMaxCounts(maxCounts);
                chest.SetPercentChance(percentChance);
            }
        }
    }
}