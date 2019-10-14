using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogController : MonoBehaviour
{
    /*
     * A class dedicated to maintaining a single InGameLog
     * This keeps track of every string ever sent to it,
     * and displays those strings on a pool of rotating 
     * LogEntry objects
     */

    // Private VARS
    private List<LogEntry> eventLog = new List<LogEntry>();
    private List<Text> textPool = new List<Text>();


    // Public VARS
    public int maxLines = 1;
    public Text prefabTextObj;
    public Scrollbar contentVertScrollbar;

    // Content
    public GameObject content;
    private float contentHeight;
    private float contentSpacing;


    private void Start()
    {
        this.contentHeight = ((RectTransform)content.transform).sizeDelta.y;
        this.contentSpacing = this.content.GetComponent<VerticalLayoutGroup>().spacing;

        for (int i = 0; i < maxLines; i++) {
            addEvent("Test adding event");
        }
    }

    public void test()
    {
        addEvent("button press test");
    }

    public void addEvent(string eventString) {
        LogEntry newLE = new LogEntry("System: ", eventString);
        eventLog.Add(newLE);

        Text newTextObj = this.buildAndAddText(newLE);
        this.contentHeight += newTextObj.rectTransform.sizeDelta.y + this.contentSpacing;
        resizeContent(this.contentHeight);
    }

   private Text buildAndAddText(LogEntry entry) {
        string t = entry.headder + entry.rawText;
        Text newObj = GameObject.Instantiate<Text>(prefabTextObj, content.transform);
        newObj.text = entry.formatString();
        return newObj;
    }

    private void resizeContent(float newHeight) {
        RectTransform contentTransform = ((RectTransform)content.transform);

        // Update the content height
        Vector2 oldSizeDelta = contentTransform.sizeDelta;
        oldSizeDelta.y = newHeight;
        contentTransform.sizeDelta = oldSizeDelta;
    }

}




