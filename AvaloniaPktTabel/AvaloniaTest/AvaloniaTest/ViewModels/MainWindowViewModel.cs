using System;
using System.Collections.ObjectModel;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using AvaloniaTest.Models;
using CsvHelper;

namespace AvaloniaTest.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ObservableCollection<MessageDataModel> _messageDataModel = new ObservableCollection<MessageDataModel>();


        private static readonly Random _random = new Random();
        private static readonly string[] MsgTypes = { "AUX", "DP", "MAIN" };
        private static readonly string[] PowerRoles = { "DUT", "Source", "Sink", "Provider", "Consumer" };

        public MainWindowViewModel()
        {
            // Load data from CSV file
            //LoadDataFromCsv(@"D:/QTRESTAPITest/AvaloniaTest/AvaloniaTest/Assets/Data.csv");

            ObservableCollection<MessageDataModel> _messageDataModel= GenerateRandomData(1000000);

            // Initialize the data source
            Source = new FlatTreeDataGridSource<MessageDataModel>(_messageDataModel)
            {
                Columns =
                {
                    new TextColumn<MessageDataModel, int>("MsgIndex", x => x.MsgIndex),
                    new TextColumn<MessageDataModel, string>("MsgType", x => x.MsgType),
                    new TextColumn<MessageDataModel, double>("MsgStartTime", x => x.MsgStartTime),
                    new TextColumn<MessageDataModel, double>("MsgStopTime", x => x.MsgStopTime),
                    new TextColumn<MessageDataModel, string>("MsgPowerRole", x => x.MsgPowerRole),
                }
            };
        }

        public FlatTreeDataGridSource<MessageDataModel> Source { get; }

        private void LoadDataFromCsv(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("CSV file not found.", filePath);
            }

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                // Register the class map for MessageDataModel
                csv.Context.RegisterClassMap<MessageDataModelMap>();

                // Read records from the CSV file
                var records = csv.GetRecords<MessageDataModel>();

                // Add records to the ObservableCollection
                foreach (var record in records)
                {
                    _messageDataModel.Add(record);
                }
            }
        }

        public static ObservableCollection<MessageDataModel> GenerateRandomData(int rowCount)
        {
            var data = new ObservableCollection<MessageDataModel>();

            for (int i = 0; i < rowCount; i++)
            {
                var msgindex = i;
                var msgType = MsgTypes[_random.Next(MsgTypes.Length)];
                var startTime = Math.Round(3.0 + _random.NextDouble(), 3); // Random start time between 3.000 and 4.000
                var stopTime = Math.Round(startTime + _random.NextDouble() * 0.5, 3); // Random stop time within 0.5 seconds of start time
                var powerRole = PowerRoles[_random.Next(PowerRoles.Length)];

                data.Add(new MessageDataModel
                {
                    MsgIndex = msgindex,
                    MsgType = msgType,
                    MsgStartTime = startTime,
                    MsgStopTime = stopTime,
                    MsgPowerRole = powerRole
                });
            }

            return data;
        }

    }


    // Class map for MessageDataModel to map CSV columns to properties
    public sealed class MessageDataModelMap : CsvHelper.Configuration.ClassMap<MessageDataModel>
    {
        public MessageDataModelMap()
        {
            Map(m => m.MsgType).Name("MsgType");
            Map(m => m.MsgStartTime).Name("MsgStartTime");
            Map(m => m.MsgStopTime).Name("MsgStopTime");
            Map(m => m.MsgPowerRole).Name("MsgPowerRole");
        }
    }
}