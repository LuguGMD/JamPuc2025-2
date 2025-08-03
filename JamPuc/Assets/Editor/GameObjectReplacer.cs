using UnityEngine;
using UnityEditor;

public class GameObjectReplacer : EditorWindow
{
    public GameObject replacer;
    public bool doCopyName;
    public bool doCopyRotation;
    public bool doCopyScale;

    private SerializedObject soReplacer;

    private SerializedProperty pReplacer;
    private SerializedProperty pDoCopyName;
    private SerializedProperty pDoCopyRotation;
    private SerializedProperty pDoCopyScale;

    [MenuItem("Tools/Lugu/Game Object Replacer")]
    public static void OpenWindow()
    {
        GameObjectReplacer window = GetWindow<GameObjectReplacer>();
        window.titleContent = new GUIContent("Game Object Replacer");
    }

    private void OnEnable()
    {
        soReplacer = new SerializedObject(this);

        pReplacer = soReplacer.FindProperty("replacer");
        pDoCopyName = soReplacer.FindProperty("doCopyName");
        pDoCopyRotation = soReplacer.FindProperty("doCopyRotation");
        pDoCopyScale = soReplacer.FindProperty("doCopyScale");
    }

    private void OnGUI()
    {
        if(replacer == null)
        {
            replacer = GameObject.FindAnyObjectByType(typeof(GameObject)) as GameObject;
        }

        //Getting Changes
        soReplacer.Update();
        
        pReplacer.objectReferenceValue = EditorGUILayout.ObjectField("Object To Replace With", replacer, typeof(GameObject), true) as GameObject;

        pDoCopyName.boolValue = EditorGUILayout.Toggle("Copy Name", doCopyName);
        pDoCopyRotation.boolValue = EditorGUILayout.Toggle("Copy Rotation", doCopyRotation);
        pDoCopyScale.boolValue = EditorGUILayout.Toggle("Copy Scale", doCopyScale);

        //Applying Changes
        soReplacer.ApplyModifiedProperties();

        if (GUILayout.Button("Replace Selected"))
        {
            //Starting Changes Group (Undo all at once)
            Undo.SetCurrentGroupName("Replaced Game Objects");
            int group = Undo.GetCurrentGroup();

            //Creating an Array to mantain selected objects
            GameObject[] createdObjects = new GameObject[Selection.gameObjects.Length];
            int createdNumber = 0;

            for (int i = 0; i < Selection.gameObjects.Length; i++)
            {
                Transform t = Selection.gameObjects[i].transform;

                GameObject instantiated;

                //Checking if object is a prefab
                if (PrefabUtility.IsPartOfPrefabAsset(replacer))
                {
                    //Creating Prefab without losing reference to being prefab
                     instantiated = PrefabUtility.InstantiatePrefab(replacer) as GameObject;
                }
                else
                {
                    //Creating GameObject
                    instantiated = Instantiate(replacer);
                }

                Undo.RegisterCreatedObjectUndo(instantiated, "Created_Replacement_Instance");

                instantiated.transform.position = t.position;

                if(doCopyName)
                instantiated.gameObject.name = t.gameObject.name;

                if(doCopyRotation)
                instantiated.transform.rotation = t.rotation;
                
                if(doCopyScale)
                instantiated.transform.localScale = t.localScale;

                //Destroying Object that is beign replaced
                Undo.DestroyObjectImmediate(Selection.gameObjects[i]);
                i--;

                createdObjects[createdNumber] = instantiated;
                createdNumber++;
            }

            //Closing Changes Group
            Undo.CollapseUndoOperations(group);

            //Mantain Objects
            Selection.objects = createdObjects;
        }
    }
}
