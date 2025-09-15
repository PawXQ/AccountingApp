using Accounting.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Repository
{
    internal interface IRecordRepository
    {
        List<RecordModel> GetRecords(DateTime datetime);
        List<RecordModel> GetRecords(DateTime startDatetime, DateTime endDatetime);

        void CreateRecords(RecordModel recordModel);

        void UpdateRecords(RecordModel recordModel);

        void RemoveRecords(RecordModel recordModel);
    }
}
