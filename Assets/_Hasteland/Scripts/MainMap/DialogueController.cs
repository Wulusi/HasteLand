using UnityEngine.Events;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public struct Dialogue
{
    public string characterName;
    public Sprite characterPortrait;
    public string[] steps;
}
public class DialogueController : MonoBehaviour
{
    public UnityEvent OnDialogueComplete = new UnityEvent();
    public TextMeshProUGUI textBox;
    public int lettersPerSecond;
    public Image portrait;

    public Dialogue dialogue;
    private IEnumerator AnimateText(string _text)
    {
        string _tempText="";
        int _index = 0;
        bool _isRunning = true;
        while (_isRunning)
        {
            if (_index < _text.Length)
            {
                _tempText = string.Format("{0}{1}", _tempText, _text[_index]);
                _index++;
                //Debug.Log(_title);
                textBox.text = _tempText;
            }
            else
            {
                _isRunning = false;
            }
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
    }

}
