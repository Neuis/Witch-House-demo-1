using System;

[Serializable]
public class LocalizationInfo
{
    public int id;
    public string name;
    public string text_ru;
    public string text_en;
    public LocalizationReplacementInfo[] replacements;
}
