using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HintButton : MonoBehaviour
{
    public Button hintButton; // Reference to the UI Button
    public MazeGenerator mazeGenerator; // Reference to the Maze Generator
    public AStarPathFinding aStarPathfinding; // Reference to AStarPathFinding script

    void Start()
    {
        if (hintButton != null)
        {
            hintButton.onClick.AddListener(OnHintButtonClicked); // Register the click event
        }
    }

    // Make sure this method is public so it appears in the OnClick dropdown
    public void OnHintButtonClicked()
    {
        // Get start and end positions
        Vector2Int startPosition = mazeGenerator.GetStartPosition();
        Vector2Int endPosition = mazeGenerator.GetEndPosition();

        // Get the MazeCell for start and end positions
        MazeCell startCell = mazeGenerator.GetMazeCellAtPosition(startPosition);
        MazeCell endCell = mazeGenerator.GetMazeCellAtPosition(endPosition);

        // Get the path as a List<MazeCell>
        List<MazeCell> path = aStarPathfinding.FindPath(startCell, endCell);

        // Convert the List<MazeCell> to a Queue<MazeCell> (if necessary)
        Queue<MazeCell> pathQueue = new Queue<MazeCell>(path);

        // Highlight the path in the maze
        mazeGenerator.HighlightPath(pathQueue);
    }
}
