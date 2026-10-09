using System;
using System.Collections.Generic;
using UnityEngine;

public class Decal : MonoBehaviour
{
	public static int dCount;

	[HideInInspector]
	public GameObject[] affectedObjects;

	public float maxAngle = 90f;

	private float angleCosine;

	[HideInInspector]
	public Bounds bounds;

	[HideInInspector]
	public float previousUVAngle;

	[HideInInspector]
	public Vector3 previousPosition;

	[HideInInspector]
	public Quaternion previousRotation;

	[HideInInspector]
	public Vector3 previousScale;

	[HideInInspector]
	public float previousMaxAngle;

	[HideInInspector]
	public float previousPushDistance = 0.009f;

	[HideInInspector]
	public Vector2 previousTiling;

	[HideInInspector]
	public Vector2 previousOffset;

	[HideInInspector]
	public bool useMeshCollider;

	public Vector2 tiling = Vector2.one;

	public Vector2 offset = Vector2.zero;

	[HideInInspector]
	public float uvAngle;

	private float uCos;

	private float vSin;

	public Material decalMaterial;

	[HideInInspector]
	public DecalMode decalMode;

	private List<DecalPolygon> startPolygons;

	private List<DecalPolygon> clippedPolygons;

	[HideInInspector]
	public Vector4 bottomPlane;

	[HideInInspector]
	public Vector4 topPlane;

	[HideInInspector]
	public Vector4 leftPlane;

	[HideInInspector]
	public Vector4 rightPlane;

	[HideInInspector]
	public Vector4 frontPlane;

	[HideInInspector]
	public Vector4 backPlane;

	[HideInInspector]
	public Vector3 decalNormal;

	[HideInInspector]
	public Vector3 decalCenter;

	[HideInInspector]
	public Vector3 decalTangent;

	[HideInInspector]
	public Vector3 decalBinormal;

	[HideInInspector]
	public Vector3 decalSize;

	public float pushDistance = 0.009f;

	private List<MeshCombineUtility.MeshInstance> instancesList;

	public bool checkAutomatically;

	public LayerMask affectedLayers;

	public bool affectOtherDecals;

	public bool affectInactiveRenderers;

	[HideInInspector]
	public bool showAffectedObjectsOptions;

	[HideInInspector]
	public bool showObjects;

	private void OnDrawGizmosSelected()
	{
		CalculateBounds();
		Gizmos.matrix = base.transform.localToWorldMatrix;
		Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
	}

	public void CalculateBounds()
	{
		bounds = new Bounds(base.transform.position, base.transform.lossyScale * 1.414214f);
	}

	public void CalculateDecal()
	{
		ClearDecals();
		maxAngle = Mathf.Clamp(maxAngle, 0f, 180f);
		angleCosine = Mathf.Cos(maxAngle * ((float)Math.PI / 180f));
		uvAngle = Mathf.Clamp(uvAngle, 0f, 360f);
		uCos = Mathf.Cos(uvAngle * ((float)Math.PI / 180f));
		vSin = Mathf.Sin(uvAngle * ((float)Math.PI / 180f));
		if (affectedObjects == null || affectedObjects.Length == 0)
		{
			return;
		}
		Matrix4x4 worldToLocalMatrix = base.transform.worldToLocalMatrix;
		instancesList = new List<MeshCombineUtility.MeshInstance>();
		for (int i = 0; i < affectedObjects.Length; i++)
		{
			if (!(affectedObjects[i] == null))
			{
				CalculateObjectDecal(affectedObjects[i], worldToLocalMatrix);
			}
		}
		if (instancesList.Count > 0)
		{
			MeshCombineUtility.MeshInstance[] array = new MeshCombineUtility.MeshInstance[instancesList.Count];
			for (int j = 0; j < array.Length; j++)
			{
				array[j] = instancesList[j];
			}
			MeshRenderer meshRenderer = base.gameObject.GetComponent<MeshRenderer>();
			if (meshRenderer == null)
			{
				meshRenderer = base.gameObject.AddComponent<MeshRenderer>();
			}
			meshRenderer.material = decalMaterial;
			MeshFilter meshFilter = base.gameObject.GetComponent<MeshFilter>();
			if (meshFilter == null)
			{
				meshFilter = base.gameObject.AddComponent<MeshFilter>();
			}
			else
			{
				UnityEngine.Object.DestroyImmediate(meshFilter.sharedMesh);
			}
			Mesh mesh = MeshCombineUtility.Combine(array, true);
			if (pushDistance > 0f)
			{
				List<List<int>> list = new List<List<int>>();
				Vector3[] vertices = mesh.vertices;
				Vector3[] normals = mesh.normals;
				bool[] array2 = new bool[vertices.Length];
				for (int k = 0; k < array2.Length; k++)
				{
					array2[k] = false;
				}
				for (int l = 0; l < vertices.Length; l++)
				{
					if (array2[l])
					{
						continue;
					}
					List<int> list2 = new List<int>();
					list2.Add(l);
					array2[l] = true;
					for (int m = l + 1; m < vertices.Length; m++)
					{
						if (!array2[m] && Vector3.Distance(vertices[l], vertices[m]) < 0.001f)
						{
							list2.Add(m);
							array2[m] = true;
						}
					}
					list.Add(list2);
				}
				foreach (List<int> item in list)
				{
					Vector3 zero = Vector3.zero;
					foreach (int item2 in item)
					{
						zero += normals[item2];
					}
					zero = (zero / item.Count).normalized;
					foreach (int item3 in item)
					{
						vertices[item3] += zero * pushDistance;
					}
				}
				mesh.vertices = vertices;
			}
			mesh.name = "DecalMesh";
			meshFilter.mesh = mesh;
			for (int n = 0; n < instancesList.Count; n++)
			{
				UnityEngine.Object.DestroyImmediate(instancesList[n].mesh);
			}
		}
		instancesList.Clear();
		instancesList = null;
	}

	public void ClearDecals()
	{
		MeshFilter component = base.gameObject.GetComponent<MeshFilter>();
		if (component != null)
		{
			UnityEngine.Object.DestroyImmediate(component.sharedMesh);
			UnityEngine.Object.DestroyImmediate(component);
		}
		MeshRenderer component2 = base.gameObject.GetComponent<MeshRenderer>();
		if (component2 != null)
		{
			UnityEngine.Object.DestroyImmediate(component2);
		}
	}

	public void CalculateObjectDecal(GameObject obj, Matrix4x4 cTransform)
	{
		Mesh mesh = null;
		if (decalMode == DecalMode.MESH_COLLIDER)
		{
			mesh = ((!(obj.GetComponent<MeshCollider>() != null)) ? null : obj.GetComponent<MeshCollider>().sharedMesh);
		}
		if (mesh == null || decalMode == DecalMode.MESH_FILTER)
		{
			if (obj.GetComponent<MeshFilter>() == null)
			{
				return;
			}
			mesh = obj.GetComponent<MeshFilter>().sharedMesh;
		}
		if (mesh == null || mesh.name.ToLower().Contains("combined"))
		{
			return;
		}
		decalNormal = obj.transform.InverseTransformDirection(base.transform.forward);
		decalCenter = obj.transform.InverseTransformPoint(base.transform.position);
		decalTangent = obj.transform.InverseTransformDirection(base.transform.right);
		decalBinormal = obj.transform.InverseTransformDirection(base.transform.up);
		decalSize = new Vector3(base.transform.lossyScale.x / obj.transform.lossyScale.x, base.transform.lossyScale.y / obj.transform.lossyScale.y, base.transform.lossyScale.z / obj.transform.lossyScale.z);
		bottomPlane = new Vector4(0f - decalBinormal.x, 0f - decalBinormal.y, 0f - decalBinormal.z, decalSize.y * 0.5f + Vector3.Dot(decalCenter, decalBinormal));
		topPlane = new Vector4(decalBinormal.x, decalBinormal.y, decalBinormal.z, decalSize.y * 0.5f - Vector3.Dot(decalCenter, decalBinormal));
		rightPlane = new Vector4(0f - decalTangent.x, 0f - decalTangent.y, 0f - decalTangent.z, decalSize.x * 0.5f + Vector3.Dot(decalCenter, decalTangent));
		leftPlane = new Vector4(decalTangent.x, decalTangent.y, decalTangent.z, decalSize.x * 0.5f - Vector3.Dot(decalCenter, decalTangent));
		frontPlane = new Vector4(decalNormal.x, decalNormal.y, decalNormal.z, decalSize.z * 0.5f - Vector3.Dot(decalCenter, decalNormal));
		backPlane = new Vector4(0f - decalNormal.x, 0f - decalNormal.y, 0f - decalNormal.z, decalSize.z * 0.5f + Vector3.Dot(decalCenter, decalNormal));
		int[] triangles = mesh.triangles;
		Vector3[] vertices = mesh.vertices;
		Vector3[] normals = mesh.normals;
		startPolygons = new List<DecalPolygon>();
		for (int i = 0; i < triangles.Length; i += 3)
		{
			int num = triangles[i];
			int num2 = triangles[i + 1];
			int num3 = triangles[i + 2];
			Vector3 vector = vertices[num];
			Vector3 vector2 = vertices[num2];
			Vector3 vector3 = vertices[num3];
			Vector3 vector4 = normals[num];
			float num4 = Vector3.Dot(vector4, -decalNormal);
			if (!(num4 <= angleCosine))
			{
				DecalPolygon decalPolygon = new DecalPolygon();
				decalPolygon.verticeCount = 3;
				decalPolygon.vertice[0] = vector;
				decalPolygon.vertice[1] = vector2;
				decalPolygon.vertice[2] = vector3;
				decalPolygon.normal[0] = vector4;
				decalPolygon.normal[1] = vector4;
				decalPolygon.normal[2] = vector4;
				decalPolygon.tangent[0] = Vector4.zero;
				decalPolygon.tangent[1] = Vector4.zero;
				decalPolygon.tangent[2] = Vector4.zero;
				startPolygons.Add(decalPolygon);
			}
		}
		Mesh mesh2 = CreateMesh(ClipMesh(), obj.transform);
		if (mesh2 != null)
		{
			MeshCombineUtility.MeshInstance item = default(MeshCombineUtility.MeshInstance);
			item.mesh = mesh2;
			item.subMeshIndex = 0;
			item.transform = base.transform.worldToLocalMatrix * obj.transform.localToWorldMatrix;
			instancesList.Add(item);
		}
		mesh2 = null;
		startPolygons.Clear();
		startPolygons = null;
		clippedPolygons.Clear();
		clippedPolygons = null;
		triangles = null;
		normals = null;
		vertices = null;
		GC.Collect();
	}

	private Mesh CreateMesh(int totalVertices, Transform to)
	{
		if (clippedPolygons == null)
		{
			return null;
		}
		if (clippedPolygons.Count <= 0)
		{
			return null;
		}
		if (totalVertices < 3)
		{
			return null;
		}
		int[] array = new int[(totalVertices - 2) * 3];
		Vector3[] array2 = new Vector3[totalVertices];
		Vector3[] array3 = new Vector3[totalVertices];
		Vector2[] array4 = new Vector2[totalVertices];
		Vector4[] array5 = new Vector4[totalVertices];
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		float num4 = 1f / decalSize.x;
		float num5 = 1f / decalSize.y;
		foreach (DecalPolygon clippedPolygon in clippedPolygons)
		{
			for (int i = 0; i < clippedPolygon.verticeCount; i++)
			{
				array2[num] = clippedPolygon.vertice[i];
				array3[num] = clippedPolygon.normal[i];
				array5[num] = clippedPolygon.tangent[i];
				if (i < clippedPolygon.verticeCount - 2)
				{
					array[num2] = num3;
					array[num2 + 1] = num + 1;
					array[num2 + 2] = num + 2;
					num2 += 3;
				}
				num++;
			}
			num3 = num;
		}
		for (int j = 0; j < array2.Length; j++)
		{
			Vector3 lhs = array2[j] - decalCenter;
			float num6 = Vector3.Dot(lhs, decalTangent) * num4;
			float num7 = Vector3.Dot(lhs, decalBinormal) * num5;
			float num8 = num6 * uCos - num7 * vSin + 0.5f;
			float num9 = num6 * vSin + num7 * uCos + 0.5f;
			num8 *= tiling.x;
			num9 *= tiling.y;
			num8 += offset.x;
			num9 += offset.y;
			array4[j] = new Vector2(num8, num9);
		}
		Mesh mesh = new Mesh();
		mesh.vertices = array2;
		mesh.normals = array3;
		mesh.triangles = array;
		mesh.uv = array4;
		mesh.uv2 = array4;
		mesh.uv2 = array4;
		mesh.tangents = array5;
		return mesh;
	}

	private int ClipMesh()
	{
		int num = 0;
		if (clippedPolygons == null)
		{
			clippedPolygons = new List<DecalPolygon>();
		}
		else
		{
			clippedPolygons.Clear();
		}
		for (int i = 0; i < startPolygons.Count; i++)
		{
			DecalPolygon polygon = startPolygons[i];
			DecalPolygon decalPolygon = DecalPolygon.ClipPolygonAgainstPlane(polygon, frontPlane);
			if (decalPolygon == null)
			{
				continue;
			}
			decalPolygon = DecalPolygon.ClipPolygonAgainstPlane(decalPolygon, backPlane);
			if (decalPolygon == null)
			{
				continue;
			}
			decalPolygon = DecalPolygon.ClipPolygonAgainstPlane(decalPolygon, rightPlane);
			if (decalPolygon == null)
			{
				continue;
			}
			decalPolygon = DecalPolygon.ClipPolygonAgainstPlane(decalPolygon, leftPlane);
			if (decalPolygon == null)
			{
				continue;
			}
			decalPolygon = DecalPolygon.ClipPolygonAgainstPlane(decalPolygon, bottomPlane);
			if (decalPolygon != null)
			{
				decalPolygon = DecalPolygon.ClipPolygonAgainstPlane(decalPolygon, topPlane);
				if (decalPolygon != null)
				{
					num += decalPolygon.verticeCount;
					clippedPolygons.Add(decalPolygon);
				}
			}
		}
		return num;
	}

	public bool HasChanged()
	{
		Transform transform = base.transform;
		bool result = false;
		maxAngle = Mathf.Clamp(maxAngle, 0f, 180f);
		uvAngle = Mathf.Clamp(uvAngle, 0f, 360f);
		if (previousPosition != transform.position)
		{
			result = true;
		}
		else if (previousScale != transform.lossyScale)
		{
			result = true;
		}
		else if (previousRotation != transform.rotation)
		{
			result = true;
		}
		else if (previousPushDistance != pushDistance)
		{
			result = true;
		}
		else if (previousTiling != tiling)
		{
			result = true;
		}
		else if (previousOffset != offset)
		{
			result = true;
		}
		else if (previousMaxAngle != maxAngle)
		{
			result = true;
		}
		else if (previousUVAngle != uvAngle)
		{
			result = true;
		}
		previousUVAngle = uvAngle;
		previousMaxAngle = maxAngle;
		previousTiling = tiling;
		previousOffset = offset;
		previousPushDistance = pushDistance;
		previousPosition = transform.position;
		previousRotation = transform.rotation;
		previousScale = transform.lossyScale;
		return result;
	}
}
