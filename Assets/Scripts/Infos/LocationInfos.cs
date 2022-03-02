using System;

[Serializable]
public class LocationInfos
{
    public LocationInfo[] locations;

    public void PrepareLocationInfos()
    {
        foreach (LocationInfo lInfo in locations)
        {
            lInfo.PrepareLocationInfo();
        }
    }
}
