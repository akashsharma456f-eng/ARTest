using UnityEngine;

public class WebARBridge : MonoBehaviour
{
    public GameObject arObject;

    private Vector3 targetPosition;
    private bool isPlaced = false;

    void Start()
    {
        //Screen.SetResolution(960, 540, false);

        Debug.Log("🟢 WebARBridge Initialized");

        if (arObject != null)
        {
            arObject.SetActive(true);
            arObject.transform.position = new Vector3(0, 0, 2);
            arObject.transform.localScale = Vector3.one * 0.2f;
        }
    }

    void Update()
    {
        if (!isPlaced && arObject != null)
        {
            arObject.transform.position = Vector3.Lerp(
                arObject.transform.position,
                targetPosition,
                Time.deltaTime * 10f
            );
        }
    }

    public void UpdatePosition(string data)
    {
        string[] v = data.Split(',');

        float x = float.Parse(v[0]);
        float y = float.Parse(v[1]);
        float z = float.Parse(v[2]);

        targetPosition = new Vector3(x, y, z);

        Debug.Log("📍 Unity Position: " + targetPosition);
    }

    public void PlaceObject(string v)
    {
        isPlaced = true;
        Debug.Log("📌 Placed");
    }

    public void RotateObject(string v)
    {
        float rot = float.Parse(v);
        arObject.transform.Rotate(Vector3.up, rot);
    }

    public void ScaleObject(string v)
    {
        float s = float.Parse(v);
        arObject.transform.localScale *= s;
    }

    public void ShowObject(string v)
    {
        if (!isPlaced) arObject.SetActive(true);
    }

    public void HideObject(string v)
    {
        if (!isPlaced) arObject.SetActive(false);
    }
}