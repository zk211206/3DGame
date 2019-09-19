using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TicTacToe : MonoBehaviour
{
    public int turn;
    public int count;
    private int[,] cells = new int[3, 3];

    void Start()
    {
        restart();
    }

    void restart()   //重新开始
    {
        turn = 1;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                cells[i, j] = 0;
            }
        }
        count = 0;
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(20, 260, 100, 50), "重新开始"))
        {
            restart();
        }

        int result = getResult();

        GUIStyle temp = new GUIStyle
        {
            fontSize = 20
        };
        temp.normal.textColor = Color.black;
        temp.fontStyle = FontStyle.Bold;

        if (result == 1)
        {
            GUI.Label(new Rect(500, 100, 100, 50), "O 赢", style: temp);
        }
        else if (result == 2)
        {
            GUI.Label(new Rect(500, 100, 100, 50), "X 赢", style: temp);
        }
        else if (result == 3)
        {
            GUI.Label(new Rect(500, 100, 100, 50), "平局", style: temp);
        }

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (cells[i, j] == 1)
                {
                    GUI.Button(new Rect(i * 50, j * 50, 50, 50), "0");
                }
                if (cells[i, j] == 2)
                {
                    GUI.Button(new Rect(i * 50, j * 50, 50, 50), "X");
                }
                if (GUI.Button(new Rect(i * 50, j * 50, 50, 50), ""))
                {
                    if (result == 0)
                    {
                        if (turn == 1) cells[i, j] = 1;
                        else cells[i, j] = 2;
                        count++;
                        turn = (turn == 1) ? 0 : 1;
                    }
                }
            }
        }
    }

    private int getResult()
    {
        for (int i = 0; i < 3; i++)    //连成一横
        {
            if (cells[i, 0] != 0 && cells[i, 0] == cells[i, 1] && cells[i, 1] == cells[i, 2])
            {
                return cells[i, 0];
            }
        }
        for (int i = 0; i < 3; i++)    //连成一竖
        {
            if (cells[0, i] != 0 && cells[0, i] == cells[1, i] && cells[1, i] == cells[2, i])
            {
                return cells[0, i];
            }
        }
        if (cells[1, 1] != 0 && ((    //对角线
            cells[0, 0] == cells[1, 1] &&
            cells[1, 1] == cells[2, 2]) ||
            (cells[0, 2] == cells[1, 1] &&
            cells[1, 1] == cells[2, 0]))
            )
        {
            return cells[1, 1];
        }
        if (count == 9) return 3;     //平局
        return 0;
    }

    void Update()
    {

    }
}