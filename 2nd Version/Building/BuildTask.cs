using UnityEngine;

[System.Serializable]
public class BuildTask
{
    public string taskName;
    public BuildType targetType;

    public int requiredAmount = 5;
    public int currentAmount = 0;

    public int reputationReward = 1;
    public bool increaseHappiness;
    public int happinessReward = 1;
}