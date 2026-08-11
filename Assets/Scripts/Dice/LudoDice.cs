using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LudoDice : MonoBehaviour
{
    public Image diceImage;
    public Sprite[] diceFaces; // 0 تا 5

    public float rollDuration = 1f;
    public float rotationSpeed = 360f;
    public float bounceHeight = 50f;
    public int bounceCount = 3;

    public int lastRoll;
    public Button rollButton;

    private float changeFaceTime = 0.08f;

    private bool isRolling = false;

    public void Roll()
    {
        if (isRolling) return;

        StartCoroutine(RollAnimation());
    }

    IEnumerator RollAnimation()
    {
        isRolling = true;

        float elapsed = 0f;
        float faceTimer = 0f;

        RectTransform rt = diceImage.GetComponent<RectTransform>();
        Vector2 originalPos = rt.anchoredPosition;

        while (elapsed < rollDuration)
        {
            // چرخش نرم
            rt.Rotate(0, 0, -rotationSpeed * Time.deltaTime);


            // پرش تاس
            float progress = elapsed / rollDuration;

            float bounce =
                Mathf.Abs(Mathf.Sin(progress * Mathf.PI * bounceCount))
                * bounceHeight
                * (1 - progress);


            rt.anchoredPosition =
                originalPos + new Vector2(0, bounce);


            // تغییر عدد تاس
            faceTimer += Time.deltaTime;

            if (faceTimer >= changeFaceTime)
            {
                diceImage.sprite =
                    diceFaces[Random.Range(0, diceFaces.Length)];

                faceTimer = 0;
            }


            elapsed += Time.deltaTime;

            yield return null;
        }


        // عدد نهایی
        lastRoll = Random.Range(1, 7);

        diceImage.sprite =
            diceFaces[lastRoll - 1];


        // برگشت صاف
        rt.rotation = Quaternion.identity;
        rt.anchoredPosition = originalPos;


        Debug.Log("Dice Result: " + lastRoll);
        LudoGameManager.Instance.DiceRolled(lastRoll);


        isRolling = false;
    }
    public void EnableRoll()
    {
        rollButton.interactable = true;
    }
    public void DisableRoll()
    {
        rollButton.interactable = false;
    }
}