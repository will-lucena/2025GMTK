using UnityEngine;
using UnityEngine.UI;

public class EnemyTurnProgressUI : MonoBehaviour
{
    public Image fillImage;

    private float duration = 2f; // How long the bar fills
    private float timer = 0f;
    private bool active = false;

    public System.Action OnComplete;

    public void StartProgress(float time)
    {
        duration = time;
        timer = 0f;
        active = true;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!active) return;

        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / duration);
        fillImage.fillAmount = t;

        if (t >= 1f)
        {
            active = false;
            gameObject.SetActive(false);
            OnComplete?.Invoke();
        }
    }
}
