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
    public partial class 作業4 : Form
    {
        public 作業4()
        {
            InitializeComponent();
           

        }
        NorthwindEntities dbContext = new NorthwindEntities();
        void AllClear()
        {
            this.treeView1.Nodes.Clear();
            this.dataGridView1.DataSource = null;
            this.dataGridView2.DataSource = null;
        }
        private void button38_Click(object sender, EventArgs e)
        {
            AllClear();
            //this.laAll.Text = "FileInfo[].Log檔";
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");

            System.IO.FileInfo[] files = dir.GetFiles();

            this.dataGridView1.DataSource = files;

            //var q = from p in files.OrderBy(p=>p.Length)
            //        group p by p.Length < 102400 into g
            //        select new { g.Key,Mygroup = g };

            var q = files.OrderByDescending(p => p.Length).GroupBy(p => p.Length > 102400  ? "大檔" : "小檔").Select(g=>new { Name=g.Key, Count=g.Count(),Mygroup = g });

            foreach (var group in q)
            {
                string s = $"{group.Name} ({group.Count})";
                TreeNode x = treeView1.Nodes.Add(s);//(group.Name.ToString());
                foreach (var item in group.Mygroup)
                {
                    x.Nodes.Add($"{item.Name} : {item.Length}");
                }
            }

            this.dataGridView2.DataSource = q.ToList();
                    
        }

        private void button6_Click(object sender, EventArgs e)
        {
            AllClear();
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");

            System.IO.FileInfo[] files = dir.GetFiles();

            this.dataGridView1.DataSource = files;

            //var q = from p in files
            //        group p by MyDatatime(p.CreationTime) into g
            //        select new { g.Key, MyGroup = g };
            var q = files.OrderByDescending(p => p.CreationTime).GroupBy(p => MyDatatime(p.CreationTime)).Select(g => new { g.Key,Count=g.Count(), MyGroup = g });

            this.dataGridView2.DataSource = q.ToList();

            foreach (var group in q)
            {
                string s = $"{group.Key} {group.Count}個 )";
                TreeNode x = treeView1.Nodes.Add(s);//(group.Key);
                foreach (var item in group.MyGroup)
                {
                    x.Nodes.Add($"{item.Name.PadRight(20)}  ( {item.Length} ) - {item.CreationTime}");

                }   
            }
        }
        private string MyDatatime(DateTime n)
        {
            DateTime data1 = new DateTime (2021,1,1, 0, 0, 0);
            //int result = DateTime.Compare(data1, n);
            if (n < data1)   //<2021
                return "早期檔案 (2021年前 ";
            else if(n.Year == data1.Year) //==2021
                return "近期檔案 (2021年間 ";
            else //
                return "當期檔案 (";
        }
                   
        private void button8_Click(object sender, EventArgs e)
        {
            AllClear();
            //NW Products 低中高 價產品 
            //var q = from p in dbContext.Products.OrderByDescending(p => p.UnitPrice)
            //group p by MyPrice(p.UnitPrice) into g
            //select new { Name = g.Key, Count = g.Count(), Mygroup = g };
            var q1 = this.dbContext.Products.OrderBy(p => p.UnitPrice).Select(p=>p);

            var q = this.dbContext.Products.AsEnumerable().OrderBy(p => p.UnitPrice).GroupBy(p => MyPrice(p.UnitPrice)).Select(g => new {Name=g.Key,Count=g.Count(), Mygroup = g }) ;

            this.dataGridView2.DataSource=q.ToList();
            this.dataGridView2.Columns[2].Visible = false;
            this.dataGridView1.DataSource = q1.ToList();


            foreach (var group in q)
            {
                string s = $"{group.Name} ({group.Count})";
                TreeNode x = treeView1.Nodes.Add(s);// (group.Name);
                foreach (var item in group.Mygroup)
                {
                    x.Nodes.Add($"{item.ProductName,-40} {item.UnitPrice,5:C2}");
                }

            }
        }

        private string MyPrice(decimal? unitPrice)
        {
            if (unitPrice == null)
                return "停產";
            if (unitPrice < 35)
                return "低單價產品";
            else if (unitPrice < 79)
                return "中高價產品";
            else
                return "高價產品";
        }

        private void button15_Click(object sender, EventArgs e)
        {
            AllClear();
            // Orders -  Group by 年
            var q = this.dbContext.Orders.OrderBy(p => p.OrderDate).GroupBy(p => p.OrderDate.Value.Year).Select(g => new { Year = g.Key,Count = g.Count() ,Mygroup=g});
            var q1 = this.dbContext.Orders.OrderBy(p => p.OrderDate);

            foreach (var group in q)
            {
                string s = $"{group.Year} ({group.Count})";
                TreeNode x = this.treeView1.Nodes.Add(s); //(group.Mygroup.Key.ToString());
                foreach (var item in group.Mygroup)
                {
                    x.Nodes.Add($"{item.CustomerID}    {item.OrderDate.Value.ToString("yyyy-MM-dd")}");
                }

            }
            this.dataGridView1.DataSource = q1.ToList();
            this.dataGridView2.DataSource= q.ToList();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            AllClear();
            var q1 = this.dbContext.Orders.OrderBy(p => p.OrderDate);

            var q = from p in dbContext.Orders
                    group p by p.OrderDate.Value.Year into g
                    select new
                    {
                        g.Key,
                        groupYear = g.GroupBy(m => m.OrderDate.Value.Month).Select(m =>new { m.Key, groupMonth=m })
                    };
            


            foreach (var group in q)
            {
                //string s = $"{group.Year} ({group.Count})";
                string s = $"{group.Key}年 ({group.groupYear.Count()})";
                TreeNode x = this.treeView1.Nodes.Add(s);//(group.Key.ToString()); //(group.Mygroup.Key.ToString());
                foreach (var Month in group.groupYear)
                {
                    TreeNode y = x.Nodes.Add(Month.groupMonth.Key.ToString());
                    foreach (var item in Month.groupMonth)
                    {
                        y.Nodes.Add(item.CustomerID);
                    }
                }

            }

            this.dataGridView1.DataSource = q1.ToList();
            this.dataGridView2.DataSource = q.ToList();
        }
        class MyTotal
        {
            public string Name { get; set; }
            public decimal? Total { get; set; }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //總銷售金額
            AllClear();
            var q1 = this.dbContext.Products.OrderByDescending(p => p.UnitPrice);

            var q2 = this.dbContext.Products.GroupBy(p=>p.UnitPrice).Select(g=>new {Sum=g.Sum(p=>p.UnitsInStock*p.UnitPrice) });
            var q3 = from p in this.dbContext.Order_Details.AsEnumerable()
                     group p by p.OrderID into g
                     select new { Total=g.Key,Sum=g.Sum(x=>x.UnitPrice*x.Quantity*(decimal)(1-x.Discount)),Mygroup=g }; 

            this.dataGridView1.DataSource = q3.ToList();
            decimal? Sum=0;
            foreach (var item in q3)
            {
                Sum += item.Sum;
            }

            List<MyTotal> list = new List<MyTotal>();
            string totalSell = "總銷售";
            MyTotal myTotal = new MyTotal();
            myTotal.Total = Math.Round((decimal)Sum,2);
            myTotal.Name = totalSell;
            list.Add(myTotal);

            this.dataGridView2.DataSource = list.ToArray();      

            foreach (var group in q3)
            {
                TreeNode x = treeView1.Nodes.Add(group.Total.ToString());
                foreach (var item in group.Mygroup)
                {
                    x.Nodes.Add(item.UnitPrice.ToString());
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //top5 
            AllClear();

            var q1 = (from p in this.dbContext.Order_Details.AsEnumerable()
                     group p by p.Order.Employee.FirstName + " " + p.Order.Employee.LastName into g
                     select new { Name=g.Key,訂單數=g.Count() ,總銷售額 = g.Sum(x => Math.Round( x.Quantity * x.UnitPrice * (decimal)(1 - x.Discount))) }).OrderByDescending(g=>g.總銷售額).Take(5);

            this.dataGridView1.DataSource = q1.ToList();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //     NW 產品最高單價前 5 筆 (包括類別名稱)
            AllClear();
            var q = (from p in this.dbContext.Products.OrderByDescending(p => p.UnitPrice)
                    select new { CategoryName=p.Category.CategoryName,p.UnitPrice }).Take(5);
            this.dataGridView1.DataSource= q.ToList();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //     NW 產品有任何一筆單價大於300 ?
            AllClear();
            
            var q = (from p in this.dbContext.Products.OrderByDescending(p => p.UnitPrice)
                     where p.UnitPrice > 300
                     select p).Any();
            if(q)
            {
                MessageBox.Show("NW 產品 單價大於300的產品為" + q);
            }
            else
            {
                MessageBox.Show("NW 產品沒有有任何一筆單價大於300");
            }

            
                    
        }

        private void button34_Click(object sender, EventArgs e)
        {
            //每年 銷售分析 &&　plot
            AllClear();
            //var q = from p in this.dbContext.Order_Details
            //        select p.p.UnitPrice*p.Quantity

            var q = from p in this.dbContext.Order_Details.AsEnumerable()
                    group p by p.Order.OrderDate.Value.Year into g

                    select new { Year=g.Key,TotalSell= g.Sum(x => Math.Round((x.Quantity * x.UnitPrice*(decimal)(1-x.Discount)),2))};
            this.dataGridView1.DataSource = q.ToList();

            this.chart1.DataSource = q;
            this.chart1.Series[0].XValueMember = "Year";
            this.chart1.Series[0].YValueMembers = "TotalSell";
            //this.chart1.Series[0].Name = "平均";
            this.chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            //this.dataGridView1.DataSource = q1.ToList();
        }





        Random rnd = new Random();
        List<int> Mix = new List<int>();
        List<int> Small = new List<int>();
        List<int> Mid = new List<int>();
        List<int> Large = new List<int>();

        TreeNode treeSmall = new TreeNode("不及格");
        TreeNode treeMid = new TreeNode("佳作");//作為根節點。
        TreeNode treeLarge = new TreeNode("優秀");//作為根節點。
        string[] firstname = { "趙", "錢", "孫", "李", "周", "吳", "鄭", "王", "馮", "陳", "褚", "衛", "蔣", "沈", "韓", "楊" };
        string[] lastname = { "家豪", "志明", "俊傑", "建宏", "俊宏", "志豪", "志偉", "文雄", "金龍", "志強", "淑芬", "淑惠", "美玲", "雅婷", "美惠", "麗華", "淑娟", "淑貞", "怡君", "淑華" };
        private void button4_Click(object sender, EventArgs e)
        {
            //int[]  分三群 - No LINQ
            AllClear();
            treeView1.Nodes.Add(treeSmall);

            treeView1.Nodes.Add(treeMid);
            
            treeView1.Nodes.Add(treeLarge);

            for (int i = 0; i < 50; i++)
            {
                int X = rnd.Next(30, 100);
                string name = firstname[rnd.Next(0, firstname.Length)] + lastname[rnd.Next(0, firstname.Length)];
                Myint(X,name);
            }


            #region
            //TreeNode small = new TreeNode();
            //small.Text = "不及格";
            //treeView1.Nodes.Add(small);

            //TreeNode node1 = new TreeNode();
            //node1.Text = "hopeone";
            //TreeNode node11 = new TreeNode();
            //node11.Text = "hopeoneone";
            //TreeNode node2 = new TreeNode();
            //node2.Text = "hopetwo";

            //node1.Nodes.Add(node11);//在node1下面在新增一個結點。

            //small.Nodes.Add(node1);//node下的兩個子節點。
            //small.Nodes.Add(node2);

            //TreeNode Mid = new TreeNode("佳作");//作為根節點。
            //treeView1.Nodes.Add(Mid);
            //TreeNode t1 = new TreeNode("basilone");
            //Mid.Nodes.Add(t1);
            //TreeNode t2 = new TreeNode("basiltwo");
            //Mid.Nodes.Add(t2);

            //TreeNode Large = new TreeNode("優秀");//作為根節點。
            //treeView1.Nodes.Add(Large);
            #endregion
        }
        int a,b,c = 0;

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            //年營收成長率=（當年營收– 去年營收）÷ 去年營收x 100%
            var q = from p in this.dbContext.Order_Details.AsEnumerable()
                    group p by p.Order.OrderDate.Value.Year into g
                    select new { Year=g.Key, TotalSell = g.Sum(x => x.Quantity * x.UnitPrice * (decimal)(1 - x.Discount)) };
            var q1 = from p in this.dbContext.Order_Details.AsEnumerable()
                     group p by p.Order.OrderDate.Value.Year into g
                    select new { Year = g.Key + 1, TotalSell = g.Sum(x => x.Quantity * x.UnitPrice * (decimal)(1 - x.Discount)) };

            var q2 = from p in q
                     join o in q1 on p.Year equals o.Year
                     select new
                     {

                         年營收成長率 = (p.TotalSell - o.TotalSell) / o.TotalSell,當年 = p.Year

                     };

            this.chart1.DataSource = q2.ToList();
            this.chart1.Series[0].XValueMember = "當年";
            this.chart1.Series[0].YValueMembers = "年營收成長率";
            this.chart1.Series[0].Name = "年營收成長率";
            this.chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            this.dataGridView1.DataSource=q2.ToList();
        }

        void Myint(int n,string s)
        {
            if (n < 60)
            {
                a++;
                treeSmall.Nodes.Add($"{s}: {n} ");
                treeSmall.Text = $"不及格  ({a})";
                
                Small.Add(n);
                Small.Sort();
            }

            else if (n < 80)
            {
                b++;
                treeMid.Nodes.Add($"{s}: {n} ");
                treeMid.Text = $"佳作  ({b})";

                Mid.Add(n);
                Mid.Sort();
            }
            else
            {
                c++;
                treeLarge.Nodes.Add($"{s}: {n} ");
                treeLarge.Text = $"優秀  ({c})";

                Large.Add(n);
                Large.Sort();
            }
                
        }
    }
}
