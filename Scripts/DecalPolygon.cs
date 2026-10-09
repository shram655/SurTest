using UnityEngine;

public class DecalPolygon
{
	public int verticeCount;

	public Vector3[] normal;

	public Vector3[] vertice;

	public Vector4[] tangent;

	public DecalPolygon()
	{
		verticeCount = 0;
		vertice = new Vector3[9];
		normal = new Vector3[9];
		tangent = new Vector4[9];
	}

	public static DecalPolygon ClipPolygonAgainstPlane(DecalPolygon polygon, Vector4 plane)
	{
		bool[] array = new bool[10];
		int num = 0;
		Vector3 vector = new Vector3(plane.x, plane.y, plane.z);
		for (int i = 0; i < polygon.verticeCount; i++)
		{
			array[i] = Vector3.Dot(polygon.vertice[i], vector) + plane.w < 0f;
			if (array[i])
			{
				num++;
			}
		}
		if (num == polygon.verticeCount)
		{
			return null;
		}
		if (num == 0)
		{
			return polygon;
		}
		DecalPolygon decalPolygon = new DecalPolygon();
		decalPolygon.verticeCount = 0;
		for (int j = 0; j < polygon.verticeCount; j++)
		{
			int num2 = ((j != 0) ? (j - 1) : (polygon.verticeCount - 1));
			if (array[j])
			{
				if (!array[num2])
				{
					Vector3 vector2 = polygon.vertice[j];
					Vector3 vector3 = polygon.vertice[num2];
					Vector3 normalized = (vector3 - vector2).normalized;
					float num3 = (0f - (Vector3.Dot(vector, vector2) + plane.w)) / Vector3.Dot(vector, normalized);
					decalPolygon.tangent[decalPolygon.verticeCount] = polygon.tangent[j] + (polygon.tangent[num2] - polygon.tangent[j]).normalized * num3;
					decalPolygon.vertice[decalPolygon.verticeCount] = vector2 + (vector3 - vector2).normalized * num3;
					decalPolygon.normal[decalPolygon.verticeCount] = polygon.normal[j] + (polygon.normal[num2] - polygon.normal[j]).normalized * num3;
					decalPolygon.verticeCount++;
				}
				continue;
			}
			if (array[num2])
			{
				Vector3 vector4 = polygon.vertice[num2];
				Vector3 vector5 = polygon.vertice[j];
				Vector3 normalized2 = (vector5 - vector4).normalized;
				float num4 = (0f - (Vector3.Dot(vector, vector4) + plane.w)) / Vector3.Dot(vector, normalized2);
				decalPolygon.tangent[decalPolygon.verticeCount] = polygon.tangent[num2] + (polygon.tangent[j] - polygon.tangent[num2]).normalized * num4;
				decalPolygon.vertice[decalPolygon.verticeCount] = vector4 + (vector5 - vector4).normalized * num4;
				decalPolygon.normal[decalPolygon.verticeCount] = polygon.normal[num2] + (polygon.normal[j] - polygon.normal[num2]).normalized * num4;
				decalPolygon.verticeCount++;
			}
			decalPolygon.tangent[decalPolygon.verticeCount] = polygon.tangent[j];
			decalPolygon.vertice[decalPolygon.verticeCount] = polygon.vertice[j];
			decalPolygon.normal[decalPolygon.verticeCount] = polygon.normal[j];
			decalPolygon.verticeCount++;
		}
		return decalPolygon;
	}
}
