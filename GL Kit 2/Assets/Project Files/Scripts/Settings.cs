using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    //----------------------------------------------------------
    //                    PATHS AND EXTENSIONS
    //----------------------------------------------------------
    public const string STR_DEFAULT_DIALOGUE                   = "...";
    public const string STR_EVENT_FIRED                        = "Event Fired on premise: ";
    
    public const string FLTYPE_TEXT                            = ".txt";
    public const string FLTYPE_JSON                            = ".json";
    public const string FLTYPE_JSON_META                       = ".json.meta";
    
    public const string PATH_TEXT_FILES                        = "Text/";
    public const string PATH_DIALOGUE                          = "Dialogue/";
    public const string PATH_PREFABS                           = "Prefabs/";
    public const string PATH_ASSETS_RESOURCES                  = "Assets/Resources/";
    
    public const string OBJ_NAME_BLANK_GAMEOBJECT              = "Blank GameObject";

    public const string JSON_DEF_IDENTIFIER_KEYWORDS           = "identifierKeywords";
    public const string JSON_DEF_DEFINED_ARRAYS                = "definedArrays";

    
    
    //----------------------------------------------------------
    //                    VALUES
    //----------------------------------------------------------
    public const float VAL_CAMERA_ZOOM_DISTANCE                = 5.0f;
    public const float VAL_CAMERA_MOVEMENT_SPEED               = 0.1f;

    public const int SYS_VAL_MAX_NUMBER_ROOM_FOCALS            = 7;
}
