﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo_OSMO
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();  
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string stn = string.Empty;

            xmlClass xml = new Demo_OSMO.xmlClass();
            // xml.CreatXmlFile("C:/Kenneth/soft/","test.xml");
            //stn= xml.CheckValueToXmlFile("C:/Kenneth/soft/test.xml", "parimater", "sample");
            //stn = "0";
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}