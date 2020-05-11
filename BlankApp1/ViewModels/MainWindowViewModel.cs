using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace BlankApp1.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "ラズパイモニター";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        private int _temperature;
        private int _humidity;
        private int _pressure;
        public int Temperature 
        { 
            get { return _temperature; } 
            set { SetProperty(ref _temperature, value); }
        }
        public int Humidity
        {
            get { return _humidity; }
            set { SetProperty(ref _humidity, value); }
        }
        public int Pressure
        {
            get { return _pressure; }
            set { SetProperty(ref _pressure, value); }
        }

        public DelegateCommand ButtonClickCommand { get; }

        public MainWindowViewModel()
        {
            ButtonClickCommand = new DelegateCommand(() =>
            {
                Thread thread = new Thread(new ThreadStart(() => {
                    try
                    {
                        HttpListener listener = new HttpListener();
                        listener.Prefixes.Add("http://192.168.1.3:8000/");
                        listener.Start();
                        while (true)
                        {
                            HttpListenerContext context = listener.GetContext();
                            HttpListenerRequest req = context.Request;
                            using (StreamReader reader = new StreamReader(req.InputStream, req.ContentEncoding))
                            {
                                string s = reader.ReadToEnd();
                                Sensor sensor = JsonConvert.DeserializeObject<Sensor>(s);
                                Temperature = sensor.Temperature;
                                Humidity = sensor.Humidity;
                                Pressure = sensor.Pressure;
                            }
                            HttpListenerResponse res = context.Response;
                            res.StatusCode = 200;
                            byte[] content = Encoding.UTF8.GetBytes("HELLO");
                            res.OutputStream.Write(content, 0, content.Length);
                            res.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }));

                thread.Start();

            });
        }
    }
}
