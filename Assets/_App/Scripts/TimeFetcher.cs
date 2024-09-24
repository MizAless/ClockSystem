using System;
using UnityEngine.Networking;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class TimeFetcher
{
    private const string _timeUrl = "http://worldtimeapi.org/api/ip";

    public async UniTask<CorrectOperation> FetchCurrentTimeAsync()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(_timeUrl))
        {
            webRequest.timeout = 1;

            try
            {
                await webRequest.SendWebRequest().ToUniTask();

                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError(webRequest.error);
                    throw new Exception("Failed to fetch time from server");
                }

                var jsonResponse = webRequest.downloadHandler.text;

                return new CorrectOperation(true, ParseTime(jsonResponse));


            }
            catch (Exception error)
            {
                Debug.LogError(error);
                return new CorrectOperation(false);
            }
        }
    }

    private long ParseTime(string json)
    {
        var data = JsonUtility.FromJson<TimeData>(json);
        return data.unixtime;
    }

    [Serializable]
    public class TimeData
    {
        public long unixtime;
    }
}

public class CorrectOperation
{
    public bool OperationResult { get; private set; }
    public long Responce { get; private set; }

    public CorrectOperation(bool operationResult, long responce = 0)
    {
        OperationResult = operationResult;
        Responce = responce;
    }
}