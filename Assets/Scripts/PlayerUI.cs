using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PlayerUI : NetworkBehaviour
{
    [SerializeField] private Text winnerName;
    [SerializeField] private Text restartingTime;
    private string restartingText;
}
