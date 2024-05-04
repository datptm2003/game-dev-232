using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public static GameObject itemBeingDragged;
    public static GameObject numHolderOfItemBeingDragged;
    Vector3 startPosition;
    Transform startParent;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        // Kiểm tra xem thành phần CanvasGroup có tồn tại không trước khi truy cập
        if (TryGetComponent(out canvasGroup) == false)
        {
            // Thêm CanvasGroup nếu không tồn tại
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        // Kiểm tra xem canvasGroup có tồn tại không trước khi sử dụng
        if (canvasGroup != null)
        {
            canvasGroup.alpha = .6f;
            canvasGroup.blocksRaycasts = false;
        }

        startPosition = transform.position;
        startParent = transform.parent;

        transform.SetParent(transform.root);
        itemBeingDragged = gameObject;

        List<GameObject> slotList = InventorySystem.Instance.slotList;
        for (int i = 0; i < slotList.Count; i++)
        {
            if (slotList[i] != null && slotList[i].transform != null && slotList[i].transform.childCount > 0)
            {
                if (slotList[i].transform.GetChild(0).name == "numHolder(Clone)")
                {
                    print("Here1");
                    numHolderOfItemBeingDragged = slotList[i].transform.GetChild(0).gameObject;
                    break;
                }
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {

        if (canvas != null)
        {
            // Thực hiện kéo thả vật phẩm
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
        else
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 0;
            transform.position = mousePos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        itemBeingDragged = null;

        if (transform.parent == startParent || transform.parent == transform.root)
        {
            transform.position = startPosition;
            transform.SetParent(startParent);
        }

        Debug.Log("OnEndDrag");
        // Kiểm tra xem canvasGroup có tồn tại không trước khi sử dụng
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }
    }
}
