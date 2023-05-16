using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Graphics : MonoBehaviour
{
    public static void InitialiseCamera()
    {
        Camera mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        mainCamera.enabled = true;
        mainCamera.orthographic = true;
        mainCamera.orthographicSize = Main.boardScale * 5;
        mainCamera.transform.position = new Vector3(Main.boardScale * 3.5f, Main.boardScale * 3.5f, -10);
    }

    public static void DrawBoard()
    {
        GameObject boardPieces = new GameObject("Board");

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                bool isLight = (x + y) % 2 != 0;

                Color squareColor = isLight ? Main.lightColor : Main.darkColor;

                var position = new Vector2(x, y);

                DrawSquare(squareColor, position);
            }
        }
    }

    private static void DrawSquare(Color squareColor, Vector2 position)
    {
        //Generate
        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quad.transform.position = position * Main.boardScale;
        quad.transform.localScale = new Vector2(Main.boardScale, Main.boardScale);
        quad.transform.parent = GameObject.Find("Board").transform;

        //Name
        string quadName = position.x.ToString() + position.y.ToString();
        quad.name = quadName;

        //Color
        Renderer quadRenderer = quad.GetComponent<Renderer>();  
        quadRenderer.material.color = squareColor;
        quadRenderer.sortingOrder = 1;
    }

    public static void DrawPieces()
    {
        GameObject boardPieces = new GameObject("Pieces");

        int count = 0;
        for(int x = 0; x < 8; x++)
        {
            for(int y = 0; y < 8; y++)
            {

                count++;
                if (Main.gameBoard.board[x, y] != null)
                {
                    Main.gameBoard.board[x,y].gameObject = DrawPiece(new Vector2(x, y));
                }
            }
        }
    }

    public static GameObject DrawPiece(Vector2 position)
    {
        char type = Main.gameBoard.board[(int)position.x, (int)position.y].type;
        char color = Main.gameBoard.board[(int)position.x, (int)position.y].color;

        //Generate Texture
        string path = "Assets\\sprites\\" + type + color + ".bytes";
        byte[] bytes = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(1, 1);
        ImageConversion.LoadImage(texture, bytes);

        //Generate Sprite
        Rect rect = new Rect(0, 0, texture.width, texture.height);
        Sprite sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f), 100.0f);

        GameObject gameObject = new GameObject(type.ToString());
        SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        spriteRenderer.sortingOrder = 10;

        gameObject.transform.position = position * Main.boardScale;
        gameObject.transform.localScale = new Vector2(2 * Main.boardScale, 2 * Main.boardScale);
        gameObject.transform.parent = GameObject.Find("Pieces").transform;

        gameObject.AddComponent<BoxCollider2D>();
        gameObject.AddComponent<PieceDrag>();

        return gameObject;
    }

    public static void DeletePiece(IPiece piece)
    {
        GameObject.Destroy(piece.gameObject);
    }

    public static void ClearBoardDot()
    {
        for(int x = 0; x < 8; x++)
        {
            for(int y = 0; y < 8; y++)
            {
                GameObject.Destroy(GameObject.Find(x.ToString() + y.ToString() + 'd'));
            }
        }
    }

    public static void AddBoardDot(Vector2 position)
    {
        GameObject parent = GameObject.Find(position.x.ToString() + position.y.ToString());

        //Generate
        GameObject dot = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        dot.transform.position = position * Main.boardScale;
        dot.transform.localScale = new Vector2(0.3f * Main.boardScale, 0.3f * Main.boardScale);
        dot.transform.parent = GameObject.Find(position.x.ToString() + position.y.ToString()).transform;

        //Name
        string dotName = position.x.ToString() + position.y.ToString() + 'd';
        dot.name = dotName;

        //Color
        Renderer dotRenderer = dot.GetComponent<Renderer>();
        dotRenderer.material.color = Main.dotColor;
        dotRenderer.sortingOrder = 5;

        if (Main.gameBoard.board[(int)position.x, (int)position.y] != null)
        {
            dot.transform.localScale *= new Vector2(0.8f/0.3f, 0.8f/0.3f);

            //Generate
            GameObject dot2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            dot2.transform.position = position * Main.boardScale;
            dot2.transform.localScale = new Vector2(0.7f * Main.boardScale,0.7f * Main.boardScale);
            dot2.transform.parent = dot.transform;

            //Name
            string dot2Name = position.x.ToString() + position.y.ToString() + "di";
            dot2.name = dot2Name;

            //Color
            Renderer dot2Renderer = dot2.GetComponent<Renderer>();

            if ((position.x + position.y) % 2 != 0) {
                dot2Renderer.material.color = Main.lightColor;
            }
            else
            {
                dot2Renderer.material.color = Main.darkColor;
            }
            dot2Renderer.sortingOrder = 6;

        }


    }
}
