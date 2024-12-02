using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private MazeCell _mazeCellPrefab;
    [SerializeField] private GameObject _boundaryWallPrefab;
    [SerializeField] private GameObject endTriggerPrefab;

    [SerializeField] private Vector2Int _startPosition = new Vector2Int(0, 0);
    private Vector2Int _endPosition;

    private MazeCell[,] _mazeGrid;
    private int _mazeWidth, _mazeLength;

    void Start()
    {
        SetMazeSize(GameManager.difficultyLevel);
        GenerateMazeStructure();
        PlaceEndTrigger();
    }

    private void SetMazeSize(int difficulty)
    {
        switch (difficulty)
        {
            case 1:
                _mazeWidth = 5;
                _mazeLength = 5;
                break;
            case 2:
                _mazeWidth = 10;
                _mazeLength = 10;
                break;
            case 3:
                _mazeWidth = 15;
                _mazeLength = 15;
                break;
        }
    }

    private void GenerateMazeStructure()
    {
        _endPosition = GetOppositeCornerPosition(_startPosition);
        _mazeGrid = new MazeCell[_mazeLength, _mazeWidth];

        // Instantiate the maze cells
        for (int x = 0; x < _mazeLength; x++)
        {
            for (int z = 0; z < _mazeWidth; z++)
            {
                _mazeGrid[x, z] = Instantiate(_mazeCellPrefab, new Vector3(x, 0, z), Quaternion.identity);
            }
        }

        OpenStartAndEndWalls();
        CreateMazeBoundary();

        // Generate the maze using DFS or another backtracking algorithm
        GenerateMaze(null, _mazeGrid[_startPosition.x, _startPosition.y]);
    }

    private Vector2Int GetOppositeCornerPosition(Vector2Int startPosition)
    {
        return new Vector2Int(_mazeLength - 1, _mazeWidth - 1);
    }

    private void OpenStartAndEndWalls()
    {
        MazeCell startCell = _mazeGrid[_startPosition.x, _startPosition.y];
        startCell.ClearDownWall();

        MazeCell endCell = _mazeGrid[_endPosition.x, _endPosition.y];
        endCell.ClearUpWall();
    }

    private void CreateMazeBoundary()
    {
        float wallHeight = _boundaryWallPrefab.transform.localScale.y;
        float wallThickness = 0.1f;
        float gap = 0.7f;

        InstantiateBoundaryWall((_mazeLength - 1) / 2f, -gap - wallThickness / 2, _mazeLength, wallHeight, wallThickness, false);
        InstantiateBoundaryWall((_mazeLength - 1) / 2f, _mazeWidth - 1 + gap + wallThickness / 2, _mazeLength, wallHeight, wallThickness, false);
        InstantiateBoundaryWall(-gap - wallThickness / 2, (_mazeWidth - 1) / 2f, _mazeWidth, wallHeight, wallThickness, true);
        InstantiateBoundaryWall(_mazeLength - 1 + gap + wallThickness / 2, (_mazeWidth - 1) / 2f, _mazeWidth, wallHeight, wallThickness, true);
    }

    private void InstantiateBoundaryWall(float posX, float posZ, float length, float height, float thickness, bool rotate)
    {
        GameObject wall = Instantiate(_boundaryWallPrefab, new Vector3(posX, height / 2f, posZ), rotate ? Quaternion.Euler(0, 90, 0) : Quaternion.identity);
        wall.transform.localScale = new Vector3(length, height, thickness);
    }

    private void GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        if (currentCell == null || currentCell.isVisited) return;

        currentCell.Visit();
        if (previousCell != null) ClearWallsBetween(previousCell, currentCell);

        var neighbors = GetUnvisitedNeighbors(currentCell).OrderBy(_ => Random.value).ToList();
        foreach (var neighbor in neighbors)
        {
            GenerateMaze(currentCell, neighbor);
        }
    }

    private IEnumerable<MazeCell> GetUnvisitedNeighbors(MazeCell cell)
    {
        Vector2Int position = GetCellPosition(cell);
        int x = position.x, z = position.y;

        if (x > 0 && !_mazeGrid[x - 1, z].isVisited) yield return _mazeGrid[x - 1, z];
        if (x < _mazeLength - 1 && !_mazeGrid[x + 1, z].isVisited) yield return _mazeGrid[x + 1, z];
        if (z > 0 && !_mazeGrid[x, z - 1].isVisited) yield return _mazeGrid[x, z - 1];
        if (z < _mazeWidth - 1 && !_mazeGrid[x, z + 1].isVisited) yield return _mazeGrid[x, z + 1];
    }

    private Vector2Int GetCellPosition(MazeCell cell)
    {
        return new Vector2Int((int)cell.transform.position.x, (int)cell.transform.position.z);
    }

    private void ClearWallsBetween(MazeCell a, MazeCell b)
    {
        Vector2Int posA = GetCellPosition(a);
        Vector2Int posB = GetCellPosition(b);

        if (posA.x < posB.x) { a.ClearRightWall(); b.ClearLeftWall(); }
        else if (posA.x > posB.x) { a.ClearLeftWall(); b.ClearRightWall(); }
        else if (posA.y < posB.y) { a.ClearUpWall(); b.ClearDownWall(); }
        else if (posA.y > posB.y) { a.ClearDownWall(); b.ClearUpWall(); }
    }

    private void PlaceEndTrigger()
    {
        Vector3 endWorldPos = new Vector3(_endPosition.x, 0, _endPosition.y);
        Instantiate(endTriggerPrefab, endWorldPos, Quaternion.identity).tag = "EndPoint";
    }

    public MazeCell GetMazeCellAtPosition(Vector2Int position)
    {
        return _mazeGrid[position.x, position.y];
    }

    public void HighlightPath(Queue<MazeCell> path)
    {
        foreach (var cell in path)
        {
            cell.Highlight();
        }
    }
    public Vector2Int GetStartPosition()
    {
        return _startPosition;
    }

    public Vector2Int GetEndPosition()
    {
        return _endPosition;
    }
}
