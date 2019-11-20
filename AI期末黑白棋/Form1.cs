using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AI期末黑白棋
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int[,] board = new int[8, 8];
        int whitescore = 0;
        int blackscore = 0;
        int humanside = 0;
        int computerside = 0;
        int canstepnum = 0;
        int AIx = 0;
        int AIy = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            //下子
            int y = Convert.ToInt32(char.Parse(textBox2.Text)) - 97;
            int x = int.Parse(textBox3.Text) - 1;
            flip(x, y, humanside,board);
            AI();
            countScore(board);
            print();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //初始化棋盤
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    board[i, j] = 0;
                }
            }
            board[3, 3] = 2;
            board[3, 4] = 1;
            board[4, 3] = 1;
            board[4, 4] = 2;
            countScore(board);
            print();
        }
        private void AI()
        {
            int tmp = 0;
            AIx = 0;
            AIy = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board[i,j] == 0 && checkcanstep(i,j,computerside,board) == true)
                    {
                        flip(i, j, computerside, board);
                        AIy = j;
                        AIx = i;
                        label26.Text = ((char)(AIy + 97)).ToString();
                        label28.Text = (AIx + 1).ToString();
                        tmp++;
                        return;
                    }
                }
            }
            if (tmp == 0)
            {
                MessageBox.Show("電腦方已無位置可下");
                label28.Text = "NO";
                label26.Text = "NO";
            }
            /*minimax(board, 0, 0, int.Parse(textBox4.Text), -999, 999, true);
            flip(AIx, AIy, computerside, board);*/
            label26.Text = ((char)(AIy + 97)).ToString();
            label28.Text = (AIx + 1).ToString();
        }
        public int minimax(int[,] newboard ,int x,int y, int dep, int alpha, int beta, bool ismax)
        {
            int[,] copy = new int[8,8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8 ; j++)
                {
                    copy[i,j] = newboard[i,j];
                }
            }
            if (dep == 0)
            {
                int result = 0;
                countScore(copy);
                if (computerside == 1)
                {
                    result = blackscore - whitescore;
                }
                else
                {
                    result = whitescore - blackscore;
                }
                return result;
            }
            if (ismax)
            {
                int maxvalue = -999;
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (copy[i,j] == 0 && checkcanstep(i, j, computerside,copy))
                        {
                            flip(i,j,computerside,copy);
                            int value = minimax(copy,i, j, dep - 1, alpha, beta, false);
                            if (value > maxvalue)
                            {
                                maxvalue = value;
                                if (dep == int.Parse(textBox4.Text))
                                {
                                    AIx = i;
                                    AIy = j;
                                }
                            }
                            alpha = Math.Max(alpha, beta);
                            copy = (int[,])newboard.Clone();
                            if (beta <= alpha)
                            {
                                break;
                            }
                        }
                    }
                }
                return maxvalue;
            }
            else
            {
                int minvalue = 999;
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (copy[i, j] == 0 && checkcanstep(i, j, humanside, copy))
                        {
                            flip(i, j, humanside, copy);
                            int value = minimax(copy,i,j,dep-1,alpha,beta,true);
                            minvalue = Math.Min(minvalue, value);
                            beta = Math.Min(beta, value);
                            copy = (int[,])newboard.Clone();
                            if (beta <= alpha)
                            {
                                break;
                            }
                        }
                    }
                }
                return minvalue;
            }
        }
        private void print() //印出目前棋盤
        {
            int tmp = 0;
            textBox1.Text = "";
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board[i, j] == 0 && checkcanstep(i,j,humanside,board) == false)
                    {
                        textBox1.AppendText("◯ ");
                    }
                    else if (board[i, j] == 1)
                    {
                        textBox1.AppendText("♜ ");
                    }
                    else if (board[i,j] == 2)
                    {
                        textBox1.AppendText("♖ ");
                    }
                    else
                    {
                        textBox1.AppendText("● ");
                        tmp++;
                    }
                }
                if (i != 7)
                {
                    textBox1.AppendText("\r\n");
                }
                Application.DoEvents();
            }
            label22.Text = blackscore.ToString();
            label23.Text = whitescore.ToString();
            label24.Text = tmp.ToString();
            canstepnum = tmp;
        }
        private void countScore(int[,] count) //計算目前分數
        {
            blackscore = 0;
            whitescore = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (count[i,j] == 1)
                    {
                        blackscore++;
                    }
                    else if (count[i, j] == 2)
                    {
                        whitescore++;
                    }
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //開始
            if (comboBox1.Text == "黑子"){
                humanside = 1;
                computerside = 2;
            }
            else{
                humanside = 2;
                computerside = 1;
                AI();
            }
            countScore(board);
            print();
        }
        bool checkcanstep(int x, int y, int chess,int[,] newboard) //檢查此座標是否能下
        {
            for (int i = x + 1; i < 8; i++) //往右
            {
                if (newboard[i, y] == 0 || newboard[x+1,y] == chess)
                {
                    break;
                }
                if (newboard[i, y] == chess && newboard[i - 1, y] != chess && newboard[i - 1, y] != 0)
                {
                    return true;
                }
            }
            for (int i = x - 1; i >= 0; i--) //往左
            {
                if (newboard[i, y] == 0 || newboard[x-1,y] == chess)
                {
                    break;
                }
                if (newboard[i, y] == chess && newboard[i + 1, y] != chess && newboard[i + 1, y] != 0)
                {
                    return true;
                }
            }
            for (int j = y + 1; j < 8; j++) //往下
            {
                if (newboard[x, j] == 0 || newboard[x,y+1] == chess)
                {
                    break;
                }
                if (newboard[x, j] == chess && newboard[x, j - 1] != chess && newboard[x, j - 1] != 0)
                {
                    return true;
                }
            }
            for (int j = y - 1; j >= 0; j--) // 往上
            {
                if (newboard[x, j] == 0 || newboard[x,y-1] == chess)
                {
                    break;
                }
                if (newboard[x, j] == chess && newboard[x, j + 1] != chess && newboard[x, j + 1] != 0)
                {
                    return true;
                }
            }
            for (int i = x + 1, j = y + 1; i < 8 && j < 8; i++, j++) //往右下
            {
                if (newboard[i, j] == 0 || newboard[x+1,y+1] == chess)
                {
                    break;
                }
                if (newboard[i, j] == chess && newboard[i - 1, j - 1] != chess && newboard[i - 1, j - 1] != 0)
                {
                    return true;
                }
            }
            for (int i = x + 1, j = y - 1; i < 8 && j >= 0; i++, j--) //往右上
            {
                if (newboard[i, j] == 0 || newboard[x+1,y-1] == chess)
                {
                    break;
                }
                if (newboard[i, j] == chess && newboard[i - 1, j + 1] != chess && newboard[i - 1, j + 1] != 0)
                {
                    int a = x;
                    int b = y;
                    return true;
                }

            }
            for (int i = x - 1, j = y - 1; i >= 0 && j >= 0; i--, j--)  //左下
            {
                if (newboard[i, j] == 0 || newboard[x-1,y-1] == chess)
                {
                    break;
                }
                if (newboard[i, j] == chess && newboard[i + 1, j + 1] != chess && newboard[i + 1, j + 1] != 0)
                {
                    return true;
                }
            }
            for (int i = x - 1, j = y + 1; i >= 0 && j < 8; i--, j++)  //左上
            {
                if (newboard[i, j] == 0 || newboard[x-1,y+1] == chess)
                {
                    break;
                }
                if (newboard[i, j] == chess && newboard[i + 1, j - 1] != chess && newboard[i + 1, j - 1] != 0)
                {
                    return true;
                }
            }
                return false;
        }
        void flip(int x, int y, int chess,int[,] newboard) //翻轉棋子
        {
            for (int i = x + 1; i < 8; i++) //往右
            {
                if (newboard[i, y] == 0 || newboard[x + 1, y] == chess)
                {
                    break;
                }
                if (newboard[i, y] == chess && newboard[i - 1, y] != chess && newboard[i - 1, y] != 0)
                {
                    newboard[x, y] = chess;
                    for (int k = x + 1; k < i; k++)
                    {
                        newboard[k, y] = chess;
                    }
                }
            }
            for (int i = x - 1; i >= 0; i--) //往左
            {
                if (newboard[i, y] == 0 || newboard[x - 1, y] == chess)
                {
                    break;
                }
                if (newboard[i, y] == chess && newboard[i + 1, y] != chess && newboard[i + 1, y] != 0)
                {
                    newboard[x, y] = chess;
                    for (int k = x - 1; k > i; k--)
                    {
                        newboard[k, y] = chess;
                    }
                }
            }
            for (int j = y + 1; j < 8; j++) //往下
            {
                if (newboard[x, j] == 0 || newboard[x, y + 1] == chess)
                {
                    break;
                }
                if (newboard[x, j] == chess && newboard[x, j - 1] != chess && newboard[x, j - 1] != 0)
                {
                    newboard[x, y] = chess;
                    for (int k = y + 1; k < j; k++)
                    {
                        newboard[x, k] = chess;
                    }
                }
            }
            for (int j = y - 1; j >= 0; j--) // 往上
            {
                if (newboard[x, j] == 0 || newboard[x, y - 1] == chess)
                {
                    break;
                }
                if (newboard[x, j] == chess && newboard[x, j + 1] != chess && newboard[x, j + 1] != 0)
                {
                    newboard[x, y] = chess;
                    for (int k = y - 1; k > j; k--)
                    {
                        newboard[x, k] = chess;
                    }
                }
            }
            for (int i = x + 1, j = y + 1; i < 8 && j < 8; i++, j++) //往右下
            {
                if (newboard[i, j] == 0 || newboard[x + 1, y + 1] == chess)
                {
                    break;
                }
                if (newboard[i, j] == chess && newboard[i - 1, j - 1] != chess && newboard[i - 1, j - 1] != 0)
                {
                    newboard[x, y] = chess;
                    for (int k = x + 1, m = y + 1; k < i && m < j; k++, m++)
                    {
                        newboard[k, m] = chess;
                    }
                }
            }
            for (int i = x + 1, j = y - 1; i < 8 && j >= 0; i++, j--) //往右上
            {
                if (newboard[i, j] == 0 || newboard[x + 1, y - 1] == chess)
                {
                    break;
                }
                if (newboard[i, j] == chess && i != x + 1 && newboard[i - 1, j + 1] != chess && newboard[i - 1, j + 1] != 0)
                {
                    newboard[x, y] = chess;
                    for (int k = x + 1, m = y - 1; k < i && m > j; k++, m--)
                    {
                        newboard[k, m] = chess;
                    }
         
                }

            }
            for (int i = x - 1, j = y - 1; i >= 0 && j >= 0; i--, j--)  //左下
            {
                if (newboard[i, j] == 0 || newboard[x - 1, y - 1] == chess)
                {
                    break;
                }
                if (newboard[i, j] == chess && i != x - 1 && newboard[i + 1, j + 1] != chess && newboard[i + 1, j + 1] != 0)
                {
                    newboard[x, y] = chess;
                    for (int k = x - 1, m = y - 1; k > i && m > j; k--, m--)
                    {
                        newboard[k, m] = chess;
                    }
                }
            }
            for (int i = x - 1, j = y + 1; i >= 0 && j < 8; i--, j++)  //左上
            {
                if (newboard[i, j] == 0 || newboard[x - 1, y + 1] == chess)
                {
                    break;
                }
                if (newboard[i, j] == chess && i != x - 1 && newboard[i + 1, j - 1] != chess && newboard[i + 1, j - 1] != 0)
                {
                    newboard[x, y] = chess;
                    for (int k = x - 1, m = y + 1; k > i && m < j; k--, m++)
                    {
                        newboard[k, m] = chess;
                    }
                }
            }
            return;
        }
    }
}
