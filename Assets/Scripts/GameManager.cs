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
    private bool win;

    private void Awake()
    {
        Instance = this;
    }

    public void Win(string nickname)
    {
        if (win) return;

        win = true;
        PlayerUI.Instance.Win(nickname);
        StartCoroutine(RemainingTimeCounter());
    }

    private IEnumerator RemainingTimeCounter()
    {
        remainingTime = restartGameTime;

        while (remainingTime > 0)
        {
            PlayerUI.Instance.UpdateTime(remainingTime);
            remainingTime--;
            yield return new WaitForSeconds(1);         
        }

        RestartGame();    
    }

    private void RestartGame()
    {
        win = false;
        PlayerUI.Instance.Clear();

        foreach (Player player in FindObjectsOfType<Player>())
        {
            player.Respawn();
        }
    }
}
