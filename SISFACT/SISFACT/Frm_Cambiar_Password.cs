﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SISFACT
{
    public partial class Frm_Cambiar_Password : Form
    {
        public Frm_Cambiar_Password()
        {
            InitializeComponent();
        }

        private void btCancelar_Click(object sender, EventArgs e)
        {
            this.Dispose();
            Frm_Login FrmLogin = new Frm_Login();
            FrmLogin.Show();
        }
    }
}
