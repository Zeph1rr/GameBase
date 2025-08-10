using UnityEngine;
using UnityEngine.UI;

namespace BaseGame.Core.LocalizationSystem
{
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField] private string _key;
        [SerializeField] private Text _text;

        private void Awake()
        {
            Localization.OnLanguageChanged += Refresh;
            Refresh();
        }

        private void OnDestroy()
        {
            Localization.OnLanguageChanged -= Refresh;
        }

        private void Refresh()
        {
            _text.text = Localization.Get(_key, _key);
        }
    }
}