using Accounting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Contracts
{
    internal class ModifyRecordContract
    {
        internal interface IModifyRecordPresenter
        {
            //Task GetRecordAsync(RecordReq requst);
            void GetRecord(DateTime startDateTime, DateTime endDatetime);
            void DeleteRecord(ModifyRecordDTO modifyRecordDTO);
            void ModifyRecord(ModifyRecordDTO modifyRecordDTO);
        }
        internal interface IModifyRecordView
        {
            /// <summary>
            /// Render AccountBookForm DataGridView data.
            /// </summary>
            /// <param name="recordList"></param>
            void RenderDateGridView(List<ModifyRecordDTO> modifyRecordDTOList);
        }
    }
}
