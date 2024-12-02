using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    [SerializeField] private GameObject _leftwall;
    [SerializeField] private GameObject _rightwall;
    [SerializeField] private GameObject _upwall;
    [SerializeField] private GameObject _downwall;
    [SerializeField] private GameObject _unvisitedBlock;

    public bool isVisited { get; private set; }
    private Vector2Int _position;
    private MazeCell[,] _grid;

    public void Initialize(int x, int z, MazeCell[,] grid)
    {
        _position = new Vector2Int(x, z);
        _grid = grid;
    }

    public void Visit()
    {
        isVisited = true;
        _unvisitedBlock.SetActive(false);
    }

    public void ClearLeftWall() => _leftwall.SetActive(false);
    public void ClearRightWall() => _rightwall.SetActive(false);
    public void ClearUpWall() => _upwall.SetActive(false);
    public void ClearDownWall() => _downwall.SetActive(false);

    public void Highlight()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.color = Color.green;
    }

    public void ResetHighlight()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.color = Color.white;
    }

    public List<MazeCell> GetNeighbors()
    {
        List<MazeCell> neighbors = new List<MazeCell>();
        int x = _position.x;
        int z = _position.y;

        if (x + 1 < _grid.GetLength(0)) neighbors.Add(_grid[x + 1, z]);
        if (x - 1 >= 0) neighbors.Add(_grid[x - 1, z]);
        if (z + 1 < _grid.GetLength(1)) neighbors.Add(_grid[x, z + 1]);
        if (z - 1 >= 0) neighbors.Add(_grid[x, z - 1]);

        return neighbors;
    }
}