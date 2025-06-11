using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance;

    public DatabaseReference dbRef;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            StartCoroutine(InitFirebase());
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator InitFirebase()
    {
        yield return new WaitForSeconds(0.1f);

        var checkTask = FirebaseApp.CheckAndFixDependenciesAsync();
        yield return new WaitUntil(() => checkTask.IsCompleted);

        if (checkTask.Result == DependencyStatus.Available)
        {
            dbRef = FirebaseDatabase
                .GetInstance("https://undeadsurvivor-77af8-default-rtdb.firebaseio.com/")
                .RootReference;

            Debug.Log("Firebase �ʱ�ȭ ����");
        }
        else
        {
            Debug.LogError("Firebase �ʱ�ȭ ����: " + checkTask.Result);
        }
    }

    public void SaveUserData()
    {
        Debug.Log("savedata");
        if (FirebaseAuth.DefaultInstance.CurrentUser == null || dbRef == null)
        {
            Debug.LogWarning("SaveUserData: ����� ���� �Ǵ� DB ������ ����");
            return;
        }

        string uid = FirebaseAuth.DefaultInstance.CurrentUser.UserId;

        Dictionary<string, object> userData = new Dictionary<string, object>
    {
        { "level", GameManager.instance.level },
        { "exp", GameManager.instance.exp },
        { "kill", GameManager.instance.kill },
        { "maxHealth", (int)GameManager.instance.player.maxHealth },
        { "playerId", GameManager.instance.playerId }
    };

        dbRef.Child("users").Child(uid).UpdateChildrenAsync(userData).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SaveUserData: ���� ��ҵ�");
            }
            else if (task.IsFaulted)
            {
                Debug.LogError("SaveUserData: ���� �� ���� �߻� - " + task.Exception);
            }
            else
            {
                Debug.Log("SaveUserData: ����� ������ ���� �Ϸ�");
            }
        });
    }


    public void ResetUserData()
    {
        if (FirebaseAuth.DefaultInstance.CurrentUser == null || dbRef == null)
        {
            Debug.LogWarning("ResetUserData: Firebase �ʱ�ȭ ���̰ų� �α��ε��� ����");
            return;
        }

        string uid = FirebaseAuth.DefaultInstance.CurrentUser.UserId;

        Dictionary<string, object> resetData = new Dictionary<string, object>
        {
            { "level", 0 },
            { "exp", 0 },
            { "kill", 0 },
            { "maxHealth", 100 },
            { "playerId", GameManager.instance.playerId }
        };

        dbRef.Child("users").Child(uid).SetValueAsync(resetData);
    }
}

















