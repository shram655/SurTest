using System;
using UnityEngine;

[Serializable]
public class DrawWaypointGizmos_Script : MonoBehaviour
{
	public Color COLOR;

	public float size;

	public DrawWaypointGizmos_Script()
	{
		size = 4f;
	}

	public void OnDrawGizmos()
	{
		Component[] componentsInChildren = gameObject.GetComponentsInChildren(typeof(Transform));
		int i = 0;
		Component[] array = componentsInChildren;
		for (int length = array.Length; i < length; i++)
		{
			Gizmos.color = COLOR;
			Gizmos.DrawSphere(((Transform)array[i]).position, size);
		}
	}

	public void Main()
	{
	}
}
