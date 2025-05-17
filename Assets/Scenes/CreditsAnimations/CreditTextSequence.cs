using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreditTextSequence : MonoBehaviour
{
    public TextMeshProUGUI creditsText;
    public TextMeshProUGUI namesText;
    public float fadeDuration = 1f;
    public float displayDuration = 2f;

    private string[] messages = new string[]
    {
        "Ýyi savaþtýn evlat… Adýn tarih kitaplarýnda yaþayacak.",
        "Topraðýn bol, anýlarýn onurlu olsun. Cesaretin unutulmayacak.",
        "Mücadele ettin, düþtün… Ama asla teslim olmadýn.",
        "Savaþ bitti, kahraman huzura erdi.",
        "Kýlýcýn pas tuttu, yüreðin hiç solmadý.",
        "Son nefesine kadar savaþtýn… Artýk dinlenme vakti."
    };

    private string[] names = new string[]
    {
        "Kaan Avdan",
        "Duhan Avci",
        "Elanur Aydogdu",
        "Ali Eray Karatas"
    };

    private void Start()
    {
        creditsText.alpha = 0f;
        namesText.alpha = 0f;
        StartCoroutine(ShowMessages());
    }

    private IEnumerator ShowMessages()
    {
        foreach (string message in messages)
        {
            creditsText.text = message;

            // Fade In
            yield return StartCoroutine(FadeTextToAlpha(creditsText, 1f));

            yield return new WaitForSeconds(displayDuration);

            // Fade Out
            yield return StartCoroutine(FadeTextToAlpha(creditsText, 0f));

            yield return new WaitForSeconds(0.5f);
        }

        // Tüm mesajlar bitti, isimleri göster
        ShowShuffledNames();
        yield return StartCoroutine(FadeTextToAlpha(namesText, 1f));
        // Fade out istersen ekleyebilirsin
    }

    private IEnumerator FadeTextToAlpha(TextMeshProUGUI textObj, float targetAlpha)
    {
        float startAlpha = textObj.alpha;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            textObj.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
            yield return null;
        }

        textObj.alpha = targetAlpha;
    }

    private void ShowShuffledNames()
    {
        List<string> shuffled = new List<string>(names);
        for (int i = 0; i < shuffled.Count; i++)
        {
            int randIndex = Random.Range(i, shuffled.Count);
            (shuffled[i], shuffled[randIndex]) = (shuffled[randIndex], shuffled[i]);
        }

        namesText.text = string.Join("\n", shuffled);
    }
}
