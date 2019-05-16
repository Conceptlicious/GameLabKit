﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    //----------------------------------------------------------
    //                    PATHS AND EXTENSIONS
    //----------------------------------------------------------
    public const string STR_DEFAULT_DIALOGUE                      = "...";
    public const string STR_EVENT_FIRED                           = "Event Fired on premise: ";

    public const string STR_WARNING_EVENT_CHAIN_GENERIC_INFO      = "WARNING: Events fired via markers use only generic info.";

    public const string FLTYPE_TEXT                               = ".txt";
    public const string FLTYPE_JSON                               = ".json";
    public const string FLTYPE_JSON_META                          = ".json.meta";

    public const string PATH_TEXT_FILES                           = "Text/";
    public const string PATH_DIALOGUE                             = "Dialogue/";
    public const string PATH_PREFABS                              = "Prefabs/";
    public const string PATH_ASSETS_RESOURCES                     = "Assets/Resources/";

    public const string OBJ_NAME_BLANK_GAMEOBJECT                 = "BlankGameObject";

    public const string JSON_DEF_IDENTIFIER_KEYWORDS              = "identifierKeywords";
    public const string JSON_DEF_DEFINED_ARRAYS                   = "definedArrays";

    public const string DEFAULT_EVENTMARKER_MARKER                = "#";

    public static readonly string[] LEVEL_NAMES                   = new string[]
    {
       
        "Loading_Tips", 
        "Room_2",
        "Room_3"
    };
    
    //----------------------------------------------------------
    //                    VALUES
    //----------------------------------------------------------
    public const float VAL_CAMERA_ZOOM_DISTANCE                   = 15.0f;
    public const float VAL_CAMERA_MOVEMENT_SPEED                  = 0.1f;
    public const float VAL_CAMERA_TRANSITION_SECONDS              = 7.0f;
    public const float VAL_CAMERA_BLUR_FOCALDISTANCE              = 0.1f;
    public const float VAL_CAMERA_BLUR_FOCALLENGTH_MAX            = 20.0f;

    public const int SYS_VAL_MAX_NUMBER_ROOM_FOCALS               = 7;
    
    //----------------------------------------------------------
    //                    OUTPUTS
    //----------------------------------------------------------
    public const string ERR_ASSERT_DIAG_CONTAINERS_NULL           = "Fetching null dialogue containers that have not been created and filled.";
    public const string ERR_ASSERT_DIAG_CONTAINERS_INVALID_INDEX  = "Trying to fetch dialogue containers with invalid index.";
    public const string ERR_DIALOGUE_INVALID_INDEX                = "Trying to fetch dialogue with an invalid index.";
    public const string ERR_JSON_FILE_INCORRECT_FORMATTING        = "Fields and arrays cannot be read from the JSON file. Please check file '{0}''s formatting.";
    public const string ERR_JSON_MISSING_FILE                     = "Trying to read dialogue from file '{0}' which is missing or doesn't exist.";
    
    //----------------------------------------------------------
    //                    ROOM DATA & VALUES
    //----------------------------------------------------------
    public enum R2_ObjectsToFind
    {
        INCORRECT,
        AWARENESS,
        EDUCATION,
        HEALTH_WELLBEING,
        TRAINING,
        ADVERTISEMENT,
        ACTIVATION,
        RECRUITMENT,
        RESEARCH
    };
}