using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mino : MonoBehaviour
{
    public float previousTime;
    // minoが落ちるタイム
    public float fallTime = 1f;
    // ステージの大きさ
    private static int width = 10;
    private static int height = 20;
    // mino回転
    public Vector3 rotationPoint;

    private static Transform[,] grid = new Transform[width, height];
    // Update is called once per frame
    void Update()
    {
        MinoMovement();
    }

    private void MinoMovement()
    {
        // Aキーで左に動く
        if(Input.GetKeyDown(KeyCode.A))
        {
            transform.position += new Vector3(-1, 0, 0);
            if(!ValidMovement())
            {
                transform.position -= new Vector3(-1, 0, 0);
            }
        }
        // Dキーで右に動く
        else if (Input.GetKeyDown(KeyCode.D))
        {
            transform.position += new Vector3(1, 0, 0);
            if (!ValidMovement())
            {
                transform.position -= new Vector3(1, 0, 0);
            }
        }
        // 自動で下に移動させつつ、Sキーでも移動する
        else if (Input.GetKey(KeyCode.S) || Time.time - previousTime >= fallTime) 
        {
            transform.position += new Vector3(0, -1, 0);
            if (!ValidMovement())
            {
                transform.position -= new Vector3(0, -1, 0);
                AddToGrid();
                CheckLines();
                this.enabled = false;
                FindObjectOfType<SpawnMino>().NewMino();
            }
            previousTime = Time.time;
        }
        else if(Input.GetKeyDown(KeyCode.Space))
        {
            // ブロックの回転
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
        }
    }

    // ラインがあるか確認
    public void CheckLines()
    {
        for(int i = height - 1; i >= 0; i--)
        {
            if(HasLine(i))
            {
                DeleteLine(i);
                RowDown(i);
            }
        }
    }
    // 列がそろっているか確認
    bool HasLine(int i)
    {
        for(int j = 0; j < width;j++)
        {
            if (grid[j, i] == null)
                return false;
        }
        FindObjectOfType<GameManagement>().AddScore();
        return true;
    }
    // ラインを消す
    void DeleteLine(int i)
    {
        for(int j = 0;j < width;j++)
        {
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
        }
    }
    // 列を下げる
    public void RowDown(int i)
    {
        for(int y = i;y < height;y++)
        {
            for(int j = 0; j < width;j++)
            {
                if(grid[j,y] != null)
                {
                    grid[j, y - 1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }

    void AddToGrid()
    {
        foreach(Transform children in transform)
        {
            int roundX = Mathf.RoundToInt(children.transform.position.x);
            int roundY = Mathf.RoundToInt(children.transform.position.y);
            grid[roundX, roundY] = children;

            // height-1 = 19のところまでブロックがきたらGameOver
            if(roundX >= height -1)
            {
                // GameOverメソッドを呼び出す
                FindObjectOfType<GameManagement>().GameOver();
            }
        }
    }

    // minoの移動範囲の制御
    bool ValidMovement()
    {
        foreach(Transform children in transform)
        {
            int roundX = Mathf.RoundToInt(children.transform.position.x);
            int roundY = Mathf.RoundToInt(children.transform.position.y);
            // minoがステージよりはみ出さないように制御
            if(roundX < 0 || roundX >= width || roundY < 0 || roundY >= height)
            {
                return false;
            }
            if(grid[roundX,roundY]!= null)
            {
                return false;
            }
        }
        return true;
    }
}
