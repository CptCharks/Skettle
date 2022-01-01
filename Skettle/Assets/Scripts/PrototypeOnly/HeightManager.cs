using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightManager : MonoBehaviour
{
	[System.Serializable]
	public class LayerInfo
	{
		public int layer;
		public bool enabled;
		public List<GameObject> objectsToEnable = new List<GameObject>();
		
		public void Enable()
		{
			foreach(GameObject go in objectsToEnable)
			{
				go.SetActive(true);
			}
		}
		
		public void Disable()
		{
			foreach(GameObject go in objectsToEnable)
			{
				go.SetActive(false);
			}
		}
	}

	public List<LayerInfo> layers = new List<LayerInfo>();

	public int startingLayer;

	void Awake()
	{
		foreach(LayerInfo li in layers)
		{
			if(li.layer == startingLayer)
			{
				li.Enable();
			}
			else
			{
				li.Disable();
			}
		}
	}
	
	public void GoToLayer(int layer)
	{
		foreach(LayerInfo li in layers)
		{
			if(li.layer == layer)
			{
				li.Enable();
			}
			else
			{
				li.Disable();
			}
		}
	}
}
