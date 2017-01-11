using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class used to instantiate an entire level with it's associated objects. Must be in every level.
/// </summary>
/// 
public class LevelMaster : MonoBehaviour
{ 
    List<GameObject> prefabs;
    

    void Start()
    {
        // Initialize this Scene, this will set GlobalGameControl.Instance.currentScene to this scene, empty or not.
        GlobalGameControl.Instance.InitializeScene(gameObject.scene.buildIndex);
        prefabs = GlobalGameControl.Instance.gamePrefabs;

        if (GlobalGameControl.Instance.IsSceneBeingLoaded || GlobalGameControl.Instance.IsSceneBeingTransitioned)
        {
            //sets currentgamestate scene to this scene (for use if we save in this scene)
            GlobalGameControl.Instance.currentGameState.activeScene = gameObject.scene.buildIndex;

            //Set up all the sceneInfo variables.
            SavedNPCList sceneNPCs = GlobalGameControl.Instance.currentScene.sceneNPCS;
            SavedLevelItemList sceneItems = GlobalGameControl.Instance.currentScene.sceneItems;
            SavedQuestList sceneQuests = GlobalGameControl.Instance.currentScene.sceneQuests;

            //Null Check, then populate new instantiations with data.  
             
            #region NPCs
            if(sceneNPCs != null && sceneNPCs.savedNPC.Count != 0)
            {
                for (int i = 0; i < sceneNPCs.savedNPC.Count; i++)
                {
                    //populate data;
                    GameObject prefab = null;

                    foreach (GameObject go in prefabs)
                    {
                        if (go.name.Equals(sceneNPCs.savedNPC[i].prefabName))
                        {
                            prefab = go;
                            break;
                        }
                    }
                    if (prefab == null)
                    {
                        Debug.Log("No Assigned Prefab for : " + sceneNPCs.savedNPC[i].prefabName);
                        break;
                    }

                    GameObject NPC = (GameObject)Instantiate(prefab);

                    NPC.transform.position = new Vector3(sceneNPCs.savedNPC[i].PositionX,
                                                         sceneNPCs.savedNPC[i].PositionY,
                                                         sceneNPCs.savedNPC[i].PositionZ);

                    SavedNPCStatistics tempStats = new SavedNPCStatistics();
                    tempStats.maxHP = sceneNPCs.savedNPC[i].maxHP;
                    tempStats.hostile = sceneNPCs.savedNPC[i].hostile;
                    tempStats.currentHP = sceneNPCs.savedNPC[i].currentHP;
                    tempStats.XPWorth = sceneNPCs.savedNPC[i].XPWorth;
                    //tempStats.weapon = sceneNPCs.savedNPC[i].weapon;
                    tempStats.prefabName = sceneNPCs.savedNPC[i].prefabName;
                    tempStats.NPCQuests = sceneNPCs.savedNPC[i].NPCQuests;
                }
            }
            else
            {
                Debug.Log("No NPCs in this scene");
            }
            #endregion

            #region Items
            if (sceneItems != null && sceneItems.itemList.Count != 0)
            {
                for (int i = 0; i < sceneItems.itemList.Count; i++)
                {

                    GameObject prefab = null;

                    foreach (GameObject go in prefabs)
                    {
                        if (go.name.Equals(sceneItems.itemList[i].prefabName))
                        {
                            prefab = go;
                            break;
                        }
                    }
                    if (prefab == null)
                    {
                        Debug.Log("No Assigned Prefab for : " + sceneItems.itemList[i].prefabName);
                        break;
                    }

                    GameObject item = (GameObject)Instantiate(prefab);

                    item.transform.position = new Vector3(sceneItems.itemList[i].PositionX,
                                                         sceneItems.itemList[i].PositionY,
                                                         sceneItems.itemList[i].PositionZ);

                    SavedItem tempItem = new SavedItem();
                    tempItem.prefabName = sceneItems.itemList[i].prefabName;
                }
            }
            else
            {
                Debug.Log("No items in this scene");
            }
            #endregion

            #region Quests
            if (sceneQuests != null && sceneQuests.savedQuests.Count != 0)
            {
                for (int i = 0; i < sceneQuests.savedQuests.Count; i++)
                {
                    SavedQuest tempQuest = new SavedQuest();

                    tempQuest.CompletionStatus = sceneQuests.savedQuests[i].CompletionStatus;
                    tempQuest.ID = sceneQuests.savedQuests[i].ID;
                    tempQuest.inProgress = sceneQuests.savedQuests[i].inProgress;
                }
            }
            else
            {
                Debug.Log("No quests in this scene");
            }
            #endregion
        }
    }
}

