using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LogEntryFactory {

    public static LogEntry createSystemLogEntry(string bodyText) {
        LogEntry result = new LogEntry("System: ", bodyText);
        // Colors
        result.bodyColor = ColorConstants.TEXT_SYSTEM; // Make the body standout 
        result.headerColor = ColorConstants.TEXT_SYSTEM;
        return result;
    }

    public static LogEntry createBattleLogEntry(string bodyText) {
        LogEntry result = new LogEntry("Battle: ", bodyText);
        // Colors
        result.bodyColor = ColorConstants.TEXT_DEFAULT_BODY;
        result.headerColor = ColorConstants.TEXT_BATTLE_HEADER;
        return result;
    }
    
    public static LogEntry createDialogueLogEntry(string bodyText) {
        return createDialogueLogEntry(bodyText, "Chat", ColorConstants.TEXT_DIALOGUE_HEADER);
    }

    public static LogEntry createDialogueLogEntry(string bodyText, string speakerName, Color speakerColor) {
        LogEntry result = new LogEntry(speakerName + ": ", bodyText);
        // Colors
        result.bodyColor = ColorConstants.TEXT_DEFAULT_BODY;
        result.headerColor = speakerColor;
        return result;
    }

    public static LogEntry createWorldEventLogEntry(string bodyText) {
        LogEntry result = new LogEntry("Battle: ", bodyText);
        // Colors
        result.bodyColor = ColorConstants.TEXT_DEFAULT_BODY;
        result.headerColor = ColorConstants.TEXT_WORLD_EVENT_HEADER;
        return result;
    }


}
