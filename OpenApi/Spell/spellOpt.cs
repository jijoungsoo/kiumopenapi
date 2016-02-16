using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenApi.Spell
{
    public class spellOpt
    {
        private static string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
        public String key;
        public String value;
        public String sRQNAME;
        public String sTrCode;
        public String stockCode;
        public String startDate;   
        public String endDate;          /*일자 = YYYYMMDD(20160101 연도4자리, 월 2자리, 일 2자리 형식)*/
        public String sScreenNo;
        public int nPrevNext;
        public String modifyGubun = "1";
        public String priceOrAmount;    /*금액수량구분 = 1:금액, 2:수량*/
        public String buyOrSell;       /*매매구분 = 0:순매수, 1:매수, 2:매도*/
        public String priceGubun = "1";/*단위구분 = 1000:천주, 1:단주*/
        public String lastStockDate;





        public spellOpt ShallowCopy() 
        {
            return (spellOpt)this.MemberwiseClone();
        }

        public String GetFileName()
        {
            path = System.IO.Path.GetDirectoryName(path);
            if (this.sTrCode.Equals("OPT10059"))
            {
                path = path + "\\" + this.sTrCode + "_" + this.stockCode + "_"+this.buyOrSell+"_"+this.priceOrAmount+".txt";
            } else { 
                path = path + "\\" + this.sTrCode + "_" + this.stockCode + ".txt";
            }
            return path;
        }

        public Boolean isNext()
        {
            if (this.nPrevNext > 0 && this.startDate.Equals("TWO"))
            {
                return true;
            }
            else
            {
                if(this.startDate.Equals("ZERO"))
                {
                    return false;
                } else 
                {
                    if(this.nPrevNext>0) { 
                        int startDate일자 = 0;
                        int lastStockDate일자 = 0;
                    
                        if (!int.TryParse(this.startDate,out startDate일자)){
                            startDate일자 = 0;
                        }
                        if (!int.TryParse(this.lastStockDate, out lastStockDate일자))
                        {
                            lastStockDate일자 = 0;
                        }
                        FileLog.PrintF("isNext() startDate일자=" + startDate일자.ToString() + ",lastStockDate일자=" + lastStockDate일자.ToString());
                        if (startDate일자 < lastStockDate일자)
                        {
                            return true;
                        } else
                        {
                            return false;
                        }
                    } else
                    {
                        return false;
                    }
                }               
            }
        }
    }
}
