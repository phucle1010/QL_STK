using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Thong_Tin_Khach_hang;
using System.Drawing.Printing;
using System.Globalization;
using System.Xml.Serialization;

namespace WindowsFormsApp1
{
    public partial class frmThongtinso : Form
    {
        DataTable dt = new DataTable();
        bool loaded = false;
        DataTable dt1 = new DataTable();
        DataTable dt2 = new DataTable();
        editPerson editPerson = new editPerson();
        DateTime date = DateTime.Now;
        ulong lai = 0;
        ulong lai1 = 0;
        ulong goclai = 0;
        int soKH = 0;
        ulong goc = 0;
        public frmThongtinso()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            MainFormManager.Instance.openChildForm(new frmthongTinKhachHang());
        }
        void LoadChiNhanh()
        {
            string st = "SELECT MaCN,TenCN FROM CHINHANH";
            cbCN.DisplayMember = "TenCN";
            cbCN.ValueMember = "MaCN";
            dt1 = dataProvider.Instance.ExecuteQuery(st);
            cbCN.DataSource = dt1;
            cbCN.Text = "";
        }
        void LoadThoiHan()
        {
            string st = "SELECT MaLoaiTK,ThoiHan FROM LOAITIETKIEM";
            cbTH.DisplayMember = "ThoiHan";
            cbTH.ValueMember = "MaLoaiTK";
            dt2 = dataProvider.Instance.ExecuteQuery(st);
            DataView dv = dt2.DefaultView;
            dv.Sort = "ThoiHan asc";
            DataTable sortedtable1 = dv.ToTable();
            cbTH.DataSource = sortedtable1;
            cbTH.Text = "";
        }
        private void frmThongtinso_Load(object sender, EventArgs e)
        {
            tabControl1.TabPages.Remove(tabPage3);
            LoadChiNhanh();
            LoadThoiHan();
            LoadData();
            loaded = true;
        }
        private string maCN(string text)
        {
            string query = "SELECT MaCN from CHINHANH where TenCN=N'" + text + "'";
            dt = dataProvider.Instance.ExecuteQuery(query);
            string macn = dt.Rows[0]["MaCN"].ToString();
            return macn;
        }
        private string maNV(string text)
        {
            string query = "SELECT MaNV from NHANVIEN where TenNV=N'" + text + "'";
            dt = dataProvider.Instance.ExecuteQuery(query);
            string maNV = dt.Rows[0]["MaNV"].ToString();
            return maNV;
        }
     
        void LoadData()
        {
            
            string st = "SELECT MaSoTK,stk.MaKH,TenKH,TenNV,TenCN,NgayMoSo,stk.SoVon,HinhThucTraLai,ltk.ThoiHan,ltk.LaiXuat FROM SOTIETKIEM stk,KHACHHANG kh, NHANVIEN nv,CHINHANH cn, LOAITIETKIEM ltk WHERE stk.MaKH = kh.MaKH AND stk.MaChiNhanh = cn.MaCN AND stk.MaLoaiTK = ltk.MaLoaiTK AND stk.MaNV = nv.MaNV AND stk.SoVon<>'0'";
            dataGridView1.DataSource = dataProvider.Instance.ExecuteQuery(st);
            dataGridView1.Columns[6].DefaultCellStyle.Format = "#,##0.##";
            dataGridView1.Columns[5].DefaultCellStyle.Format = "dd/MM/yyyy";
            DateTime dtn = DateTime.Now;

           
        }
        void KhoiTao()
        {
            txtMaSo.Text = txtTenKH.Text = txtMaKH.Text = txtChiNhanh.Text = txtNhanVien.Text = txtHinhThucTL.Text = txtSoDu.Text = txtNgayMoSo.Text = txtThoiHan.Text = "";


            LoadData();
            radioButton1.Checked = radioButton2.Checked = radioButton3.Checked = radioButton4.Checked = radioButton5.Checked = false;
            tbTimKiem.Text = "";
            cbCN.Text = cbTH.Text = "";

        }



        private void dtgDSSo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //MaSoTK,stk.MaKH,TenKH,TenNV,TenCN,NgayMoSo,stk.SoVon,HinhThucTraLai,ltk.ThoiHan,ltk.LaiXuat
            int i;
            i = e.RowIndex;
            if (i == -1)
            {
                return;
            }
            dataGridView1.Rows[i].Selected = true;
            txtMaSo.Text = dataGridView1.Rows[i].Cells[0].Value.ToString();
            txtTenKH.Text = dataGridView1.Rows[i].Cells[2].Value.ToString();
            txtMaKH.Text = dataGridView1.Rows[i].Cells[1].Value.ToString();
            txtNhanVien.Text = dataGridView1.Rows[i].Cells[3].Value.ToString();
            txtChiNhanh.Text = dataGridView1.Rows[i].Cells[4].Value.ToString();
            txtThoiHan.Text = dataGridView1.Rows[i].Cells[8].Value.ToString();
            DateTime ngaymo = DateTime.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString());
            txtNgayMoSo.Text = ngaymo.ToString("dd/MM/yyyy");
            txtHinhThucTL.Text = dataGridView1.Rows[i].Cells[7].Value.ToString();
            txtLaiXuat.Text = dataGridView1.Rows[i].Cells[9].Value.ToString();
            txtSoDu.Text = String.Format("{0:0,0}", float.Parse(dataGridView1.Rows[i].Cells[6].Value.ToString()));
            TinhLai(i);
            txtTienLai.Text = String.Format("{0:0,0}", float.Parse(lai.ToString()));
            txttienlaimoi.Text = String.Format("{0:0,0}", float.Parse(lai1.ToString()));
        }


        private void bthoiTao_Click(object sender, EventArgs e)
        {
            DateTime dtn = DateTime.Now;
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                int Result;
                bool a = int.TryParse(dataGridView1.Rows[i].Cells[9].Value.ToString(), out Result);
                DateTime dt = DateTime.Parse(dataGridView1.Rows[i].Cells[6].Value.ToString()).AddDays(30 * Result);
                MessageBox.Show(dt.ToString() + "--" + dataGridView1.Rows[i].Cells[9].Value.ToString());
                if (DateTime.Compare(dtn, dt) >= 0)
                {
                    dataGridView1.Rows[i].Cells[10].Value = true;
                }
                else
                {
                    dataGridView1.Rows[i].Cells[10].Value = false;
                }
            }
        }
        void TimKiem()
        {
            string st = "";
            try
            {
                if (!string.IsNullOrEmpty(tbTimKiem.Text) || cbCN.Text != "" || cbTH.Text != "")
                {
                    if (radioButton1.Checked)
                    {
                        st = "SELECT MaSoTK,stk.MaKH,TenKH,TenNV,TenCN,NgayMoSo,stk.SoVon,HinhThucTraLai,ltk.ThoiHan,ltk.LaiXuat FROM SOTIETKIEM stk,KHACHHANG kh, NHANVIEN nv,CHINHANH cn, LOAITIETKIEM ltk WHERE stk.MaKH = kh.MaKH AND stk.MaChiNhanh = cn.MaCN AND stk.MaLoaiTK = ltk.MaLoaiTK AND stk.MaNV = nv.MaNV AND stk.SoVon<>'0' AND stk.MaSoTK like '%" + tbTimKiem.Text.ToString() + "%' ";
                        
                    }
                    else if (radioButton2.Checked)
                    {
                        st = "SELECT MaSoTK,stk.MaKH,TenKH,TenNV,TenCN,NgayMoSo,stk.SoVon,HinhThucTraLai,ltk.ThoiHan,ltk.LaiXuat FROM SOTIETKIEM stk,KHACHHANG kh, NHANVIEN nv,CHINHANH cn, LOAITIETKIEM ltk WHERE stk.MaKH = kh.MaKH AND stk.MaChiNhanh = cn.MaCN AND stk.MaLoaiTK = ltk.MaLoaiTK AND stk.MaNV = nv.MaNV AND stk.SoVon<>'0' AND kh.TenKH like N'%" + tbTimKiem.Text.ToString() + "%' ";
                    }
                    else if (radioButton3.Checked)
                    {
                        st = "SELECT MaSoTK,stk.MaKH,TenKH,TenNV,TenCN,NgayMoSo,stk.SoVon,HinhThucTraLai,ltk.ThoiHan,ltk.LaiXuat FROM SOTIETKIEM stk,KHACHHANG kh, NHANVIEN nv,CHINHANH cn, LOAITIETKIEM ltk WHERE stk.MaKH = kh.MaKH AND stk.MaChiNhanh = cn.MaCN AND stk.MaLoaiTK = ltk.MaLoaiTK AND stk.MaNV = nv.MaNV AND stk.SoVon<>'0' AND stk.MaKH like '%" + tbTimKiem.Text.ToString() + "%' ";
                    }
                    else if (radioButton4.Checked)
                    {
                        st = "SELECT MaSoTK,stk.MaKH,TenKH,TenNV,TenCN,NgayMoSo,stk.SoVon,HinhThucTraLai,ltk.ThoiHan,ltk.LaiXuat FROM SOTIETKIEM stk,KHACHHANG kh, NHANVIEN nv,CHINHANH cn, LOAITIETKIEM ltk WHERE stk.MaKH = kh.MaKH AND stk.MaChiNhanh = cn.MaCN AND stk.MaLoaiTK = ltk.MaLoaiTK AND stk.MaNV = nv.MaNV AND stk.SoVon<>'0' AND nv.TenNV like N'%" + tbTimKiem.Text.ToString() + "%' ";
                    }
                    else
                    {
                        st = "SELECT MaSoTK,stk.MaKH,TenKH,TenNV,TenCN,NgayMoSo,stk.SoVon,HinhThucTraLai,ltk.ThoiHan,ltk.LaiXuat FROM SOTIETKIEM stk,KHACHHANG kh, NHANVIEN nv,CHINHANH cn, LOAITIETKIEM ltk WHERE stk.MaKH = kh.MaKH AND stk.MaChiNhanh = cn.MaCN AND stk.MaLoaiTK = ltk.MaLoaiTK AND stk.MaNV = nv.MaNV AND stk.SoVon<>'0' AND stk.MaNV like '%" + tbTimKiem.Text.ToString() + "%' ";
                        
                    }
                    if (cbCN.Text != "")
                    {
                        st += " AND cn.TenCN = N'" + cbCN.Text.ToString() + "'";
                    }
                    if (cbTH.Text != "")
                    {
                        st += " AND ltk.ThoiHan = N'" + cbTH.Text.ToString() + "'";
                    }
                    dataGridView1.DataSource = dataProvider.Instance.ExecuteQuery(st);

                }
                else
                {
                    LoadData();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void tbTimKiem_TextChanged(object sender, EventArgs e)
        {
            TimKiem();
        }

        private void cbCN_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loaded)
            {
                TimKiem();
            }
        }

        private void cbTH_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loaded)
            {
                TimKiem();
            }
        }
        private void checkBox1_Click(object sender, EventArgs e)
        {
            TimKiem();
        }

        private void checkBox2_Click(object sender, EventArgs e)
        {
            TimKiem();
        }

        private void btKhoiTao_Click(object sender, EventArgs e)
        {
            KhoiTao();
        }

        private void iconButton3_Click_1(object sender, EventArgs e)
        {
            tabControl1.TabPages.Remove(tabPage3);
            tabControl1.TabPages.Add(tabPage1);
        }
        private bool checkThongTinSoIn()
        {
            if (string.IsNullOrEmpty(txtTenKH.Text) || string.IsNullOrEmpty(txtMaKH.Text) || string.IsNullOrEmpty(txtSoDu.Text))
            {
                return false;
            }
            return true;
        }
        public int Random()
        {
            Random rd = new Random();
            return rd.Next(100000, 999999);
        }

        string checkPhieuGR(string tenphieu)
        {
            string maPhieu = "";
            if (tenphieu == "goi")
            {
                maPhieu = Random().ToString();
                string st = "SELECT * FROM PHIEUGOITIEN WHERE MaPhieu='" + maPhieu + "' ";
                while (!CheckMa(st))
                {
                    maPhieu = Random().ToString();
                }
            }
            else
            {
                maPhieu = Random().ToString();
                string st = "SELECT * FROM PHIEURUTTIEN WHERE MaPhieu='" + maPhieu + "' ";
                while (!CheckMa(st))
                {
                    maPhieu = Random().ToString();
                }
            }
            return maPhieu;
        }
        bool CheckMa(string st)
        {
            DataTable dtb = new DataTable();
            dtb = dataProvider.Instance.ExecuteQuery(st);
            if (dtb.Rows.Count == 0)
            {
                return true;
            }
            return false;
        }

        void LoadTab3()
        {
            string query = "select CCCD,DiaChi,GioiTinh,SDT,Email from khachhang where maKh='" + txtMaKH.Text + "'";
            string query1 = "select TenLoaiTK, LaiXuat from LOAITIETKIEM where ThoiHan='" + txtThoiHan.Text + "'";
            dt = dataProvider.Instance.ExecuteQuery(query);           
            //khách hàng
            txtmaKH1.Text = txtMaKH.Text;
            txtten1.Text = txtTenKH.Text;
            txtdiaChi1.Text = dt.Rows[0]["DiaChi"].ToString();
            txtcccd1.Text = dt.Rows[0]["CCCD"].ToString();
            txtSDT1.Text = dt.Rows[0]["SDT"].ToString();
            txtGmail1.Text = dt.Rows[0]["Email"].ToString();

            //sổ

            tbMS1.Text = txtMaSo.Text;
            cbCN1.Text = txtHinhThucTL.Text;
            cbNV1.Text = txtNhanVien.Text;
            tbST1.Text = txtSoDu.Text;
            tbNgayMS.Text = txtNgayMoSo.Text;
            tbTH1.Text = txtThoiHan.Text;
            dt = dataProvider.Instance.ExecuteQuery(query1);
            tbLoaiTK1.Text = dt.Rows[0]["TenLoaiTK"].ToString();
            tbLX1.Text = dt.Rows[0]["LaiXuat"].ToString();
            tbHinhTTL.Text = txtHinhThucTL.Text;
            if(tbHinhTTL.Text== "Tất toán sổ ")
            {
                rdbKhong.Checked = true;
                rdbCo.Checked = false;
                lbGH1.Hide();
                tbGH1.Hide();
                lbGhiChu1.Text = "Sổ sẽ tự động tất toán khi đáo hạn,";
                lbGhiChu2.Text= "cả gốc và lãi được chuyển vào số dư của khách hàng";
            }
            else
            {
                rdbKhong.Checked = false;
                rdbCo.Checked = true;
                lbGH1.Show();
                tbGH1.Show();
                tbGH1.Text = soKH.ToString();
                if(tbHinhTTL.Text == "Lãi trả vào tài khoản khách hàng ")
                {
                    lbGhiChu1.Text = "Sổ sẽ tự động gia hạn khi đáo hạn với số tiền gốc ban ";
                    lbGhiChu2.Text = "đầu, còn lãi được chuyển vào số dư của khách hàng";
                
                }
                if (tbHinhTTL.Text == "Lãi nhập gốc")
                {
                    lbGhiChu1.Text = "Sổ sẽ tự động gia hạn khi đáo hạn và";
                    lbGhiChu2.Text = " số tiền vốn bằng sô tiền ban đầu và lãi";
                }
            }
            tbNgayDH.Text = txtngayDH.Text;
            tbLai1.Text = txtTienLai.Text;
            tblaimoi.Text = txttienlaimoi.Text;
            tbTongTien.Text = String.Format("{0:0,0}", (goc + lai));
        }

      
        void TinhLai(int i)
        {
            TinhLaiContext ctx = new TinhLaiContext(new LaiTatToan());
            if (txtHinhThucTL.Text == "Tất toán sổ")
            {
                goc = (ulong)(float.Parse(txtSoDu.Text));
                float laixuat = float.Parse(txtLaiXuat.Text);             
                DateTime ngaygoi = DateTime.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString());
                int thangTH;
                bool a = int.TryParse(txtThoiHan.Text, out thangTH);
                DateTime ngaydh = ngaygoi.AddDays(30 * thangTH);
                txtngayDH.Text = ngaydh.ToString("dd/MM/yyyy");
                lai =0;
                lai1 = ctx.TinhLai(goc, laixuat,0);
            }
            else
            {
                if (txtHinhThucTL.Text == "Lãi nhập gốc")
                {
                    ctx.ChonStrategy(new LaiNhapGoc());
                    goc = (ulong)(float.Parse(txtSoDu.Text));
                    float laixuat = float.Parse(txtLaiXuat.Text);
                    DateTime ngaygoi = DateTime.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString());
                    TimeSpan interval = DateTime.Now.Subtract(ngaygoi);
                    int intI = interval.Days;
                    int thangGoi = (int)(intI / 30);
                    int thangKH = int.Parse(txtThoiHan.Text);
                    soKH = (int)(thangGoi / thangKH);
                    DateTime ngayDHSaptToi = ngaygoi.AddDays(30 * thangKH * (soKH + 1));
                    txtngayDH.Text = ngayDHSaptToi.ToString("dd/MM/yyyy");
                    //goclai = (ulong)(goc * (Math.Pow((1 + laixuat / 100), soKH)));
                    //ulong goclai1 = (ulong)(goc * (Math.Pow((1 + laixuat / 100), (soKH + 1))));
                    lai = ctx.TinhLai(goc, laixuat, soKH); 
                    lai1 = ctx.TinhLai(goc, laixuat, soKH+1);
                }
                else
                {
                    ctx.ChonStrategy(new LaiTraVeTaiKhoan());
                    goc = (ulong)(float.Parse(txtSoDu.Text));
                    float laixuat = float.Parse(txtLaiXuat.Text);
                    DateTime ngaygoi = DateTime.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString()).Date;
                    TimeSpan interval = DateTime.Now.Subtract(ngaygoi);
                    int intI = interval.Days;
                    int thangGoi = (int)(intI / 30);
                    int thangKH = int.Parse(txtThoiHan.Text);
                    soKH = (int)(thangGoi / thangKH);
                    DateTime ngayDHSaptToi = ngaygoi.AddDays(30 * thangKH * (soKH + 1));
                    txtngayDH.Text = ngayDHSaptToi.ToString("dd/MM/yyyy");                
                    lai = ctx.TinhLai(goc, laixuat, soKH);
                    lai1 = ctx.TinhLai(goc, laixuat, soKH+1);
                }
            }
        }

       
        private void tbGT1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
     (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        public string maLoaiTK(string x)
        {
            string query = "select MaLoaiTK from LOAITIETKIEM where TenLoaiTK=N'" + x + "'";
            dt = dataProvider.Instance.ExecuteQuery(query);
            return dt.Rows[0]["MaLoaiTK"].ToString();
        }

       
        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void txtSoDu_TextChanged(object sender, EventArgs e)
        {
            if (txtTienLai.Text == "00")
            {
                txtTienLai.Text = "0";
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }       

        private void button1_Click_1(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                MessageBox.Show(dataGridView1.Rows[0].Cells[i].Value.ToString() + "++i");
            }
        }

        private void btnTatToan_Click(object sender, EventArgs e)
        {
            int i;
            i = dataGridView1.CurrentRow.Index;
            if (txtMaSo.Text == "") 
            {
                MessageBox.Show("Mời chọn sổ muốn tất toán!", "Thông báo!");
                return;
            }
            DateTime ngaymo = DateTime.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString());
            string st = "SELECT * FROM THAMSO";
            DataTable dt = dataProvider.Instance.ExecuteQuery(st);
            DataRow dtr = dt.Rows[0];
            double SoNgayDuocRutSauGoi = double.Parse(dtr[2].ToString());
            DateTime sau15ngay = ngaymo.AddDays(SoNgayDuocRutSauGoi);
            if (sau15ngay > DateTime.Now)
            {
                MessageBox.Show("Sổ chỉ được tất toán sau " + SoNgayDuocRutSauGoi + " ngày kể từ ngày mở sổ!", "Thông báo!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                tabControl1.TabPages.Remove(tabPage1);
                tabControl1.TabPages.Add(tabPage3);
                LoadTab3();
                btTatToan.Show();
            }
        }

        private void btTatToan_Click(object sender, EventArgs e)
        {
            if (tbHinhTTL.Text == "Tất toán sổ ")
            {
                DialogResult dg = MessageBox.Show("Bạn có chắc chắn muốn tất toán sổ trước kì hạn?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dg == DialogResult.Yes)
                {

                    string sodubd = dataProvider.Instance.ExecuteScalar("SELECT SoDu From KHACHHANG where MaKH='" + txtmaKH1.Text + "'").ToString();
                    decimal sodu = decimal.Parse(sodubd) + decimal.Parse(tbTongTien.Text);
                    string query= "UPDATE SOTIETKIEM SET SoVon='0' WHERE MaSoTK='"+tbMS1.Text.ToString()+"'";
                    string query1 = "update KHACHHANG set SoDu='" + sodu + "'where MaKH='" + txtmaKH1.Text + "'";
                    string maPhieu = Random().ToString();
                    string st = "SELECT * FROM PHIEUGOITIEN WHERE MaPhieu='" + maPhieu + "' ";
                    while (!CheckMa(st))
                    {
                        maPhieu = Random().ToString();
                    }
                    string query2 = "INSERT INTO PHIEUGOITIEN (MaPhieu,MaKH,MaNV,NgayGoi,SoTienGoi,MaCN,NoiDungGiaoDich) VALUES('" + maPhieu + "','" + txtmaKH1.Text + "','" + MainFormManager.Instance.maNV().ToString() + "','" + DateTime.Today.ToString("yyyy/MM/dd") + "','" + decimal.Parse(tbTongTien.Text) + "','" + MainFormManager.Instance.maCN().ToString() + "',N'Tất toán sổ tiết kiệm')";
                    if (dataProvider.Instance.ExecuteNonQuery(query) > 0 && dataProvider.Instance.ExecuteNonQuery(query1)>0 && dataProvider.Instance.ExecuteNonQuery(query2)>0)
                    {
                        MessageBox.Show("Tất toán thành công, tiền gốc đã được chuyển vào tài khoản khách hàng!");
                    }
                    else
                    {
                        MessageBox.Show("Tất toán không thành công, mời thử lại!");
                    }
                }
            }
            if (tbHinhTTL.Text == "Lãi trả vào tài khoản khách hàng ")
            {
                DialogResult dg = MessageBox.Show("Bạn có chắc chắn muốn tất toán sổ trước kì hạn?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dg == DialogResult.Yes)
                {
                    string sodubd = dataProvider.Instance.ExecuteScalar("SELECT SoDu From KHACHHANG where MaKH='" + txtmaKH1.Text + "'").ToString();
                    decimal sodu = decimal.Parse(sodubd) + decimal.Parse(tbTongTien.Text);
                    string query = "UPDATE SOTIETKIEM SET SoVon='0' WHERE MaSoTK='" + tbMS1.Text.ToString() + "'";
                    string query1 = "update KHACHHANG set SoDu='" + sodu + "'where MaKH='" + txtmaKH1.Text + "'";
                    string maPhieu = Random().ToString();
                    string st = "SELECT * FROM PHIEUGOITIEN WHERE MaPhieu='" + maPhieu + "' ";
                    while (!CheckMa(st))
                    {
                        maPhieu = Random().ToString();
                    }
                    string query2 = "INSERT INTO PHIEUGOITIEN (MaPhieu,MaKH,MaNV,NgayGoi,SoTienGoi,MaCN,NoiDungGiaoDich) VALUES('" + maPhieu + "','" + txtmaKH1.Text + "','" + MainFormManager.Instance.maNV().ToString() + "','" + DateTime.Today.ToString("yyyy/MM/dd") + "','" + decimal.Parse(tbTongTien.Text) + "','" + MainFormManager.Instance.maCN().ToString() + "',N'Tất toán sổ tiết kiệm')";
                    if (dataProvider.Instance.ExecuteNonQuery(query) > 0 && dataProvider.Instance.ExecuteNonQuery(query1) > 0 && dataProvider.Instance.ExecuteNonQuery(query2) > 0)
                    {
                        MessageBox.Show("Tất toàn thành công, tiền gốc đã được chuyển vào tài khoản khách hàng");
                    }
                    else
                    {
                        MessageBox.Show("Tất toán không thành công, mời thử lại!");
                    }
                }
            }
            if (tbHinhTTL.Text == "Lãi nhập gốc")
            {
                DialogResult dg = MessageBox.Show("Bạn có chắc chắn muốn tất toán sổ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dg == DialogResult.Yes)
                {
                    string sodubd = dataProvider.Instance.ExecuteScalar("SELECT SoDu From KHACHHANG where MaKH='" + txtmaKH1.Text + "'").ToString();
                    decimal sodu = decimal.Parse(sodubd) + decimal.Parse(tbTongTien.Text);
                    string query = "UPDATE SOTIETKIEM SET SoVon='0' WHERE MaSoTK='" + tbMS1.Text.ToString() + "'";
                    string query1 = "update KHACHHANG set SoDu='" + sodu + "'where MaKH='" + txtmaKH1.Text + "'";
                    string maPhieu = Random().ToString();
                    string st = "SELECT * FROM PHIEUGOITIEN WHERE MaPhieu='" + maPhieu + "' ";
                    while (!CheckMa(st))
                    {
                        maPhieu = Random().ToString();
                    }
                    string query2 = "INSERT INTO PHIEUGOITIEN (MaPhieu,MaKH,MaNV,NgayGoi,SoTienGoi,MaCN,NoiDungGiaoDich) VALUES('" + maPhieu + "','" + txtmaKH1.Text + "','" + MainFormManager.Instance.maNV().ToString() + "','" + DateTime.Today.ToString("yyyy/MM/dd") + "','" + decimal.Parse(tbTongTien.Text) + "','" + MainFormManager.Instance.maCN().ToString() + "',N'Tất toán sổ tiết kiệm')";
                    if (dataProvider.Instance.ExecuteNonQuery(query) > 0 && dataProvider.Instance.ExecuteNonQuery(query1) > 0 && dataProvider.Instance.ExecuteNonQuery(query2) > 0)
                    {
                        MessageBox.Show("Tất toàn thành công, tiền gốc và lãi đã được chuyển vào tài khoản khách hàng");
                    }
                    else
                    {
                        MessageBox.Show("Tất toán không thành công, mời thử lại!");
                    }
                }
            }
        }

        private void iconButton6_Click_1(object sender, EventArgs e)
        {
            tabControl1.TabPages.Remove(tabPage3);
            tabControl1.TabPages.Add(tabPage1);
            LoadData();
            KhoiTao();
        }
    }
}
