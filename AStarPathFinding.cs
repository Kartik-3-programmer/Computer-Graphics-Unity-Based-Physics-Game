using System.Collections.Generic;
using UnityEngine;

public class AStarPathFinding : MonoBehaviour
{
    public List<MazeCell> FindPath(MazeCell startCell, MazeCell endCell)
    {
        var openSet = new SortedSet<MazeCell>(new MazeCellComparer());
        var cameFrom = new Dictionary<MazeCell, MazeCell>();
        var gScore = new Dictionary<MazeCell, float>();
        var fScore = new Dictionary<MazeCell, float>();

        gScore[startCell] = 0;
        fScore[startCell] = Heuristic(startCell, endCell);

        openSet.Add(startCell);

        while (openSet.Count > 0)
        {
            MazeCell current = GetCellWithLowestFScore(openSet, fScore);

            if (current == endCell)
                return ReconstructPath(cameFrom, current);

            openSet.Remove(current);

            foreach (MazeCell neighbor in current.GetNeighbors())
            {
                float tentativeGScore = gScore[current] + DistanceBetween(current, neighbor);

                if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor, endCell);

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        return new List<MazeCell>(); // Return empty if no path found
    }

    private float Heuristic(MazeCell a, MazeCell b)
    {
        Vector3 posA = a.transform.position;
        Vector3 posB = b.transform.position;
        return Mathf.Abs(posA.x - posB.x) + Mathf.Abs(posA.z - posB.z); // Manhattan distance
    }

    private float DistanceBetween(MazeCell a, MazeCell b)
    {
        return 1; // Uniform cost
    }

    private MazeCell GetCellWithLowestFScore(SortedSet<MazeCell> openSet, Dictionary<MazeCell, float> fScore)
    {
        MazeCell lowest = null;
        float minScore = float.MaxValue;

        foreach (MazeCell cell in openSet)
        {
            float score = fScore.ContainsKey(cell) ? fScore[cell] : float.MaxValue;
            if (score < minScore)
            {
                minScore = score;
                lowest = cell;
            }
        }

        return lowest;
    }

    private List<MazeCell> ReconstructPath(Dictionary<MazeCell, MazeCell> cameFrom, MazeCell current)
    {
        var path = new List<MazeCell> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(current);
        }

        path.Reverse();
        return path;
    }
}

public class MazeCellComparer : IComparer<MazeCell>
{
    public int Compare(MazeCell x, MazeCell y)
    {
        return x.GetInstanceID().CompareTo(y.GetInstanceID());
    }
}