using UnityEngine;

public class BulletMarks : MonoBehaviour
{
	public Texture2D[] concrete;

	public Texture2D[] wood;

	public Texture2D[] metal;

	public Texture2D[] oldMetal;

	public Texture2D[] glass;

	public Texture2D[] generic;

	public Texture2D[] tile;

	public Texture2D[] blood;

	private void Start()
	{
	}

	public void GenerateDecal(int type, GameObject go)
	{
		Texture2D texture2D;
		switch (type)
		{
		case 1:
		{
			if (concrete == null || concrete.Length == 0)
			{
				return;
			}
			int num8 = Random.Range(0, concrete.Length);
			texture2D = concrete[num8];
			base.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
			break;
		}
		case 2:
		{
			if (wood == null || wood.Length == 0)
			{
				return;
			}
			int num5 = Random.Range(0, wood.Length);
			texture2D = wood[num5];
			base.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
			break;
		}
		case 3:
		{
			if (metal == null || metal.Length == 0)
			{
				return;
			}
			int num9 = Random.Range(0, metal.Length);
			texture2D = metal[num9];
			base.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
			break;
		}
		case 4:
		{
			if (oldMetal == null || oldMetal.Length == 0)
			{
				return;
			}
			int num3 = Random.Range(0, oldMetal.Length);
			texture2D = oldMetal[num3];
			base.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
			break;
		}
		case 5:
		{
			if (glass == null || glass.Length == 0)
			{
				return;
			}
			int num6 = Random.Range(0, glass.Length);
			texture2D = glass[num6];
			base.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
			break;
		}
		case 6:
		{
			if (generic == null || generic.Length == 0)
			{
				return;
			}
			int num2 = Random.Range(0, generic.Length);
			texture2D = generic[num2];
			break;
		}
		case 7:
		{
			if (tile == null || tile.Length == 0)
			{
				return;
			}
			int num7 = Random.Range(0, tile.Length);
			texture2D = tile[num7];
			base.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
			break;
		}
		case 8:
		{
			if (blood == null || blood.Length == 0)
			{
				return;
			}
			int num4 = Random.Range(0, blood.Length);
			texture2D = blood[num4];
			base.transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
			break;
		}
		default:
			if (wood != null && wood.Length != 0)
			{
				int num = Random.Range(0, wood.Length);
				texture2D = wood[num];
			}
			return;
		}
		base.transform.Rotate(new Vector3(0f, 0f, Random.Range(-180f, 180f)));
		Decal.dCount++;
		Decal component = base.gameObject.GetComponent<Decal>();
		component.affectedObjects = new GameObject[1];
		component.affectedObjects[0] = go;
		component.decalMode = DecalMode.MESH_COLLIDER;
		component.pushDistance = Random.Range(0.009f, 0.006f);
		Material material = new Material(component.decalMaterial);
		material.mainTexture = texture2D;
		component.decalMaterial = material;
		component.CalculateDecal();
		component.transform.parent = go.transform;
	}
}
