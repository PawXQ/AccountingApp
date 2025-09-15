using Accounting.Models;
using Accounting.Repository;
using Accounting.Repository.Entities;
using Accounting.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Accounting.Contracts.AddRecordContract;

namespace Accounting.Presenter
{
    internal class AddRecordPresenter : IAddRecordPresenter
    {
        IAddRecordView addRecordView;
        IRecordRepository recordRepository;
        IFormDataRepository formDataRepository;
        string csvPath;
        string imageLocation;

        public AddRecordPresenter(IAddRecordView addRecordView)
        {
            this.csvPath = ConfigurationManager.AppSettings["filePath"];
            this.imageLocation = "_imageLocation";

            this.addRecordView = addRecordView;
            this.recordRepository = new CSVRecordRepository();

            this.formDataRepository = new FormDataRepository();
        }
        public void AddRecord(RecordDTO recordDTO)
        {
            string imageLocationFullPath = Path.Combine(this.csvPath, recordDTO.datetime.ToString(), this.imageLocation);

            if (!Directory.Exists(imageLocationFullPath))
            {
                Directory.CreateDirectory(imageLocationFullPath);
            }

            Guid g1 = Guid.NewGuid();
            Guid g2 = Guid.NewGuid();

            Bitmap image1Compress1 = ImageCompress.Compress(recordDTO.image1, 10L);
            Bitmap image1Compress2 = ImageCompress.Compress(recordDTO.image1, 40, 40);
            Bitmap image2Compress1 = ImageCompress.Compress(recordDTO.image2, 10L);
            Bitmap image2Compress2 = ImageCompress.Compress(recordDTO.image2, 40, 40);

            image1Compress1.Save($"{imageLocationFullPath}\\pop_{g1}.jpg");
            image1Compress2.Save($"{imageLocationFullPath}\\{g1}.jpg");
            image2Compress1.Save($"{imageLocationFullPath}\\pop_{g2}.jpg");
            image2Compress2.Save($"{imageLocationFullPath}\\{g2}.jpg");

            RecordModel recordModel = new RecordModel()
            {
                datetime = recordDTO.datetime,
                money = recordDTO.money,
                type = recordDTO.type,
                detail = recordDTO.detail,
                target = recordDTO.target,
                payment = recordDTO.payment,
                image1 = Path.Combine(imageLocationFullPath, $"{g1}.jpg"),
                image2 = Path.Combine(imageLocationFullPath, $"{g2}.jpg"),
            };

            recordRepository.CreateRecords(recordModel);
        }

        public void GetComboBoxDataList()
        {
            List<string> typeList = formDataRepository.GetTypeList();
            List<string> detailList = formDataRepository.GetDetailList(typeList[0]);
            List<string> targetList = formDataRepository.GetTargetList();
            List<string> paymentList = formDataRepository.GetPaymentList();
            FormDataOptionDTO formDataOptionDTO = new FormDataOptionDTO()
            {
                typeList = typeList,
                detailList = detailList,
                targetList = targetList,
                paymentList = paymentList
            };
            addRecordView.RenderCombox(formDataOptionDTO);
        }

        public void GetTypeDetailList(string typeName)
        {
            List<string> detailList = formDataRepository.GetDetailList(typeName);
            addRecordView.RenderDetailCombox(detailList);
        }
    }
}
