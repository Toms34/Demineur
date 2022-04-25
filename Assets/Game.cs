using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Game : MonoBehaviour
{
    public int width=8;
    private int height=8;
    private int bomb=8;
    public Grid grid;
    public Camera camera;
    public Cell[,] state;
    public bool loose=false;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("Taille",0)!=0)width=PlayerPrefs.GetInt("Taille");
        height=width;
        if(width==8)bomb=10;
        else if(width==16) bomb=40;
        else bomb=99;
        camera.orthographicSize = width/2;
        Application.targetFrameRate = 60;
        grid = GetComponentInChildren<Grid>();
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        grid = GetComponentInChildren<Grid>();
        if(Input.GetKeyDown(KeyCode.R)) Init();
        if(Input.GetKeyDown(KeyCode.Escape)) SceneManager.LoadScene("MainMenu",LoadSceneMode.Single);
        if(!loose){
            if(Input.GetMouseButtonDown(1)){
                flag();
                DidYouWin();
            }
            else if(Input.GetMouseButtonDown(0)){
                Reveal();
                DidYouWin();
            }
        }
        else{
            gameover();
        }
    }

    private void Init(){
        loose=false;
        state=new Cell[width,height];

        for(int i=0;i<width;i++){ // creation Cell
            for(int j=0;j<height;j++){
                Cell cell=new Cell();
                cell.position.x=i;
                cell.position.y=j;
                cell.position.z=0;
                cell.type =Cell.Type.Vide;
                state[i,j]=cell;
            }
        }
        

        int mine=0;
        while(mine<bomb){//on creer des mines
            int i=Random.Range(0,width);
            int j=Random.Range(0,height);
            if(GetCell(i,j).type==Cell.Type.Bombe) continue;
            GetCell(i,j).type=Cell.Type.Bombe;
            mine++;
        }
        NombreGen();
        Camera.main.transform.position = new Vector3(width / 2f, height / 2f, -10f);
        grid.Draw(state);
    }
    private void NombreGen(){
        for(int i=0;i<width;i++){ //IOAT
            for(int j=0;j<height;j++){//GReatest if of all times
                if(GetCell(i,j).type==Cell.Type.Bombe)continue;
                state[i,j].nombre=CountMines(i,j);
                if(GetCell(i,j).nombre>0) state[i,j].type=Cell.Type.Nombre;
            }
        }

    }

    private int CountMines(int cellX, int cellY)
    {
        int count = 0;

        for (int adjacentX = -1; adjacentX <= 1; adjacentX++){
            for (int adjacentY = -1; adjacentY <= 1; adjacentY++){
                if (adjacentX == 0 && adjacentY == 0)continue;

                int x = cellX + adjacentX;
                int y = cellY + adjacentY;

                if (IsValid(x,y) && GetCell(x, y).type == Cell.Type.Bombe)count++;
            }
        }
        return count;
    }

    private void flag(){
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = grid.tilemap.WorldToCell(worldPosition);
        Cell cell=GetCell(cellPosition.x , cellPosition.y);

        if(!cell.reveal){
            cell.flag=!cell.flag;
            state[cellPosition.x , cellPosition.y]=cell;
        }
        grid.Draw(state);
    }

    private void Reveal(){
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = grid.tilemap.WorldToCell(worldPosition);
        Cell cell=GetCell(cellPosition.x , cellPosition.y);
        grid.Click(cell);
        if(cell.flag) return;
        switch(cell.type){
            case Cell.Type.Vide: 
                RevealVide(cellPosition.x,cellPosition.y);
                break;
            case Cell.Type.Bombe: 
                Bomberino(cell);
                break;
            default:
                RevealNombre(cell);
                break;
        }
        grid.Draw(state);
    }
    private void Bomberino(Cell cell){
        loose=true;
        cell.explostion=true;
        cell.reveal=true;
    }
    private void RevealNombre(Cell cell){
        cell.reveal=true;
    }
    private void gameover(){
        for(int i=0;i<width;i++){
            for(int j=0;j<height;j++) state[i,j].reveal=true;
        }
        grid.Draw(state);
    }
    private void RevealVide(int x,int y){
        if(IsValid(x,y) && GetCell(x,y).type==Cell.Type.Vide && !GetCell(x,y).reveal){
            state[x,y].reveal=true;
            Cell cell=state[x,y];
            RevealVide(cell.position.x,cell.position.y+1);
            RevealVide(cell.position.x,cell.position.y-1);
            RevealVide(cell.position.x+1,cell.position.y);
            RevealVide(cell.position.x-1,cell.position.y);
        }
        else if(IsValid(x,y) && GetCell(x,y).type==Cell.Type.Nombre) state[x,y].reveal=true;
    }
    private Cell GetCell(int x, int y)
    {
        if (IsValid(x, y)) {
            return state[x, y];
        } else {
            return new Cell();
        }
    }

    private bool IsValid(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }
    private void DidYouWin(){
        int count=0;
        for (int i = 0; i < width; i++){
            for (int j = 0; j < height; j++){
                if(state[i,j].type==Cell.Type.Bombe) {count++;}
                else if(state[i,j].reveal==true && state[i,j].explostion==false) {count++;}
            }
        }
        if(count==width*height) Win();    
    }
    private void Win(){
        for (int i = 0; i < width; i++){
            for (int j = 0; j < height; j++){
                state[i,j].reveal=true;
                state[i,j].flag=false;
            }
        }
        grid.Draw(state);
    }
}
