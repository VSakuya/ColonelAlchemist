[System.Serializable]
public class L_DiagDataArray
{
    public L_DiagData[] items;
}

[System.Serializable]
public class L_DiagData
{
    public string key;
    public L_DiagItem[] value;
}

[System.Serializable]
public class L_DiagItem
{
    public string speaker;
    public string animation;
    public string text;
}