using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettingsUI : MonoBehaviour
{
    public Slider slider;
    public string param;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float multiplier;

    public void SetSliderValue(float value) => audioMixer.SetFloat(param, Mathf.Log10(value) * multiplier);

    public void LoadSlider(float value) => slider.value = value >= 0.001f ? value : slider.value;


}