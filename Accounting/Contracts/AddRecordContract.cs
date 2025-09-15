using Accounting.Repository.Entities;
using Accounting.Models;
using Accounting.Repository;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Contracts
{
    internal class AddRecordContract
    {
        internal interface IAddRecordPresenter
        {
            void GetComboBoxDataList();
            void GetTypeDetailList(string type);
            void AddRecord(RecordDTO recordDTO);
        }
        internal interface IAddRecordView
        {
            /// <summary>
            /// Render AccountForm all combox value.
            /// </summary>
            /// <param name="formDataOptionDTO"></param>
            void RenderCombox(FormDataOptionDTO formDataOptionDTO);
            /// <summary>
            /// Render AccountForm "detail" combox.
            /// </summary>
            /// <param name="detailList"></param>
            void RenderDetailCombox(List<string> detailList);
            /// <summary>
            /// Initial AccountForm view.
            /// </summary>
            void Initial();
        }
    }
}
