using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsAppKeep01
{
    public partial class Form1 : Form
    {
        private Stack<string> textHistory = new Stack<string>();
        private const int MaxHistoryCount = 10; // 最多紀錄10個紀錄
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)//開啟檔案
        {
            // 設置對話方塊標題
            openFileDialog1.Title = "選擇檔案";
            // 設置對話方塊篩選器，限制使用者只能選擇特定類型的檔案
            openFileDialog1.Filter = "文字檔案 (*.txt)|*.txt|所有檔案 (*.*)|*.*";
            // 如果希望預設開啟的檔案類型是文字檔案，可以這樣設置
            openFileDialog1.FilterIndex = 1;
            // 如果希望對話方塊在開啟時顯示的初始目錄，可以設置 InitialDirectory
            openFileDialog1.InitialDirectory = "C:\\";
            // 允許使用者選擇多個檔案
            openFileDialog1.Multiselect = true;

            // 顯示對話方塊，並等待使用者選擇檔案
            DialogResult result = openFileDialog1.ShowDialog();

            // 檢查使用者是否選擇了檔案
            if (result == DialogResult.OK)
            {
                try
                {
                    // 使用者在OpenFileDialog選擇的檔案
                    string selectedFileName = openFileDialog1.FileName;

                    // 使用 FileStream 打開檔案
                    // 建立一個檔案資料流，並且設定檔案名稱與檔案開啟模式為「開啟檔案」
                    FileStream fileStream = new FileStream(selectedFileName, FileMode.Open, FileAccess.Read);
                    // 讀取資料流
                    StreamReader streamReader = new StreamReader(fileStream);
                    // 將檔案內容顯示到 RichTextBox 中
                    richTextBox1.Text = streamReader.ReadToEnd();
                    // 關閉資料流與讀取資料流
                    fileStream.Close();
                    streamReader.Close();

                }
                catch (Exception ex)
                {
                    // 如果發生錯誤，用MessageBox顯示錯誤訊息
                    MessageBox.Show("讀取檔案時發生錯誤: " + ex.Message, "錯誤訊息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("使用者取消了選擇檔案操作。", "訊息");
            }

        }

        private void button2_Click(object sender, EventArgs e)//儲存
        {
            // 設置對話方塊標題
            saveFileDialog1.Title = "儲存檔案";
            // 設置對話方塊篩選器，限制使用者只能選擇特定類型的檔案
            saveFileDialog1.Filter = "文字檔案 (*.txt)|*.txt|所有檔案 (*.*)|*.*";
            // 如果希望預設儲存的檔案類型是文字檔案，可以這樣設置
            saveFileDialog1.FilterIndex = 1;
            // 如果希望對話方塊在開啟時顯示的初始目錄，可以設置 InitialDirectory
            saveFileDialog1.InitialDirectory = "C:\\";// 顯示對話方塊，並等待使用者指定儲存的檔案
            DialogResult result = saveFileDialog1.ShowDialog();
            FileStream fileStream = null;
            // 檢查使用者是否選擇了檔案
            if (result == DialogResult.OK)
            {
                try
                {
                    // 使用者指定的儲存檔案的路徑
                    string saveFileName = saveFileDialog1.FileName;

                    // 使用 FileStream 建立檔案，如果檔案已存在則覆寫
                    fileStream = new FileStream(saveFileName, FileMode.Create, FileAccess.Write);
                    // 將 RichTextBox 中的文字寫入檔案中
                    byte[] data = Encoding.UTF8.GetBytes(richTextBox1.Text);
                    fileStream.Write(data, 0, data.Length);

                    MessageBox.Show("檔案儲存成功。", "訊息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    // 如果發生錯誤，用 MessageBox 顯示錯誤訊息
                    MessageBox.Show("儲存檔案時發生錯誤: " + ex.Message, "錯誤訊息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // 關閉資源
                    fileStream.Close();
                }
            }
            else
            {
                MessageBox.Show("使用者取消了儲存檔案操作。", "訊息");
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            // 將當前的文本內容加入堆疊
            textHistory.Push(richTextBox1.Text);

            // 確保堆疊中只保留最多10個紀錄
            if (textHistory.Count > MaxHistoryCount)
            {
                // 用一個臨時堆疊，將除了最下面一筆的文字記錄之外，將文字紀錄堆疊由上而下，逐一移除再堆疊到臨時堆疊之中
                Stack<string> tempStack = new Stack<string>();
                for (int i = 0; i < MaxHistoryCount; i++)
                {
                    tempStack.Push(textHistory.Pop());
                }
                textHistory.Clear(); // 清空堆疊
                                     // 文字編輯堆疊紀錄清空之後，再將暫存堆疊（tempStack）中的資料，逐一放回到文字編輯堆疊紀錄
                foreach (string item in tempStack)
                {
                    textHistory.Push(item);
                }
            }
            UpdateListBox(); // 更新 ListBox          
        }

        // 更新 ListBox
        void UpdateListBox()
        {
            listBox1.Items.Clear(); // 清空 ListBox 中的元素

            // 將堆疊中的內容逐一添加到 ListBox 中
            foreach (string item in textHistory)
            {
                listBox1.Items.Add(item);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
