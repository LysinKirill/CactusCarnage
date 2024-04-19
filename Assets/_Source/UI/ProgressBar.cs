using Core.Controllers;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ProgressBar : MonoBehaviour
    {
        private Slider _progressSlider;
        private TMP_Text _percentage;
        private void Awake()
        {
            _progressSlider = GetComponent<Slider>();
            _percentage = GetComponentInChildren<TMP_Text>();
        }

        private void Start()
        {
            Subscribe();
            UpdateProgress();
        }

        private void UpdateProgress()
        {
            var progressManager = PlayerProgressManager.Instance;

            var levels = progressManager.Levels;
            var completedCount= levels.Count(x => x.Status == LevelStatus.Completed);

            var progress = (float)completedCount / levels.Count;
            UpdateSlider(progress);
            UpdatePercentage(progress);
        }

        void UpdateSlider(float value)
        {
            if (_progressSlider == null)
                return;
            _progressSlider.value = value;
        }

        void UpdatePercentage(float value)
        {
            if (_percentage == null)
                return;
            _percentage.text = $"{Math.Round(value * 100)}%";
        }

        void Subscribe()
        {
            PlayerProgressManager.Instance.OnLevelStatusChanged += (_, _) => UpdateProgress();
        }
    }
}
