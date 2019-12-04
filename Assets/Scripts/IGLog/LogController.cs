using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using UnityEngine.UI;

public class LogController : MonoBehaviour {
    /*
     * A class dedicated to maintaining a single InGameLog
     * This keeps track of every string ever sent to it,
     * and displays those strings on a pool of rotating 
     * LogEntry objects
     */
    
    // Public VARS
    public int initialLines = 6;
    public const int MAX_LINES_DISPLAYED = 64;
    public int fontSize = 32;
    public TextMeshProUGUI prefabTMP_Obj;

    // Private Vars
    private LinkedList<LogEntry> eventLog = new LinkedList<LogEntry>();  // The string objects of all the text
    private LinkedList<GameObject> entryObjects = new LinkedList<GameObject>();  // The root-most GameObject of each text entry is a background Image
    // TODO: Consider implimenting a TextMeshProUGUI object pooler. 
    private StringBuilder sb = new StringBuilder();

    // Content
    public GameObject content;
    private RectTransform contentTransform; // Used to change the size of newly created Text objects. 
    private /*readonly*/ float contentWidth; // Used to instantiate new Text objects to the correct width
                                             // It's a fake readonly because Monobehaviors don't allow for readonly
    // Text background
    public Image BackgroundA;
    public Image BackgroundB;
    private bool backgroundSwitch = true;

    private void Start() {
        this.contentTransform = (RectTransform)content.transform;
        this.contentWidth = contentTransform.sizeDelta.x;
        
        for (int i = 0; i < initialLines; i++) {
            LogEntry newEntry = new LogEntry("System: ", "Test adding event blab" + i + "mmmm%mmmm% mmmm% mmmm%mmmm%mmmm% &mmmm%mmmm% mmmm% mmmm%mmmm%mmmm% ");
            addEvent(newEntry);
        }

        addEvent(LogEntryFactory.createBattleLogEntry("Goblin inflicts 10 DMG to Player 1"));
        addEvent(LogEntryFactory.createDialogueLogEntry("Run for your lives! The goblins are attacking!"));
        addEvent(LogEntryFactory.createSystemLogEntry("System reset in 10 minutes."));
        addEvent(LogEntryFactory.createWorldEventLogEntry("The Goblins have declared War on the small town."));
    }

    public void addEvent(LogEntry entry) {
        GameObject newEntryObject = this.buildAndDisplayTMP(entry);

        eventLog.AddFirst(entry);
        entryObjects.AddFirst(newEntryObject);
        if (entryObjects.Count > MAX_LINES_DISPLAYED) {
            GameObject oldEntry = entryObjects.Last.Value;
            // TODO: Impliment object pooling here
            Destroy(oldEntry);
            // Keep all the strings/ LogEntries, but remove old GameObjects
            // Why keep strings? 
        }
    }

    private GameObject buildAndDisplayTMP(LogEntry entry) {
        /**
         * This function converts a LogEntry into a well formatted text object
         * and adds it to the Content of this LogController. 
         * Well formatted includes
         *  - Providing a background
         *  - Adjusting entry size
         *  - Formatting including font, text size, colors etc.
         *  
         * The returned game object is the root-most element of the entry
         * (which is usually a Background Image).
         */

        // First make the Background
        Image backgroundObj = (backgroundSwitch) ? 
                               GameObject.Instantiate<Image>(BackgroundA, contentTransform) : 
                               GameObject.Instantiate<Image>(BackgroundB, contentTransform);
        backgroundSwitch = !backgroundSwitch;

        // Now create the text Obj as a child of the BG
        TextMeshProUGUI newTextObject = GameObject.Instantiate<TextMeshProUGUI>(prefabTMP_Obj, backgroundObj.transform);
        newTextObject.fontSize = this.fontSize;
        newTextObject.font = entry.font;
        // Format the text to be displaied (including the colors)
        newTextObject.text = buildTextString(entry);

        // Update the size of the Text Object box
        // This is actually more complex than it seems. If you're curious see the note at the
        // bottom of this file (NOTE A)
        float leftTextPadding = 5;
        float rightTextPadding = 10;
        newTextObject.rectTransform.sizeDelta = new Vector2(this.contentWidth - (leftTextPadding + rightTextPadding), this.fontSize);
        newTextObject.rectTransform.Translate(new Vector2(leftTextPadding + rightTextPadding, 0));

        // Update the size of the Background object box
        backgroundObj.rectTransform.sizeDelta = new Vector2(this.contentWidth, newTextObject.preferredHeight);

        return backgroundObj.gameObject;
    }

    private string buildTextString(LogEntry entry) {
        // Build the Header
        sb.Append("<color=#");
        sb.Append(NocabColorUtility.ToHtmlStringRGB(entry.headerColor));
        sb.Append(">");
        sb.Append(entry.header);
        // Build the body
        sb.Append("<color=#");
        sb.Append(NocabColorUtility.ToHtmlStringRGB(entry.bodyColor));
        sb.Append(">");
        sb.Append(entry.body);

        string result = sb.ToString();
        sb.Clear();
        return result;
    }  
}




/** NOTE A
 * Update the width of the text box. This may cause the text to overflow onto the next line.
 * However, the vertical resizing should be done by the Content Size Fitter component attached 
 * to the newTextObject prefab instantiation.
 * 
 * This method is prefered because the exact number of overflow lines is relativly un-predictible.
 * Consider the case where there are extra long words in the displaied text: Each row of text is
 * under-utilized because the large word is started on the new line. 
 * Example: 
 * +-----------------------------------------+
 * |System: abcd                             |
 * |ThisIsAReallyLongWordThatGetsItsOwnLine  |
 * |ShortWord.                               |
 * +-----------------------------------------+
 * 
 * In the above example, simply counting the letters and estimating via font size may produce
 * an estimation that is only 2 lines instead of the actual 3. 
 * It's better to define the width, let the text istelf calculate it's preferd heigh. So let the
 * Content Size Fitter component take care of that for you. 
 * 
 * After setting the newTextObject's sizeDelta, use the preferredHeight attribute instead of 
 * the actual height. 
 */
