using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Thong_Tin_Khach_hang;
using System.Text.RegularExpressions;

namespace WindowsFormsApp1
{
    public interface IAuthenticationService
    {
        bool Authenticate( string username, string password );
        bool IsValidatedData( string username, string password );
    }

    public class RealAuthenticationService : IAuthenticationService
    {
        public bool Authenticate( string username, string password ) {
            string query = "select MatKhau,SDT from NHANVIEN where SDT='" + username + "' and MatKhau='" + password + "'";
            return dataProvider.Instance.ExecuteReader(query);
        }

        public bool IsValidatedData( string username, string password )
        {
            return false;
        }
    }


    public class AuthenticationProxy : IAuthenticationService
    {
        private RealAuthenticationService realAuthenticationService;

        public AuthenticationProxy()
        {
            this.realAuthenticationService = new RealAuthenticationService();
        }

        public bool Authenticate( string username, string password )
        {
            // Kiểm tra điều kiện trước khi thực hiện login
            if (realAuthenticationService.IsValidatedData(username, password))
            {
                // Nếu điều kiện hợp lệ, chuyển gọi đến đối tượng thực
                return realAuthenticationService.Authenticate(username, password);
            }
            return false;
        }

        public bool IsValidatedData( string username, string password )
        {
            if (
                username == "Phone Number" || 
                username.Length < 10 && username.Length > 11 ||
                !username.StartsWith("0") ||
                (password == "Password")
            )
            {
                return false;
            }
            return true;
        }
    }


    public partial class Login : Form
    {

        public static string sdt = "";
        private bool check = false;
        IAuthenticationService authenticationService = new AuthenticationProxy();

        public Login()
        {
            InitializeComponent();
            
        }


        private void rjTextBox1_Enter(object sender, EventArgs e)
        {
            if (txtSDT.Text == "Phone Number")
            {
                txtSDT.Text = "";
                txtSDT.ForeColor = Color.Black;
            }
            this.isValidAccount();
        }

        private void txtGmail_Leave(object sender, EventArgs e)
        {
            if (txtSDT.Text == "")
            {
                txtSDT.Text = "Phone Number";
                txtSDT.ForeColor = Color.FromArgb(64, 64, 64);
            }
            this.isValidAccount();
        }

        private void txtMatKhau_Leave(object sender, EventArgs e)
        {
            if (txtMatKhau.Text == "")
            {
                
                txtMatKhau.Text = "Password";
                txtMatKhau.ForeColor = Color.FromArgb(64, 64, 64);
                txtMatKhau.PasswordChar = '\0';
            }
            this.isValidAccount();
        }

        private void txtMatKhau_Enter(object sender, EventArgs e)
        {
            if (txtMatKhau.Text == "Password")
            {
                txtMatKhau.Text = "";
                txtMatKhau.ForeColor = Color.Black;
            }
            this.isValidAccount();
        }


        private void txtMatKhau_TextChanged(object sender, EventArgs e)
        {
            if(txtMatKhau.Text==""||txtMatKhau.Text== "Password")
            {
                ptbeye.Hide();
                ptbeyeslash.Hide();
                txtMatKhau.PasswordChar = '\0';
                if (txtMatKhau.Text == "")
                check=false;
            }
            else
            {
                if (check)
                {
                    ptbeye.Show();
                    txtMatKhau.PasswordChar = '\0';
                }
                else
                {
                    ptbeyeslash.Show();
                    txtMatKhau.PasswordChar = '*';
                }
            }  
        }

        private void rjButton1_Click(object sender, EventArgs e)
        {
            // Thực hiện login thông qua Proxy
            bool result = authenticationService.Authenticate(txtSDT.Text, txtMatKhau.Text);

            if (result)
            {
                Thread th;
                sdt = txtSDT.Text;
                this.Close();
                th = new Thread(OpenAdminForm);
                th.SetApartmentState(ApartmentState.STA);
                th.Start();
            }
            else
            {
                // Login thất bại
                this.ShowInValidDataError();
            }

        }

        private void isValidAccount()
        {
            bool isValidData = authenticationService.IsValidatedData(txtSDT.Text, txtMatKhau.Text);
            if (!isValidData)
            {
                this.ShowInValidDataError();
                return;
            }
        }

        private void ShowInValidDataError()
        {
            lblerror.Text = "Số điện thoại hoặc mật khẩu không đúng!";
            lblerror.ForeColor = Color.FromArgb(255, 90, 120);
            /*txtMatKhau.Focus();*/
            txtMatKhau.Text = "";
            lblerror.Show();
        }
        
        private void OpenAdminForm()
        {
            Application.Run(new MainFormManager());
        }

        private void Login_Load(object sender, EventArgs e)
        {
            lblerror.Hide();
            ptbeye.Hide();
            ptbeyeslash.Hide();
            this.ActiveControl = label1;
        }

        private void txtGmail_TextChanged(object sender, EventArgs e)
        {
            lblerror.Hide();
        }

        private void iconPictureBox3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ptbeyeslash_Click(object sender, EventArgs e)
        {
            check = true;
            ptbeyeslash.Hide();
            ptbeye.Show();
            txtMatKhau.PasswordChar = '\0';
        }

        private void ptbeye_Click(object sender, EventArgs e)
        {
            check = false;
            ptbeye.Hide();
            ptbeyeslash.Show();
            txtMatKhau.PasswordChar = '*';
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmForgotPassWord fg = new frmForgotPassWord();
            fg.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
