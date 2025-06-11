using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UIElements;

// ���� �ʿ�. ���� 
public class PoolManager : NetworkBehaviour
{
    public GameObject[] prefabs;
    List<GameObject>[] pools;

    private void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }
    }

    public GameObject Get(int index, Vector3 position, Quaternion rotation)
    {
        GameObject select = null;

        // ������ Ǯ�� ��Ȱ��ȭ�� ���� ������Ʈ ����
        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                // �߰��ϸ� select ������ �Ҵ�
                select = item;

                select.transform.position = position;
                select.transform.rotation = rotation;

                //select.SetActive(true);
                NetworkServer.Spawn(select);

                break;
            }
        }

        if (!select)
        {
            select = Instantiate(prefabs[index], position, rotation);
            //select.SetActive(false);
            NetworkServer.Spawn(select);
        }

        return select;
    }

    [Server]
    public void ReturnToPool(GameObject obj)
    {
        NetworkServer.UnSpawn(obj); // ��Ʈ��ũ���� ������Ʈ ���� ����
        obj.SetActive(false); 
    }
}