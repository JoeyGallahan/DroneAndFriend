using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    float deltaTime = 0.0f;
    TextMeshProUGUI UIText;
    int fpsCtr = 0;
    float sum = 0.0f;
    float avg = 0.0f;

    private void Awake()
    {
        UIText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f; float msec = deltaTime * 1000.0f;
        fpsCtr++;
        float fps = 1.0f / deltaTime;
        sum += fps;
        avg = (sum / fpsCtr);
        string text = avg.ToString("F0") + " avg fps";
        UIText.SetText(text);
    }

}
