using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Widgets
{
    public class DialogBarWidget : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        [SerializeField] private string[] _importantPhrases; //Будут выведены в первую очередь
        [SerializeField] private string[] _randomPhrases; //Рандомно будут проявляться

        [SerializeField] private float _typingTime;

        private int _importantPhrasesIndex;

        private void Start()
        {
            StartCoroutine(Type());
        }

        private IEnumerator Type()
        {
            while (enabled)
            {
                yield return TypingText();
                yield return new WaitForSeconds(15f);
            }
        }

        private IEnumerator TypingText()
        {
            _text.text = string.Empty;
            string phrase;

            if (_importantPhrasesIndex < _importantPhrases.Length)
            {
                phrase = _importantPhrases[_importantPhrasesIndex];
                _importantPhrasesIndex++;
            }
            else
                phrase = _randomPhrases[Random.Range(0, _randomPhrases.Length)];

            foreach (var symbol in phrase)
            {
                _text.text += symbol;
                yield return new WaitForSeconds(_typingTime);
            }
        }
    }
}