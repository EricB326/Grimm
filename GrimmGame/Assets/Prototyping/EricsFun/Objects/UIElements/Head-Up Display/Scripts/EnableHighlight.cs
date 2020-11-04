using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnableHighlight : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Button button;
    public Image highlighted;

	public void OnPointerEnter(PointerEventData eventData)
	{
		highlighted.gameObject.SetActive(true);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		highlighted.gameObject.SetActive(false);
	}

	public void OnSelect(BaseEventData eventData)
	{
		highlighted.gameObject.SetActive(true);
	}

	public void OnDeselect(BaseEventData eventData)
	{
		highlighted.gameObject.SetActive(false);
	}
}
