using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace Csharp_Arduino_DHT22_Sensor_Monitoring
{
    public partial class Form1 : Form
    {
        double temperature=0, humidity=0;
        bool updateData = false;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            button_open.Enabled = true;
            button_close.Enabled = false;

            chart1.Series["Temperature"].Points.AddXY(1, 1);
            chart1.Series["Humidity"].Points.AddXY(1, 1);
        }
        private void comboBox_portLists_DropDown(object sender, EventArgs e)
        {
            string[] portLists = SerialPort.GetPortNames();
            comboBox_portLists.Items.Clear();
            comboBox_portLists.Items.AddRange(portLists);

        }

        private void button_close_Click(object sender, EventArgs e)
        {
            try
            {

                serialPort1.Close();

                button_open.Enabled = true;
                button_close.Enabled = false;

                MessageBox.Show("Disconnected to Arduino Board");
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {

                serialPort1.Close();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string dataIn = serialPort1.ReadTo("\n");
            Data_Parsing(dataIn);
            this.BeginInvoke(new EventHandler(Show_Data));
        }
        private void Show_Data(object sender, EventArgs e)
        {
            if (updateData ==  true)
            {
                label_temperature.Text = string.Format("Temperature = {0} °C", temperature.ToString());
                label_humidity.Text = string.Format("Humidity = {0} %RH", humidity.ToString());

                chart1.Series["Temperature"].Points.Add(temperature);
                chart1.Series["Humidity"].Points.Add(humidity);
            }

        }

        private void button_open_Click_1(object sender, EventArgs e)
        {
            try
            {
                serialPort1.PortName = comboBox_portLists.Text;
                serialPort1.BaudRate = Convert.ToInt32(comboBox_baudRate.Text);
                serialPort1.Open();


                button_open.Enabled = false;
                button_close.Enabled = true;

                chart1.Series["Temperature"].Points.Clear();
                chart1.Series["Humidity"].Points.Clear();
                MessageBox.Show("Successs Connected to Arduino Board");
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void Data_Parsing(string data)
        {
            sbyte indexOf_startDataCharacter = (sbyte)data.IndexOf("@");
            sbyte indexOfA = (sbyte)data.IndexOf("A");
            sbyte indexOfB = (sbyte)data.IndexOf("B");

            // if charactes "A", "B", and "@" exist in the data Package
            if (indexOfA != -1 && indexOfB != -1 && indexOf_startDataCharacter != -1)
            {
                try
                {
                    string str_temperature = data.Substring(indexOf_startDataCharacter + 1, (indexOfA - indexOf_startDataCharacter) - 1);
                    string str_humidity = data.Substring(indexOfA + 1, (indexOfB - indexOfA) - 1);

                     temperature = Convert.ToDouble(str_temperature);
                     humidity = Convert.ToDouble(str_humidity);

                     updateData = true;
                }
                catch (Exception)
                {

                }
            }
            else
            {
                updateData = false;
            }
        }

      }
   }
