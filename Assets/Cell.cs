using UnityEngine;

public class Cell
{
    public enum Type{
        Vide,
        Bombe,
        Nombre,
    }
    
    public Type type;
    public int nombre;
    public Vector3Int position=new Vector3Int();
    //bool
    public bool flag=false;
    public bool explostion=false;
    public bool reveal=false;
}
