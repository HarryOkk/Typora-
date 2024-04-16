using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;

namespace BA
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            radioButton12.Invoke(new Action(() => radioButton12.BackColor = Color.Red));
            radioButton21.Invoke(new Action(() => radioButton21.BackColor = Color.Red));
            radioButton40.Invoke(new Action(() => radioButton40.BackColor = Color.Red));
            radioButton48.Invoke(new Action(() => radioButton48.BackColor = Color.Red));
            textBox6.Text = "1"+"h";
            textBox5.Text = "1" ;
            textBox4.Text = "1" ;
            textBox3.Text = "1" ;
            textBox2.Text = "1" ;
            textBox7.Text = "1";
            textBox8.Text = "1" ;
            textBox9.Text = "1" ;

            serial_port_init();//初始化串口
            
        }
        private void serial_port_init()
        {
            try
            {
                string[] str = SerialPort.GetPortNames();   //获取连接到电脑的串口号并存进数组
                comboBoxPort.Items.Clear();           //清除串口号下拉框的内容
                comboBoxPort.Items.AddRange(str);     //将串口号添加到下拉框

                if (str.Length > 0)
                {
                    comboBoxPort.SelectedIndex = 1;   //设置ComboBox框的初始值
                    comboBoxBauds.SelectedIndex = 0;
                    comboBoxDataBit.SelectedIndex = 3;
                    comboBoxStopBit.SelectedIndex = 2;
                    comboBoxParity.SelectedIndex = 1;
                }
                else
                {
                    MessageBox.Show("当前无串口连接！");
                }
            }
            catch
            {
                MessageBox.Show("无串口设备!/r/n请检查是否连接设备!/r/n请检查设备驱动!");
            }
        }
        private void serial_port_set()
        {
            if (!serialPort.IsOpen)
            {
                if (comboBoxPort.SelectedItem == null)
                {
                    MessageBox.Show("请选择正确的串口", "提示");
                    return;
                }

                //设置串口参数
                serialPort.PortName = comboBoxPort.Text.ToString();  //serialPort1是serialPort组件的Name 
                serialPort.BaudRate = Convert.ToInt32(comboBoxBauds.SelectedItem.ToString());
                serialPort.DataBits = Convert.ToInt32(comboBoxDataBit.SelectedItem.ToString());
                serialPort.StopBits = StopBits.One;
                serialPort.Parity = Parity.Even;
                //设置停止位
                //if (comboBoxStopBit.Text == "One")
                //{
                //    serialPort.StopBits = StopBits.One;
                //}
                //else if (comboBoxStopBit.Text == "Two")
                //{
                //    serialPort.StopBits = StopBits.Two;
                //}
                //else if (comboBoxStopBit.Text == "None")
                //{
                //    serialPort.StopBits = StopBits.None;
                //}

                //设置奇偶校验位
                //if (comboBoxParity.Text == "Odd")
                //{
                //    serialPort.Parity = Parity.Odd;
                //}
                //else if (comboBoxParity.Text == "Even")
                //{
                //    serialPort.Parity = Parity.Even;
                //}
                //else if (comboBoxParity.Text == "None")
                //{
                //    serialPort.Parity = Parity.None;
                //}
            }
        }
        private void ButtonStart_Click(object sender, EventArgs e)
        {
            if (ButtonStart.Text == "发送查询")
            {
                ModBusSendData();
                serialPort.DataReceived += new SerialDataReceivedEventHandler(ModbusDataReceive);
                ButtonStart.Text = "关闭查询";
            }
            else
            {
                ButtonStart.Text = "发送查询";
            }
        }
        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            try
            {
                if (bytes != null)
                {
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        returnStr += bytes[i].ToString("X2");
                        returnStr += " ";//两个16进制用空格隔开,方便看数据
                    }
                }
                return returnStr;
            }
            catch (Exception)
            {
                return returnStr;
            }
        }
        private void ModBusSendData()
        { 
            if (serialPort.IsOpen)
            {
                byte[] buf = new byte[8] { 0x02, 0x03, 0x00, 0x40, 0x00, 0x17, 0x04, 0x23 };

 
                try
                {
                    serialPort.Write(buf, 0, 8);    //写数据
                    string str = Encoding.Default.GetString(buf);//Byte值根据ASCII码表转为 String
                    datashowbox.AppendText("发：" + byteToHexStr(buf) + "\r\n");
      
                }
                catch
                {
                    MessageBox.Show("发送失败！！");
                }
            }
            else
            {
                MessageBox.Show("串口未打开！");
            }

        }
        private void ModbusDataReceive(object sender, SerialDataReceivedEventArgs e)
        {
            if (serialPort.IsOpen == false)
            {
                serialPort.Close();
                return;
            }
             
            int len = serialPort.BytesToRead;//获取可以读取的字节数
            if (len >= 48)
            {
                byte[] buff = new byte[len];//创建缓存数据数组
                serialPort.Read(buff, 0, len);//把数据读取到buff数组
                string str = Encoding.Default.GetString(buff);//Byte值根据ASCII码表转为 String
                datashowbox.Invoke(new Action(() => datashowbox.AppendText("收：" + byteToHexStr(buff) + "\r\n")));//从不是创建控件“datashowbox”的线程访问它时，要用invoke
                modbushandledata(buff);
            }

        }
        private void modbushandledata(byte[] buff)
        {
            
            string hexValue = Convert.ToString(buff[4]); // 十六进制数65L
            string binaryString = Convert.ToString(Convert.ToInt32(hexValue, 16), 2).PadLeft(8, '0'); // 转换为二进制字符串并补齐位数
            if (binaryString[5] == 1)//综合故障
            {
                radioButton12.Invoke(new Action(() => radioButton12.BackColor = Color.Red));
            }
            else
            {
                radioButton12.Invoke(new Action(() => radioButton12.BackColor = SystemColors.Control));
            }
            hexValue = Convert.ToString(buff[5]); // 十六进制数66H
            binaryString = Convert.ToString(Convert.ToInt32(hexValue, 16), 2).PadLeft(8, '0'); // 转换为二进制字符串并补齐位数
            if (binaryString[7] == 1)//安全回路断路
            {
                radioButton11.Invoke(new Action(() => radioButton11.BackColor = Color.Red));
            }
            else
            {
                radioButton11.Invoke(new Action(() => radioButton11.BackColor = SystemColors.Control));
            }
            hexValue = Convert.ToString(buff[7]); // 十六进制数67H
            binaryString = Convert.ToString(Convert.ToInt32(hexValue, 16), 2).PadLeft(8, '0'); // 转换为二进制字符串并补齐位数
            if (binaryString[0] == 1)//关门故障
            {
                radioButton10.Invoke(new Action(() => radioButton10.BackColor = Color.Red));
            }
            else
            {
                radioButton10.Invoke(new Action(() => radioButton10.BackColor = SystemColors.Control));
            }
            if (binaryString[1] == 1)//门区外停梯
            {
                radioButton9.Invoke(new Action(() => radioButton9.BackColor = Color.Red));
            }
            else
            {
                radioButton9.Invoke(new Action(() => radioButton9.BackColor = SystemColors.Control));
            }
            if (binaryString[2] == 1)//门轿厢意外移动 
            {
                radioButton8.Invoke(new Action(() => radioButton8.BackColor = Color.Red));
            }
            else
            {
                radioButton8.Invoke(new Action(() => radioButton8.BackColor = SystemColors.Control));
            }
            if (binaryString[3] == 1)//运行时间限制器动作
            {
                radioButton7.Invoke(new Action(() => radioButton7.BackColor = Color.Red));
            }
            else
            {
                radioButton7.Invoke(new Action(() => radioButton7.BackColor = SystemColors.Control));
            }
            if (binaryString[4] == 1)//楼层位置丢失
            {
                radioButton13.Invoke(new Action(() => radioButton13.BackColor = Color.Red));
            }
            else
            {
                radioButton13.Invoke(new Action(() => radioButton13.BackColor = SystemColors.Control));
            }
            if (binaryString[5] == 1)//开门故障
            {
                radioButton20.Invoke(new Action(() => radioButton20.BackColor = Color.Red));
            }
            else
            {
                radioButton20.Invoke(new Action(() => radioButton20.BackColor = SystemColors.Control));
            }
            if (binaryString[6] == 1)//防止电梯再运行故障
            {
                radioButton19.Invoke(new Action(() => radioButton19.BackColor = Color.Red));
            }
            else
            {
                radioButton19.Invoke(new Action(() => radioButton19.BackColor = SystemColors.Control));
            }
            hexValue = Convert.ToString(buff[9]); // 十六进制数68H
            binaryString = Convert.ToString(Convert.ToInt32(hexValue, 16), 2).PadLeft(8, '0'); // 转换为二进制字符串并补齐位数
            if (binaryString[7] == 1)//其它故障
            {
                radioButton17.Invoke(new Action(() => radioButton17.BackColor = Color.Red));
            }
            else
            {
                radioButton17.Invoke(new Action(() => radioButton17.BackColor = SystemColors.Control));
            }
            hexValue = Convert.ToString(buff[11]); // 十六进制数69H
            binaryString = Convert.ToString(Convert.ToInt32(hexValue, 16), 2).PadLeft(8, '0'); // 转换为二进制字符串并补齐位数
            if (binaryString[0] == 1)//电梯困人
            {
                radioButton16.Invoke(new Action(() => radioButton16.BackColor = Color.Red));
            }
            else
            {
                radioButton16.Invoke(new Action(() => radioButton16.BackColor = SystemColors.Control));
            }
            if (binaryString[1] == 1)//轿厢在门锁区域外停止
            {
                radioButton15.Invoke(new Action(() => radioButton15.BackColor = Color.Red));
            }
            else
            {
                radioButton15.Invoke(new Action(() => radioButton15.BackColor = SystemColors.Control));
            }
            if (binaryString[2] == 1)//非平层停梯
            {
                radioButton27.Invoke(new Action(() => radioButton27.BackColor = Color.Red));
            }
            else
            {
                radioButton27.Invoke(new Action(() => radioButton27.BackColor = SystemColors.Control));
            }
            if (binaryString[3] == 1)//防止电梯再运行故障
            {
                radioButton26.Invoke(new Action(() => radioButton26.BackColor = Color.Red));
            }
            else
            {
                radioButton26.Invoke(new Action(() => radioButton26.BackColor = SystemColors.Control));
            }
            if (binaryString[4] == 1)//启动失败
            {
                radioButton25.Invoke(new Action(() => radioButton25.BackColor = Color.Red));
            }
            else
            {
                radioButton25.Invoke(new Action(() => radioButton25.BackColor = SystemColors.Control));
            }
            if (binaryString[5] == 1)//安全回路断路
            {
                radioButton24.Invoke(new Action(() => radioButton24.BackColor = Color.Red));
            }
            else
            {
                radioButton24.Invoke(new Action(() => radioButton24.BackColor = SystemColors.Control));
            }
            hexValue = Convert.ToString(buff[12]); // 十六进制数69L
            binaryString = Convert.ToString(Convert.ToInt32(hexValue, 16), 2).PadLeft(8, '0'); // 转换为二进制字符串并补齐位数
            if (binaryString[1] == 1)//电动机运转时间限制器动作
            {
                radioButton23.Invoke(new Action(() => radioButton23.BackColor = Color.Red));
            }
            else
            {
                radioButton23.Invoke(new Action(() => radioButton23.BackColor = SystemColors.Control));
            }
            if (binaryString[2] == 1)//运行超时
            {
                radioButton21.Invoke(new Action(() => radioButton21.BackColor = Color.Red));
            }
            else
            {
                radioButton21.Invoke(new Action(() => radioButton21.BackColor = SystemColors.Control));
            }
            if (binaryString[4] == 1)//通讯故障(万科)
            {
                radioButton18.Invoke(new Action(() => radioButton18.BackColor = Color.Red));
            }
            else
            {
                radioButton18.Invoke(new Action(() => radioButton18.BackColor = SystemColors.Control));
            }
            if (binaryString[5] == 1)//上极限动作(万科)
            {
                radioButton41.Invoke(new Action(() => radioButton41.BackColor = Color.Red));
            }
            else
            {
                radioButton41.Invoke(new Action(() => radioButton41.BackColor = SystemColors.Control));
            }
            if (binaryString[6] == 1)//轿厢冲顶
            {
                radioButton40.Invoke(new Action(() => radioButton40.BackColor = Color.Red));
            }
            else
            {
                radioButton40.Invoke(new Action(() => radioButton40.BackColor = SystemColors.Control));
            }
            if (binaryString[7] == 1)//上限位信号异常
            {
                radioButton39.Invoke(new Action(() => radioButton39.BackColor = Color.Red));
            }
            else
            {
                radioButton39.Invoke(new Action(() => radioButton39.BackColor = SystemColors.Control));
            }
            hexValue = Convert.ToString(buff[13]); // 十六进制数70H
            binaryString = Convert.ToString(Convert.ToInt32(hexValue, 16), 2).PadLeft(8, '0'); // 转换为二进制字符串并补齐位数
            if (binaryString[0] == 1)//下极限动作(万科)
            {
                radioButton38.Invoke(new Action(() => radioButton38.BackColor = Color.Red));
            }
            else
            {
                radioButton38.Invoke(new Action(() => radioButton38.BackColor = SystemColors.Control));
            }
            if (binaryString[1] == 1)//轿厢蹲底
            {
                radioButton37.Invoke(new Action(() => radioButton37.BackColor = Color.Red));
            }
            else
            {
                radioButton37.Invoke(new Action(() => radioButton37.BackColor = SystemColors.Control));
            }
            if (binaryString[2] == 1)//下限位信号异常
            {
                radioButton36.Invoke(new Action(() => radioButton36.BackColor = Color.Red));
            }
            else
            {
                radioButton36.Invoke(new Action(() => radioButton36.BackColor = SystemColors.Control));
            }
            if (binaryString[3] == 1)//开门走梯
            {
                radioButton35.Invoke(new Action(() => radioButton35.BackColor = Color.Red));
            }
            else
            {
                radioButton35.Invoke(new Action(() => radioButton35.BackColor = SystemColors.Control));
            }
            if (binaryString[4] == 1)//运行中开门
            {
                radioButton34.Invoke(new Action(() => radioButton34.BackColor = Color.Red));
            }
            else
            {
                radioButton34.Invoke(new Action(() => radioButton34.BackColor = SystemColors.Control));
            }
            if (binaryString[5] == 1)//超速
            {
                radioButton33.Invoke(new Action(() => radioButton33.BackColor = Color.Red));
            }
            else
            {
                radioButton33.Invoke(new Action(() => radioButton33.BackColor = SystemColors.Control));
            }
            if (binaryString[6] == 1)//速度异常
            {
                radioButton32.Invoke(new Action(() => radioButton32.BackColor = Color.Red));
            }
            else
            {
                radioButton32.Invoke(new Action(() => radioButton32.BackColor = SystemColors.Control));
            }
            if (binaryString[7] == 1)//电梯制动系统故障
            {
                radioButton31.Invoke(new Action(() => radioButton31.BackColor = Color.Red));
            }
            else
            {
                radioButton31.Invoke(new Action(() => radioButton31.BackColor = SystemColors.Control));
            }
            hexValue = Convert.ToString(buff[14]); // 十六进制数70L
            binaryString = Convert.ToString(Convert.ToInt32(hexValue, 16), 2).PadLeft(8, '0'); // 转换为二进制字符串并补齐位数
            if (binaryString[0] == 1)//电梯控制装置故障
            {
                radioButton29.Invoke(new Action(() => radioButton29.BackColor = Color.Red));
            }
            else
            {
                radioButton29.Invoke(new Action(() => radioButton29.BackColor = SystemColors.Control));
            }
            if (binaryString[1] == 1)//驱动系统故障
            {
                radioButton30.Invoke(new Action(() => radioButton30.BackColor = Color.Red));
            }
            else
            {
                radioButton30.Invoke(new Action(() => radioButton30.BackColor = SystemColors.Control));
            }
            if (binaryString[2] == 1)//电梯曳引机故障
            {
                radioButton28.Invoke(new Action(() => radioButton28.BackColor = Color.Red));
            }
            else
            {
                radioButton28.Invoke(new Action(() => radioButton28.BackColor = SystemColors.Control));
            }
            if (binaryString[3] == 1)//主电源故障
            {
                radioButton55.Invoke(new Action(() => radioButton55.BackColor = Color.Red));
            }
            else
            {
                radioButton55.Invoke(new Action(() => radioButton55.BackColor = SystemColors.Control));
            }
            if (binaryString[4] == 1)//电梯平层故障
            {
                radioButton54.Invoke(new Action(() => radioButton54.BackColor = Color.Red));
            }
            else
            {
                radioButton54.Invoke(new Action(() => radioButton54.BackColor = SystemColors.Control));
            }
            if (binaryString[5] == 1)//变频器故障
            {
                radioButton53.Invoke(new Action(() => radioButton53.BackColor = Color.Red));
            }
            else
            {
                radioButton53.Invoke(new Action(() => radioButton53.BackColor = SystemColors.Control));
            }
            if (binaryString[6] == 1)//报警按钮动作
            {
                radioButton52.Invoke(new Action(() => radioButton52.BackColor = Color.Red));
            }
            else
            {
                radioButton52.Invoke(new Action(() => radioButton52.BackColor = SystemColors.Control));
            }
            if (binaryString[7] == 1)//轿厢按钮报警
            {
                radioButton51.Invoke(new Action(() => radioButton51.BackColor = Color.Red));
            }
            else
            {
                radioButton51.Invoke(new Action(() => radioButton51.BackColor = SystemColors.Control));
            }
            hexValue = Convert.ToString(buff[15]); // 十六进制数71H
            binaryString = Convert.ToString(Convert.ToInt32(hexValue, 16), 2).PadLeft(8, '0'); // 转换为二进制字符串并补齐位数
            if (binaryString[0] == 1)//电梯停电
            {
                radioButton50.Invoke(new Action(() => radioButton50.BackColor = Color.Red));
            }
            else
            {
                radioButton50.Invoke(new Action(() => radioButton50.BackColor = SystemColors.Control));
            }
           
            if (binaryString[3] == 1)//过流故障(万科)
            {
                radioButton48.Invoke(new Action(() => radioButton48.BackColor = Color.Red));
            }
            else
            {
                radioButton48.Invoke(new Action(() => radioButton48.BackColor = SystemColors.Control));
            }
            if (binaryString[4] == 1)//欠压或者过压故障(万科)
            {
                radioButton47.Invoke(new Action(() => radioButton47.BackColor = Color.Red));
            }
            else
            {
                radioButton47.Invoke(new Action(() => radioButton47.BackColor = SystemColors.Control));
            }
            if (binaryString[5] == 1)//高温故障(万科)
            {
                radioButton46.Invoke(new Action(() => radioButton46.BackColor = Color.Red));
            }
            else
            {
                radioButton46.Invoke(new Action(() => radioButton46.BackColor = SystemColors.Control));
            }
            if (binaryString[6] == 1)//通信故障
            {
                radioButton45.Invoke(new Action(() => radioButton45.BackColor = Color.Red));
            }
            else
            {
                radioButton45.Invoke(new Action(() => radioButton45.BackColor = SystemColors.Control));
            }
            if (binaryString[7] == 1)//门锁短接故障
            {
                radioButton43.Invoke(new Action(() => radioButton43.BackColor = Color.Red));
            }
            else
            {
                radioButton43.Invoke(new Action(() => radioButton43.BackColor = SystemColors.Control));
            }
            hexValue = Convert.ToString(buff[17] << 24 + buff[18] << 24 + buff[19] << 24 + buff[20] << 24); // 十六进制数72H+72L+73H+73L,累计运行小时数（h）6
            textBox6.Text = hexValue;
            hexValue = Convert.ToString(buff[21] << 24 + buff[22] << 24 + buff[23] << 24 + buff[24] << 24); // 十六进制数72H+72L+73H+73L,电梯启动次数5
            textBox5.Text = hexValue;
            hexValue = Convert.ToString(buff[25] << 24 + buff[26] << 24 + buff[27] << 24 + buff[28] << 24); // 十六进制数72H+72L+73H+73L,电梯运行距离（m）3
            textBox3.Text = hexValue;
            hexValue = Convert.ToString(buff[29] << 24 + buff[30] << 24 + buff[31] << 24 + buff[32] << 24); // 十六进制数72H+72L+73H+73L,轿门开关门次数4
            textBox4.Text = hexValue;
            hexValue = Convert.ToString(buff[33] << 24 + buff[34] << 24 + buff[35] << 24 + buff[36] << 24); // 十六进制数72H+72L+73H+73L,累计用电量（0.1kwh）2
            textBox2.Text = hexValue;
            hexValue = Convert.ToString(buff[37] << 24 + buff[38] << 24 + buff[39] << 24 + buff[40] << 24); // 十六进制数72H+72L+73H+73L,累计节能量（0.1kwh）8）
            textBox8.Text = hexValue;
            hexValue = Convert.ToString(buff[41] << 24 + buff[42] << 24 + buff[43] << 24 + buff[44] << 24); // 十六进制数72H+72L+73H+73L,乘梯人数9
            textBox9.Text = hexValue;
            hexValue = Convert.ToString(buff[45] << 24 + buff[46] << 24 + buff[47] << 24 + buff[48] << 24); // 十六进制数72H+72L+73H+73L,电梯实时速度）7
            textBox7.Text = hexValue;







        }
        private void portopen_Click(object sender, EventArgs e)
        {
            if (portopen.Text == "打开串口")
            {
                serial_port_set();
                try
                {
                    //禁止操作组件
                    comboBoxPort.Enabled = false;
                    comboBoxBauds.Enabled = false;
                    comboBoxDataBit.Enabled = false;
                    comboBoxStopBit.Enabled = false;
                    comboBoxParity.Enabled = false;
                    

                    serialPort.Open(); //设置完参数后打开串口
                    portopen.Text = "关闭串口";

                }
                catch
                {
                    MessageBox.Show("串口打开失败！");
                }
            }
            else
            { 
                
                try
                {
                    comboBoxPort.Enabled = true;
                    comboBoxBauds.Enabled = true;
                    comboBoxDataBit.Enabled = true;
                    comboBoxStopBit.Enabled = true;
                    comboBoxParity.Enabled = true;
                  
                    serialPort.Close();//关闭串口
                }
                catch (Exception) { }
                portopen.Text = "打开串口";//按钮显示打开

            }
        }

        private void clearbox_Click(object sender, EventArgs e)
        {
            datashowbox.Clear();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            
        }






      


    }
}
