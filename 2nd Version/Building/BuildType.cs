using UnityEngine;

public enum BuildType
{
    StreetLight,
    Bench,
    Road,
    Building,
    Tree
}

public class BuildObject : MonoBehaviour
{
    public BuildType type;
}