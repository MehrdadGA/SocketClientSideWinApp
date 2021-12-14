using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//-----------------------------------------------
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MulClient
{
    public partial class Form1 : Form
    {

        public Socket socClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public Form1()
        {
            InitializeComponent();
        }

        public void getData()
        {
            while (true)
            {
                try
                {
                    byte[] b = new byte[1024];
                    int r = socClient.Receive(b);
                    if (r > 0)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            lst_Message.Items.Add(Encoding.Unicode.GetString(b, 0, r));
                        });
                    }
                }
                catch (Exception)
                {
                    ;
                }
            }
        }

        private void btn_Connect_Click(object sender, EventArgs e)
        {
            try
            {
                socClient.Connect(new IPEndPoint(IPAddress.Parse(txt_IP.Text), int.Parse(txt_Port.Text)));
                Thread tr = new Thread(getData);
                tr.IsBackground = true;
                tr.Start();
                lblConnect.Text = "Connected";
            }
            catch (Exception)
            {
                MessageBox.Show("You,re Connected now");
            }
        }

        private void btn_Send_Click(object sender, EventArgs e)
        {
            byte[] b = Encoding.Unicode.GetBytes(Environment.MachineName + " > " + txt_Send.Text);
            socClient.Send(b);
            lst_Message.Items.Add(Environment.MachineName + " > " + txt_Send.Text);

            txt_Send.Text = "";
        }

        private void txt_Send_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btn_Send.Focus();
            }
        }
    }
}
