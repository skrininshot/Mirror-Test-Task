using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private int restartGameTime;
    [SerializeField] private int winningConfusesAmount;
    public int WinningConfusesAmount { get => winningConfusesAmount; }
    private int remainingTime;

    private void Awake()
    {
        Instance = this;
    }

    public void Win()
    {
        StartCoroutine(RemainingTimeCounter());
    }

    private IEnumerator RemainingTimeCounter()
    {
        PlayerUI.Instance.Win();
        remainingTime = restartGameTime;
        while (remainingTime > 0)
        {
            PlayerUI.Instance.UpdateTime(remainingTime);
            remainingTime--;
            yield return new WaitForSeconds(1);         
        }
        RestartGame();
        PlayerUI.Instance.Clear();
    }

    private void RestartGame()
    {
        foreach(Player player in FindObjectsOfType<Player>())
        {
            player.Respawn();
        }
    }
}
