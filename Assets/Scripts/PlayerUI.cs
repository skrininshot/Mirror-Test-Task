using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public static PlayerUI Instance;

    [SerializeField] private Text winnerName;
    [SerializeField] private Text restartingTime;
    [SerializeField] private Image background;
    private string restartingText;

    private void Awake()
    {
        Instance = this;
        restartingText = restartingTime.text;
    }

    public void Win(string nickname)
    {
        winnerName.gameObject.SetActive(true);
        winnerName.text = nickname;
        restartingTime.gameObject.SetActive(true);
        background.gameObject.SetActive(true);
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
        background.gameObject.SetActive(false);
    }
}
