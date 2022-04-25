using UnityEngine;
using UnityEngine.Tilemaps;
[RequireComponent(typeof(Tilemap))]
public class Grid : MonoBehaviour
{
    public Tile tileUnknown;
    public Tile tileEmpty;
    public Tile tileMine;
    public Tile tileExploded;
    public Tile tileFlag;
    public Tile tileNum1;
    public Tile tileNum2;
    public Tile tileNum3;
    public Tile tileNum4;
    public Tile tileNum5;
    public Tile tileNum6;
    public Tile tileNum7;
    public Tile tileNum8;
    //private  unsafe Tile* tileList = (tileNum1, tileNum2, tileNum3, tileNum4, tileNum5, tileNum6, tileNum7, tileNum8);
    public Tilemap tilemap{get;private set;}
    //public unsafe Tile* TileList { get => tileList; set => tileList = value; }

    public void Awake(){
        tilemap = GetComponent<Tilemap>();
    }

    public Tile GetTile(Cell cell){
        if(cell.reveal) return Click(cell);
        if(cell.flag) return tileFlag;
        return tileUnknown;
    }
    public Tile Click(Cell cell){
        switch(cell.type){
            case Cell.Type.Nombre : return NombreClick(cell);
            case Cell.Type.Bombe : return BombeClick(cell);
            default: return VideClick(cell);
        }
    }
    public void Draw(Cell[,] state){
        int width = state.GetLength(0);
        int height = state.GetLength(1);
        for(int i=0;i<width;i++){
            for(int j=0;j<height;j++){
                Cell cell=state[i,j];
                tilemap.SetTile(cell.position,GetTile(cell));
            }
        }
    }
    public Tile Reveler(Cell cell){
        switch(cell.type){
            case Cell.Type.Vide : return tileEmpty;
            case Cell.Type.Bombe : return cell.explostion ? tileExploded : tileMine;
            case Cell.Type.Nombre : return NombreClick(cell);
            default :return null;
        }
    }

    public Tile VideClick(Cell cell){
        return tileEmpty;
    }
    
    public Tile BombeClick(Cell cell){
        if(cell.explostion) return tileExploded;
        return tileMine;
    }
    public Tile NombreClick(Cell cell){
        switch (cell.nombre)
        {
            case 1: return tileNum1;
            case 2: return tileNum2;
            case 3: return tileNum3;
            case 4: return tileNum4;
            case 5: return tileNum5;
            case 6: return tileNum6;
            case 7: return tileNum7;
            default: return tileNum8;
        }
    }


}
