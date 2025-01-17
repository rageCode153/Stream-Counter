﻿using System;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;

namespace Stream_Counter
{
    public partial class Main : Form
    {
        bool format;
        int outSeconds;
        string timeLeftText = "Time left: ";
        string directory;

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            getDirectory();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {            
            if (outSeconds<=0)
            {
                timer1.Stop();
                enableAll();
                StreamWriter sw = new StreamWriter(directory + "streamCounterOutput.txt", false);
                sw.Write(endingTitle.Text);
                sw.Close();
            }
            else
            {
                if (format)
                    formatTime1(outSeconds);
                else
                    formatTime2(outSeconds);
                outSeconds--;
            }
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            Settings settingsForm = new Settings();
            settingsForm.ShowDialog();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            getDirectory();
            calcTime();
            timer1.Start();
            disableAll();
            stopButton.Enabled = true;
            startButton.Enabled = false;
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            enableAll();
            StreamWriter sw = new StreamWriter(directory + "\\streamCounterOutput.txt", false);
            sw.Write(endingTitle.Text);
            sw.Close();
            outSeconds = 0;
            stopButton.Enabled = false;
            startButton.Enabled = true;
        }

        private void mmRadio(object sender, EventArgs e)
        {
            hours.Enabled = false;
            hours.Value = 0;
            format = false;
            timeLeft.Text = timeLeftText + "00:00";
        }

        private void hhRadio(object sender, EventArgs e)
        {
            hours.Enabled = true;
            hours.Value = 0;
            format = true;
            timeLeft.Text = timeLeftText + "00:00:00";
        }

        private void calcTime()
        {
            int _hours = Convert.ToInt32(hours.Value);
            int _minutes = Convert.ToInt32(minutes.Value);
            int _seconds = Convert.ToInt32(seconds.Value);
            outSeconds = _hours * 3600 + _minutes * 60 + _seconds;
        }

        private void formatTime1(int _time)
        {
            StreamWriter sw = new StreamWriter(directory + "\\streamCounterOutput.txt", false);
            string _hours = (_time / 3600).ToString();
            int _remTime = _time - Convert.ToInt32(_hours) * 3600;
            string _minutes = (_remTime / 60).ToString();
            string _seconds = (_remTime - Convert.ToInt32(_minutes) * 60).ToString();

            if (Convert.ToInt32(_hours) < 10)
                _hours = "0" + _hours;
            if (Convert.ToInt32(_minutes) < 10)
                _minutes = "0" + _minutes;
            if (Convert.ToInt32(_seconds) < 10)
                _seconds = "0" + _seconds;
            string outputTime = _hours + ":" + _minutes + ":" + _seconds;
            timeLeft.Text = timeLeftText + outputTime;
            sw.Write(outputTime);
            sw.Close();
        }

        private void formatTime2(int _time)
        {
            StreamWriter sw = new StreamWriter(directory + "\\streamCounterOutput.txt", false);
            string _minutes = (_time / 60).ToString();
            string _seconds = (_time - Convert.ToInt32(_minutes) * 60).ToString();
            if (Convert.ToInt32(_minutes) < 10)
                _minutes = "0" + _minutes;
            if (Convert.ToInt32(_seconds) < 10)
                _seconds = "0" + _seconds;
            string outputTime = _minutes + ":" + _seconds;
            timeLeft.Text = timeLeftText + outputTime;
            sw.Write(outputTime);
            sw.Close();
        }

        private void disableAll()
        {
            hours.Enabled = false;
            minutes.Enabled = false;
            seconds.Enabled = false;
            hoursRadio.Enabled = false;
            minutesRadio.Enabled = false;
            endingTitle.Enabled = false;
        }

        private void enableAll()
        {
            if (format)
                hours.Enabled = true;
            else
                hours.Enabled = false;
            if (format)
                timeLeft.Text = timeLeftText + "00:00:00";
            else
                timeLeft.Text = timeLeftText + "00:00";
            minutes.Enabled = true;
            seconds.Enabled = true;
            endingTitle.Enabled = true;
            minutesRadio.Enabled = true;
            hoursRadio.Enabled = true;
            hours.Value = 0;
            minutes.Value = 0;
            seconds.Value = 0;
        }

        private void getDirectory()
        {
            Settings settingsFl = new Settings();
            var output = settingsFl.readFromFile();
            directory = output.directory;
        }
    }
}
