using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private Inventory playerInventory;
    private GameObject objectToPlace;
    [SerializeField] private LayerMask placeLayer;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private Vector3 placementCheckSize = new Vector3(1, 1, 1);

    private Transform playerCam;

    private GameObject phantomObject;

    private MaterialPropertyBlock block;

    private List<Renderer> objectRenderer = new List<Renderer>();

    private const string ColorPropertyName = "_BaseColor";

    private InventorySlot slot;

    private void OnEnable()
    {
        EventBus<SlotSelectedEvent>.Subscribe(SetObject);
    }
    private void OnDisable()
    {
        EventBus<SlotSelectedEvent>.Unsubscribe(SetObject);
    }
    private void Start()
    {
        block = new MaterialPropertyBlock();
        playerCam = Camera.main.transform;
    }
    private void Update()
    {
        TryPlace();
    }
    public void SetObject(SlotSelectedEvent evt)
    {
        slot = evt.InventorySlot;

        if (evt.InventorySlot.ItemInSlot is PlaceableItem placeable)
        {
            objectRenderer.Clear();
            objectToPlace = placeable.ObjectToPlace;
            if (objectToPlace != null)
            {
                if (phantomObject == null)
                {
                    phantomObject = Instantiate(objectToPlace);
                    phantomObject.SetActive(false);
                }
                else if (phantomObject != objectToPlace)
                {
                    Destroy(phantomObject);
                    phantomObject = Instantiate(objectToPlace);
                    phantomObject.SetActive(false);
                }
            }

            if (phantomObject.TryGetComponent(out Renderer renderer))
            {
                objectRenderer.Add(renderer);
                for (int i = 0; i < phantomObject.transform.childCount; i++)
                {
                    if (phantomObject.transform.GetChild(i).TryGetComponent(out Renderer childRenderer))
                    {
                        objectRenderer.Add(childRenderer);
                    }
                }
            }
            foreach (var obj in objectRenderer)
            {
                obj.GetPropertyBlock(block);
            }
        }
        else
        {
            Destroy(phantomObject);
        }
    }
    private void TryPlace()
    {
        if (phantomObject == null) return;

        RaycastHit hit;

        bool canPlace = Physics.Raycast(playerCam.position, playerCam.forward, out hit, 5f, placeLayer);

        if (canPlace)
        {
            phantomObject.SetActive(true);
            phantomObject.transform.position = hit.point;

            if (Input.GetKey(KeyCode.R))
            {
                phantomObject.transform.Rotate(Vector3.up, 45 * Time.deltaTime);
            }

            bool isValid = IsPlacementValid(hit.point, phantomObject.transform.rotation);

            block.SetColor(ColorPropertyName, isValid ? Color.green : Color.red);

            foreach (var obj in objectRenderer)
            {
                obj.SetPropertyBlock(block);
            }

            if (isValid && Input.GetButtonDown("Fire1"))
            {
                PlaceObject();
            }
        }
    }
    private void PlaceObject()
    {
        if (phantomObject.TryGetComponent(out IPlaceable placeable))
            placeable.Activate();
        if (phantomObject.TryGetComponent(out Collider col))
            col.enabled = true;

        foreach (var obj in objectRenderer)
        {
            block.Clear();
            obj.SetPropertyBlock(block);
        }
        playerInventory.RemoveItem(1, slot);

        phantomObject = null;
    }
    private bool IsPlacementValid(Vector3 position, Quaternion rotation)
    {
        Bounds localBounds = GetLocalBounds(phantomObject);

        Vector3 worldCenter = position + rotation * localBounds.center;
        Vector3 worldExtents = Vector3.Scale(localBounds.extents, phantomObject.transform.lossyScale);

        DrawDebugBox(worldCenter, worldExtents * 0.9f, rotation);

        Collider[] overlaps = Physics.OverlapBox(
            worldCenter,
            worldExtents * 0.9f,
            rotation,
            obstacleLayer
        );

        foreach (var col in overlaps)
        {
            Debug.Log("Obstacle detected: " + col.gameObject.name);
        }
        return overlaps.Length == 0;
    }

    private Bounds GetLocalBounds(GameObject obj)
    {
        MeshFilter[] meshFilters = obj.GetComponentsInChildren<MeshFilter>();

        if (meshFilters.Length == 0)
            return new Bounds(Vector3.zero, Vector3.one);

        bool initialized = false;
        Bounds bounds = new Bounds();

        foreach (var mf in meshFilters)
        {
            Bounds meshBounds = mf.sharedMesh.bounds;

            Vector3 localCenter = obj.transform.InverseTransformPoint(
                mf.transform.TransformPoint(meshBounds.center)
            );

            Vector3 relativeScale = new Vector3(
                mf.transform.lossyScale.x / obj.transform.lossyScale.x,
                mf.transform.lossyScale.y / obj.transform.lossyScale.y,
                mf.transform.lossyScale.z / obj.transform.lossyScale.z
            );
            Vector3 scaledExtents = Vector3.Scale(meshBounds.extents, relativeScale);

            Bounds localBounds = new Bounds(localCenter, scaledExtents * 2f);

            if (!initialized) { bounds = localBounds; initialized = true; }
            else bounds.Encapsulate(localBounds);
        }

        return bounds;
    }
    private void DrawDebugBox(Vector3 center, Vector3 extents, Quaternion rotation)
    {
        Vector3[] corners = new Vector3[8];
        corners[0] = center + rotation * new Vector3(-extents.x, -extents.y, -extents.z);
        corners[1] = center + rotation * new Vector3(extents.x, -extents.y, -extents.z);
        corners[2] = center + rotation * new Vector3(-extents.x, extents.y, -extents.z);
        corners[3] = center + rotation * new Vector3(extents.x, extents.y, -extents.z);
        corners[4] = center + rotation * new Vector3(-extents.x, -extents.y, extents.z);
        corners[5] = center + rotation * new Vector3(extents.x, -extents.y, extents.z);
        corners[6] = center + rotation * new Vector3(-extents.x, extents.y, extents.z);
        corners[7] = center + rotation * new Vector3(extents.x, extents.y, extents.z);

        Debug.DrawLine(corners[0], corners[1], Color.cyan, 0.1f);
        Debug.DrawLine(corners[0], corners[2], Color.cyan, 0.1f);
        Debug.DrawLine(corners[1], corners[3], Color.cyan, 0.1f);
        Debug.DrawLine(corners[2], corners[3], Color.cyan, 0.1f);
        Debug.DrawLine(corners[4], corners[5], Color.cyan, 0.1f);
        Debug.DrawLine(corners[4], corners[6], Color.cyan, 0.1f);
        Debug.DrawLine(corners[5], corners[7], Color.cyan, 0.1f);
        Debug.DrawLine(corners[6], corners[7], Color.cyan, 0.1f);
        Debug.DrawLine(corners[0], corners[4], Color.cyan, 0.1f);
        Debug.DrawLine(corners[1], corners[5], Color.cyan, 0.1f);
        Debug.DrawLine(corners[2], corners[6], Color.cyan, 0.1f);
        Debug.DrawLine(corners[3], corners[7], Color.cyan, 0.1f);
    }
}
