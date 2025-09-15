using Accounting.Models;
using Accounting.Repository;
using Accounting.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Accounting.Contracts.AddRecordContract;
using static Accounting.Contracts.ModifyRecordContract;

namespace Accounting.Presenter
{
    internal class ModifyRecordPresenter : IModifyRecordPresenter
    {
        IModifyRecordView modifyRecordView;
        IRecordRepository recordRepository;
        public ModifyRecordPresenter(IModifyRecordView modifyRecordView)
        {
            this.modifyRecordView = modifyRecordView;
            this.recordRepository = new CSVRecordRepository();
        }

        public void GetRecord(DateTime startDateTime, DateTime endDatetime)
        {
            List<RecordModel> recordDTOList = recordRepository.GetRecords(startDateTime, endDatetime);

            List<ModifyRecordDTO> modifyRecordDTOLIST = recordDTOList
               .Select(record => new ModifyRecordDTO
               {
                   datetime = record.datetime,
                   money = record.money,
                   type = record.type,
                   detail = record.detail,
                   target = record.target,
                   payment = record.payment,
                   image1 = record.image1,
                   image2 = record.image2,
               }).ToList();

            modifyRecordView.RenderDateGridView(modifyRecordDTOLIST);
        }

        public void ModifyRecord(ModifyRecordDTO modifyRecordDTO)
        {
            RecordModel recordModel = new RecordModel()
            {
                datetime = modifyRecordDTO.datetime,
                money = modifyRecordDTO.money,
                type = modifyRecordDTO.type,
                detail = modifyRecordDTO.detail,
                target = modifyRecordDTO.target,
                payment = modifyRecordDTO.payment,
                image1 = modifyRecordDTO.image1,
                image2 = modifyRecordDTO.image2,
            };
            recordRepository.UpdateRecords(recordModel);

        }

        public void DeleteRecord(ModifyRecordDTO modifyRecordDTO)
        {
            RecordModel recordModel = new RecordModel()
            {
                datetime = modifyRecordDTO.datetime,
                money = modifyRecordDTO.money,
                type = modifyRecordDTO.type,
                detail = modifyRecordDTO.detail,
                target = modifyRecordDTO.target,
                payment = modifyRecordDTO.payment,
                image1 = modifyRecordDTO.image1,
                image2 = modifyRecordDTO.image2,
            };
            recordRepository.RemoveRecords(recordModel);
        }
    }
}
