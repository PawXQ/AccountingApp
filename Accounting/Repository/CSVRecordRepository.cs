using Accounting.Attributes;
using Accounting.Repository.Entities;
using Accounting.Models;
using CSVLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Repository
{
    internal class CSVRecordRepository : IRecordRepository
    {
        string csvPath;
        string recordFile;
        public CSVRecordRepository()
        {
            this.csvPath = ConfigurationManager.AppSettings["filePath"];
            this.recordFile = "records.csv";
        }
        public List<RecordModel> GetRecords(DateTime datetime)
        {
            string directory = datetime.ToString("yyyy-MM-dd");
            string filepath = Path.Combine(this.csvPath, directory, this.recordFile);
            if (!File.Exists(filepath)) throw new FileNotFoundException();
            return CSVHelper.Read<RecordModel>(filepath);
        }

        public List<RecordModel> GetRecords(DateTime startDatetime, DateTime endDatetime)
        {
            List<RecordModel> recordsList = new List<RecordModel>();
            var diff = endDatetime - startDatetime;
            int diffDays = diff.Days;

            for (int i = 0; i < diffDays; i++)
            {
                string directory = startDatetime.AddDays(i).ToString("yyyy-MM-dd");
                string filepath = Path.Combine(this.csvPath, directory, this.recordFile);
                if (!File.Exists(filepath)) { continue; }
                recordsList.AddRange(CSVHelper.Read<RecordModel>(filepath));
            }
            if (recordsList.Count == 0) throw new Exception("No records found.");
            return recordsList;
        }

        public void CreateRecords(RecordModel recordModel)
        {
            string directory = recordModel.datetime;
            string filepath = Path.Combine(this.csvPath, directory, this.recordFile);

            CSVHelper.Write<RecordModel>(filepath, recordModel, true);
        }

        public void RemoveRecords(RecordModel recordModel)
        {
            string removeDatetime = recordModel.datetime;
            string reWriteRecordFile = Path.Combine(this.csvPath, removeDatetime, this.recordFile);
            string image1Guid = recordModel.image1;

            List<RecordModel> recordList = GetRecords(DateTime.Parse(removeDatetime));

            int recordIndex = recordList.FindIndex(x => x.image1 == image1Guid);

            recordList.RemoveAt(recordIndex);
            string directoryName = Path.GetDirectoryName(recordModel.image1.ToString());
            if (recordList.Count == 0)
            {
                Directory.Delete(Path.Combine(directoryName, recordModel.datetime), true);
                return;
            }

            var props = typeof(RecordModel).GetProperties().Where(p => p.Name.StartsWith("image"));

            foreach (var prop in props)
            {
                var imageLocation = prop.GetValue(recordModel).ToString();
                var imagePopLocation = reOrgImagePath(imageLocation);
                File.Delete(imageLocation.ToString());
                File.Delete(imagePopLocation.ToString());
            }

            File.Delete(reWriteRecordFile);
            CSVHelper.WriteList(reWriteRecordFile, recordList, true);
        }

        public void UpdateRecords(RecordModel recordModel)
        {
            string reWriteDatetime = recordModel.datetime;
            string reWriteRecordFile = Path.Combine(this.csvPath, reWriteDatetime, this.recordFile);
            string image1Guid = recordModel.image1;

            List<RecordModel> recordList = GetRecords(DateTime.Parse(reWriteDatetime));

            int recordIndex = recordList.FindIndex(x => x.image1 == image1Guid);
            recordList[recordIndex] = recordModel;

            File.Delete(reWriteRecordFile);
            CSVHelper.WriteList(reWriteRecordFile, recordList, true);
        }
        private string reOrgImagePath(string path)
        {
            string[] pathArray = path.ToString().Split('\\');
            string fileName = pathArray[pathArray.Length - 1];
            string newFileName = "pop_" + fileName;
            pathArray[pathArray.Length - 1] = newFileName;
            string newPath = string.Join("\\", pathArray);

            return newPath;
        }
    }
}
