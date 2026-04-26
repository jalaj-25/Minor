using UnityEngine;

public class GridLines : MonoBehaviour
{
    public float cellSize = 0.5f;
    public Color gridColor = Color.black;

    private Material lineMaterial;
    private bool showGrid = false;

    void Start()
    {
        Shader shader = Shader.Find("Hidden/Internal-Colored");
        lineMaterial = new Material(shader);
    }

    void Update()
    {
        // optional manual toggle
        if (Input.GetKeyDown(KeyCode.B))
        {
            showGrid = !showGrid;
        }
    }

    public void ShowGrid()
    {
        showGrid = true;
    }

    public void HideGrid()
    {
        showGrid = false;
    }

    void OnRenderObject()
    {
        if (!showGrid) return;
        if (!lineMaterial) return;

        lineMaterial.SetPass(0);

        Renderer r = GetComponent<Renderer>();
        if (!r) return;

        Vector3 size = r.bounds.size;
        Vector3 origin = r.bounds.min;

        int width = Mathf.RoundToInt(size.x / cellSize);
        int height = Mathf.RoundToInt(size.z / cellSize);

        GL.PushMatrix();
        GL.Begin(GL.LINES);
        GL.Color(gridColor);

        for (int x = 0; x <= width; x++)
        {
            GL.Vertex(new Vector3(origin.x + x * cellSize, origin.y + 0.02f, origin.z));
            GL.Vertex(new Vector3(origin.x + x * cellSize, origin.y + 0.02f, origin.z + height * cellSize));
        }

        for (int z = 0; z <= height; z++)
        {
            GL.Vertex(new Vector3(origin.x, origin.y + 0.02f, origin.z + z * cellSize));
            GL.Vertex(new Vector3(origin.x + width * cellSize, origin.y + 0.02f, origin.z + z * cellSize));
        }

        GL.End();
        GL.PopMatrix();
    }
}