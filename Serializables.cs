using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/**
 * 
 * File containing all serializable information for use with save and load.
 * 
 * */

[Serializable]
public class PlayerStatistics
{
    public int SceneID;
    public float PositionX, PositionY, PositionZ;

    public float MaxHP = 100.0f;
    public float currentHP = 100.0f;
    public float Ammo;
    public float XP = 0.0f;
    public float Level = 1.0f;
    public float XPtoLevel = 100.0f;
    public float HPRegen = 0.2f;
    public List<SavedQuest> currentQuests;
}

[Serializable]
public class SavedNPCStatistics
{
    public int ID;
    public float PositionX, PositionY, PositionZ;
    public string prefabName;
    public float maxHP = 100.0f;
    public float currentHP = 100.0f;
    public int XPWorth = 100;
    public bool hostile;
    public List<SavedQuest> NPCQuests;

    public SavedNPCStatistics()
    {
        NPCQuests = new List<SavedQuest>();
    }
    //public WeaponReferenceBase weapon;
}

[Serializable]
public class SavedNPCList
{
    public int SceneID;
    public List<SavedNPCStatistics> savedNPC;

    public SavedNPCList(int newSceneID)
    {
        this.SceneID = newSceneID;
        this.savedNPC = new List<SavedNPCStatistics>();
    }
}

[Serializable]
public class SavedItem
{
    public float PositionX, PositionY, PositionZ;
    public string prefabName;
}

[Serializable]
public class SavedLevelItemList
{
    public int SceneID;
    public List<SavedItem> itemList;

    public SavedLevelItemList(int newSceneID)
    {
        this.SceneID = newSceneID;
        this.itemList = new List<SavedItem>();
    }
}

[Serializable]
public class SavedQuest
{
    public string QuestName;
    public string Details;
    public string CompletionRequirements;

    public float ExperienceReward;
    public float GoldReward;

    public int ID;
    public int NPC;
    public int LevelRequirement;
    public int Difficulty;
    public bool CompletionStatus;
    public bool inProgress;

    public override string ToString()
    {
        return "Quest: " + QuestName + "\nLevel Requirement: "
                    + LevelRequirement + " \n\n" + Details + "\n" + CompletionRequirements + "\n\n"
                    + "Gold Reward:  " + GoldReward + "\nExperience Reward:  " + ExperienceReward;
    }
}

[Serializable]
public class SavedQuestList
{
    public int SceneID;
    public List<SavedQuest> savedQuests;

    public SavedQuestList(int newSceneID)
    {
        this.SceneID = newSceneID;
        savedQuests = new List<SavedQuest>();
    }
}

[Serializable]
public class SavedInventoryItem
{

}

[Serializable]
public class SavedInventory
{

}

// Saves Entire Scene Info
[Serializable]
public class SceneInfo
{
    public int sceneID;
    public SavedQuestList sceneQuests;
    public SavedLevelItemList sceneItems;
    public SavedNPCList sceneNPCS;

    public SceneInfo(int newSceneID)
    {
        sceneID = newSceneID;
        sceneQuests = new SavedQuestList(newSceneID);
        sceneItems = new SavedLevelItemList(newSceneID);
        sceneNPCS = new SavedNPCList(newSceneID);
    }
}

//Saves entire game, all scenes, all NPCs, all Items;

[Serializable]
public class SaveData
{
    public List<SceneInfo> sceneInfo;
    public int activeScene;
    public PlayerStatistics playerStats;
    public SavedInventory savedInventory;

    public SaveData()
    {
        sceneInfo = new List<SceneInfo>();
    }
}

[System.Serializable]
public class WeaponReferenceBase
{
    public int itemID;
    public string type = "weapon";
    public GameObject weaponModel;
    public Animator modelAnimator;
    public GameObject ikHolder;
    public Transform rightHandTarget;
    public Transform leftHandTarget;
    public Transform lookTarget;
    public ParticleSystem[] muzzle;
    public Transform bulletSpawner;
    public Transform casingSpawner;
    public WeaponStats weaponStats;

    public bool dis_LHIK_notAiming;

    public GameObject pickablePrefab;
    public GameObject casingPrefab;
    public GameObject bulletPrefab;
}

[System.Serializable]
public class WeaponStats
{
    public int curBullets;
    public int maxBullets;
    public float fireRate;
    public AudioClip shootSound;
    public int weaponDamage;
    public int minRange;
    public int maxRange;
    //etc.
}