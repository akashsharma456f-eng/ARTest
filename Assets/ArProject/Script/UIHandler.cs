using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private Image TakeProhance;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float Duration;
    private Coroutine canvasGroupCor;

    private void Awake()
    {
        canvasGroup.alpha = 0;
    }
    private void Start()
    {  
        EventHandler.Instance.Subscribe(GlobleEventEnum.Show, Show);
        EventHandler.Instance.Subscribe(GlobleEventEnum.Hide, Hide);
    }


    public void Show()
    {
        canvasGroupCor = StartCoroutine(FadeCanvasGroup(0f, 1f, 2f));
        TakeProhance.gameObject.SetActive(true);        
        Invoke("Hide", 3);
    }
    public void Hide()
    {
        canvasGroupCor = StartCoroutine(FadeCanvasGroup(1f, 0f, 1f));        
    }

    public IEnumerator FadeCanvasGroup(float startAlpha, float endAlpha, float time)
    {
        float elapsedTime = 0f;

        canvasGroup.alpha = startAlpha;

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / time;           
            t = Mathf.SmoothStep(0f, 1f, t);

            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t);

            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }
    private void OnDisable()
    {
        if(canvasGroupCor!=null)
        StopCoroutine(canvasGroupCor);
        canvasGroup.alpha = 0;
        EventHandler.Instance.UnSubscribe(GlobleEventEnum.Show, Show);
        EventHandler.Instance.UnSubscribe(GlobleEventEnum.Hide, Hide);
        CancelInvoke("Hide");
       
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Splash");
        }
    }

}
