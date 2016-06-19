using UnityEngine;
using System.Collections;

public class KeyboardManager : MonoBehaviour 
{
    public KeyCode restartGameKey;

    public KeyCode player1LeftKey = KeyCode.LeftArrow;
    public KeyCode player1RightKey = KeyCode.RightArrow;
    public KeyCode player1JumpKey = KeyCode.UpArrow;
    public KeyCode player1DownKey = KeyCode.DownArrow;
    public KeyCode player1ShootKey = KeyCode.S;

    public KeyCode player2LeftKey = KeyCode.A;
    public KeyCode player2RightKey = KeyCode.D;
    public KeyCode player2JumpKey = KeyCode.W;
    public KeyCode player2DownKey = KeyCode.S;
    public KeyCode player2ShootKey = KeyCode.Q;

    public KeyCode mainMenuKey = KeyCode.Escape;
}
