using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// Title : ACK 과제
/// DBMS : MariaDB 10.5
/// IDE : VisualStudio2019 (Winforms)
/// Author : 신은철
/// </summary>
namespace ackTestApp
{
    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dbSelect();
        }


        /// <summary>
        /// DataTable GetChanges를 이용한 데이터 insert   -   DataRowState.Added 추가로 인한 변경점
        /// https://docs.microsoft.com/ko-kr/dotnet/api/system.data.datatable.getchanges?view=net-5.0
        /// 마지막으로 로드되거나 DataTable가 호출된 후에 변경되어 AcceptChanges()를 기준으로 필터링된 내용이 모두 들어 있는 DataRowState의 복사본을 가져옵니다.
        /// using System.Data;
        /// </summary>
        public void dbInsert()
        {
            try
            {
                string connstr = "Server = 127.0.0.1;Port = 3306;Database=db_ack;Uid=root;pwd=1101";
                MySqlConnection conn = new MySqlConnection(connstr);

                DataTable dtChanges;
                DataTable dtProcessFlag = (DataTable)dataGridView1.DataSource;

                dtChanges = dtProcessFlag.GetChanges(DataRowState.Added);
                conn.Open();

                String insertSql = string.Empty;
                if(dtChanges != null)
                {
                    for(int i = 0; i < dtChanges.Rows.Count; i++)
                    {
                        insertSql = @"insert into student values(" +
                        string.Format("{0},{1},{2},'{3}','{4}')",
                        dtChanges.Rows[i]["no"].ToString(),
                        dtChanges.Rows[i]["grade"].ToString(),
                        dtChanges.Rows[i]["cclass"].ToString(),
                        dtChanges.Rows[i]["name"].ToString(),
                        dtChanges.Rows[i]["score"].ToString());

                    }
                }

                MySqlCommand cmd = new MySqlCommand(insertSql, conn);
                cmd.ExecuteNonQuery();
                conn.Close();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        /// <summary>
        ///데이터 집합을 채우고 MySQL 데이터베이스를 업데이트하는 데 사용되는 데이터 명령 집합및 데이터베이스 연결을 나타냅니다.
        ///https://dev.mysql.com/doc/dev/connector-net/6.10/html/T_MySql_Data_MySqlClient_MySqlDataAdapter.htm
        ///using MySql.Data.MySqlClient;
        /// </summary>
        public void dbSelect()
        {
            string connstr = "Server=127.0.0.1;port=3306;Database=db_ack;Uid=root;pwd=1101";
            MySqlConnection conn = new MySqlConnection(connstr);

            string sql = "select * from student";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            
            conn.Open();

            MySqlDataAdapter adpt = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adpt.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();
        }


        /// <summary>
        /// DataTable GetChanges를 이용한 데이터 insert - DataRowState.Modified 수정으로 인한 변경점
        /// </summary>
        public void dbUpdate()
        {
            try
            {
                string connstr = "Server = 127.0.0.1;Port = 3306;Database=db_ack;Uid=root;pwd=1101";
                MySqlConnection conn = new MySqlConnection(connstr);
                conn.Open();

                DataTable dtChanges;
                DataTable dtProcessFlag = (DataTable)dataGridView1.DataSource;

                dtChanges = dtProcessFlag.GetChanges(DataRowState.Modified);
                

                String updateSql = string.Empty;
                if (dtChanges != null)
                {
                    for (int i = 0; i < dtChanges.Rows.Count; i++)
                    {
                        updateSql = @"update student set no = '#no' where name = '#name'";
                        updateSql = updateSql.Replace("#no", dtChanges.Rows[i]["no"].ToString());
                        updateSql = updateSql.Replace("#name", dtChanges.Rows[i]["name"].ToString());
                    }
                }

                MySqlCommand cmd = new MySqlCommand(updateSql, conn);
                cmd.ExecuteNonQuery();
                conn.Close();

            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        ///삭제하고 싶은 행을 선택하여 버튼을 누릅니다. 
        ///
        /// </summary>
        public void dbDelete()
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                return;
            }
            string connstr = "Server = 127.0.0.1;Port = 3306;Database=db_ack;Uid=root;pwd=1101";
            MySqlConnection conn = new MySqlConnection(connstr);
            conn.Open();

            String selectDeleteNo = dataGridView1.CurrentRow.Cells["no"].Value.ToString();
            String deleteSql = @"Delete from student where no = '#no'";
            deleteSql = deleteSql.Replace("#no", selectDeleteNo);

            MySqlCommand cmd = new MySqlCommand(deleteSql, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        private void read_Click(object sender, EventArgs e)
        {
            dbSelect();
        }

        private void create_Click(object sender, EventArgs e)
        {
            dbInsert();
            dbSelect();
        }

        private void update_Click(object sender, EventArgs e)
        {
            dbUpdate();
            dbSelect();
        }

        private void delete_Click(object sender, EventArgs e)
        {
            dbDelete();
            dbSelect();
        }
        
    }
    
}

