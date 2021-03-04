using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Grille))]
public class GrilleEditor : Editor
{
    private Grille m_Grille;
    private Tuile[,] m_Tuiles;

    private int m_Rowcount, m_ColumnCount;


    private void OnEnable()
    {
        m_Grille = (Grille)target;

        RefreshTilesArray();
    }

    private void RefreshTilesArray()
    {
        m_Rowcount = serializedObject.FindProperty("m_RowCount").intValue;
        m_ColumnCount = serializedObject.FindProperty("m_ColumnCount").intValue;
        m_Tuiles = new Tuile[m_ColumnCount, m_Rowcount];

        foreach (Tuile t_Tuile in m_Grille.GetComponentsInChildren<Tuile>())
        {
            m_Tuiles[t_Tuile.x, t_Tuile.y] = t_Tuile;
        }
    }

    private void OnSceneGUI()
    {
        // Si on (control + click)
        if (Event.current.type == EventType.MouseDown && Event.current.control)
        {
            // Pour prévenir la sélection par défaut, on génère un ID d'objet non sélectionnable
            GUIUtility.hotControl = GUIUtility.GetControlID(FocusType.Passive);

            // Trouver la position dans la grille où était la souris lors de l'event
            Vector3 t_ClickPosition = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
            Vector2Int t_GrillePos = m_Grille.WorldToGrid(t_ClickPosition);

            // Trouve l'ID de la tuile à instancier
            int t_SelectedTileID = serializedObject.FindProperty("m_SelectedTileID").intValue;

            // Trouver la taille de notre array de tuiles disponibles
            SerializedProperty t_TileArray = serializedObject.FindProperty("m_AvailableTiles");
            int t_TileArraySize = t_TileArray.arraySize;

            // Si l'ID de tuile est invalide
            if(t_SelectedTileID >= t_TileArraySize)
                throw new GrilleException("Selected Tile Out of bounds.");

            // Trouver la référence vers le prefab à instancier
            GameObject t_TilePrefab = (GameObject)t_TileArray.GetArrayElementAtIndex(t_SelectedTileID).objectReferenceValue;

            // Trouver les coordonnées du centre de la cellule
            Vector3 t_CellCenter = m_Grille.GridToWorld(t_GrillePos);

            // Instancier la tuile en tant que prefab, parentée avec grid
            GameObject t_Tile = (GameObject)PrefabUtility.InstantiatePrefab(t_TilePrefab, m_Grille.transform);
            //On le place au centre de la cellule
            t_Tile.transform.position = t_CellCenter;

            if (t_GrillePos.x >= m_ColumnCount || t_GrillePos.y >= m_Rowcount)
                RefreshTilesArray();

            if(m_Tuiles[t_GrillePos.x, t_GrillePos.y] != null)
            {
                DestroyImmediate(m_Tuiles[t_GrillePos.x, t_GrillePos.y].gameObject);
            }

            m_Tuiles[t_GrillePos.x, t_GrillePos.y] = t_Tile.GetComponent<Tuile>();
            m_Tuiles[t_GrillePos.x, t_GrillePos.y].x = (uint)t_GrillePos.x;
            m_Tuiles[t_GrillePos.x, t_GrillePos.y].y = (uint)t_GrillePos.y;

            float t_CellSize = serializedObject.FindProperty("m_CellSize").floatValue;
            Sprite t_Sprite = t_Tile.GetComponent<SpriteRenderer>().sprite;
            float t_Scale = t_CellSize / (t_Sprite.rect.width / t_Sprite.pixelsPerUnit);
            t_Tile.transform.localScale = new Vector3(t_Scale, t_Scale, t_Scale);
        }
    }
}
