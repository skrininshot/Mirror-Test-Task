using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;
    [SerializeField] private int maxRestartGameTime;
    [SerializeField] private int winningConfusesAmount;
    public int WinningConfusesAmount { get => winningConfusesAmount; }
    private int remainingTime;

    private void Awake()
    {
        Instance = this;
    }

    [ClientRpc]
    public void Win()
    {
        Debug.Log("Win");
    }
}
