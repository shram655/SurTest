using UnityEngine;

public class MeshCombineUtility
{
	public struct MeshInstance
	{
		public Mesh mesh;

		public int subMeshIndex;

		public Matrix4x4 transform;
	}

	public static Mesh Combine(MeshInstance[] combines, bool generateStrips)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < combines.Length; i++)
		{
			MeshInstance meshInstance = combines[i];
			if (!meshInstance.mesh)
			{
				continue;
			}
			num += meshInstance.mesh.vertexCount;
			if (!generateStrips)
			{
				continue;
			}
			int num4 = meshInstance.mesh.GetTriangles(meshInstance.subMeshIndex).Length;
			if (num4 != 0)
			{
				if (num3 != 0)
				{
					num3 = (((num3 & 1) != 1) ? (num3 + 2) : (num3 + 3));
				}
				num3 += num4;
			}
			else
			{
				generateStrips = false;
			}
		}
		if (!generateStrips)
		{
			for (int j = 0; j < combines.Length; j++)
			{
				MeshInstance meshInstance2 = combines[j];
				if ((bool)meshInstance2.mesh)
				{
					num2 += meshInstance2.mesh.GetTriangles(meshInstance2.subMeshIndex).Length;
				}
			}
		}
		Vector3[] array = new Vector3[num];
		Vector3[] array2 = new Vector3[num];
		Vector4[] array3 = new Vector4[num];
		Vector2[] array4 = new Vector2[num];
		Vector2[] array5 = new Vector2[num];
		Vector2[] array6 = new Vector2[num];
		Color[] array7 = new Color[num];
		int[] array8 = new int[num2];
		int[] array9 = new int[num3];
		int offset = 0;
		for (int k = 0; k < combines.Length; k++)
		{
			MeshInstance meshInstance3 = combines[k];
			if ((bool)meshInstance3.mesh)
			{
				Copy(meshInstance3.mesh.vertexCount, meshInstance3.mesh.vertices, array, ref offset, meshInstance3.transform);
			}
		}
		offset = 0;
		for (int l = 0; l < combines.Length; l++)
		{
			MeshInstance meshInstance4 = combines[l];
			if ((bool)meshInstance4.mesh)
			{
				Matrix4x4 transform = meshInstance4.transform;
				transform = transform.inverse.transpose;
				CopyNormal(meshInstance4.mesh.vertexCount, meshInstance4.mesh.normals, array2, ref offset, transform);
			}
		}
		offset = 0;
		for (int m = 0; m < combines.Length; m++)
		{
			MeshInstance meshInstance5 = combines[m];
			if ((bool)meshInstance5.mesh)
			{
				Matrix4x4 transform2 = meshInstance5.transform;
				transform2 = transform2.inverse.transpose;
				CopyTangents(meshInstance5.mesh.vertexCount, meshInstance5.mesh.tangents, array3, ref offset, transform2);
			}
		}
		offset = 0;
		for (int n = 0; n < combines.Length; n++)
		{
			MeshInstance meshInstance6 = combines[n];
			if ((bool)meshInstance6.mesh)
			{
				Copy(meshInstance6.mesh.vertexCount, meshInstance6.mesh.uv, array4, ref offset);
			}
		}
		offset = 0;
		for (int num5 = 0; num5 < combines.Length; num5++)
		{
			MeshInstance meshInstance7 = combines[num5];
			if ((bool)meshInstance7.mesh)
			{
				Copy(meshInstance7.mesh.vertexCount, meshInstance7.mesh.uv2, array5, ref offset);
			}
		}
		float num6 = 0f;
		for (int num7 = 0; num7 < combines.Length; num7++)
		{
			MeshInstance meshInstance8 = combines[num7];
			if ((bool)meshInstance8.mesh)
			{
				num6 += meshInstance8.mesh.bounds.size.x * meshInstance8.mesh.bounds.size.y * meshInstance8.mesh.bounds.size.z;
			}
		}
		offset = 0;
		float num8 = 0f;
		for (int num9 = 0; num9 < combines.Length; num9++)
		{
			MeshInstance meshInstance9 = combines[num9];
			if ((bool)meshInstance9.mesh)
			{
				float num10 = meshInstance9.mesh.bounds.size.x * meshInstance9.mesh.bounds.size.y * meshInstance9.mesh.bounds.size.z / num6;
				CopyUV2(meshInstance9.mesh.vertexCount, meshInstance9.mesh.uv2, array6, ref offset, num10, num8);
				num8 += num10;
			}
		}
		offset = 0;
		for (int num11 = 0; num11 < combines.Length; num11++)
		{
			MeshInstance meshInstance10 = combines[num11];
			if ((bool)meshInstance10.mesh)
			{
				CopyColors(meshInstance10.mesh.vertexCount, meshInstance10.mesh.colors, array7, ref offset);
			}
		}
		int num12 = 0;
		int num13 = 0;
		int num14 = 0;
		for (int num15 = 0; num15 < combines.Length; num15++)
		{
			MeshInstance meshInstance11 = combines[num15];
			if (!meshInstance11.mesh)
			{
				continue;
			}
			if (generateStrips)
			{
				int[] triangles = meshInstance11.mesh.GetTriangles(meshInstance11.subMeshIndex);
				if (num13 != 0)
				{
					if ((num13 & 1) == 1)
					{
						array9[num13] = array9[num13 - 1];
						array9[num13 + 1] = triangles[0] + num14;
						array9[num13 + 2] = triangles[0] + num14;
						num13 += 3;
					}
					else
					{
						array9[num13] = array9[num13 - 1];
						array9[num13 + 1] = triangles[0] + num14;
						num13 += 2;
					}
				}
				for (int num16 = 0; num16 < triangles.Length; num16++)
				{
					array9[num16 + num13] = triangles[num16] + num14;
				}
				num13 += triangles.Length;
			}
			else
			{
				int[] triangles2 = meshInstance11.mesh.GetTriangles(meshInstance11.subMeshIndex);
				for (int num17 = 0; num17 < triangles2.Length; num17++)
				{
					array8[num17 + num12] = triangles2[num17] + num14;
				}
				num12 += triangles2.Length;
			}
			num14 += meshInstance11.mesh.vertexCount;
		}
		Mesh mesh = new Mesh();
		mesh.name = "Combined Mesh";
		mesh.vertices = array;
		mesh.normals = array2;
		mesh.colors = array7;
		mesh.uv = array4;
		mesh.uv2 = array5;
		mesh.uv2 = array6;
		mesh.tangents = array3;
		if (generateStrips)
		{
			mesh.SetTriangles(array9, 0);
		}
		else
		{
			mesh.triangles = array8;
		}
		return mesh;
	}

	private static void Copy(int vertexcount, Vector3[] src, Vector3[] dst, ref int offset, Matrix4x4 transform)
	{
		for (int i = 0; i < src.Length; i++)
		{
			dst[i + offset] = transform.MultiplyPoint(src[i]);
		}
		offset += vertexcount;
	}

	private static void CopyNormal(int vertexcount, Vector3[] src, Vector3[] dst, ref int offset, Matrix4x4 transform)
	{
		for (int i = 0; i < src.Length; i++)
		{
			dst[i + offset] = transform.MultiplyVector(src[i]).normalized;
		}
		offset += vertexcount;
	}

	private static void Copy(int vertexcount, Vector2[] src, Vector2[] dst, ref int offset)
	{
		for (int i = 0; i < src.Length; i++)
		{
			dst[i + offset] = src[i];
		}
		offset += vertexcount;
	}

	private static void CopyUV2(int vertexcount, Vector2[] src, Vector2[] dst, ref int offset, float influence, float x)
	{
		for (int i = 0; i < src.Length; i++)
		{
			dst[i + offset] = src[i] * influence + new Vector2(x, 0f);
		}
		offset += vertexcount;
	}

	private static void CopyColors(int vertexcount, Color[] src, Color[] dst, ref int offset)
	{
		for (int i = 0; i < src.Length; i++)
		{
			dst[i + offset] = src[i];
		}
		offset += vertexcount;
	}

	private static void CopyTangents(int vertexcount, Vector4[] src, Vector4[] dst, ref int offset, Matrix4x4 transform)
	{
		for (int i = 0; i < src.Length; i++)
		{
			Vector4 vector = src[i];
			Vector3 v = new Vector3(vector.x, vector.y, vector.z);
			v = transform.MultiplyVector(v).normalized;
			dst[i + offset] = new Vector4(v.x, v.y, v.z, vector.w);
		}
		offset += vertexcount;
	}
}
