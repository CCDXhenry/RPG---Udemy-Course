using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_VolumeSlider : MonoBehaviour , ISaveManager
{
    public Slider slider;
    public string parameter;

    [SerializeField] private AudioMixer audioMixer;
    private const float volumeMax = 20;
    private const float volumeMin = -80;

    private void Awake()
    {
        // 初始化Slider事件监听
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float linearValue)
    {
        // 1. 边界保护：避免Log10(0)报错
        if (linearValue <= 0.0001f)
        {
            audioMixer.SetFloat(parameter, volumeMin);
            return;
        }

        // 2. 对数映射：线性值 → 分贝值（符合人耳感知）
        float volumeDB = Mathf.Log10(linearValue) * 20;

        // 3. 设置AudioMixer参数
        bool success = audioMixer.SetFloat(parameter, volumeDB);
        if (!success)
            Debug.LogWarning($"AudioMixer参数'{parameter}'设置失败，请检查名称拼写或范围");

        // 4. 可选：保存音量设置
        PlayerPrefs.SetFloat(parameter, linearValue);
    }

    //提供静音方法
    public void Mute() => audioMixer.SetFloat(parameter, volumeMin);

    public void LoadGame(GameData _gameData)
    {
        slider.value = _gameData.volumeSlider.TryGetValue(parameter,out float value) ? value : 0.75f;
        OnSliderValueChanged(slider.value);
    }

    public void SaveGame(ref GameData _gameData)
    {
        _gameData.volumeSlider[parameter] = slider.value;
    }
}
