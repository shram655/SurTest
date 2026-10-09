using System.Collections.Generic;
using UnityEngine;

public class PickaxeChop : MonoBehaviour
{
	public int ChopPower;

	public AudioClip[] ChoppingWoodChips;

	public GameObject[] FallingTreePrefab;

	private int m_ChopDamage;

	private int m_CurrentChoppingTreeIndex = -1;

	private TerrainData terrain;

	private TreeInstance[] treeInstances;

	private void Awake()
	{
		if ((bool)Terrain.activeTerrain)
		{
			terrain = Terrain.activeTerrain.terrainData;
			treeInstances = terrain.treeInstances;
		}
	}

	public void TreeChop(RaycastHit hit)
	{
		if (!Terrain.activeTerrain || ((bool)Terrain.activeTerrain && hit.collider.name != Terrain.activeTerrain.name))
		{
			return;
		}
		float num = Terrain.activeTerrain.SampleHeight(hit.point);
		if (!(hit.point.y <= num + 0.1f))
		{
			int num2 = ChopTree(hit);
			if (num2 == -1)
			{
			}
		}
	}

	public int ChopTree(RaycastHit hit)
	{
		TerrainData terrainData = Terrain.activeTerrain.terrainData;
		TreeInstance[] array = terrainData.treeInstances;
		float num = float.MaxValue;
		Vector3 vector = default(Vector3);
		int num2 = 0;
		TreeInstance treeInstance = default(TreeInstance);
		for (int i = 0; i < array.Length; i++)
		{
			TreeInstance treeInstance2 = array[i];
			Vector3 vector2 = Vector3.Scale(treeInstance2.position, terrainData.size) + Terrain.activeTerrain.transform.position;
			float num3 = Vector3.Distance(vector2, hit.point);
			if (num3 < num)
			{
				num = num3;
				num2 = i;
				vector = vector2;
				treeInstance = treeInstance2;
			}
		}
		int prototypeIndex = treeInstance.prototypeIndex;
		if (num > 2f)
		{
			return -1;
		}
		PlayChopSound(hit.point);
		if (m_CurrentChoppingTreeIndex != num2)
		{
			m_ChopDamage = ChopPower;
			m_CurrentChoppingTreeIndex = num2;
		}
		else
		{
			m_ChopDamage += ChopPower;
		}
		if (m_ChopDamage >= 100)
		{
			BaseLogic.AddItem(33, 1);
			List<TreeInstance> list = new List<TreeInstance>(terrainData.treeInstances);
			list.RemoveAt(num2);
			terrainData.treeInstances = list.ToArray();
			GameObject gameObject = (GameObject)Object.Instantiate(FallingTreePrefab[prototypeIndex], vector, Quaternion.AngleAxis(2f, Vector3.right));
			gameObject.transform.localScale = new Vector3(treeInstance.widthScale * gameObject.transform.localScale.x, treeInstance.heightScale * gameObject.transform.localScale.y, treeInstance.widthScale * gameObject.transform.localScale.z);
			gameObject.GetComponent<Rigidbody>().angularVelocity = new Vector3(Random.Range(-0.9f, 0.9f), 0.1f, Random.Range(-0.9f, 0.9f));
			if ((bool)Network.PLAYER)
			{
				Network.PLAYER.GetComponent<PhotonView>().RPC("DestroyTree", PhotonTargets.Others, vector);
			}
			m_ChopDamage = 0;
		}
		return prototypeIndex;
	}

	private void DestroyTree(Vector3 posV)
	{
		float num = float.MaxValue;
		Vector3 position = default(Vector3);
		int index = 0;
		TreeInstance treeInstance = default(TreeInstance);
		for (int i = 0; i < treeInstances.Length; i++)
		{
			TreeInstance treeInstance2 = treeInstances[i];
			Vector3 vector = Vector3.Scale(treeInstance2.position, terrain.size) + Terrain.activeTerrain.transform.position;
			float num2 = Vector3.Distance(vector, posV);
			if (num2 < num)
			{
				num = num2;
				index = i;
				position = vector;
				treeInstance = treeInstance2;
			}
		}
		int prototypeIndex = treeInstance.prototypeIndex;
		if (!(num > 2f))
		{
			List<TreeInstance> list = new List<TreeInstance>(terrain.treeInstances);
			list.RemoveAt(index);
			terrain.treeInstances = list.ToArray();
			GameObject gameObject = (GameObject)Object.Instantiate(FallingTreePrefab[prototypeIndex], position, Quaternion.AngleAxis(2f, Vector3.right));
			gameObject.transform.localScale = new Vector3(treeInstance.widthScale * gameObject.transform.localScale.x, treeInstance.heightScale * gameObject.transform.localScale.y, treeInstance.widthScale * gameObject.transform.localScale.z);
			gameObject.GetComponent<Rigidbody>().angularVelocity = new Vector3(Random.Range(-0.9f, 0.9f), 0.1f, Random.Range(-0.9f, 0.9f));
		}
	}

	private void PlayChopSound(Vector3 point)
	{
		int num = Random.Range(0, 2);
		base.GetComponent<AudioSource>().clip = ChoppingWoodChips[num];
		base.GetComponent<AudioSource>().Play();
	}
}
