using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ClockView : MonoBehaviour
{
    [SerializeField] private Transform _hourHand;
    [SerializeField] private Transform _minuteHand;
    [SerializeField] private Transform _secondHand;
    [SerializeField] private Text _timeText;

    private Clock _clock;

    private int CurrentHour => _clock.CurrentHour;
    private int CurrentMinute => _clock.CurrentMinute;
    private int CurrentSecond => _clock.CurrentSecond;

    private void OnDisable()
    {
        RemoveListeners();
    }

    public void Init(Clock clock)
    {
        _clock = clock;

        AddListeners();
    }

    private void AddListeners()
    {
        _clock.SecondsChanged += UpdateSecondsHand;
        _clock.MinutesChanged += UpdateMinutesHand;
        _clock.HoursChanged += UpdateHoursHand;
        _clock.Changed += OnChanged;
    }

    private void RemoveListeners()
    {
        _clock.SecondsChanged -= UpdateSecondsHand;
        _clock.MinutesChanged -= UpdateMinutesHand;
        _clock.HoursChanged -= UpdateHoursHand;
        _clock.Changed -= OnChanged;
    }

    private void OnChanged()
    {
        UpdateTimeText();
    }


    private void UpdateHoursHand()
    {
        float hourRotation = -((CurrentHour % 12) * 30);
        _hourHand.DORotate(new Vector3(0, 0, hourRotation), 2f).SetEase(Ease.OutBack);
    }

    private void UpdateMinutesHand()
    {
        float minuteRotation = -(CurrentMinute * 6);
        _minuteHand.DORotate(new Vector3(0, 0, minuteRotation), 1f).SetEase(Ease.InOutExpo);
    }

    private void UpdateSecondsHand()
    {
        float secondRotation = -(CurrentSecond * 6);
        _secondHand.DORotate(new Vector3(0, 0, secondRotation), 0.3f).SetEase(Ease.InOutExpo);
    }

    private void UpdateTimeText()
    {
        _timeText.text = $"{CurrentHour:D2}:{CurrentMinute:D2}:{CurrentSecond:D2}";
    }
}
