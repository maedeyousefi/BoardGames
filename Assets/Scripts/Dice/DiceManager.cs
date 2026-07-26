using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DiceManager : MonoBehaviour
{
    public Image diceImage;
    public Sprite[] diceFaces; // اینجا 6 تا عکس رو به ترتیب میذاریم (dice1 تا dice6)

    private int lastRoll;
    private bool isRolling = false;

    public void RollDice()
    {
        if (isRolling) return; // اگه در حال چرخیدنه، اجازه نده دوباره کلیک بشه

        StartCoroutine(RollAnimation());
    }

    private IEnumerator RollAnimation()
    {
        isRolling = true;

        float rollDuration = 2f; // چقدر طول بکشه چرخش (به ثانیه)
        float elapsed = 0f;
        float interval = 0.07f; // هر چند ثانیه عکس عوض بشه

        while (elapsed < rollDuration)
        {
            int randomFace = Random.Range(0, diceFaces.Length);
            diceImage.sprite = diceFaces[randomFace];

            yield return new WaitForSeconds(interval);
            elapsed += interval;
        }

        // در آخر، عدد نهایی رو مشخص کن
        lastRoll = Random.Range(1, 7); // بین 1 تا 6
        diceImage.sprite = diceFaces[lastRoll - 1]; // چون آرایه از 0 شروع میشه

        Debug.Log("Player rolled: " + lastRoll);

        isRolling = false;
    }

    public int GetLastRoll()
    {
        return lastRoll;
    }
}