using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;

namespace FillWords
{
    public partial class Form1 : Form
    {
        string path = "ce.txt";
        Hashtable hashtable;
        string filePath = "files";
        public Form1()
        {
            InitializeComponent();
            hashtable = this.readFile(path);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String text = this.textBox1.Text;
            if (text.Trim().Length == 0)
            {
                MessageBox.Show("请输入一句话！", "提示框", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.textBox1.Focus();
            }
            else if (text.IndexOf(" ") == -1)
            {
                MessageBox.Show("请输入空格代表要填的词！", "提示框", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.textBox1.Focus();
            }
            else
            {
                handle(text);
            }
        }

        private void handle(string text)
        {
            if (text.LastIndexOf(' ') == text.Length - 1)
            {
                text = text + "。";
            }
            if (text.IndexOf(' ') == 0)
            {
                text = '。' + text;
            }
            string[] SplitTests = text.Split(' ');//以空格切分句子
            List<ArrayList> spaceWords = new List<ArrayList>();
            List<string[]> spaceForeAfter = new List<string[]>();
            for (int ii = 0; ii < SplitTests.Length; ii++)
            {
                if (SplitTests[ii].Trim().Length == 0)
                    continue;
                if (SplitTests[ii] == "。")
                {
                    continue;
                }
                string t_text = SplitTests[ii];
                ArrayList list = new ArrayList();
                getWords(t_text, list);//分词
                spaceWords.Add(list);
            }

            for (int i = 0; i < spaceWords.Count; i++)
            {
                ArrayList temp = spaceWords[i];
                string tempString = null;
                if (SplitTests[0] != "。" && i == 0)
                {
                    continue;
                }
                if (i > 0)
                    tempString = spaceWords[i - 1][spaceWords[i - 1].Count - 1].ToString();
                string[] strs = new string[2];
                strs[0] = tempString;
                strs[1] = temp[0].ToString();
                spaceForeAfter.Add(strs);
            }
            if (SplitTests[SplitTests.Length - 1] == "。")
            {
                string[] strs = new string[2];
                ArrayList temp = spaceWords[spaceWords.Count - 1];
                strs[0] = temp[temp.Count - 1].ToString();
                strs[1] = null;
                spaceForeAfter.Add(strs);
            }
            //this.printString(spaceForeAfter);
            DirectoryInfo dir = new DirectoryInfo(filePath);
            FileInfo[] infos = dir.GetFiles();
            List<string> fillWords = new List<string>();
            for (int i = 0; i < spaceForeAfter.Count; i++)
            {
                List<string> countForeWords = new List<string>();
                List<string> countAfterWords = new List<string>();
                string[] hintWords = spaceForeAfter[i];
                for (int j = 0; j < infos.Length; j++)
                {
                    string[] allLines = File.ReadAllLines(infos[j].FullName, Encoding.Default);//获取文件的所有行
                    string current = "";
                    for (int k = 0; k < allLines.Length; k++)
                    {
                        current = allLines[k];
                        for (int s = 0; s < hintWords.Length; s++)
                        {
                            if (hintWords[s] == null)
                            {
                                continue;
                            }
                            if (s == 0) //对于空格前面的词
                            {
                                while (current.IndexOf(hintWords[s]) != -1)
                                {
                                    if (current.IndexOf(hintWords[s]) == current.Length - hintWords[s].Length)
                                    {
                                        ArrayList temp = new ArrayList();
                                        string t_temp = allLines[k + 1];
                                        if (t_temp.Length > 6)
                                        {
                                            t_temp = t_temp.Substring(0, 6);
                                        }
                                        getWords(t_temp, temp);
                                        if (temp.Count > 0)
                                            countForeWords.Add(temp[0].ToString());
                                    }
                                    else
                                    {
                                        int index = current.IndexOf(hintWords[s]);
                                        int length = current.Substring(index + hintWords[s].Length).Length;
                                        string t_current = "";
                                        if (length > 6)
                                        {
                                            t_current = current.Substring(index + hintWords[s].Length, 6);
                                        }
                                        else
                                        {
                                            t_current = current.Substring(index + hintWords[s].Length);
                                        }
                                        ArrayList list = new ArrayList();
                                        getWords(t_current, list);
                                        if (list.Count > 0)
                                            countForeWords.Add(list[0].ToString());
                                    }
                                    current = current.Substring(current.IndexOf(hintWords[s]) + hintWords[s].Length);
                                }

                            }
                            else
                            {
                                while (current.IndexOf(hintWords[s]) != -1)
                                {
                                    if (current.IndexOf(hintWords[s]) == 0 && k != 0)
                                    {
                                        ArrayList temp = new ArrayList();
                                        string t_str = allLines[k - 1];
                                        if (t_str.Length > 6)
                                        {
                                            t_str = t_str.Substring(t_str.Length - 6);
                                        }
                                        getWords(t_str, temp);
                                        if (temp.Count > 0)
                                            countAfterWords.Add(temp[temp.Count - 1].ToString());
                                    }
                                    else if (current.IndexOf(hintWords[s]) != 0)
                                    {
                                        int index = current.IndexOf(hintWords[s]);
                                        int length = current.Substring(0, index).Length;
                                        string t_current = "";
                                        if (length > 6)
                                        {
                                            t_current = current.Substring(index - 6, 6);
                                        }
                                        else
                                        {
                                            t_current = current.Substring(0, index);
                                        }

                                        ArrayList list = new ArrayList();
                                        getWords(t_current, list);
                                        if (list.Count > 0)
                                            countAfterWords.Add(list[list.Count - 1].ToString());
                                    }
                                    current = current.Substring(current.IndexOf(hintWords[s]) + hintWords[s].Length);
                                }
                            }
                        }
                    }
                }
                List<string> unique = new List<string>();
                List<string> same = new List<string>();
                List<int> counts = new List<int>();
                for (int j = 0; j < countForeWords.Count; j++)
                {
                    string compare = countForeWords[j];
                    if (!unique.Contains(compare))
                    {
                        unique.Add(compare);
                    }
                    else
                    {
                        continue;
                    }
                    if (countAfterWords.Contains(compare))
                    {
                        same.Add(compare);
                        int count = 0;
                        for (int k = 0; k < countAfterWords.Count; k++)
                        {
                            if (countAfterWords[k] == compare)
                                count++;
                        }
                        counts.Add(count);
                    }
                }
                StreamWriter sw = new StreamWriter(new FileStream("probability.txt", FileMode.Append, FileAccess.Write), Encoding.Default);
                int total = 0;
                for (int j = 0; j < counts.Count; j++)
                {
                    total += counts[j];
                }
                for (int j = 0; j < counts.Count; j++)
                {
                    string str = same[j] + "\t" + (double)counts[j] / total;
                    sw.WriteLine(str);
                }
                sw.Close();
                if (same.Count > 0)
                {
                    
                    int maxIndex = 0;
                    int maxCount = counts[0];
                    for (int j = 1; j < counts.Count; j++)
                    {
                        if (maxCount < counts[j])
                        {
                            maxCount = counts[j];
                            maxIndex = j;
                        }
                    }
                    fillWords.Add(same[maxIndex]);
                }
                else
                {
                    //分别求空格前后最大的概率
                    string[] fore = null;
                    string[] after = null;
                    double t_fore = 0;
                    double t_after = 0;
                    if (countForeWords.Count != 0)
                    {
                        fore = this.getMaxCount(countForeWords);
                        double temp = int.Parse(fore[1]);
                        t_fore = temp / countForeWords.Count;
                    }
                    if (countAfterWords.Count != 0)
                    {
                        after = this.getMaxCount(countAfterWords);
                        double temp = int.Parse(after[1]);
                        t_after = temp / countAfterWords.Count;
                    }
                    if (t_fore > t_after)
                    {
                        fillWords.Add(fore[0]);
                    }
                    else if (t_after > t_fore)
                    {
                        fillWords.Add(after[0]);
                    }
                }
            }
            string finalText = this.textBox1.Text;
            string[] split = finalText.Split(' ');
            string final = "";
            int cnt = 0;
            for (int i = 0; i < split.Length; i++)
            {
                final += split[i];
                Console.WriteLine(cnt);
                if (i != split.Length - 1 && cnt < fillWords.Count)
                    final += fillWords[cnt++];
            }
            this.textBox2.Text = final;
        }

        private string[] getMaxCount(List<string> list)
        {
            List<string> unique = new List<string>();
            List<int> counts = new List<int>();
            for (int i = 0; i < list.Count;i++ )
            {
                if (!unique.Contains(list[i]))
                {
                    unique.Add(list[i]);
                }
            }
            int maxCount = 0;
            int maxIndex = 0;
            for (int i = 0; i < unique.Count;i++ )
            {
                int count = 0;
                for (int j = 0; j < list.Count; j++)
                {
                    if (list[j] == unique[i])
                    {
                        count++;
                    }
                }
                if (maxCount < count)
                {
                    maxCount = count;
                    maxIndex = i;
                }
                counts.Add(count);
            }
            int total = 0;
            for(int i=0;i<counts.Count;i++){
                total += counts[i];
            }
            return new string[2] { unique[maxIndex], maxCount.ToString() };
        }

        private void getWords(string t_text,ArrayList list) 
        {
            while(t_text != "")
                {//用最长匹配
                    char startText = t_text[0];
                    if (startText >= 19968 && startText <= 40869)
                    {
                        ArrayList keys = new ArrayList();
                        ArrayList values = new ArrayList();
                        foreach (DictionaryEntry entry in hashtable)
                        {
                            string key = entry.Key.ToString();
                            if (key.IndexOf(startText) == 0)
                            {
                                keys.Add(key);
                                values.Add(entry.Value.ToString());
                            }
                        }
                        if (keys.Count == 0)
                        {
                            //Console.WriteLine(startText+"，不识别！！！");
                            t_text = t_text.Substring(startText.ToString().Length);
                            continue;
                        }
                        //排序，长度最长的在前面
                        for (int i = 0; i < keys.Count; i++)
                        {
                            for (int j = 0; j < keys.Count - i - 1; j++)
                            {
                                if (keys[j].ToString().Length < keys[j + 1].ToString().Length)
                                {
                                    string temp = keys[j].ToString();
                                    keys[j] = keys[j + 1];
                                    keys[j + 1] = temp;
                                    temp = values[j].ToString();
                                    values[j] = values[j + 1];
                                    values[j + 1] = temp;
                                }
                            }
                        }
                        for (int i = 0; i < keys.Count; i++)
                        {
                            string t_key = keys[i].ToString();
                            if (t_text.IndexOf(t_key) == 0)
                            {
                                //Console.WriteLine(t_key+"  "+values[i].ToString());
                                list.Add(t_key);
                                t_text = t_text.Substring(t_key.Length);
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (startText.ToString().Trim().Length == 0)
                        {
                            string print = "空格";
                            //Console.WriteLine(print + ":不是汉字！");
                        }
                        else
                        {
                            //Console.WriteLine(startText + ":不是汉字！");
                        }
                        t_text = t_text.Substring(startText.ToString().Length);
                    }
                }
        }

        private Hashtable readFile(string path)
        {
            Hashtable hash = new Hashtable();
            StreamReader sr = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read), Encoding.Default);
            string strLine;
            while ((strLine = sr.ReadLine()) != null)
            {
                string key = strLine.Substring(0, strLine.IndexOf(','));
                int index = strLine.IndexOf(',');
                string value = strLine.Substring(index + 1);
                if (!hash.ContainsKey(key))
                {
                    hash.Add(key, value);
                }
            }
            sr.Close();
            return hash;
        }
    }
}
