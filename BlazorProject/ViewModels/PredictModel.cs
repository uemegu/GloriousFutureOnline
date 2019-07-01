using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.JSInterop;

namespace BlazorProject.ViewModels
{
    public class PredictModel
    {
        public event EventHandler PropertyChanged;
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;
        
        private readonly List<string> ALPHABET = new List<string>()
        {
            "A","B","C","D","E","F","G","H","I","J","K","L","M","N",
            "O","P","Q","R","S","T","U","V","W","X","Y","Z"
        };
        public History Word { get; set; } = new History();
        public string ButtonText { get; private set; }
        private PredictDataDetail _data;
        private bool _isRunning = false;
        private Timer _timer;

        public PredictModel(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            ButtonText = "予言開始";
            this._httpClient = httpClient;
            this._jsRuntime = jsRuntime;
        }

        public async Task Load() {
            var json = await this._httpClient.GetStringAsync("./PredictData.json");
            this._data = JsonConvert.DeserializeObject<PredictData>(json).Future;
        }

        public void Tap()
        {
            if (_isRunning)
            {
                ButtonText = "予言開始";
                Stop();
            }
            else
            {
                ButtonText = "予言決定";
                Start();
            }
        }

        private void Start()
        {
            _isRunning = true;
            _timer = new Timer((obj) => {
                var random = new Random(DateTime.Now.Millisecond + (int)DateTime.Now.Ticks);
                Word.Word1 = _data.First[random.Next(_data.First.Count)];
                if (Word.Word1 == "氏が")
                {
                    Word.Word1 = ALPHABET[random.Next(ALPHABET.Count)] + Word.Word1;
                }
                Word.Year = DateTime.Now.AddYears(random.Next(3)).Year.ToString("00");
                Word.Month = DateTime.Now.AddMonths(random.Next(12)).Month.ToString("00");
                Word.Date = DateTime.Now.AddDays(random.Next(30)).Day.ToString("00");
                Word.Word2 = _data.Second[random.Next(_data.Second.Count)];
                Word.Word3 = _data.Third[random.Next(_data.Third.Count)];
                Word.Word4 = _data.Forth[random.Next(_data.Forth.Count)];
                Word.Word5 = _data.Fifth[random.Next(_data.Fifth.Count)];
                Word.Word6 = _data.Sixth[random.Next(_data.Sixth.Count)];
                if(PropertyChanged != null) PropertyChanged.Invoke(this, EventArgs.Empty);
            }, null, 0, 50);
        }

        private void Stop()
        {
            _isRunning = false;
            _timer.Dispose();
            _timer = null;
            _jsRuntime.InvokeAsync<object>("notifyPredict", Word.Day, Word.ToString());
        }

        [JSInvokable]
        public void SetWord(string word) {
            var words = word.Split(' ');
            Word.Year = words[0].Substring(0, 4);
            Word.Month = words[0].Substring(5, 2);
            Word.Date = words[0].Substring(8, 2);
            Word.Word1 = words[1];
            if(PropertyChanged != null) PropertyChanged.Invoke(this, EventArgs.Empty);
        }

    }

    public class PredictData
    {
        public PredictDataDetail Future = new PredictDataDetail();
    }

    public class PredictDataDetail
    {
        public List<string> First = new List<string>();
        public List<string> Second = new List<string>();
        public List<string> Third = new List<string>();
        public List<string> Forth = new List<string>();
        public List<string> Fifth = new List<string>();
        public List<string> Sixth = new List<string>();
    }

    public class History
    {
        public string Word1 { get; set; } = " ";
        public string Word2 { get; set; } = " ";
        public string Word3 { get; set; } = " ";
        public string Word4 { get; set; } = " ";
        public string Word5 { get; set; } = " ";
        public string Word6 { get; set; } = " ";
        public string Year { get; set; }
        public string Month { get; set; }
        public string Date { get; set; }
        public string Day => string.IsNullOrEmpty(Year) ? "　" : $"{Year}年{Month}月{Date}日";

        public override string ToString()
        {
            return $"{Word1}{Word2}{Word3}{Word4}{Word5}{Word6}";
        }
    }
}
