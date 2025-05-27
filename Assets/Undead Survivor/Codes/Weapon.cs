using UnityEngine;
using Mirror;
using UnityEngine.UIElements;
using UnityEngine.TextCore.Text;

public class Weapon : NetworkBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;


    public override void OnStartServer()
    {
        Init();
    }

    void Update()
    {
        if (!isServer)
        {
            return;
        }

        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;

            default:

                break;
        }

        if (Input.GetButtonDown("Jump"))   
        {
            LevelUp(20, 5);
        }
    }


    public void LevelUp(float damage, int count)
    {
        this.damage = damage ;
        this.count += count;

        if (id == 0)
            Batch();
    }

    [Server]
    public void Init()
    {
        switch (id)
        {
            case 0: // ���� ����
                speed = -150; // ȸ�� �ӵ�
                Batch();      // �������� �� ��ġ
                break;
            default: // ���Ÿ� ����
                speed = 0.3f; // �߻� ������
                break;
        }
    }

    [Server]
    void Batch()
    {
        for (int index = 0; index < count; index++)
        {
            float angle = 360f * index / count;
            Quaternion rot = Quaternion.Euler(0f, 0f, angle);
            Vector3 offset = rot * Vector3.up * 1.5f;

            GameObject bulletObj = GameManager.instance.pool.Get(prefabId, transform.position, Quaternion.identity);
            Bullet bullet = bulletObj.GetComponent<Bullet>();

            bullet.followTarget = transform; // weapon
            bullet.offset = offset;          // �ʱ� ��ġ offset

            bullet.Init(damage, -1, Vector3.zero);
        }
    }

}