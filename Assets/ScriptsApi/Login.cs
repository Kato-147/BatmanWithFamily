using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
//đổi màn
using UnityEngine.SceneManagement;

using Newtonsoft.Json;


public class Login : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_InputField edtUser, edtPassword;
    public TMP_Text txtErorr;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void KiemTraDangNhap()
    {
        string user = edtUser.text;
        string pass = edtPassword.text;

        ModelsLogin modelsLogin = new ModelsLogin(user, pass);
        CheckLogin(modelsLogin);
        StartCoroutine(CheckLogin(modelsLogin));
    }
    IEnumerator CheckLogin(ModelsLogin modelsLogin)
    {
        string jsonStringRequest = JsonConvert.SerializeObject(modelsLogin);

        var request = new UnityWebRequest("https://hoccungminh.dinhnt.com/fpt/login", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonStringRequest);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            var jsonString = request.downloadHandler.text.ToString();
            ResponseLogin responseLogin = JsonConvert.DeserializeObject<ResponseLogin>(jsonString);
            if (responseLogin.status == 0)
            {
                txtErorr.text = responseLogin.notification;
            }
            else
            {
                //đổi màn
                SceneManager.LoadScene("Man1");
            }
        }
        request.Dispose();
    }

}
