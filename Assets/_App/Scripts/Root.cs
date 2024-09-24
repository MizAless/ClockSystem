using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Root : MonoBehaviour
{
    [SerializeField] private ClockView _clockView;

    private Clock _clock;
    private TimeFetcher _timeFetcher;

    private void Awake()
    {
        _clock = new Clock();
        _timeFetcher = new TimeFetcher();
        _clockView.Init(_clock);
    }

    private void Start()
    {
        InitializeClock().Forget();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Time.timeScale = 100f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    private async UniTask InitializeClock()
    {
        try
        {
            CorrectOperation correctOpetarion = await Validate();

            _clock.HoursChanged += CheckTimeSynchronization;

            if (_clock.IsEqual(correctOpetarion.Responce) == false)
            {
                _clock.SetClock(correctOpetarion.Responce);
                _clock.Restart();


                
                Debug.Log("Time synchronization was successful");
            }
            else
            {
                Debug.Log("Time synchronization was not required");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    private void CheckTimeSynchronization()
    {
        _clock.HoursChanged -= CheckTimeSynchronization;
        InitializeClock().Forget();
    }

    private async UniTask<CorrectOperation> Validate()
    {
        CorrectOperation correctOpetarion;

        do
        {
            correctOpetarion = await _timeFetcher.FetchCurrentTimeAsync();

            if (correctOpetarion.OperationResult == true)
                break;
        }
        while (correctOpetarion.OperationResult == false);

        return correctOpetarion;
    }
}
