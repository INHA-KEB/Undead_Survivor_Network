using Firebase.Auth;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField inputEmail;
    public TMP_InputField inputPassword;

    public void OnLoginButtonClick()
    {
        string email = inputEmail.text.Trim();
        string password = inputPassword.text.Trim();

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            Debug.LogWarning("�̸��� �Ǵ� ��й�ȣ�� ��� �ֽ��ϴ�.");
            return;
        }

        FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled || task.IsFaulted)
                {
                    Debug.LogWarning("�α��� ����, ����� �������� ���� �� ����. ȸ������ �õ�...");

                    FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(email, password)
                        .ContinueWithOnMainThread(createTask =>
                        {
                            if (createTask.IsCanceled || createTask.IsFaulted)
                            {
                                Debug.LogError("ȸ������ ����: " + createTask.Exception?.Flatten().InnerExceptions[0].Message);
                                return;
                            }

                            Debug.Log("ȸ������ ����");
                            SessionData.userId = createTask.Result.User.UserId;
                            FirebaseManager.Instance.LoadMaxKill();

                            SceneManager.LoadScene("SampleScene"); // playerId ���� �ٷ� �� ��ȯ
                        });
                }
                else
                {
                    Debug.Log("�α��� ����");
                    SessionData.userId = task.Result.User.UserId;
                    FirebaseManager.Instance.LoadMaxKill();

                    SceneManager.LoadScene(1); // playerId ���� �ٷ� �� ��ȯ
                }
            });
    }
}





