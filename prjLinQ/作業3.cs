using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prjLinQ
{
    public partial class 作業3 : Form
    {
        public 作業3()
        {
            InitializeComponent();
            this.Text = "LINQ";

            students_scores = new List<Student>()
                                         {
                                            new Student{ Name = "廖家毅", Class = "CS_101", Chi = rnd.Next(20,100), Eng = rnd.Next(20,100), Math = rnd.Next(20,100), Gender = "男" },
                                            new Student{ Name = "王婷薇", Class = "CS_102", Chi = rnd.Next(20,100), Eng = rnd.Next(20,100), Math = rnd.Next(20,100), Gender = "女" },
                                            new Student{ Name = "李沛軒", Class = "CS_101", Chi = rnd.Next(20,100), Eng = rnd.Next(20,100), Math = rnd.Next(20,100), Gender = "男" },
                                            new Student{ Name = "鄭　凱", Class = "CS_102", Chi = rnd.Next(20,100), Eng = rnd.Next(20,100), Math = rnd.Next(20,100), Gender = "男" },
                                            new Student{ Name = "洪暐婷", Class = "CS_101", Chi = rnd.Next(20,100), Eng = rnd.Next(20,100), Math = rnd.Next(20,100), Gender = "女" },
                                            new Student{ Name = "陳苡錚", Class = "CS_102", Chi = rnd.Next(20,100), Eng = rnd.Next(20,100), Math = rnd.Next(20,100), Gender = "女" },
                                            new Student{ Name = "游曉雯", Class = "CS_102", Chi = rnd.Next(20,100), Eng = rnd.Next(20,100), Math = rnd.Next(20,100), Gender = "女" }
                                          };
        }
        void AllClear()
        {
            this.treeView1.Nodes.Clear();
            this.dataGridView1.DataSource = null;
            this.chart1.DataSource = null;
        }
        Random rnd = new Random();

        List<Student> students_scores;


        public class Student
        {
            public string Name { get; set; }
            public string Class { get; set; }
            public int Chi { get; set; }
            public int Eng { get; internal set; }
            public int Math { get; set; }
            public string Gender { get; set; }

            public int sum
            {
                get
                {
                    return Chi + Eng + Math;
                }
            }
            public int avg
            {
                get
                {
                    return (Chi + Eng + Math)/3;
                }
            }
            public int Max
            {
                get
                {
                    int max = 0;
                    if (Chi > Eng && Chi > Math)
                        max = Chi;
                    if (Eng > Chi && Eng > Math)
                        max = Eng;
                    if (Math > Chi && Math > Eng)
                        max = Math;
                    return max;
                }
            }
            public int Min
            {
                get
                {
                    int min = 0;
                    if (Chi < Eng && Chi < Math)
                        min = Chi;
                    if (Eng < Chi && Eng < Math)
                        min = Eng;
                    if (Math < Chi && Math < Eng)
                        min = Math;
                    return min;
                }
            }
        }
        private void button36_Click(object sender, EventArgs e)
        {
            var q = from p in this.students_scores
                    select new[] { p.Chi, p.Eng, p.Math }.Sum();
            chart1.DataSource = q.ToList();
        }
        private string MyNums(int n)
        {
            if (n < 170)
                return "低標  170以下";
            else if (n <= 200)
                return "均標  170-200";
            else
                return "高標  200以上";
        }
        private void setGridStyle()
        {
            for (int i = 1; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].Width = 55;
            }
            dataGridView1.Columns[0].Width = 70;
        }
        private void button33_Click(object sender, EventArgs e)
        {
            AllClear();
            chart2.Visible = false;
            chart1.Visible = true;

            //var q = (from p in this.students_scores
            //        orderby p.Chi descending
            //        select p).Take(3);

            //var q = this.students_scores.Select(p => p.sum);
            var q = from p in this.students_scores.OrderByDescending(p=>p.sum)
                    group p by MyNums(p.sum) into g
                    //orderby g.Count()
                    select new { Sum=g.Key,Count=g.Count(),Mygroup=g};

            foreach (var group in q)
            {
                string s = $"{group.Sum} ({group.Count})";

                TreeNode x = treeView1.Nodes.Add(s);

                foreach (var item in group.Mygroup)
                {
                    x.Nodes.Add($"{item.Name}  國文:{item.Chi}  英文:{item.Eng}  數學:{item.Math}  平均:{item.avg}  總分:{item.sum}");
                } 
            }
            var q1 = from p in this.students_scores.OrderBy(p=>p.sum)
                     select new { 姓名 = p.Name, 國文 = p.Chi, 英文 = p.Eng, 數學 = p.Math, Sum = p.sum, Avg = p.avg, min = p.Min, Max = p.Max, Gender = p.Gender };
            
            this.chart1.DataSource = q1;
            this.chart1.Series[0].XValueMember = "姓名";
            this.chart1.Series[0].YValueMembers = "Sum";
            this.chart1.Series[0].Name = "三科總分";
            this.chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn;

            this.chart1.Series[2].YValueMembers = "Avg";
            this.chart1.Series[2].Name = "平均";
            this.chart1.Series[2].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;

            this.chart1.Series[3].YValueMembers = "Max";
            this.chart1.Series[3].Name = "最高分";
            this.chart1.Series[3].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;

            this.chart1.Series[1].YValueMembers = "min";
            this.chart1.Series[1].Name = "最低分";
            this.chart1.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;

            this.dataGridView1.DataSource= q1.ToList();
            setGridStyle();
        }

        private void cboName_MouseDown(object sender, MouseEventArgs e)
        {
            AllClear();
            var q = this.students_scores.Select(p => p.Name);
            cboName.DataSource = q.ToList();
        }

        private void cboName_SelectedValueChanged(object sender, EventArgs e)
        {
            AllClear();
            this.chart2.DataSource = null;
            chart1.Visible = false;
            chart2.Visible = true;


            var q = from p in this.students_scores
                    where p.Name == cboName.Text
                    select new { 姓名 = p.Name, 國文 = p.Chi, 英文 = p.Eng, 數學 = p.Math, Sum = p.sum, Avg = p.avg, min = p.Min, Max = p.Max, Gender = p.Gender };
            this.dataGridView1.DataSource = q.ToList();

            var q1 = from p in this.students_scores
                    select new { 姓名 = p.Name, 國文 = p.Chi, 英文 = p.Eng, 數學 = p.Math, Sum = p.sum, Avg = p.avg, min = p.Min, Max = p.Max, Gender = p.Gender };
            foreach (var group in q1)
            {
                string s = $"{group.姓名} ({group.Gender})";
                TreeNode x = treeView1.Nodes.Add(s);//(Mygrade(group.國文).ToString());
                x.Nodes.Add($"國文: {Mygrade(group.國文)}");
                x.Nodes.Add($"英文: {Mygrade(group.英文)}");
                x.Nodes.Add($"數學: {Mygrade(group.數學)}");
                x.Nodes.Add($"最低: {group.min}");
                x.Nodes.Add($"最高: {group.Max}");
                x.Nodes.Add($"平均: {group.Avg}");
                x.Nodes.Add($"總分: {group.Sum}  {MyNums(group.Sum)}");
            }


            this.chart2.DataSource = q;
            this.chart2.Series[0].XValueMember = "姓名";

            this.chart2.Series[0].YValueMembers = "國文";
            this.chart2.Series[0].Name = "國文";
            this.chart2.Series[0].Label = "國文" ;


            this.chart2.Series[1].YValueMembers = "英文";
            this.chart2.Series[1].Name = "英文";
            this.chart2.Series[1].Label = "英文";

            this.chart2.Series[2].YValueMembers = "數學";
            this.chart2.Series[2].Name = "數學";
            this.chart2.Series[2].Label = "數學";

            this.chart2.Series[0].LabelBorderColor = Color.Black;
            this.chart2.Series[1].LabelBorderColor = Color.Black;
            this.chart2.Series[2].LabelBorderColor = Color.Black;

            this.chart2.Series[0].LabelBackColor = Color.White;
            this.chart2.Series[1].LabelBackColor = Color.White;
            this.chart2.Series[2].LabelBackColor = Color.White;

        }
        private string Mygrade(int n)
        {
            if (n < 60)
                return n +"   不及格";
            else if (n < 90)
                return n + "   佳作";
            else
                return n + "   優良";
        }
        private void button37_Click(object sender, EventArgs e)
        {

        }
    }
}
