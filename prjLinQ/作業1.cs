using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace prjLinQ
{
    public partial class 作業1 : Form
    {
        public 作業1()
        {
            InitializeComponent();
            this.ordersTableAdapter1.Fill(this.nwDataSet1.Orders);
            this.productsTableAdapter1.Fill(this.nwDataSet1.Products);
            this.order_DetailsTableAdapter1.Fill(this.nwDataSet1.Order_Details);
            this.Text = "LINQ";

            students_scores = new List<Student>()
                                         {
                                            new Student{ Name = "aaa", Class = "CS_101", Chi = 80, Eng = 80, Math = 50, Gender = "Male" },
                                            new Student{ Name = "bbb", Class = "CS_102", Chi = 80, Eng = 80, Math = 100, Gender = "Male" },
                                            new Student{ Name = "ccc", Class = "CS_101", Chi = 60, Eng = 50, Math = 75, Gender = "Female" },
                                            new Student{ Name = "ddd", Class = "CS_102", Chi = 80, Eng = 70, Math = 85, Gender = "Female" },
                                            new Student{ Name = "eee", Class = "CS_101", Chi = 80, Eng = 80, Math = 50, Gender = "Female" },
                                            new Student{ Name = "fff", Class = "CS_102", Chi = 80, Eng = 80, Math = 80, Gender = "Female" },
                                          };
        }
        int count;
        List<Student> students_scores;

        public class Student
        {
            public string Name { get; set; }
            public string Class { get; set; }
            public int Chi { get; set; }
            public int Eng { get; internal set; }
            public int Math { get; set; }
            public string Gender { get; set; }
        }
        private void button14_Click(object sender, EventArgs e)
        {

            this.laAll.Text = "FileInfo[].Log檔";
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");

            System.IO.FileInfo[] files = dir.GetFiles();

            this.dataGridView3.DataSource = files;
        }

        private void button2_Click(object sender, EventArgs e)
        {

            this.laAll.Text = "FileInfo[]   -2017";
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");


            System.IO.FileInfo[] files = dir.GetFiles();

            var q = from x in dir.GetFiles()
                    where x.CreationTime.Year == 2019
                    orderby x.CreationTime.Minute ascending
                    select x;
            this.dataGridView3.DataSource = q.ToList();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.laAll.Text = "FileInfo[]   -大檔案";
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");


            System.IO.FileInfo[] files = dir.GetFiles();
            var q = from x in dir.GetFiles()
                    where x.Length > 102400
                    orderby x.Length descending
                    select x;

            this.dataGridView3.DataSource = q.ToList();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            laAll.Text = "Northwind - Orders 所有訂單";
            count = 0;
            var p = from q in nwDataSet1.Orders
                    select new
                    {
                        q.CustomerID,
                        q.EmployeeID,
                        q.OrderDate,
                        q.RequiredDate,
                        //q.ShippedDate,
                        q.ShipVia,
                        q.Freight,
                        q.ShipName,
                        q.ShipAddress,
                        q.ShipCity,
                        //q.ShipRegion,
                        //q.ShipPostalCode,
                        q.ShipCountry
                    };
            dataGridView3.DataSource = p.ToList();
            var r = from s in nwDataSet1.Orders
                    group s by s.OrderDate.Year into Y
                    select Y.Key;
            //comboBox1.Items.Add(p.Distinct());
            comboBox1.DataSource = r.ToList();
            //comboBox1.Items.Add(p.Distinct());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            count = 0;
            if (comboBox1.SelectedItem == null)
                return;
            var q1 = from p1 in nwDataSet1.Orders
                     join r1 in nwDataSet1.Order_Details
                     on p1.OrderID equals r1.OrderID
                     where p1.OrderDate.Year == (int)comboBox1.SelectedItem
                     select new
                     {
                         p1.CustomerID,
                         p1.EmployeeID,
                         p1.OrderDate,
                         p1.RequiredDate,
                         //p1.ShippedDate,
                         p1.ShipVia,
                         p1.Freight,
                         p1.ShipName,
                         p1.ShipAddress,
                         p1.ShipCity,
                         //p1.ShipRegion,
                         //p1.ShipPostalCode,
                         p1.ShipCountry
                     };
            var q2 = from p2 in nwDataSet1.Orders
                     where p2.OrderDate.Year == (int)comboBox1.SelectedItem
                     select new
                     {
                         p2.CustomerID,
                         p2.EmployeeID,
                         p2.OrderDate,
                         p2.RequiredDate,
                         //p2.ShippedDate,
                         p2.ShipVia,
                         p2.Freight,
                         p2.ShipName,
                         p2.ShipAddress,
                         p2.ShipCity,
                         //p2.ShipRegion,
                         //p2.ShipPostalCode,
                         p2.ShipCountry
                     };


            dataGridView2.DataSource = q1.ToList();
            dataGridView1.DataSource = q2.ToList();

            //dataGridView2.
        }

        private void button12_Click(object sender, EventArgs e)
        {
            //this.nwDataSet1.Products.Take(10);//Top 10 Skip(10)

            if (count - 1 <= 0)
                return;
            int num = int.Parse(textBox1.Text);
            count--;
            var q = from p in nwDataSet1.Products.Take(count * num).Skip((count - 1) * num)
                    orderby p.ProductID
                    select p;
            this.dataGridView3.DataSource = q.ToList();
            laAll.Text = $"瀏覽範圍為{count * num}";
        }

        private void button13_Click(object sender, EventArgs e)
        {

            count++;
            int num = int.Parse(textBox1.Text);

            var q = from p in nwDataSet1.Products.Take(count * num).Skip((count - 1) * num)
                    orderby p.ProductID
                    select p;
            //MessageBox.Show(nwDataSet1.Products.Count + "");
            if (nwDataSet1.Products.Count < (count - 1) * num)
            { count--; return; }

            this.dataGridView3.DataSource = q.ToList();
            laAll.Text = $"瀏覽範圍為{count * num}";

            //this.nwDataSet1.Products.Take(10);//Top 10 Skip(10)

            //Distinct()
        }

        private void button36_Click(object sender, EventArgs e)
        {
            laAll.Text = $"班級學生各科平均分數";

            this.dataGridView3.DataSource = students_scores;
            var q1 = this.students_scores.Select(p =>
                    new
                    {
                        p.Name,
                        p.Chi,
                        p.Eng,
                        p.Math,
                        SUM = p.Chi + p.Eng + p.Math,
                        min = new[] { p.Chi, p.Eng, p.Math }.Min(),
                        max = new[] { p.Chi, p.Eng, p.Math }.Max(),
                        avg = new[] { p.Chi, p.Eng, p.Math }.Average().ToString("0.0")
                    });
            this.dataGridView3.DataSource = q1.ToList();

        }

        private void button37_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString()== comboBox1.Text)
            {
                lblMaster.Text = comboBox1.Text+"的訂單資料";
                labQuestion.Text = comboBox1.Text + "的訂單明細";
            }
            var q1 = from p1 in nwDataSet1.Orders
                     join r1 in nwDataSet1.Order_Details
                     on p1.OrderID equals r1.OrderID
                     where p1.OrderDate.Year == (int)comboBox1.SelectedItem
                     select new
                     {
                         p1.CustomerID,
                         p1.EmployeeID,
                         p1.OrderDate,
                         p1.RequiredDate,
                         //p1.ShippedDate,
                         p1.ShipVia,
                         p1.Freight,
                         p1.ShipName,
                         p1.ShipAddress,
                         p1.ShipCity,
                         //p1.ShipRegion,
                         //p1.ShipPostalCode,
                         p1.ShipCountry
                     };
            var q2 = from p2 in nwDataSet1.Orders
                     where p2.OrderDate.Year == (int)comboBox1.SelectedItem
                     select new
                     {
                         p2.CustomerID,
                         p2.EmployeeID,
                         p2.OrderDate,
                         p2.RequiredDate,
                         //p2.ShippedDate,
                         p2.ShipVia,
                         p2.Freight,
                         p2.ShipName,
                         p2.ShipAddress,
                         p2.ShipCity,
                         //p2.ShipRegion,
                         //p2.ShipPostalCode,
                         p2.ShipCountry
                     };
            dataGridView2.DataSource = q1.ToList();
            dataGridView1.DataSource = q2.ToList();
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            laAll.Text = $"班級學生各科平均分數";

            this.dataGridView3.DataSource = students_scores;
            var q1 = this.students_scores.Select(p =>
                    new
                    {
                        p.Name,
                        p.Chi,
                        p.Eng,
                        p.Math,
                        SUM = p.Chi + p.Eng + p.Math,
                        min = new[] { p.Chi, p.Eng, p.Math }.Min(),
                        max = new[] { p.Chi, p.Eng, p.Math }.Max(),
                        avg = new[] { p.Chi, p.Eng, p.Math }.Average().ToString("0.0")
                    });
            this.dataGridView3.DataSource = q1.ToList();
            lblMaster.Text = "所有成績";
            this.dataGridView1.DataSource = students_scores;
            if (comboBox2.SelectedItem.ToString()== "共幾個學員成績")
            {
                labQuestion.Text = "共幾個學員成績";

                var q = from p in students_scores
                        select p;

                dataGridView2.DataSource = q.ToList();
            }

            if (comboBox2.SelectedItem.ToString() == "前面三個學員所有科目成績")
            {
                labQuestion.Text = "前面三個學員所有科目成績";

                var q = from p in students_scores
                    select p;
                dataGridView2.DataSource = q.Take(3).ToList();
            }

            if (comboBox2.SelectedItem.ToString() == "後面兩個學員所有科目成績")
            {
                labQuestion.Text = "後面兩個學員所有科目成績";

                var q = from p in students_scores
                        orderby p.Name descending
                        select p;

                dataGridView2.DataSource = q.Take(2).ToList();
            }

            if (comboBox2.SelectedItem.ToString() == "'aaa','bbb','ccc'的學成績")
            {
                labQuestion.Text = "'aaa','bbb','ccc'的學成績";

                var q = this.students_scores.Where(p => p.Name == "aaa" || p.Name == "bbb" || p.Name == "ccc");
                //var q = from p in students_scores
                //        where p.Name == "aaa" || p.Name == "bbb" || p.Name == "ccc"
                //        select p;

                dataGridView2.DataSource = q.ToList();
            }

            if (comboBox2.SelectedItem.ToString() == "找出學員'bbb'的成績")
            {
                labQuestion.Text = "找出學員'bbb'的成績";

                var q = this.students_scores.Where(p => p.Name == "bbb");
                //var q = from p in students_scores
                //        where p.Name == "bbb"
                //        select p;
                dataGridView2.DataSource = q.ToList();
            }

            if (comboBox2.SelectedItem.ToString() == "除了'bbb'學員的所有成績")
            {
                labQuestion.Text = "除了'bbb'學員的所有成績";

                var q = students_scores.Where(p => p.Name != "bbb");
                //var q = from p in students_scores
                //        where p.Name != "bbb"
                //        select p;
                dataGridView2.DataSource = q.ToList();
            }

            if (comboBox2.SelectedItem.ToString() == "數學不及格...是誰")
            {
                labQuestion.Text = "數學不及格...是誰";

                //var q = this.students_scores.Select(p => p.Math < 60);
                var q = from p in students_scores
                        where p.Math < 60
                        select p;

                dataGridView2.DataSource = q.ToList();
            }

            if (comboBox2.SelectedItem.ToString() == "'aaa','bbb''ccc'學員國文數學兩科科目成績")
            {
                labQuestion.Text = "'aaa','bbb''ccc'學員國文數學兩科科目成績";

                var q = this.students_scores.Where(p => p.Name == "aaa" || p.Name == "bbb" || p.Name == "ccc").Select(p => new { p.Name, p.Chi, p.Math });
                this.dataGridView2.DataSource = q.ToList();
            }

        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
