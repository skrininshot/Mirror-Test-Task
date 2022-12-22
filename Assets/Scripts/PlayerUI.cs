using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public static PlayerUI Instance;

    [SerializeField] private Text winnerName;
    [SerializeField] private Text restartingTime;
    private string restartingText;

    private void Awake()
    {
        Instance = this;
    }

    public void Win()
    {
        winnerName.gameObject.SetActive(true);
        restartingTime.gameObject.SetActive(true);
        restartingText = restartingTime.text;
    }

    public void UpdateTime(int seconds)
    {
        restartingTime.text = restartingText + seconds;
    }

    public void Clear()
    {
        restartingTime.text = restartingText;
        winnerName.gameObject.SetActive(false);
        restartingTime.gameObject.SetActive(false);
    }
}
