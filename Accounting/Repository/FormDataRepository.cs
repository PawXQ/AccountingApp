using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Repository
{
    internal class FormDataRepository : IFormDataRepository
    {
        public List<string> type = new List<string>() { "食", "衣", "住", "行", "育", "樂" };
        public List<string> type_Food = new List<string>() { "飯", "麵", "湯", "餃", "肉", "菜" };
        public List<string> type_Cloth = new List<string>() { "衣", "褲" };
        public List<string> type_Stay = new List<string>() { "water", "electric", "rent" };
        public List<string> type_Transport = new List<string>() { "Bus", "Uber", "MRT", "Train" };
        public List<string> type_Edu = new List<string>() { "Lesson", "Class", "Book", "Certificate" };
        public List<string> type_Fun = new List<string>() { "Movie", "Netflix", "Disney", "Youtube", "Twitch" };
        public List<string> target = new List<string>() { "F", "M", "GF", "GM", "B", "S" };
        public List<string> payment = new List<string>() { "ApplePay", "LinePay", "Cash", "CreditCard", "Crypto" };
        public Dictionary<string, List<string>> Type_Mapping = null;
        public FormDataRepository()
        {
            Type_Mapping = new Dictionary<string, List<string>>() {
                { "食",type_Food },
                { "衣",type_Cloth },
                { "住",type_Stay },
                { "行",type_Transport },
                { "育",type_Edu },
                { "樂",type_Fun }
            };
        }
        public List<string> GetTypeList()
        {
            return this.type;
        }
        public List<string> GetDetailList(string typeName)
        {
            return this.Type_Mapping[typeName];
        }
        public List<string> GetTargetList()
        {
            return this.target;
        }

        public List<string> GetPaymentList()
        {
            return this.payment;
        }
    }
}
