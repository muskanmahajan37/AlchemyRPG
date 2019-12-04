using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LogEntry {
    // To represent a "line" of text in the log. It could actually be longer than a single displayed line of text
    public string body;
    public string header;

    public Color bodyColor;
    public Color headerColor;

    public TMP_FontAsset font;

    public LogEntry(string header, string rawText) {
        this.body = rawText;
        this.header = header;
        this.setDefaults();
    }

    private void setDefaults() {
        this.bodyColor = ColorConstants.TEXT_DEFAULT_BODY;
        this.headerColor = ColorConstants.TEXT_DEFAULT_HEADER;
        this.font = FontLookup.singleton.getFont(EFontName.Default);
    }

    public void setFont(EFontName fontName) {
        this.font = FontLookup.singleton.getFont(fontName);
    }

    public string formatString() { return header + body; }
    public override string ToString() { return this.formatString(); }


}