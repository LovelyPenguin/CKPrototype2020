using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    public enum PART
    {
        FACE,
        ARM,
        LEG,
        NECK
    }

    public PART part;

    public string GetPartString()
    {
        return GetPartString(part);
    }
    public static string GetPartString(PART part)
    {
        switch (part)
        {
            case PART.ARM:
                return "팔";
            case PART.FACE:
                return "얼굴";
            case PART.LEG:
                return "다리";
            case PART.NECK:
                return "목";
            default:
                return "";
        }
    }
}
