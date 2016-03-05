using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenApi.Spell;

namespace OpenApi.ReceiveTrData
{
    public abstract class ReceiveTrData
    {
        public SpellOpt spell;

        public abstract void ReceivedData(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e);
        public abstract int Run(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, SpellOpt spell);
        public void Send(String key, String message,String className) {
            FileLog.PrintF(String.Format("Send [className][Key][Message]=>[{0}][{1}][{2}]",className,key,message));
        }


        protected Boolean isNotNull(String value)
        {
            if (value != null && !value.Trim().Equals(""))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected void DisconnectRealData(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI,String sScrNo)
        {
            FileLog.PrintF(String.Format("DisconnectRealData sScrNo=>" + sScrNo));
            axKHOpenAPI.DisconnectRealData(sScrNo);
        }

        protected void SetRealRemove(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, String sScrNo,String strDelCode)
        {
            FileLog.PrintF(String.Format("SetRealRemove sScrNo=>" +sScrNo + ",strDelCode=>" + strDelCode));
            axKHOpenAPI.SetRealRemove(sScrNo, strDelCode);
        }

        protected void putReceivedQueueAndsetNextSpell(String key,String message, int prevNext, String lastStockDate)
        {
            OpenApi.Spell.SpellOpt tmp = spell.ShallowCopy();
            tmp.nPrevNext = prevNext;
            tmp.lastStockDate = lastStockDate;
            tmp.value = message;
            Class1.getClass1Instance().removeSpellDictionary(key);
            Class1.getClass1Instance().AddSpellDictionary(key, tmp);
            Class1.getClass1Instance().EnqueueByReceivedQueue(tmp);
        }

    }


}
