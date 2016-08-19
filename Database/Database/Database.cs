using System;
using System.Collections.Generic;
using System.Data.SqlClient;
#pragma warning disable
namespace Databases
{
    //Clase para Conectar con SQL Server Database
    public class SqlServer
    {
        //atributos
        private const string vl_usuario = "SegAdmin";
        private const string vl_passwd = "$3Fin2010";
        private const string vl_segfrase = "/.&G@nicu$2016#3cl3$iast3$";
        private bool vl_estado_conexion = false;
        private List<string> ListaParametros;
        private List<string> ListaValores;
        private string vl_procedimiento;

        //Propiedades de Construccion del SQL Statement
        private string vl_campos;
        private string vl_tabla;
        private string vl_filtro;
        private string vl_usar_filtro;
        private string vl_sentencia;
        private string vl_resultado;
        private Int32 vlresultado;
        private bool ResultadoGlobal;
        private string vl_Estado_Usuario;
        private string vl_Existe_Usuario;

        //variables privadas de Sql Server 
        private SqlConnection SQLConexion;
        private SqlCommand SQLComando;
        private SqlDataAdapter SQLAdapter;
        private SqlDataReader SQLLeer;
        private SqlCommandBuilder SQLComandos;
        private System.Data.DataSet SQLDatos = new System.Data.DataSet();
        private string SQLMessages;
        private const string vl_cadena = "Data Source=COMVIVA-PC; Initial Catalog=SISFACT;User ID=" + vl_usuario + ";Password=" + vl_passwd + ";";

        /*--------------------------------------------------------
         * Metodo: Conectar
         * Proposito: Establecer la conexion con la base de datos
         * -------------------------------------------------------
         */
        public string Conectar()
        {
            /*Se instancia la clase*/
            SQLConexion = new SqlConnection();

            /*Bloque Try-Catch*/
            try
            {
                //Se asigna la cadena de conexion 
                SQLConexion.ConnectionString = vl_cadena;
                //Se Establece la Conexion
                SQLConexion.Open();
                //Se retorna Verdadero si se establecio la conexion
                SQLMessages = "Conexion Establecida";
                vl_estado_conexion = true;
                return SQLMessages;
            }
            catch (SqlException sqlEx)
            {
                //Se validan los numeros de error de SQL Server.
                switch (sqlEx.Number)
                {
                    //Password Incorrecto
                    case 18456:
                        SQLMessages = "Usuario y/o Password es incorrecto.";
                        //Se liberan los recursos
                        SQLConexion.Dispose();
                        break;
                    //Nombre del Servidor Incorrecto
                    case 18482:
                        SQLMessages = "El Servidor de Base de Datos no esta diposnible o nombre de Servidor Incorrecto.";
                        //Se liberan los recursos
                        SQLConexion.Dispose();
                        break;
                    //Cuenta de Usuario Bloqueada
                    case 18486:
                        SQLMessages = "Intento de conexion fallido para el usuario: " + vl_usuario + ", la cuenta esta bloqueada. Contactar al DBA.";
                        //Se liberan los recursos
                        SQLConexion.Dispose();
                        break;
                    //Password de la Cuenta Expirado
                    case 18487:
                        SQLMessages = "Intento de conexion fallido para el usuario: " + vl_usuario + ", el password ha expirado. Contactar al DBA.";
                        //Se liberan los recursos
                        SQLConexion.Dispose();
                        break;
                    //Password de la Cuenta Expirado y Debe ser Cambiado
                    case 18488:
                        SQLMessages = "Intento de conexion fallido para el usuario: " + vl_usuario + ", el password debe ser cambiado. Contactar al DBA.";
                        //Se liberan los recursos
                        SQLConexion.Dispose();
                        break;
                    //Cuenta de Usuario Deshabilitada
                    case 18470:
                        SQLMessages = "Intento de conexion fallido para el usuario: " + vl_usuario + ", la cuenta de usuario esta deshabilitada. Contactar al DBA.";
                        //Se liberan los recursos
                        SQLConexion.Dispose();
                        break;
                    default:
                        SQLMessages = sqlEx.Message;
                        //Se liberan los recursos
                        SQLConexion.Dispose();
                        break;
                }
                ////Se retorna Falso si hay algun error
                return SQLMessages;
            }
        }

        /*--------------------------------------------------------
        * Metodo: Desconectar
        * Proposito: Cierra la conexion a la base de datos
        * -------------------------------------------------------
        */
        public void Desconectar()
        {
            //Se valida el estado de la conexion
            if (SQLConexion.State == System.Data.ConnectionState.Open)
            {
                //Se procede a cerrar la conexion
                SQLConexion.Close();
                SQLConexion.Dispose();
                //Se asigna false para indicar que la conexion no esta OPEN.
                vl_estado_conexion = false;
            }
        }

        /*--------------------------------------------------------
        * Metodo: ConsultaSimple
        * Proposito: Devuelve un solo registro de la Base de Datos
        * --------------------------------------------------------
        */
        public bool ConsultaSimple()
        {
            //Se valida el estado de la conexion
            if (vl_estado_conexion == false)
            {
                try
                {
                    //Se asigna la cadena de conexion 
                    SQLConexion.ConnectionString = vl_cadena;
                    //Se apertura la conexion
                    SQLConexion.Open();
                    //Se construye la sentencia
                    vl_sentencia = "SELECT " + vl_campos + " FROM " + vl_tabla + " WHERE " + vl_filtro;
                    //Se crea el objeto de tipo SqlCommand
                    SQLComando = new SqlCommand(vl_sentencia, SQLConexion);
                    //Se obtiene el resultado de ejecutar la sentencia SQL
                    vl_resultado = Convert.ToString(SQLComando.ExecuteScalar());
                    //Se cierra la conexion
                    SQLConexion.Close();
                    //Se liberan los recursos
                    SQLConexion.Dispose();
                    //Se retorna Exitoso
                    return true;
                }
                catch (SqlException sqlEx)
                {
                    SQLMessages = sqlEx.Message;
                    //Se retorna Error
                    return false;
                }
            }
            else
            {
                try
                {
                    //Se construye la sentencia
                    vl_sentencia = "SELECT " + vl_campos + " FROM " + vl_tabla + " WHERE " + vl_filtro;
                    //Se crea el objeto de tipo SqlCommand
                    SQLComando = new SqlCommand(vl_sentencia, SQLConexion);
                    //Se obtiene el resultado de ejecutar la sentencia SQL
                    vl_resultado = Convert.ToString(SQLComando.ExecuteScalar());
                    //Se cierra la conexion
                    SQLConexion.Close();
                    //Se liberan los recursos
                    SQLConexion.Dispose();
                    //Se retorna Exitoso
                    return true;
                }
                catch (SqlException sqlEx)
                {
                    SQLMessages = sqlEx.Message;
                    //Se retorna Error
                    return false;
                }
            }
        }

        /*--------------------------------------------------------
       * Metodo: LlenarGrid
       * Proposito: Devuelve un solo registro de la Base de Datos
       * --------------------------------------------------------
       */
        public bool LlenarGrid()
        {
            //Se valida el estado de la conexion
            if (vl_estado_conexion == false)
            {
                try
                {
                    //Se valida si esta usando filtro o no
                    if (vl_usar_filtro == "SI")
                    {
                        //Se construye la sentencia
                        vl_sentencia = "SELECT " + vl_campos + " FROM " + vl_tabla + " WHERE " + vl_filtro;
                    }
                    else
                    {
                        //Se construye la sentencia
                        vl_sentencia = "SELECT " + vl_campos + " FROM " + vl_tabla;
                    }
                    //Se asigna la cadena de conexion 
                    SQLConexion.ConnectionString = vl_cadena;
                    //Se apertura la conexion
                    SQLConexion.Open();
                    //Se limpia el SQLDatos
                    SQLDatos.Clear();
                    //Se crea el objeto de tipo SqlDataAdapter
                    SQLAdapter = new SqlDataAdapter(vl_sentencia, SQLConexion);
                    //Se crea el objeto de tipo SqlCommandBuilder
                    SQLComandos = new SqlCommandBuilder(SQLAdapter);
                    //Se obtiene el resultado de ejecutar la sentencia SQL
                    SQLAdapter.Fill(SQLDatos, vl_tabla);
                    //Se cierra la conexion
                    SQLConexion.Close();
                    //Se liberan los recursos
                    SQLConexion.Dispose();
                    //Se retorna Exitoso
                    return true;
                }
                catch (SqlException sqlEx)
                {
                    SQLMessages = sqlEx.Message;
                    //Se retorna Error
                    return false;
                }
            }
            else
            {
                try
                {
                    //Se valida si esta usando filtro o no
                    if (vl_usar_filtro == "SI")
                    {
                        //Se construye la sentencia
                        vl_sentencia = "SELECT " + vl_campos + " FROM " + vl_tabla + " WHERE " + vl_filtro;
                    }
                    else
                    {
                        //Se construye la sentencia
                        vl_sentencia = "SELECT " + vl_campos + " FROM " + vl_tabla;
                    }
                    //Se limpia el SQLDatos
                    SQLDatos.Clear();
                    //Se crea el objeto de tipo SqlDataAdapter
                    SQLAdapter = new SqlDataAdapter(vl_sentencia, SQLConexion);
                    //Se crea el objeto de tipo SqlCommandBuilder
                    SQLComandos = new SqlCommandBuilder(SQLAdapter);
                    //Se obtiene el resultado de ejecutar la sentencia SQL
                    SQLAdapter.Fill(SQLDatos, vl_tabla);
                    //Se cierra la conexion
                    SQLConexion.Close();
                    //Se liberan los recursos
                    SQLConexion.Dispose();
                    //Se retorna Exitoso
                    return true;
                }
                catch (SqlException sqlEx)
                {
                    SQLMessages = sqlEx.Message;
                    //Se retorna Error
                    return false;
                }
            }
        }

        /*--------------------------------------------------------
        * Metodo: sqlInsertar
        * Proposito: Se inserta un registro en la base de datos
        * --------------------------------------------------------
        */
        public bool sqlInsertar()
        {

            //Se valida el estado de la conexion
            if (vl_estado_conexion == false)
            {
                try
                {
                    //Se asigna la cadena de conexion 
                    SQLConexion.ConnectionString = vl_cadena;
                    //Se apertura la conexion
                    SQLConexion.Open();
                    //Se construye la sentencia
                    vl_sentencia = "INSERT INTO " + vl_tabla + " VALUES (" + vl_campos + ")";
                    //Se crea el objeto de tipo SqlCommand
                    SQLComando = new SqlCommand(vl_sentencia, SQLConexion);
                    //Se obtiene el resultado de ejecutar la sentencia SQL
                    vlresultado = SQLComando.ExecuteNonQuery();
                    if (vlresultado > 0)
                    { return true; }

                    else
                    { return false; }

                    //Se cierra la conexion
                    SQLConexion.Close();
                    //Se liberan los recursos
                    SQLConexion.Dispose();
                }
                catch (SqlException sqlEx)
                {
                    SQLMessages = sqlEx.Message;
                    //Se retorna Error
                    return false;
                }
            }
            else
            {
                try
                {
                    //Se asigna la cadena de conexion 
                    SQLConexion.ConnectionString = vl_cadena;
                    //Se apertura la conexion
                    SQLConexion.Open();
                    //Se construye la sentencia
                    vl_sentencia = "INSERT INTO " + vl_tabla + " VALUES (" + vl_campos + ")";
                    //Se crea el objeto de tipo SqlCommand
                    SQLComando = new SqlCommand(vl_sentencia, SQLConexion);
                    //Se obtiene el resultado de ejecutar la sentencia SQL
                    vlresultado = SQLComando.ExecuteNonQuery();
                    if (vlresultado > 0)
                    { return true; }

                    else
                    { return false; }
                    //Se cierra la conexion
                    SQLConexion.Close();
                    //Se liberan los recursos
                    SQLConexion.Dispose();

                }
                catch (SqlException sqlEx)
                {
                    SQLMessages = sqlEx.Message;
                    //Se retorna Error
                    return false;
                }
            }
        }

        /*--------------------------------------------------------
        * Metodo: sqlActualizar
        * Proposito: Actualiza Registros en la Base de Datos
        * --------------------------------------------------------
        */
        public bool sqlActualizar()
        {

            //Se valida el estado de la conexion
            if (vl_estado_conexion == false)
            {
                try
                {
                    //Se valida si esta usando filtro o no
                    if (vl_usar_filtro == "SI")
                    {
                        //Se construye la sentencia
                        vl_sentencia = "UPDATE " + vl_tabla + " SET " + vl_campos + " WHERE " + vl_filtro;
                    }
                    else
                    {
                        //Se construye la sentencia
                        vl_sentencia = "UPDATE " + vl_tabla + " SET " + vl_campos;
                    }

                    //Se asigna la cadena de conexion 
                    SQLConexion.ConnectionString = vl_cadena;
                    //Se apertura la conexion
                    SQLConexion.Open();
                    //Se crea el objeto de tipo SqlCommand
                    SQLComando = new SqlCommand(vl_sentencia, SQLConexion);
                    //Se obtiene el resultado de ejecutar la sentencia SQL
                    vlresultado = SQLComando.ExecuteNonQuery();
                    if (vlresultado > 0)
                    { return true; }

                    else
                    { return false; }

                    //Se cierra la conexion
                    SQLConexion.Close();
                    //Se liberan los recursos
                    SQLConexion.Dispose();


                }
                catch (SqlException sqlEx)
                {
                    SQLMessages = sqlEx.Message;
                    //Se retorna Error
                    return false;
                }
            }
            else
            {
                try
                {
                    //Se valida si esta usando filtro o no
                    if (vl_usar_filtro == "SI")
                    {
                        //Se construye la sentencia
                        vl_sentencia = "UPDATE " + vl_tabla + " SET " + vl_campos + " WHERE " + vl_filtro;
                    }
                    else
                    {
                        //Se construye la sentencia
                        vl_sentencia = "UPDATE " + vl_tabla + " SET " + vl_campos;
                    }
                    //Se asigna la cadena de conexion 
                    SQLConexion.ConnectionString = vl_cadena;
                    //Se apertura la conexion
                    SQLConexion.Open();
                    //Se crea el objeto de tipo SqlCommand
                    SQLComando = new SqlCommand(vl_sentencia, SQLConexion);
                    //Se obtiene el resultado de ejecutar la sentencia SQL
                    vlresultado = SQLComando.ExecuteNonQuery();
                    if (vlresultado > 0)
                    { return true; }

                    else
                    { return false; }
                    //Se cierra la conexion
                    SQLConexion.Close();
                    //Se liberan los recursos
                    SQLConexion.Dispose();

                }
                catch (SqlException sqlEx)
                {
                    SQLMessages = sqlEx.Message;
                    //Se retorna Error
                    return false;
                }
            }
        }

        /*--------------------------------------------------------
        * Metodo: sqlEliminar
        * Proposito: Eliminar un Registro de la Base de Datos
        * --------------------------------------------------------
        */
        public bool sqlEliminar()
        {

            //Se valida el estado de la conexion
            if (vl_estado_conexion == false)
            {
                try
                {
                    //Se valida si esta usando filtro o no
                    if (vl_usar_filtro == "SI")
                    {
                        //Se construye la sentencia
                        vl_sentencia = "DELETE FROM " + vl_tabla + " WHERE " + vl_filtro;
                    }
                    else
                    {
                        //Se construye la sentencia
                        vl_sentencia = "DELETE FROM " + vl_tabla;
                    }
                    //Se asigna la cadena de conexion 
                    SQLConexion.ConnectionString = vl_cadena;
                    //Se apertura la conexion
                    SQLConexion.Open();
                    //Se crea el objeto de tipo SqlCommand
                    SQLComando = new SqlCommand(vl_sentencia, SQLConexion);
                    //Se obtiene el resultado de ejecutar la sentencia SQL
                    vlresultado = SQLComando.ExecuteNonQuery();
                    if (vlresultado > 0)
                    { return true; }

                    else
                    { return false; }

                    //Se cierra la conexion
                    SQLConexion.Close();
                    //Se liberan los recursos
                    SQLConexion.Dispose();
                }
                catch (SqlException sqlEx)
                {
                    SQLMessages = sqlEx.Message;
                    //Se retorna Error
                    return false;
                }
            }
            else
            {
                try
                {
                    //Se valida si esta usando filtro o no
                    if (vl_usar_filtro == "SI")
                    {
                        //Se construye la sentencia
                        vl_sentencia = "DELETE FROM " + vl_tabla + " WHERE " + vl_filtro;
                    }
                    else
                    {
                        //Se construye la sentencia
                        vl_sentencia = "DELETE FROM " + vl_tabla;
                    }
                    //Se asigna la cadena de conexion 
                    SQLConexion.ConnectionString = vl_cadena;
                    //Se apertura la conexion
                    SQLConexion.Open();
                    //Se crea el objeto de tipo SqlCommand
                    SQLComando = new SqlCommand(vl_sentencia, SQLConexion);
                    //Se obtiene el resultado de ejecutar la sentencia SQL
                    vlresultado = SQLComando.ExecuteNonQuery();
                    if (vlresultado > 0)
                    { return true; }

                    else
                    { return false; }
                    //Se cierra la conexion
                    SQLConexion.Close();
                    //Se liberan los recursos
                    SQLConexion.Dispose();

                }
                catch (SqlException sqlEx)
                {
                    SQLMessages = sqlEx.Message;
                    //Se retorna Error
                    return false;
                }
            }
        }

        /*--------------------------------------------------------
        * Metodo: GenerarListaParametros
        * Proposito: Obtener los parametros de un procedimiento o funcion
        *            creados en la based de datos.
        * --------------------------------------------------------
        */
        private void GenerarListaParametros()
        {
            //Se asigna la cadena de conexion 
            SQLConexion.ConnectionString = vl_cadena;
            //Se apertura la conexion
            SQLConexion.Open();
            //Se construye la sentencia a ejecutar
            vl_sentencia = "SELECT parameter_name FROM INFORMATION_SCHEMA.PARAMETERS WHERE SPECIFIC_NAME = '" + vl_procedimiento + "'";
            //Se crea el objeto de tipo SqlCommand
            SQLComando = new SqlCommand(vl_sentencia, SQLConexion);
            //Se obtiene el resultado de ejecutar la sentencia SQL
            SQLLeer = SQLComando.ExecuteReader();
            //Se valida si hay registros a leer
            if (SQLLeer.HasRows)
            { //Ciclo While para leer el contenido
                while (SQLLeer.Read())
                {
                    //Se asignan los valores a la lista
                    ListaParametros.Add(SQLLeer.GetString(0));
                }
            }
            //Se cierra la conexion
            SQLConexion.Close();
            //Se liberan los recursos
            SQLConexion.Dispose();
        }

        /*--------------------------------------------------------
        * Metodo: sqlEjecutar
        * Proposito: Obtener los parametros de un procedimiento o funcion
        *            creados en la based de datos.
        * --------------------------------------------------------
        */

        public bool sqlEjecutar()
        {
            //Se asigna la cadena de conexion 
            SQLConexion.ConnectionString = vl_cadena;
            //Se apertura la conexion
            SQLConexion.Open();
            //Se crea el objeto de tipo SqlCommand
            SQLComando = new SqlCommand(vl_procedimiento, SQLConexion);
            //Se indica que se ejecutara un procedimiento almacenado de la Base de Datos
            SQLComando.CommandType = System.Data.CommandType.StoredProcedure;
            //Se lee la lista de parametros
            for (var i = 0; i < ListaParametros.Count; i++)
            {
                SQLComando.Parameters.AddWithValue(ListaParametros[i].ToString(), ListaValores[i].ToString());
            }
            //Se obtiene el resultado de ejecutar la sentencia SQL
            vlresultado = SQLComando.ExecuteNonQuery();
            if (vlresultado > 0)
            { return true; }

            else
            { return false; }
            //Se cierra la conexion
            SQLConexion.Close();
            //Se liberan los recursos
            SQLConexion.Dispose();

        }

        /*--------------------------------------------------------
        * Metodo: EstadoUsuario_BD
        * Proposito: Obtener los parametros de un procedimiento o funcion
        *            creados en la based de datos.
        * --------------------------------------------------------
        */
        public string EstadoUsuario_BD(string usuario)
        {
            //Se asignan los valores
            vl_campos = "activo";
            vl_tabla = "seg.USUARIOS";
            vl_filtro = "usuario = '" + usuario + "'";
            //Se obtiene el resultado
            ResultadoGlobal = ConsultaSimple();
            //Se valida el resultado del query
            if (ResultadoGlobal == true)
            {
                //Se obtiene el valor
                vl_Estado_Usuario = vl_resultado;
            }
            //Se retorna el resultado
            return vl_Estado_Usuario;
            Desconectar();
        }

        /*--------------------------------------------------------
       * Metodo: VerificarUsuario_BD
       * Proposito: Obtener los parametros de un procedimiento o funcion
       *            creados en la based de datos.
       * --------------------------------------------------------
       */
        public bool VerificarUsuario_BD(string usuario)
        {
            //Se asignan los valores
            vl_campos = "ISNULL(COUNT(USUARIO),0)";
            vl_tabla = "seg.USUARIOS";
            vl_filtro = "usuario = '" + usuario + "'";
            //Se obtiene el resultado
            ResultadoGlobal = ConsultaSimple();
            //Se valida el resultado del query
            if (ResultadoGlobal == true)
            {
                //Se valida el resultado
                if (vl_resultado == "0")
                { ResultadoGlobal = false; }
                else
                { ResultadoGlobal = true; }
            }
            return ResultadoGlobal;
            Desconectar();
        }

      /*--------------------------------------------------------
      * Metodo: PrimerAcceso_Sistema
      * Proposito: Obtener los parametros de un procedimiento o funcion
      *            creados en la based de datos.
      * --------------------------------------------------------
      */
        public bool PrimerAcceso_Sistema(string usuario)
        {
            //Se asignan los valores
            vl_campos = "primer_acceso";
            vl_tabla = "seg.USUARIOS";
            vl_filtro = "usuario = '" + usuario + "'";
            //Se obtiene el resultado
            ResultadoGlobal = ConsultaSimple();
            //Se valida el resultado del query
            if (ResultadoGlobal == true)
            {
                //Se valida el resultado
                if (vl_resultado == "Y")
                { ResultadoGlobal = true; }
                else
                { ResultadoGlobal = false; }
            }
            return ResultadoGlobal;
            Desconectar();
        }

        //Se obtiene el valor de la propiedad SQLMessages
        public string getSQLMessages
        { get { return SQLMessages; } }

        //Se obtiene el valor de la propiedad SegFrase
        public string getFraseSeguridad
        { get { return vl_segfrase; } }

        //Se obtiene el valor del Query
        public string getResultadoQuery
        { get { return vl_resultado; } }

        //Se obtiene el estado de la conexion
        public bool getEstadoConexion
        { get { return vl_estado_conexion; } }

        //Propiedad de Solo Escritura
        public string setCamposTabla
        { set { vl_campos = value; } }

        //Propiedad de Solo Escritura
        public string setNombreTabla
        { set { vl_tabla = value; } }

        //Propiedad de Solo Escritura
        public string setFiltroTabla
        { set { vl_filtro = value; } }

        //Propiedad de Solo Escritura
        public string setUsarFiltro
        { set { vl_usar_filtro = value; } }

        //Propiedad de Solo Escritura
        public string setProcedimiento
        { set { vl_procedimiento = value; } }

        //Propiedad de Solo Escritura
        public string setValores_Procedimiento
        { set { vl_procedimiento = value; } }
    }

    //Clase para Conectar con Oracle Database
    public class Oracle
    {
    }
}
