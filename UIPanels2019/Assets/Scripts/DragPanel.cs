using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragPanel : MonoBehaviour,IPointerDownHandler,IDragHandler
{
	private Vector2 pointerOffset;
	private RectTransform canvasRectTransform;
	private RectTransform panelRectTransform;



	private void Awake()
    {
		Canvas canvas = GetComponentInParent<Canvas> ();
		if (canvas != null) {
			canvasRectTransform = canvas.transform as RectTransform;
			panelRectTransform = transform.parent as RectTransform;
		}
	}

	public void OnPointerDown(PointerEventData data)
    {
		panelRectTransform.SetAsLastSibling ();
		RectTransformUtility.ScreenPointToLocalPointInRectangle (panelRectTransform, data.position, data.pressEventCamera, out pointerOffset);
	}

	public void OnDrag(PointerEventData data)
    {
		if (panelRectTransform == null) {
			return;
		}

		Vector2 pointerPosition = ClampToWindow (data);
		Vector2 localPointerPosition;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle (canvasRectTransform, pointerPosition, data.pressEventCamera, out 
			localPointerPosition)) {
			panelRectTransform.localPosition = localPointerPosition - pointerOffset;           
        }
	}

	private Vector2 ClampToWindow(PointerEventData data)
    {
		Vector2 rawPointerPosition = data.position;
		Vector3[] canvasCorners=new Vector3[4];

		canvasRectTransform.GetWorldCorners (canvasCorners);

		float clampedX = Mathf.Clamp (rawPointerPosition.x, canvasCorners [0].x+Screen.width*0.02f, canvasCorners [2].x-Screen.width*0.02f);
		float clampedY = Mathf.Clamp (rawPointerPosition.y, canvasCorners [0].y+Screen.height*0.02f, canvasCorners [2].y-Screen.height*0.02f);

		Vector2 newPointerPosition = new Vector2 (clampedX, clampedY);
		return newPointerPosition;
	}
}
