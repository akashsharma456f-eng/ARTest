using UnityEngine;

public class Player : MonoBehaviour
{
    public void Show()
    {
        Debug.Log("called show");
        EventHandler.Instance.Notify(GlobleEventEnum.Show);
    }

    public void Hide()
    {
        Debug.Log("called Hide");
    }
}
