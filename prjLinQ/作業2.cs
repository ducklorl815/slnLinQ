using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prjLinQ
{
    public partial class 作業2 : Form
    {
        public 作業2()
        {
            InitializeComponent();
            this.Text = "LINQ";
            this.productPhotoTableAdapter1.Fill(this.awDataSet1.ProductPhoto);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            lblMaster.Text = "所有資料";
            var q = from p in this.awDataSet1.ProductPhoto
                    select new
                    {
                        p.ProductPhotoID,
                        p.ThumbNailPhoto,
                        p.ThumbnailPhotoFileName,
                        p.LargePhotoFileName,
                        p.ModifiedDate,
                        p.LargePhoto
                    };

            var y = from p in this.awDataSet1.ProductPhoto
                    orderby p.ModifiedDate
                    select p.ModifiedDate.Year;
            comboBox3.DataSource = y.Distinct().ToList();

            this.dataGridView1.DataSource = q.ToList();

            setGridStyle();
            //DataView dv = dataGridView1.DataSource as DataView;
            //DataRow row = dv.Table.Rows[_postion];
            //var q2 = from p in q
            //         where 

            //var q1 = from p in q
            //         where  dataGridView1.SelectedRows == _postion
            //         select new
            //         {
            //             p.LargePhoto
            //         };

            //this.pictureBox1.Image = q1.LargePhoto;
        }
        private void setGridStyle()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Height = 30;
            }
               
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem == null)
                return;
            if (comboBox2.Text == "第一季")
            {
                var q = from p in this.awDataSet1.ProductPhoto
                        where p.ModifiedDate.Month < 4
                        select p;
                this.dataGridView1.DataSource = q.ToList();
            }
            if (comboBox2.Text == "第二季")
            {
                var q = from p in this.awDataSet1.ProductPhoto
                        where 3 < p.ModifiedDate.Month && p.ModifiedDate.Month < 7
                        select p;
                this.dataGridView1.DataSource = q.ToList();
            }
            if (comboBox2.Text == "第三季")
            {
                var q = from p in this.awDataSet1.ProductPhoto
                        where 6 < p.ModifiedDate.Month && p.ModifiedDate.Month < 10
                        select p;
                this.dataGridView1.DataSource = q.ToList();
            }

            if (comboBox2.Text == "第四季")
            {
                var q = from p in this.awDataSet1.ProductPhoto
                        where 9 < p.ModifiedDate.Month && p.ModifiedDate.Month < 12
                        select p;
                this.dataGridView1.DataSource = q.ToList();
            }

            setGridStyle();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            
            if (comboBox3.SelectedItem == null)
                return;
            var q = from p in this.awDataSet1.ProductPhoto
                    where p.ModifiedDate.Year == int.Parse(comboBox3.Text)
                    select new
                    {
                        p.ProductPhotoID,
                        p.ThumbNailPhoto,
                        p.ThumbnailPhotoFileName,
                        p.LargePhotoFileName,
                        p.ModifiedDate,
                        p.LargePhoto
                    };
            this.dataGridView1.DataSource = q.ToList();
            setGridStyle();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            this.lblMaster.Text = $"{dateTimePicker1.Value.ToString("yyyy-MM-dd")} 到 {dateTimePicker2.Value.ToString("yyyy-MM-dd")} 的區間資料";
            var q = from p in this.awDataSet1.ProductPhoto
                    where p.ModifiedDate > dateTimePicker1.Value && p.ModifiedDate < dateTimePicker2.Value
                    select new
                    {
                        p.ProductPhotoID,
                        p.ThumbNailPhoto,
                        p.ThumbnailPhotoFileName,
                        p.LargePhotoFileName,
                        p.ModifiedDate,
                        p.LargePhoto
                    };
            this.dataGridView1.DataSource = q.ToList();
            setGridStyle();
        }

        private void dataGridView1_MouseUp(object sender, MouseEventArgs e)
        {
            lblDetails.Text = dataGridView1.CurrentRow.Cells["ThumbnailPhotoFileName"].Value.ToString();
            lblMaster.Text = dataGridView1.CurrentRow.Cells["LargePhotoFileName"].Value.ToString();
            byte[] ByteData = (byte[])dataGridView1.CurrentRow.Cells["LargePhoto"].Value;
            MemoryStream memoryStream = new MemoryStream(ByteData);
            pictureBox1.Image = Image.FromStream(memoryStream);
            memoryStream.Close();
        }

        private void comboBox3_SelectedValueChanged(object sender, EventArgs e)
        {
            this.lblMaster.Text = $"{comboBox3.Text}";
            var q = from p in this.awDataSet1.ProductPhoto
                    where p.ModifiedDate.Year == int.Parse(comboBox3.Text)
                    select new
                    {
                        p.ProductPhotoID,
                        p.ThumbNailPhoto,
                        p.ThumbnailPhotoFileName,
                        p.LargePhotoFileName,
                        p.ModifiedDate,
                        p.LargePhoto
                    };
            this.dataGridView1.DataSource = q.ToList();
            setGridStyle();
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            this.lblMaster.Text=comboBox2.Text;
            if (comboBox2.Text == "第一季")
            {
                var q = from p in this.awDataSet1.ProductPhoto
                        where p.ModifiedDate.Month < 4
                        select p;
                this.dataGridView1.DataSource = q.ToList();
            }
            if (comboBox2.Text == "第二季")
            {
                var q = from p in this.awDataSet1.ProductPhoto
                        where 3 < p.ModifiedDate.Month && p.ModifiedDate.Month < 7
                        select p;
                this.dataGridView1.DataSource = q.ToList();
            }
            if (comboBox2.Text == "第三季")
            {
                var q = from p in this.awDataSet1.ProductPhoto
                        where 6 < p.ModifiedDate.Month && p.ModifiedDate.Month < 10
                        select p;
                this.dataGridView1.DataSource = q.ToList();
            }

            if (comboBox2.Text == "第四季")
            {
                var q = from p in this.awDataSet1.ProductPhoto
                        where 9 < p.ModifiedDate.Month && p.ModifiedDate.Month < 12
                        select p;
                this.dataGridView1.DataSource = q.ToList();
            }

            setGridStyle();
        }
    }
}
