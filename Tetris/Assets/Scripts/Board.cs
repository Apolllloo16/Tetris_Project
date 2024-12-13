using UnityEngine;
using UnityEngine.Tilemaps;


public class Board : MonoBehaviour
{
    public static Board instance;

    public Tilemap tilemap {get; private set;}
    public Piece activepiece {get; private set;}
    public Pause_Menu pauseMenu {get; private set;}
    public TetrominoData[] tetrominoes;
    public Vector3Int spawnPosition;
    public Vector2Int boardSize = new Vector2Int(10,20);
    public bool dead = false; 

    public int score { get; private set; }
    public int level { get; private set; }
    public int linesCleared { get; private set; }

    private int linesPerLevel = 10; 

    public int linesTillNextLevel
    {
        get { return (level * linesPerLevel) - linesCleared; }
    }

    public RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-this.boardSize.x/2,-this.boardSize.y/2);
            return new RectInt(position, this.boardSize);
        }
    } 

    private void Awake()
    {
        instance = this;

        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activepiece = GetComponentInChildren<Piece>();
        this.pauseMenu = GetComponentInChildren<Pause_Menu>();

        this.score = 0;
        this.level = 1;
        this.linesCleared = 0;

        for (int i=0; i < this.tetrominoes.Length; i++ )
        {
            this.tetrominoes[i].Initialise();
        }
    }

    private void Start()
    {
        SpawnPiece();
        score = 0;
        level = 1;
        linesCleared = 0;
        dead = false;
    }

    public void SpawnPiece()
    {
        int random = Random.Range(0,this.tetrominoes.Length);
        TetrominoData data = this.tetrominoes[random];

        this.activepiece.Initialize(this, this.spawnPosition, data);

        pauseMenu = FindFirstObjectByType<Pause_Menu>();
    if (pauseMenu != null)
    {
        pauseMenu.Initialize(this);
    }

        if (IsValidPosition(this.activepiece, this.spawnPosition))
        {
            Set(this.activepiece);
        }
        else
        {
            GameOver();
        }

        Set(this.activepiece);
    }

    private void GameOver()
    {
        dead = true;
        this.tilemap.ClearAllTiles();
    }

    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

      public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, null);
        }
    }

    public bool IsValidPosition(Piece piece, Vector3Int position)
    {
        RectInt bounds = this.Bounds;

        if(dead)
        {
            return false;
        }


        for(int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + position;

            if(!bounds.Contains((Vector2Int)tilePosition))
            {
                return false;
            }

            if(this.tilemap.HasTile(tilePosition))
            {
                return false;
            }            
        }

        return true;
    }


    public void ClearLines()
    {

        int linesClearedThisTurn = 0;
        

        RectInt bounds = this.Bounds;
        int row = bounds.yMin;

        while(row < bounds.yMax)
        {
            if(IsLineFull(row))
            {
                LineClear(row);
                linesClearedThisTurn++;
            }
            else 
            {
                row++;
            }
        }

        if (linesClearedThisTurn > 0)
        {
            UpdateScore(linesClearedThisTurn);
            if (linesCleared > (linesTillNextLevel * level))
            {
                UpdateLevel();
            }
        }

    }

    private void UpdateScore(int linesCleared)
    {
        // Scoring system: multiply based on number of lines cleared at once
        int points = 0;
        switch (linesCleared)
        {
            case 1: points = 100; break;
            case 2: points = 300; break;
            case 3: points = 500; break;
            case 4: points = 800; break;
        }

        this.score += points * this.level;
        this.linesCleared += linesCleared;

        Debug.Log($"Score: {this.score}, Level: {this.level}, Lines Cleared: {this.linesCleared}");
    }

    private void UpdateLevel()
    {
        int newLevel = (this.linesCleared / this.linesPerLevel) + 1;
        if (newLevel > this.level)
        {
            this.level = newLevel;
            this.activepiece.UpdateStepDelay(this.level); // Update piece speed
        }
    }

    private bool IsLineFull(int row)
    {
        RectInt bounds = this.Bounds;

        for(int col = bounds.xMin; col < bounds.xMax; col++ )
        {
            Vector3Int position = new Vector3Int(col,row,0);

            if (!this.tilemap.HasTile(position))
            {
                return false;
            }
        }

        return true;

    }

    private void LineClear(int row)
    {
        RectInt bounds = this.Bounds;

        for(int col = bounds.xMin; col < bounds.xMax; col++ )
        {
            Vector3Int position = new Vector3Int(col,row,0);
            this.tilemap.SetTile(position,null);
        }

        while (row < bounds.yMax)
        {
            for(int col = bounds.xMin; col < bounds.xMax; col++ )
            {
                Vector3Int position = new Vector3Int(col,row+1,0);
                TileBase above = this.tilemap.GetTile(position);

                position = new  Vector3Int (col,row,0);
                this.tilemap.SetTile(position,above);
            }

            row++;
        }
    }
}


