using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResizePanel : MonoBehaviour,IPointerDownHandler,IDragHandler
{
	[SerializeField] private Vector2 minSize;    

    private RectTransform canvasRectTransform;
    private RectTransform panelRectTransform;
    private RectTransform thisRectTransform;
    private Vector2 currentPointerPosition;
	private Vector2 previousPointerPosition;



	void Awake()
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            canvasRectTransform = canvas.transform as RectTransform;            
        }
        panelRectTransform = transform.parent.GetComponent<RectTransform> ();
        thisRectTransform = GetComponent<RectTransform>();
	}

	public void OnPointerDown(PointerEventData data)
    {
		panelRectTransform.SetAsLastSibling ();
		RectTransformUtility.ScreenPointToLocalPointInRectangle (panelRectTransform, data.position, data.pressEventCamera, out previousPointerPosition);
	}

	public void OnDrag(PointerEventData data)
    {
		if (panelRectTransform == null) {
			return;
		}

        Vector2 pointerPosition = ClampToCanvas(data);        

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(panelRectTransform, pointerPosition, data.pressEventCamera, out
            currentPointerPosition))
        {
            Vector2 sizeDelta = panelRectTransform.sizeDelta;
            Vector2 resizeValue = currentPointerPosition - previousPointerPosition;            
            sizeDelta += new Vector2(resizeValue.x, -resizeValue.y);

            if(sizeDelta.x>minSize.x)
            {                
                panelRectTransform.sizeDelta = sizeDelta;
                previousPointerPosition.x = currentPointerPosition.x;            
            }

            if (sizeDelta.y > minSize.y)
            {
                panelRectTransform.sizeDelta = sizeDelta;
                previousPointerPosition.y = currentPointerPosition.y;                
            }

            if (sizeDelta.x < minSize.x )
            {
                sizeDelta.x= minSize.x;
                panelRectTransform.sizeDelta = sizeDelta;
                previousPointerPosition.x = thisRectTransform.localPosition.x;
            }

            if(sizeDelta.y < minSize.y)
            {
                sizeDelta.y = minSize.y;
                panelRectTransform.sizeDelta = sizeDelta;
                previousPointerPosition.y = thisRectTransform.localPosition.y;
            }
        }      
	}

    private Vector2 ClampToCanvas(PointerEventData data)
    {
        Vector2 rawPointerPosition = data.position;
        Vector3[] canvasCorners = new Vector3[4];

        canvasRectTransform.GetWorldCorners(canvasCorners);

        float clampedX = Mathf.Clamp(rawPointerPosition.x, canvasCorners[0].x + Screen.width * 0.02f, canvasCorners[2].x - Screen.width * 0.02f);
        float clampedY = Mathf.Clamp(rawPointerPosition.y, canvasCorners[0].y + Screen.height * 0.02f, canvasCorners[2].y - Screen.height * 0.02f);

        Vector2 newPointerPosition = new Vector2(clampedX, clampedY);
        return newPointerPosition;
    }
}
