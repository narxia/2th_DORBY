using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NextUI.ViewModels.Machine
{
    public class SensorModel : BaseViewModel
    {
        private int _no;
        public int No
        {
            get => _no;
            set
            {
                _no = value;
                OnPropertyChanged();
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private bool _value;
        public bool Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged();
            }
        }
        public SensorModel()
        {
            _no = 0;
            _name = string.Empty;
            _value = false;
        }
            


    }
    public class SensorInfo :BaseViewModel
    {
        private int _channel;
        public int Channel
        {
            get => _channel;
            set
            {
                _channel = value;
                OnPropertyChanged();
            }
        }
        private string _channelName;
        public string ChannelName
        {
            get => _channelName;
            set
            {
                _channelName = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<SensorModel> Sensors { get; set; }
        public SensorInfo()
        {
            Sensors = new ObservableCollection<SensorModel>(); 
        }
        
    }
    public class IOViewModel : BaseViewModel
    {
        private int _currentInputChannel;
        public int CurrentInputChannel
        {
            get => _currentInputChannel;
            set
            {
                _currentInputChannel = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested(); // ✅ 버튼 상태 갱신
                OnPropertyChanged(nameof(CurrentInputSensor));
            }
        }
        private int _currentOutputChannel;
        public int CurrentOutputChannel
        {
            get => _currentOutputChannel;
            set
            {
                _currentOutputChannel = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested(); // ✅ 버튼 상태 갱신
                OnPropertyChanged(nameof(CurrentOutputSensor));


            }
        }
        public ICommand NextInputCommand { get; }
        public ICommand PrevInputCommand { get; }
        public ICommand NextOutputCommand { get; }
        public ICommand PrevOutputCommand { get; }
        public ObservableCollection<SensorInfo> InputSensors { get; set; }
        public ObservableCollection<SensorInfo> OutputSensors { get; set; }
        public IOViewModel()
        {

            InputSensors = new ObservableCollection<SensorInfo>();
            OutputSensors = new ObservableCollection<SensorInfo>();
            LoadConfig();
            SaveConfig();
            NextInputCommand = new RelayCommand(
                _ => CurrentInputChannel += 1,
                _ => CurrentInputChannel < InputSensors.Count - 1
            );
            PrevInputCommand = new RelayCommand(
                _ => CurrentInputChannel -= 1,
                _ => CurrentInputChannel > 0
            );
            NextOutputCommand = new RelayCommand(
                _ => CurrentOutputChannel += 1,
                _ => CurrentOutputChannel < OutputSensors.Count - 1
            );
            PrevOutputCommand = new RelayCommand(
                _ => CurrentOutputChannel -= 1,
                _ => CurrentOutputChannel > 0
            );
        }
        private void LoadConfig()
        {
            string baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config");

            // 🔧 디렉터리 존재 여부 확인
            if (!Directory.Exists(baseDir))
                Directory.CreateDirectory(baseDir); // 필요 시 생성

            string inputFile = Path.Combine(baseDir, "InputIOCfg.json");

            // 🔧 파일 존재 여부 확인
            if (!File.Exists(inputFile)) return;

            string Injson = File.ReadAllText(inputFile);
            var InsensorList = JsonSerializer.Deserialize<List<SensorInfo>>(Injson);

            // 명시적 변환
            InputSensors = new ObservableCollection<SensorInfo>(InsensorList);

            string OutputFile = Path.Combine(baseDir, "OutputIOCfg.json");

            // 🔧 파일 존재 여부 확인
            if (!File.Exists(OutputFile)) return;

            string Outjson = File.ReadAllText(OutputFile);
            var OutsensorList = JsonSerializer.Deserialize<List<SensorInfo>>(Outjson);

            // 명시적 변환
            OutputSensors = new ObservableCollection<SensorInfo>(OutsensorList);
        }
        private void SaveConfig()
        {
            string baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config");

            // 🔧 디렉터리 존재 여부 확인
            if (!Directory.Exists(baseDir))
                Directory.CreateDirectory(baseDir); // 필요 시 생성

            string inputFile = Path.Combine(baseDir, "InputIOCfg.json");


            var InputSensorList = InputSensors.ToList(); // ObservableCollection → List 변환
            var Inputjson = JsonSerializer.Serialize(InputSensorList, new JsonSerializerOptions
            {
                WriteIndented = true // 보기 좋게 들여쓰기
            });

            File.WriteAllText(inputFile, Inputjson);
            string outputFile = Path.Combine(baseDir, "OutputIOCfg.json");


            var OutputSensorList = OutputSensors.ToList(); // ObservableCollection → List 변환
            var Outputjson = JsonSerializer.Serialize(OutputSensorList, new JsonSerializerOptions
            {
                WriteIndented = true // 보기 좋게 들여쓰기
            });

            File.WriteAllText(outputFile, Outputjson);
        }
        public SensorInfo CurrentInputSensor
        {
            get => InputSensors.ElementAtOrDefault(CurrentInputChannel);
        }
        public SensorInfo CurrentOutputSensor
        {
            get => OutputSensors.ElementAtOrDefault(CurrentOutputChannel);
        }
    }
}
