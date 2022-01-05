using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIDraggable : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {
	RectTransform rt;
	MaskableGraphic mg;
 
	public bool resetOnDragEnd;
	Vector3 startPos;

	public bool applyBounds;
	//-x,-y,+x,+y
	public Vector4 customBounds;
	static Canvas canvas; 
	static Vector4 canvasBounds;

	public UnityEvent onHitBounds;
	public bool boundsEventFired; 

	void OnEnable(){
		if (canvas==null) {
			canvas = FindObjectOfType<Canvas> ();
			canvasBounds = new Vector4 (canvas.worldCamera.ScreenToWorldPoint(Vector3.zero).x, 
										canvas.worldCamera.ScreenToWorldPoint(Vector3.zero).y, 
										canvas.worldCamera.ScreenToWorldPoint(new Vector3(Screen.width,0,0)).x, 
										canvas.worldCamera.ScreenToWorldPoint(new Vector3(0,Screen.height,0)).y);
		}

		if (rt == null) rt = GetComponent<RectTransform> ();
		if (mg == null) mg = GetComponent<MaskableGraphic> ();
        mg.raycastTarget = true;
    }

	public void OnBeginDrag(PointerEventData eventData)
	{  
		//print ("OnBeginDrag");
		mg.raycastTarget = false;
		startPos = rt.position; 
		lastPointerPos = currentPointerPos;
	} 

	Vector3 pointerDelta, lastPointerPos;
	Vector3 currentPointerPos{
		get{ 
			if (Input.mousePresent) {
				return canvas.worldCamera.ScreenToWorldPoint (Input.mousePosition);
			} else if (Input.touches.Length > 0) {
				return canvas.worldCamera.ScreenToWorldPoint (Input.touches [0].position); 
			} else
				return Vector3.zero;
		}
	}
	public void OnDrag(PointerEventData eventData)
	{ 
		pointerDelta = currentPointerPos - lastPointerPos;
		lastPointerPos = currentPointerPos; 
		int boundHit = 0; 
		Vector3 p = rt.position + pointerDelta;
		if (applyBounds) {
			if (p.x < startPos.x + customBounds.x) boundHit = 1; 
			if (p.y < startPos.y + customBounds.y) boundHit = 2; 
			if (p.x > startPos.x + customBounds.z) boundHit = 3; 
			if (p.y > startPos.y + customBounds.w) boundHit = 4;  
		}
		if ((p.x > canvasBounds.z) 
			|| (p.y > canvasBounds.w) 
			|| (p.x < canvasBounds.x) 
			|| (p.y < canvasBounds.y)) boundHit = 5; 

		if (boundHit > 0) {
			switch (boundHit) {
			case 1:
				p = new Vector3 (startPos.x + customBounds.x, rt.position.y, rt.position.z);
				if (!boundsEventFired && customBounds.x != 0) {
					onHitBounds.Invoke ();
					boundsEventFired = true;
				}
				break;
			case 2:
				p = new Vector3 (rt.position.x, startPos.y + customBounds.y, rt.position.z);
				if (!boundsEventFired && customBounds.y != 0) {
					onHitBounds.Invoke ();
					boundsEventFired = true;
				}
				break;
			case 3:
				p = new Vector3 (startPos.x + customBounds.z, rt.position.y, rt.position.z);
				if (!boundsEventFired && customBounds.z != 0) {
					onHitBounds.Invoke ();
					boundsEventFired = true;
				}
				break;
			case 4:
				p = new Vector3 (rt.position.x, startPos.y + customBounds.w, rt.position.z);
				if (!boundsEventFired && customBounds.w != 0) {
					onHitBounds.Invoke ();
					boundsEventFired = true;
				}
				break;
			default:
				p = rt.position;
				break;
			}
		} 
		rt.position = p; 
	}

	public void OnEndDrag(PointerEventData eventData)
	{  
		print ("OnEndDrag");
		mg.raycastTarget = true;
		boundsEventFired = false;
		if (resetOnDragEnd) {
			rt.position = startPos;
		}
	} 
}
