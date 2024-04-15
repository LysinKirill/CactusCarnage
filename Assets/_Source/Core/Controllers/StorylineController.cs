using Core.Environment;
using ScriptableObjects.Story;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Controllers
{
    public class StorylineController : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private List<StorylineTrigger> storylineTriggers;
        
        private static readonly HashSet<string> ShownStoryParts = new HashSet<string>();
        private GameObject _activeStoryPanel;


        private void Awake()
        {
            foreach (var storyTrigger in storylineTriggers)
                storyTrigger.OnTriggerActivated += OpenStoryInsertion;
        }

        public void OpenStoryInsertion(StoryInsertionAsset storyInsertion)
        {
            if (!storyInsertion.IsRepeatable && ShownStoryParts.Contains(storyInsertion.Name))
                return;
            
            ShownStoryParts.Add(storyInsertion.Name);
            
            
            _activeStoryPanel = Instantiate(storyInsertion.storyPanelPrefab, Vector3.zero, Quaternion.identity);
            _activeStoryPanel.transform.SetParent(canvas.transform, false);
            
            var text = _activeStoryPanel.GetComponentInChildren<TMP_Text>();
            text.SetText(storyInsertion.Text);
            StartCoroutine(FadeOut(storyInsertion.FadeOutDuration));
        }
        
        private IEnumerator FadeOut(float duration)
        {
            if (!_activeStoryPanel.TryGetComponent(out Image panelImage))
                yield break;

            var text = _activeStoryPanel.GetComponentInChildren<TMP_Text>();
            
            float currentTime = 0;
            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;

                var x = currentTime / duration;
                var opacity = 1 - Mathf.Pow(x, 12);
                
                var color = panelImage.color;
                color.a = opacity;
                panelImage.color = color;

                var vertexColor = text.color;
                vertexColor.a = opacity;
                text.color = vertexColor;
                yield return null;
            }
            
            Destroy(_activeStoryPanel);
            _activeStoryPanel = null;
        }
    }
}
