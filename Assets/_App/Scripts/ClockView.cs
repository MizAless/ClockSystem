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

    private Tween _hourTween;

    private bool _isFirstChange = true;

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
        _clock.Changed += OnChanged;
    }

    private void RemoveListeners()
    {
        _clock.Changed -= OnChanged;
    }

    private void OnChanged()
    {
        UpdateClockHands();
        UpdateTimeText();

        _isFirstChange = false;
    }

    private void UpdateClockHands()
    {
        float secondRotation = -(CurrentSecond * 6);
        float minuteRotation = -(CurrentMinute * 6);
        float hourRotation = -((CurrentHour % 12) * 30);

        _secondHand.DORotate(new Vector3(0, 0, secondRotation), 0.3f).SetEase(Ease.InOutExpo);
        _minuteHand.DORotate(new Vector3(0, 0, minuteRotation), 1f).SetEase(Ease.InOutExpo);
        _hourHand.DORotate(new Vector3(0, 0, hourRotation), 1f).SetEase(Ease.InExpo);
    }

    private void UpdateTimeText()
    {
        _timeText.text = $"{CurrentHour:D2}:{CurrentMinute:D2}:{CurrentSecond:D2}";
    }
}
