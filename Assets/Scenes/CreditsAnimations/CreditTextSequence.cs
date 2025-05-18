using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // TextMeshPro kullanýyorsan

public class CreditTextSequence : MonoBehaviour
{
    public TMP_Text textDisplay; // UI Text yerine TMP_Text
    [TextArea(3, 10)]
    public List<string> loreTexts = new List<string>();

    public float typingSpeed = 0.02f;

    private int currentTextIndex = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    void Start()
    {
        DisplayNextText();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                // Eðer yazý yazýlýyorsa ve týklanýrsa, tamamýný birden yazdýr
                StopCoroutine(typingCoroutine);
                textDisplay.text = loreTexts[currentTextIndex];
                isTyping = false;
            }
            else
            {
                // Bir sonraki metne geç
                currentTextIndex++;
                if (currentTextIndex < loreTexts.Count)
                {
                    DisplayNextText();
                }
                else
                {
                    SceneManager.LoadScene("01_MainMenu");
                }
            }
        }
    }

    void DisplayNextText()
    {
        typingCoroutine = StartCoroutine(TypeText(loreTexts[currentTextIndex]));
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        textDisplay.text = "";
        foreach (char letter in text)
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }
}
