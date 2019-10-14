using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class LogEntry
{
    // To represent a "line" of text in the log. It could actually be longer than a single displayed line of text
    public string rawText;
    public string headder;

    public LogEntry(string headder, string rawText)
    {
        this.rawText = rawText;
        this.headder = headder;
    }

    public string formatString()
    {
        return headder + rawText;
    }
}