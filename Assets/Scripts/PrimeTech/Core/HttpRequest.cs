using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class HttpRequest {

    public delegate void HttpResponseHandler(int statusCode, string responseText, byte[] responseData);

    private static IEnumerator CreateCoroutine(string method, string url, Dictionary<string, string> headers, byte[] body, HttpResponseHandler handler) {
        var request = new UnityWebRequest(url, method);

        if (headers != null) {
            foreach (var item in headers) {
                request.SetRequestHeader(item.Key, item.Value);
            }
        }

        if (body != null)
            request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        handler?.Invoke((int)request.responseCode, request.downloadHandler.text, request.downloadHandler.data);
    }

    public static void Send(MonoBehaviour monoBehaviour, string method, string url, Dictionary<string, string> headers, byte[] body, HttpResponseHandler handler) {
        monoBehaviour.StartCoroutine(CreateCoroutine(method, url, headers, body, handler));
    }

    public static void Send(MonoBehaviour monoBehaviour, string method, string url, Dictionary<string, string> headers, string body, HttpResponseHandler handler) {
        monoBehaviour.StartCoroutine(CreateCoroutine(method, url, headers, Encoding.UTF8.GetBytes(body), handler));
    }
}