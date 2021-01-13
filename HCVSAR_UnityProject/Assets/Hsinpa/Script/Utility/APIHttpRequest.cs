using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Ocsp;
using UnityEngine.Networking;
using System.Text;
using thelab.core;

public class APIHttpRequest
{
    public static System.Action OnDynamicDomainIsGet;

    public static IEnumerator LoadServerCSVLink()
    {
        UnityWebRequest.ClearCookieCache();

        using (UnityWebRequest webRequest = UnityWebRequest.Get(Expect.StaticAsset.StringAsset.Domain.ServerDomainCSV))
        {
            webRequest.timeout = 5;
            webRequest.useHttpContinue = false;

            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
            if (webRequest.isNetworkError)
            {
                Debug.Log("WebUpdateSheet Error: " + webRequest.error);
            }
            else
            {
                CSVFile csvFile = new CSVFile(webRequest.downloadHandler.text);

                string domain = csvFile.Get<string>(0, "Domain");
                Debug.Log("CSV Domain " + domain);
                Expect.StaticAsset.StringAsset.Domain.DynamicDomain = domain;
            }

            if (OnDynamicDomainIsGet != null)
                OnDynamicDomainIsGet();
        }
    }

    public static void Curl(string url, HTTPMethods httpMethods, string rawJsonObject, System.Action<bool, string> callback) {
        var r = new BestHTTP.HTTPRequest(new System.Uri(url), httpMethods, (request, response) =>
        {
            if (callback == null) return;

            if (request.State == HTTPRequestStates.Finished)
            {
                callback(true, response.DataAsText);
            }
            else {
                Debug.LogError("Request Finished with Error! " + (request.Exception != null ? (request.Exception.Message + "\n" + request.Exception.StackTrace) : "No Exception"));
                callback(false, "");
            }
        });
        
        if (!string.IsNullOrEmpty(rawJsonObject)) {
            r.SetHeader("Content-Type", "application/json; charset=UTF-8");
            r.RawData = System.Text.Encoding.UTF8.GetBytes(rawJsonObject);
        }

        //r.Timeout = System.TimeSpan.FromSeconds(4);
        //r.ConnectTimeout = System.TimeSpan.FromSeconds(4);
        r.DisableCache = true;
        
        r.Send();
    }



    public static IEnumerator NativeCurl(string url, string httpMethods, string rawJsonObject, System.Action<string> success_callback, System.Action fail_callback)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.timeout = 40;
            webRequest.method = httpMethods;

            if (rawJsonObject != null) {
                webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(rawJsonObject));
                webRequest.uploadHandler.contentType = "application/json";
            }

            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError) {
                if (fail_callback != null) fail_callback();
                Debug.Log("Web  Error " + webRequest.error);

                yield break;
            }

            try
            {
                string rawJSON = webRequest.downloadHandler.text;
                var DatabaseResult = JsonUtility.FromJson<TypeFlag.SocketDataType.GeneralDatabaseType>(rawJSON);
                Debug.Log(rawJSON);
                Debug.Log(DatabaseResult.result);

                if (DatabaseResult.status && !string.IsNullOrEmpty(DatabaseResult.result))
                {
                    if (success_callback != null) success_callback(DatabaseResult.result);
                }
                else {
                    if (fail_callback != null) fail_callback();
                }
            }
            catch {
                if (fail_callback != null) fail_callback();
                Debug.Log("Web  Error " + webRequest.error);
            }
        }
    }

    public static IEnumerator CheckInternectCinnection(System.Action<bool> callback)
    {
        string url = "https://www.google.com.tw/";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.timeout = 10;
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
                callback(false);
            else
                callback(true);
        }
    }
}
