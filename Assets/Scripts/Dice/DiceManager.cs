using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DiceManager : MonoBehaviour
{
    public Image diceImage;
    public Sprite[] diceFaces;

    public float rollDuration = 1f;
    public float rotationSpeed = 720f;
    public float bounceHeight = 80f;     // چقدر بالا بپره
    public int bounceCount = 3;          // چند بار بالا-پایین بشه

    private int lastRoll;
    private bool isRolling = false;
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = diceImage.transform.localPosition;
    }

    public void RollDice()
    {
        if (isRolling) return;

        StartCoroutine(RollAnimation());
    }

    private IEnumerator RollAnimation()
    {
        isRolling = true;

        float elapsed = 0f;
        float interval = 0.07f;
        float intervalTimer = 0f;

        while (elapsed < rollDuration)
        {
            // چرخش پیوسته
            diceImage.transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);

            // محاسبه ارتفاع بالا-پایین (Bounce) که کم‌کم کمتر میشه
            float progress = elapsed / rollDuration; // از 0 تا 1
            float damping = 1f - progress; // هرچی جلوتر بریم، ارتفاع کمتر میشه
            float bounce = Mathf.Abs(Mathf.Sin(progress * bounceCount * Mathf.PI)) * bounceHeight * damping;

            diceImage.transform.localPosition = originalPosition + new Vector3(0f, bounce, 0f);

            // عوض کردن عکس هر چند فریم یک‌بار
            intervalTimer += Time.deltaTime;
            if (intervalTimer >= interval)
            {
                int randomFace = Random.Range(0, diceFaces.Length);
                diceImage.sprite = diceFaces[randomFace];
                intervalTimer = 0f;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        // برگردوندن به حالت اولیه (صاف و سرجاش)
        diceImage.transform.rotation = Quaternion.identity;
        diceImage.transform.localPosition = originalPosition;

        // عدد نهایی
        lastRoll = Random.Range(1, 7);
        diceImage.sprite = diceFaces[lastRoll - 1];

        Debug.Log("Player rolled: " + lastRoll);

        isRolling = false;
    }

    public int GetLastRoll()
    {
        return lastRoll;
    }
}