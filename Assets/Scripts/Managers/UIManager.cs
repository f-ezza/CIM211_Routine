using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class UIManager : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] private TMP_Text timeLeftText;



    private void Update()
    {
        if (GameManager.Instance.GamemodeRunning && GameManager.Instance.CurrentTime < GameManager.Instance.TimeToComplete)
        {
            double remaining = GameManager.Instance.TimeToComplete - GameManager.Instance.CurrentTime;
            TimeSpan time = TimeSpan.FromSeconds(remaining);
            timeLeftText.text = $"Time Left: {time.ToString(@"mm\:ss")}";
        }
    }
}
