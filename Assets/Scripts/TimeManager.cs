using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float timer { get; set; }

    public TextMeshProUGUI timeText;

    public bool startTimer = false;

    // Update is called once per frame
    void Update()
    {
        if (startTimer)
        {
            timer -= Time.deltaTime;
            if (timer < 0.0f)
            {
                timer = 0.0f;
            }

            timeText.text = $"Time Left: {timer.ToString("F1")}";
        }
    }
}
