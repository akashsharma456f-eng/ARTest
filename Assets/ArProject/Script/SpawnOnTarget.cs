using UnityEngine;
using Vuforia;

public class SpawnOnTarget : MonoBehaviour
{
    public GameObject modelPrefab;
    public GameObject playButton;

    [Range(0.1f, 1f)]
    public float coveragePercent = 0.6f; // 🔥 how much area object should cover

    private ObserverBehaviour observer;
    private GameObject spawnedObject;
    private Animator animator;

    private bool isVisible = false;
    [SerializeField] private bool useCustomSscale;
    [SerializeField] private float customScaleValue = 1;
    [SerializeField] private bool useCustomHeight;
    [SerializeField] private float customHeightValue = 1;



    void Start()
    {
        observer = GetComponent<ObserverBehaviour>();

        if (observer != null)
            observer.OnTargetStatusChanged += OnStatusChanged;

        if (playButton != null)
            playButton.SetActive(false);
    }

    private void OnStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        Debug.Log("#### 📡 Tracking Status: " + status.Status);

        bool tracked = status.Status == Status.TRACKED;

        if (tracked)
        {
            if (spawnedObject == null)
                SpawnObject();

            if (!isVisible)
            {
                spawnedObject.SetActive(true);
                isVisible = true;

                if (playButton != null)
                    playButton.SetActive(true);
            }
        }
        else
        {
            if (spawnedObject != null && isVisible)
            {
                spawnedObject.SetActive(false);
                isVisible = false;

                if (playButton != null)
                    playButton.SetActive(false);
            }
        }
    }

    void SpawnObject()
    {
        spawnedObject = Instantiate(modelPrefab, transform);

        // Reset transform
        spawnedObject.transform.localRotation = Quaternion.identity;

        Renderer r = spawnedObject.GetComponentInChildren<Renderer>();

        if (r != null)
        {
            // 🔥 TARGET SIZE (from Vuforia width)
            float targetWidth = transform.localScale.x;

            Debug.Log("#### 📐 Target Width: " + targetWidth);

            // 🔥 MODEL SIZE
            Vector3 size = r.bounds.size;
            float modelSize = Mathf.Max(size.x, size.z);

            Debug.Log("#### 📦 Model Size: " + size);

            // 🔥 SCALE BASED ON TARGET WIDTH
            float desiredSize = targetWidth * coveragePercent;
            float scaleFactor = desiredSize / modelSize;

            spawnedObject.transform.localScale = Vector3.one * (useCustomSscale ? customScaleValue : scaleFactor);

            Debug.Log("#### 🔧 Scale Factor: " + scaleFactor);

            // 🔥 AUTO HEIGHT (place on image)
            float height = r.bounds.extents.y * scaleFactor;
            spawnedObject.transform.localPosition = Vector3.up * (useCustomHeight ? customHeightValue : height);

            Debug.Log("#### 📏 Height: " + height);
        }
        else
        {
            Debug.LogWarning("#### ⚠ Renderer not found");

            spawnedObject.transform.localScale = Vector3.one * 0.1f;
            spawnedObject.transform.localPosition = new Vector3(0f, 0.05f, 0f);
        }

        // Animator
        animator = spawnedObject.GetComponent<Animator>();
        if (animator != null)
            animator.enabled = false;

        Debug.Log("#### ✅ Object Spawned (Proper Scale)");
    }

    public void PlayAnimation()
    {
        if (animator != null)
        {
            animator.enabled = true;
            Debug.Log("#### 🎬 Animation Started");
        }
    }

    private void OnDestroy()
    {
        if (observer != null)
            observer.OnTargetStatusChanged -= OnStatusChanged;
    }

    public void SetCustomScaleValue(float scaleFactor)
    {
        spawnedObject.transform.localScale = Vector3.one * scaleFactor;
    }

    public void SetCustomHeightValue(float height)
    {
        spawnedObject.transform.localPosition = Vector3.up * height;
    }

    private void OnValidate()
    {
        if (spawnedObject != null)
        {
            if (spawnedObject.transform.localScale.y != customScaleValue)
            {
                spawnedObject.transform.localScale = Vector3.one * customScaleValue;
            }

            if (spawnedObject.transform.localPosition.y != customHeightValue)
            {
                spawnedObject.transform.localPosition = Vector3.up * customHeightValue;
            }
        }
    }
}