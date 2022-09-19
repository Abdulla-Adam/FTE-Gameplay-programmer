using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[System.Serializable]
public class Level
{
    public int LevelNumber;
    public List<Objective> Objectives;

    public int TotalObjectives;
    
}

[System.Serializable]
public class Objective
{
    public int ObjectiveNumber;
    public GameObject ObjectiveTrigger;

    public bool ObjectiveComplete;

    public string ObjectiveCompletionText;
}

public class LevelManager : MonoBehaviour
{


    public static LevelManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public int CurrentLevel;
    public int CurrentObjective;

    public int CurrentObjectivePopup;

    public List<Level> Levels = new List<Level>();

    public GameObject Ladder;

    public void CheckLevelUpdate(string Obj)
    {
        int Index = Levels[CurrentLevel].Objectives.FindIndex(x=> x.ObjectiveTrigger.name.Contains(Obj));
        if(Index>-1 && !Levels[CurrentLevel].Objectives[Index].ObjectiveComplete)
        {
            Levels[CurrentLevel].Objectives[CurrentObjective].ObjectiveComplete = true;
            CurrentObjective++;
        }
    }

    public void BringUpLadder()
    {
        Ladder.transform.Translate(new Vector3(0,7,0));
    }

}
