using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using static UnityEditor.ShaderData;

public class Register : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_InputField edtUser, edtPassword, edtRePass;
    public TMP_Text txtErorr;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void KiemTraDangKi()
    {
        string user = edtUser.text;
        string pass = edtPassword.text;
        string rePass = edtRePass.text;

        if (pass != rePass)
        {
            Debug.Log("edtRePass phai trung voi edtPassword");
            txtErorr.text = "mật khẩu bạn nhập không khớp";
            return; // Ngừng thực hiện phương thức nếu điều kiện không được đáp ứng
        }
        ModelsLogin modelsLogin = new ModelsLogin(user, pass);
        CheckRegister(modelsLogin);
        StartCoroutine(CheckRegister(modelsLogin));
    }
    IEnumerator CheckRegister(ModelsLogin modelsLogin)
    {
        string jsonStringRequest = JsonConvert.SerializeObject(modelsLogin);

        var request = new UnityWebRequest("https://hoccungminh.dinhnt.com/fpt/register", "POST");
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
            ResponseRegister responseRegister = JsonConvert.DeserializeObject<ResponseRegister>(jsonString);
            if (responseRegister.status == 0)
            {
                txtErorr.text = responseRegister.notification;
            }
            else
            {
                
                txtErorr.text = responseRegister.notification;
                edtUser.text = "";
                edtPassword.text = "";
                edtRePass.text = "";
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            }
        }
        request.Dispose();
    }
}




