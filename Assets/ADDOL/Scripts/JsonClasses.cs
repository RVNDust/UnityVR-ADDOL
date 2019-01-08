using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonClasses {
    public static T[] getJsonArray<T>(string json)
    {
        string newJson = "{ \"array\": " + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
}

[System.Serializable]
public class Coord
{
    public double lon ;
    public double lat ;
}

[System.Serializable]
public class Weather
{
    public int id ;
    public string main ;
    public string description ;
    public string icon ;
}

[System.Serializable]
public class Main
{
    public double temp;
    public int pressure ;
    public int humidity ;
    public int temp_min ;
    public int temp_max ;
}

[System.Serializable]
public class Wind
{
    public double speed ;
    public int deg ;
}

[System.Serializable]
public class Clouds
{
    public int all ;
}

[System.Serializable]
public class Sys
{
    public int type ;
    public int id ;
    public double message ;
    public string country ;
    public int sunrise ;
    public int sunset ;
}

[System.Serializable]
public class RootObject
{
    public Coord coord ;
    public List<Weather> weather ;
    public string @base ;
    public Main main ;
    public int visibility ;
    public Wind wind ;
    public Clouds clouds ;
    public int dt ;
    public Sys sys ;
    public int id ;
    public string name ;
    public int cod ;
}