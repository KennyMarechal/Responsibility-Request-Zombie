using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grille : MonoBehaviour
{
    private Tuile[,] m_Tuiles;

    [SerializeField]
    private uint m_RowCount = 10;
    [SerializeField]
    private uint m_ColumnCount = 10;
    [SerializeField]
    private float m_CellSize = 1;

    [SerializeField]
    private Color m_GridColor = Color.yellow;
    [SerializeField]
    private bool m_ShowGrid = true;
    public bool AutoFetchTile = false;

#if UNITY_EDITOR
    [Space]
    [Header("Grille Editor")]

    [SerializeField]
    private GameObject[] m_AvailableTiles;
    [SerializeField]
    private uint m_SelectedTileID;
#endif

    public uint RowCount
    {
        get { return m_RowCount; }
    }

    public uint ColumnCount
    {
        get { return m_ColumnCount; }
    }

    private void OnDrawGizmos()
    {
        if (!m_ShowGrid)
            return;

        Gizmos.color = m_GridColor;

        for (int i = 0; i <= m_RowCount; i++)
        {
            float t_DepartX = transform.position.x;
            float t_ArriveX = t_DepartX + (m_ColumnCount * m_CellSize);

            float t_Y = transform.position.y + (i * m_CellSize);

            Gizmos.DrawLine(new Vector3(t_DepartX, t_Y, 0), new Vector3(t_ArriveX, t_Y, 0));
        }

        for (int i = 0; i <= m_ColumnCount; i++)
        {
            float t_X = transform.position.x + (i * m_CellSize);

            float t_DepartY = transform.position.y;
            float t_ArriveY = t_DepartY + (m_RowCount * m_CellSize);

            Gizmos.DrawLine(new Vector3(t_X, t_DepartY, 0), new Vector3(t_X, t_ArriveY, 0));
        }
    }

    private void Awake()
    {
        if (AutoFetchTile)
        {
        m_Tuiles = new Tuile[m_ColumnCount, m_RowCount];

        foreach (Tuile t_Tuile in GetComponentsInChildren<Tuile>())
        {
            m_Tuiles[t_Tuile.x, t_Tuile.y] = t_Tuile;
        }
        }
        
    }

    public Vector3 GridToWorld(Vector2Int a_GridPos)
    {
        if (!IsInGrid(a_GridPos))
            throw new GrilleException("Position out of grid.");

        float t_WorldX = transform.position.x + (a_GridPos.x * m_CellSize) + (m_CellSize / 2);
        float t_WorldY = transform.position.y + (a_GridPos.y * m_CellSize) + (m_CellSize / 2);

        return new Vector3(t_WorldX, t_WorldY, 0);
    }

    public Vector2Int WorldToGrid(Vector3 a_WorldPos)
    {
        if (!IsInGrid(a_WorldPos))
            throw new GrilleException("Position out of grid.");

        int t_GridX = (int)((a_WorldPos.x - transform.position.x) / m_CellSize);
        int t_GridY = (int)((a_WorldPos.y - transform.position.y) / m_CellSize);

        return new Vector2Int(t_GridX, t_GridY);
    }

    public Tuile GetTuile(Vector2Int a_GridPos)
    {
        if (!IsInGrid(a_GridPos))
            throw new GrilleException("Position out of grid.");

        return m_Tuiles[a_GridPos.x, a_GridPos.y];
    }

    private bool IsInGrid(Vector2Int a_GridPos)
    {
        if (a_GridPos.x >= m_ColumnCount || a_GridPos.y >= m_RowCount)
            return false;

        if (a_GridPos.x < 0 || a_GridPos.y < 0)
            return false;

        return true;
    }

    private bool IsInGrid(Vector3 a_WorldPos)
    {
        //Si la coordonnée demandée est plus petite que la position de notre grille (à gauche ou en dessous)
        if (a_WorldPos.x < transform.position.x || a_WorldPos.y < transform.position.y)
            return false;

        if (a_WorldPos.x >= transform.position.x + (m_ColumnCount * m_CellSize) || a_WorldPos.y >= transform.position.y + (m_RowCount * m_CellSize))
            return false;

        return true;
    }

    public void AppellerTuiles(Transform a_parent)
    {
        m_Tuiles = new Tuile[m_ColumnCount, m_RowCount];

        foreach (Tuile t_Tuile in a_parent.GetComponentsInChildren<Tuile>())
        {
            m_Tuiles[t_Tuile.x, t_Tuile.y] = t_Tuile;
        }
    }

    public Tuile[,] Tuiles
    {
        get { return m_Tuiles; }
    }
}
