using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1;
namespace Thong_Tin_Khach_hang
{
    public class GiaoDich
    {
        public string makh { get; set; }
        public string maSTK { get; set; }
        public decimal sodu { get; set; }

        private QuanLyGiaoDich quanLyGiaoDich;

        editPerson edit = new editPerson();
        public phieuGuiTien pg { get; set; }
        public phieuRutTien pr { get; set; }
        public Passbook pb { get; set; }
        public GiaoDich()
        {
            quanLyGiaoDich = new QuanLyGiaoDich();
        }


        public bool MoSo()
        {
            string query2 = "update KHACHHANG set SoDu=" + this.sodu + "where MaKH='" + this.makh + "'";
            if (edit.InsertPassBook(pb) && dataProvider.Instance.ExecuteNonQuery(query2) != 0 && edit.InsertphieuRutTien(pr))
            {
                return true;
            }
            return false;

        }

        public bool DongSo(object sender, EventArgs e)
        {
            string query = "UPDATE SOTIETKIEM SET SoVon='0' WHERE MaSoTK='" + this.maSTK + "'";
            string query1 = "update KHACHHANG set SoDu='" + this.sodu + "'where MaKH='" + this.makh + "'";

            if (dataProvider.Instance.ExecuteNonQuery(query) > 0 && dataProvider.Instance.ExecuteNonQuery(query1) > 0 && edit.InsertphieuGuiTien(pg))
            {
                return true;
            }
            return false;
        }

        public bool RutTien(object sender, EventArgs e)
        {
            string query = "update KHACHHANG set SoDu='" + this.sodu + "'where MaKH='" + this.makh + "'";
            if (edit.InsertphieuRutTien(pr) && dataProvider.Instance.ExecuteNonQuery(query) != 0)
            {
                return true;
            }
            return false;
        }

        public bool GuiTien()
        {
            string query = "update KHACHHANG set SoDu=" + this.sodu + "where MaKH='" + this.makh + "'";
            if (edit.InsertphieuGuiTien(pg) && dataProvider.Instance.ExecuteNonQuery(query) != 0)
            {
                return true;
            }
            return false;
        }

        public bool GiaoDichMoSo()
        {
            ICommand command = new MoSoCommand(this);
            quanLyGiaoDich.SetCommand(command);
            return quanLyGiaoDich.ExecuteCommand();
        }

        public bool GiaoDichDongSo()
        {
            ICommand command = new DongSoCommand(this);
            quanLyGiaoDich.SetCommand(command);
            return quanLyGiaoDich.ExecuteCommand();
        }

        public bool GiaoDichGuiTien()
        {
            ICommand command = new GuiTienCommand(this);
            quanLyGiaoDich.SetCommand(command);
            return quanLyGiaoDich.ExecuteCommand();
        }

        public bool GiaoDichRutTien()
        {
            ICommand command = new RutTienCommand(this);
            quanLyGiaoDich.SetCommand(command);
            return quanLyGiaoDich.ExecuteCommand();
        }

    }
    public interface ICommand
    {
        bool Execute();
    }

    public class MoSoCommand : ICommand
    {
        private GiaoDich giaoDich;
        public MoSoCommand(GiaoDich giaoDich)
        {
            this.giaoDich = giaoDich;
        }

        public bool Execute()
        {
            return giaoDich.MoSo();
        }
    }

    public class DongSoCommand : ICommand
    {
        private GiaoDich giaoDich;

        public DongSoCommand(GiaoDich giaoDich)
        {
            this.giaoDich = giaoDich;



        }

        public bool Execute()
        {
            return giaoDich.DongSo(null, null);
        }
    }

    public class GuiTienCommand : ICommand
    {
        private GiaoDich giaoDich;

        public GuiTienCommand(GiaoDich giaoDich)
        {

            this.giaoDich = giaoDich;
        }

        public bool Execute()
        {
            return giaoDich.GuiTien();
        }
    }

    public class RutTienCommand : ICommand
    {
        private GiaoDich giaoDich;

        public RutTienCommand(GiaoDich giaoDich)
        {
            this.giaoDich = giaoDich;
        }

        public bool Execute()
        {
            return giaoDich.RutTien(null, null);
        }
    }

    public class QuanLyGiaoDich
    {
        private ICommand command;

        public void SetCommand(ICommand command)
        {
            this.command = command;
        }

        public bool ExecuteCommand()
        {
            return command.Execute();
        }
    }
}
