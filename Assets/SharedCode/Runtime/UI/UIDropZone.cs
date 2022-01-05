using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class UIDropZone : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	public void OnPointerEnter(PointerEventData eventData)
	{  
		print ("OnPointerEnter");
	} 
	public void OnPointerExit(PointerEventData eventData)
	{  
		print ("OnPointerExit");
	} 
}
