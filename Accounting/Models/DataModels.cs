using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Models
{
    internal class DataModels
    {
        public static List<string> type = new List<string>() { "食", "衣", "住", "行", "育", "樂" };
        public static List<string> type_Food = new List<string>() { "飯", "麵", "湯", "餃", "肉", "菜" };
        public static List<string> type_Cloth = new List<string>() { "衣", "褲" };
        public static List<string> type_Stay = new List<string>() { "water", "electric", "rent" };
        public static List<string> type_Transport = new List<string>() { "Bus", "Uber", "MRT", "Train" };
        public static List<string> type_Edu = new List<string>() { "Lesson", "Class", "Book", "Certificate" };
        public static List<string> type_Fun = new List<string>() { "Movie", "Netflix", "Disney", "Youtube", "Twitch" };

        public static Dictionary<string, List<string>> Type_Mapping = new Dictionary<string, List<string>>() {
            { "食",type_Food },
            { "衣",type_Cloth },
            { "住",type_Stay },
            { "行",type_Transport },
            { "育",type_Edu },
            { "樂",type_Fun }
        };

        public static List<string> target = new List<string>() { "F", "M", "GF", "GM", "B", "S" };
        public static List<string> payment = new List<string>() { "ApplePay", "LinePay", "Cash", "CreditCard", "Crypto" };
    }
}
