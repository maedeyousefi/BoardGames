using UnityEngine;
public enum PawnColor { Blue, Orange, Green, Purple }
public class LudoPawn : MonoBehaviour 
{
    [Header("Pawn Info")]
    public PawnColor pawnColor;
    public int pawnIndex;
    [Header("State")]
    public bool isInHome = true;
    public bool hasFinished = false;
    [Header("Path")]
    public int pathIndex = -1;
}