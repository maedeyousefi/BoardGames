using System.Collections.Generic;
using UnityEngine;

public class LudoGameManager : MonoBehaviour
{
    public static LudoGameManager Instance;

    [Header("Players")]
    public int playerCount = 2;
    public int currentPlayer = 0;

    [Header("Players Pawns")]
    public List<LudoPawn> allPawns = new List<LudoPawn>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}