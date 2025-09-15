using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Repository
{
    internal interface IFormDataRepository
    {
        List<string> GetTypeList();
        List<string> GetDetailList(string typeName);
        List<string> GetTargetList();
        List<string> GetPaymentList();
    }
}
