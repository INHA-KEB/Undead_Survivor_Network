using Firebase;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseInit : MonoBehaviour
{
    void Awake() // ? �ݵ�� Awake�� ����
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;

                // ? Database URL ����
                app.Options.DatabaseUrl = new System.Uri("https://undeadsurvivor-77af8-default-rtdb.firebaseio.com/");

                Debug.Log("? Firebase �ʱ�ȭ ���� + Database URL ���� �Ϸ�");
            }
            else
            {
                Debug.LogError("? Firebase �ʱ�ȭ ����: " + task.Result);
            }
        });
    }
}

