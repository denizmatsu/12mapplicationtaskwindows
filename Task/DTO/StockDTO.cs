using System.Collections.Generic;

public class StockDTO
{
    public List<StockDataDTO> Result { get; set; }
}

public class StockDataDTO
{
    public int siraNo { get; set; }
    public string islemTur { get; set; }
    public string evrakNo { get; set; }
    public string tarih { get; set; }
    public decimal girisMiktar { get; set; }
    public decimal cikisMiktar { get; set; }
    public decimal stokMiktar { get; set; }
}