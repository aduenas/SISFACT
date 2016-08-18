using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Databases;

namespace SISFACT
{
    public partial class Frm_Login : Form
    {

        public Frm_Login()
        {
            InitializeComponent();
        }

        //Variables
        private string EstadoUsuario;
        private bool Resultado;
        

        private void Login_Load(object sender, EventArgs e)
        {
            //Se deshabilita el boton Login hasta que se ingrese un usuario
            btLogin.Enabled = false;
        }

        //Codigo del Boton Login
        private void btLogin_Click(object sender, EventArgs e)
        {
            //Se instancia la DLL Database
            SqlServer Conexion = new SqlServer();
            //Se valida si la conexion se establecio satisfactoriamente
            if (Conexion.Conectar() == "Conexion Establecida")
            {
                //Se cierra la conexion
                Conexion.Desconectar();
                //Se valida si el usuario existe en el sistema
                Resultado = Conexion.VerificarUsuario_BD(txtUsuario.Text);
                if (Resultado == false)
                {
                    MessageBox.Show("ERR-01: Usuario: " + txtUsuario.Text + " no existe en el sistema.", "SISFACT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //Se limpian los campos
                    txtUsuario.Text = "";
                    txtPassword.Text = "";
                    txtUsuario.Focus();
                }
                else
                {
                    //Se valida si el usuario esta bloqueado
                    EstadoUsuario = Conexion.EstadoUsuario_BD(txtUsuario.Text);
                    if (EstadoUsuario == "B")
                    {
                        MessageBox.Show("ERR-02: Usuario: " + txtUsuario.Text + " esta BLOQUEADO en el sistema.", "SISFACT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //Se limpian los campos
                        txtUsuario.Text = "";
                        txtPassword.Text = "";
                        txtUsuario.Focus();
                    }
                    else
                    {
                        //Se valida que el password sea correcto
                        
                        ////Se valida si es Primer Acceso al Sistema
                        //Resultado = Conexion.PrimerAcceso_Sistema(txtUsuario.Text);
                        //if (Resultado == true)
                        //{
                        //    //Se llama al formulario de Cambiar Password
                        //    this.Hide();
                        //    Frm_Cambiar_Password FrmCambiarPasswd = new Frm_Cambiar_Password();
                        //    FrmCambiarPasswd.Show();
                        //}
                        

                    }
                }
            }
        }
        
        //Se valida si el texto de Usuario esta vacio
        private void txtUsuario_TextChanged (object sender, EventArgs e)
        {
            //Se habilita el boton de Login
            btLogin.Enabled = true;
        }

        //Codigo del Boton Cancelar
        private void btCancelar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
