using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Stage : MonoBehaviour
{
    //�ʿ� �ҽ� �ҷ�����
    [Header("Source")]
    public GameObject tilePrefab;
    public Transform backgroundNode;
    public Transform boardNode;
    public Transform tetrominoNode;

    public GameObject gameoverPanel;
    public Text score;
    public Text level;
    public Text line;

    [Header("Setting")]
    [Range(4, 40)]
    public int boardWidth = 10;
    //���� ����
    [Range(5, 20)]
    public int boardHeight = 20;
    //�������� �ӵ�
    public float fallCycle = 1.0f;
    //��ġ ����
    public float offset_x = 0f;
    public float offset_y = 0f;

    private int halfWidth;
    private int halfHeight;

    private float nextFallTime; // ������ ��Ʈ�ι̳밡 ������ �ð��� ����

    // UI ���� ����
    private int scoreVal = 0;
    private int levelVal = 1;
    private int lineVal;


    private void Start() // ������ ���۵ǰ� �� ���� ����
    {
        // ���� ���۽� text ����
        lineVal = levelVal * 2;   // �ӽ� ���� ������
        score.text = "" + scoreVal;
        level.text = "" + levelVal;
        line.text = "" + lineVal;

        // ���� ���۽� ���ӿ��� �г� off
        gameoverPanel.SetActive(false);

        halfWidth = Mathf.RoundToInt(boardWidth * 0.5f);    // �ʺ��� �߰��� �������ֱ�
        halfHeight = Mathf.RoundToInt(boardHeight * 0.5f);   // ������ �߰��� �������ֱ�

        nextFallTime = Time.time + fallCycle;   // ������ ��Ʈ�ι̳밡 ������ �ð� ����

        CreateBackground(); // ��� �����

        // ���̸�ŭ �� ��� ������ֱ�
        for (int i = 0; i < boardHeight; ++i)
        {
            // ToString�� �̿��Ͽ� ������Ʈ �̸� ����
            var col = new GameObject((boardHeight - i - 1).ToString());
            // ��ġ���� -> �� ��ġ�� ����, ���� �߾�
            col.transform.position = new Vector3(0, halfHeight - i, 0);
            // ���� ����� �ڽ����� ����
            col.transform.parent = boardNode;
        }

        CreateTetromino(); // ��Ʈ�ι̳� �����
    }


    void Update()   // �� �����Ӹ��� ����
    {
        // ���ӿ��� ó��
        if (gameoverPanel.activeSelf)
        {
            if (Input.GetKeyDown("r"))
            {
                SceneManager.LoadScene(0);
            }
        }
        // ���� ó��
        else
        {
            //�ʱ�ȭ
            Vector3 moveDir = Vector3.zero; //�̵� ���� �����
            bool isRotate = false;  // ȸ�� ���� �����

            //�� Ű�� ���� �̵� ���� Ȥ�� ȸ�� ���θ� �������ݴϴ�.
            if (Input.GetKeyDown("a"))
            {
                moveDir.x = -1;

            }
            else if (Input.GetKeyDown("d"))
            {
                moveDir.x = 1;
            }

            if (Input.GetKeyDown("w"))
            {
                isRotate = true;
            }
            else if (Input.GetKeyDown("s"))
            {
                moveDir.y = -1;
            }


            if (Input.GetKeyDown("space"))
            {
                // ��Ʈ�ι̳밡 �ٴڿ� ���� ������ �Ʒ��� �̵�
                while (MoveTetromino(Vector3.down, false))
                {
                }
            }

            if (Input.GetKeyDown("r"))
            {
                // SceneManager�� �̿��Ͽ� ���� ������ϱ�
                // ���� ���� using UnityEngine.SceneManagement; �߰� �ʿ�
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

            // �Ʒ��� �������� ���� ������ �̵���ŵ�ϴ�.
            if (Time.time > nextFallTime)
            {
                nextFallTime = Time.time + fallCycle;   // ���� ������ �ð� �缳��
                moveDir.y = -1; //  �Ʒ��� �� ĭ �̵�
                isRotate = false;   // ������ �̵��� ȸ�� ����
            }

            // �ƹ��� Ű �Է��� ������� Tetromino �������� �ʰ��ϱ�
            if (moveDir != Vector3.zero || isRotate)
            {
                MoveTetromino(moveDir, isRotate);
            }
        }
    }

    // �̵��� �����ϸ� true, �Ұ����ϸ� false�� return
    bool MoveTetromino(Vector3 moveDir, bool isRotate)
    {
        //�̵� or ȸ�� �Ұ��� ���ư��� ���� �� ����
        Vector3 oldPos = tetrominoNode.transform.position;
        Quaternion oldRot = tetrominoNode.transform.rotation;

        //�̵� & ȸ�� �ϱ�
        tetrominoNode.transform.position += moveDir;
        if (isRotate)
        {
            //���� ��Ʈ�ι̳� ��忡 90�� ȸ���� ���� ��.
            tetrominoNode.transform.rotation *= Quaternion.Euler(0, 0, 90);
        }

        // �̵� �Ұ��� ���� ��ġ, ȸ�� ���� ���ư���
        if (!CanMoveTo(tetrominoNode))
        {
            tetrominoNode.transform.position = oldPos;
            tetrominoNode.transform.rotation = oldRot;

            //�ٴڿ� ��Ҵٴ� �ǹ� = �̵� �Ұ��ϰ� ���� �Ʒ��� �������� �ִ� ��Ȳ
            if ((int)moveDir.y == -1 && (int)moveDir.x == 0 && isRotate == false)
            {
                AddToBoard(tetrominoNode);
                CheckBoardColumn();
                CreateTetromino();

                //��Ʈ�ι̳� ���� �߰� ���� �̵� ���� Ȯ��
                if (!CanMoveTo(tetrominoNode))
                {
                    gameoverPanel.SetActive(true);
                }
            }

            return false;
        }

        return true;
    }

    // ��Ʈ�ι̳븦 ���忡 �߰�
    void AddToBoard(Transform root)
    {
        while (root.childCount > 0)
        {
            var node = root.GetChild(0);

            //����Ƽ ��ǥ�迡�� ��Ʈ���� ��ǥ��� ��ȯ
            int x = Mathf.RoundToInt(node.transform.position.x + halfWidth);
            int y = Mathf.RoundToInt(node.transform.position.y + halfHeight - 1);

            //�θ��� : �� ���(y ��ġ), ������Ʈ �̸� : x ��ġ
            node.parent = boardNode.Find(y.ToString());
            node.name = x.ToString();
        }
    }

    // ���忡 �ϼ��� ���� ������ ����
    void CheckBoardColumn()
    {
        bool isCleared = false;
        //�ѹ��� ����� �� ���� Ȯ�ο�
        int linecount = 0;

        // �ϼ��� �� == ���� �ڽ� ������ ���� ũ��
        foreach (Transform column in boardNode)
        {
            if (column.childCount == boardWidth)
            {
                //���� ��� �ڽ��� ����
                foreach (Transform tile in column)
                {
                    Destroy(tile.gameObject);
                }
                // ���� ��� �ڽĵ���� ���� ����
                column.DetachChildren();
                isCleared = true;
                linecount++; // �ϼ��� �� �ϳ��� linecount ����
            }
        }

        // �ϼ��� ���� ������� ��������
        if (linecount != 0)
        {
            scoreVal += linecount * linecount * 100;
            score.text = "" + scoreVal;
        }

        // �ϼ��� ���� ������� �������� ����
        if (linecount != 0)
        {
            lineVal -= linecount;
            // ���������� �ʿ� ���� ���ް�� (�ִ� ���� 10���� ����)
            if (lineVal <= 0 && levelVal <= 10)
            {
                levelVal += 1;  // ��������
                lineVal = levelVal * 2 + lineVal;   // ���� ���� ����
                fallCycle = 0.1f * (10 - levelVal); // �ӵ� ����
            }
            level.text = "" + levelVal;
            line.text = "" + lineVal;
        }

        // ��� �ִ� ���� �����ϸ� �Ʒ��� ������
        if (isCleared)
        {
            //���� �ٴ� ���� ���� �ʿ䰡 �����Ƿ� index 1 ���� for�� ����
            for (int i = 1; i < boardNode.childCount; ++i)
            {
                var column = boardNode.Find(i.ToString());

                // �̹� ��� �ִ� ���� ����
                if (column.childCount == 0)
                    continue;

                // ���� �� �Ʒ��ʿ� �� ���� �����ϴ��� Ȯ��, �� �ุŭ emptyCol ����
                int emptyCol = 0;
                int j = i - 1;
                while (j >= 0)
                {
                    if (boardNode.Find(j.ToString()).childCount == 0)
                    {
                        emptyCol++;
                    }
                    j--;
                }

                // ���� �� �Ʒ��ʿ� �� �� ����� �Ʒ��� ����
                if (emptyCol > 0)
                {
                    var targetColumn = boardNode.Find((i - emptyCol).ToString());

                    while (column.childCount > 0)
                    {
                        Transform tile = column.GetChild(0);
                        tile.parent = targetColumn;
                        tile.transform.position += new Vector3(0, -emptyCol, 0);
                    }
                    column.DetachChildren();
                }
            }
        }
    }


    // �̵� �������� üũ �� True or False ��ȯ�ϴ� �޼���
    bool CanMoveTo(Transform root)  //tetrominoNode�� �Ű����� root�� ��������
    {
        // tetrominoNode�� �ڽ� Ÿ���� ��� �˻�
        for (int i = 0; i < root.childCount; ++i)
        {
            var node = root.GetChild(i);
            // ����Ƽ ��ǥ�迡�� ��Ʈ���� ��ǥ��� ��ȯ
            int x = Mathf.RoundToInt(node.transform.position.x + halfWidth);
            int y = Mathf.RoundToInt(node.transform.position.y + halfHeight - 1);
            // �̵� ������ ��ǥ���� Ȯ�� �� ��ȯ
            if (x < 0 || x > boardWidth - 1)
                return false;
            if (y < 0)
                return false;

            // �̹� �ٸ� Ÿ���� �ִ��� Ȯ��
            var column = boardNode.Find(y.ToString());

            if (column != null && column.Find(x.ToString()) != null)
                return false;
        }
        return true;
    }


    // Ÿ�� ����
    Tile CreateTile(Transform parent, Vector2 position, Color color, int order = 1)
    {
        var go = Instantiate(tilePrefab); // tilePrefab�� ������ ������Ʈ ����
        go.transform.parent = parent; // �θ� ����
        go.transform.localPosition = position; // ��ġ ����

        var tile = go.GetComponent<Tile>(); // tilePrefab�� Tile��ũ��Ʈ �ҷ�����
        tile.color = color; // ���� ����
        tile.sortingOrder = order; // ���� ����

        return tile;
    }

    // ��� Ÿ���� ����
    void CreateBackground()
    {
        Color color = Color.gray;   // ��� �� ����(���ϴ� ������ ���� ����)

        // Ÿ�� ����
        color.a = 0.5f; // �׵θ��� ���� ���� ���� �ٲٱ�
        for (int x = -halfWidth; x < halfWidth; ++x)
        {
            for (int y = halfHeight; y > -halfHeight; --y)
            {
                CreateTile(backgroundNode, new Vector2(x, y), color, 0);
            }
        }

        // �¿� �׵θ�
        color.a = 1.0f;
        for (int y = halfHeight; y > -halfHeight; --y)
        {
            CreateTile(backgroundNode, new Vector2(-halfWidth - 1, y), color, 0);
            CreateTile(backgroundNode, new Vector2(halfWidth, y), color, 0);
        }

        // �Ʒ� �׵θ�
        for (int x = -halfWidth - 1; x <= halfWidth; ++x)
        {
            CreateTile(backgroundNode, new Vector2(x, -halfHeight), color, 0);
        }
    }


    // ��Ʈ�ι̳� ����
    void CreateTetromino()
    {
        int index = Random.Range(0, 7); // �������� 0~6 ������ �� ����
        //���� �ڵ� ��� �Ʒ��� ���� �ڵ�� ���ϴ� Index��� Ȯ�� ����
        //int index = 1; 

        Color32 color = Color.white;

        // ȸ�� ��꿡 ����ϱ� ���� ���ʹϾ� Ŭ����
        tetrominoNode.rotation = Quaternion.identity;
        // ��Ʈ�ι̳� ���� ��ġ (�߾� ���)   
        tetrominoNode.position = new Vector2(offset_x, halfHeight + offset_y);

        switch (index)
        {
            // ������ ���� ��Ʈ���� ��翡 ����ϰ� ����� ǥ�� (I, J, L ,O, S, T, Z)

            case 0: // I
                color = new Color32(115, 251, 253, 255);    // �ϴû�
                CreateTile(tetrominoNode, new Vector2(-2f, 0.0f), color);
                CreateTile(tetrominoNode, new Vector2(-1f, 0.0f), color);
                CreateTile(tetrominoNode, new Vector2(0f, 0.0f), color);
                CreateTile(tetrominoNode, new Vector2(1f, 0.0f), color);
                break;

            case 1: // J
                color = new Color32(0, 33, 245, 255);    // �Ķ���
                CreateTile(tetrominoNode, new Vector2(-1f, 0.0f), color);
                CreateTile(tetrominoNode, new Vector2(0f, 0.0f), color);
                CreateTile(tetrominoNode, new Vector2(1f, 0.0f), color);
                CreateTile(tetrominoNode, new Vector2(-1f, 1.0f), color);
                break;

            case 2: // L
                color = new Color32(243, 168, 59, 255);    // ��Ȳ��
                CreateTile(tetrominoNode, new Vector2(-1f, 0.0f), color);
                CreateTile(tetrominoNode, new Vector2(0f, 0.0f), color);
                CreateTile(tetrominoNode, new Vector2(1f, 0.0f), color);
                CreateTile(tetrominoNode, new Vector2(1f, 1.0f), color);
                break;

            case 3: // O 
                color = new Color32(255, 253, 84, 255);    // �����
                CreateTile(tetrominoNode, new Vector2(0f, 0f), color);
                CreateTile(tetrominoNode, new Vector2(1f, 0f), color);
                CreateTile(tetrominoNode, new Vector2(0f, 1f), color);
                CreateTile(tetrominoNode, new Vector2(1f, 1f), color);
                break;

            case 4: //  S
                color = new Color32(117, 250, 76, 255);    // ���
                CreateTile(tetrominoNode, new Vector2(-1f, -1f), color);
                CreateTile(tetrominoNode, new Vector2(0f, -1f), color);
                CreateTile(tetrominoNode, new Vector2(0f, 0f), color);
                CreateTile(tetrominoNode, new Vector2(1f, 0f), color);
                break;

            case 5: //  T
                color = new Color32(155, 47, 246, 255);    // ���ֻ�
                CreateTile(tetrominoNode, new Vector2(-1f, 0f), color);
                CreateTile(tetrominoNode, new Vector2(0f, 0f), color);
                CreateTile(tetrominoNode, new Vector2(1f, 0f), color);
                CreateTile(tetrominoNode, new Vector2(0f, 1f), color);
                break;

            case 6: // Z
                color = new Color32(235, 51, 35, 255);    // ������
                CreateTile(tetrominoNode, new Vector2(-1f, 1f), color);
                CreateTile(tetrominoNode, new Vector2(0f, 1f), color);
                CreateTile(tetrominoNode, new Vector2(0f, 0f), color);
                CreateTile(tetrominoNode, new Vector2(1f, 0f), color);
                break;
        }
    }

}
