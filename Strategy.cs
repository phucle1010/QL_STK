using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TinhLaiContext
{
    private StrategyTinhLai CTTinhLai;

    public TinhLaiContext(StrategyTinhLai CTTinhLai)
    {
        this.CTTinhLai = CTTinhLai;
    }
    public void ChonStrategy(StrategyTinhLai Export)
    {
        this.CTTinhLai = Export;
    }
    public ulong TinhLai(ulong TienGoc,float LaiXuat,int SoKiHan)
    {
       return  CTTinhLai.TinhLaiXuat(TienGoc, LaiXuat, SoKiHan);
    }
}

public interface StrategyTinhLai
{
    ulong TinhLaiXuat(ulong TienGoc, float LaiXuat, int SoKiHan);
}

public class LaiTatToan : StrategyTinhLai
{
    public ulong TinhLaiXuat(ulong TienGoc, float LaiXuat, int SoKiHan)
    {
       return (ulong)(TienGoc * LaiXuat / 100); ;
    }
}

public class LaiNhapGoc : StrategyTinhLai
{
    public ulong TinhLaiXuat(ulong TienGoc, float LaiXuat, int SoKiHan)
    {
        ulong goclai = (ulong)(TienGoc * (Math.Pow((1 + LaiXuat / 100), (SoKiHan))));
        return goclai - TienGoc;
    }
}

public class LaiTraVeTaiKhoan : StrategyTinhLai
{
    public ulong TinhLaiXuat(ulong TienGoc, float LaiXuat, int SoKiHan)
    {
        return (ulong)((TienGoc * LaiXuat / 100)*SoKiHan);
    }
}


namespace WindowsFormsApp1
{
    internal class Strategy
    {
    }
}
