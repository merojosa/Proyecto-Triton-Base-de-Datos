﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Interfaz_Triton
{
	public partial class Form1 : Form
	{
        private bool descuento;

        public Form1()
        {
            InitializeComponent();

            descuento = false;

            Label_Codigo_Descuento.Visible = false;
            Label_Fecha_Descuento.Visible = false;
            Label_Porcentaje_Descuento.Visible = false;

            Porcentaje_TB_Descuento.Visible = false;
            Fecha_TB_Descuento.Visible = false;
            Codigo_TB_Descuento.Visible = false;

            Cantidad_Semanas_TB_Conta.Visible = false;
            Label_Cantidad_Semanas_Conta.Visible = false;
            Semanal_TB_Conta.Visible = false;
            Label_Monto_Semanal_Conta.Visible = false;

            Label_Monto_Mensual_Conta.Visible = false;
            Monto_Mensual_TB_Conta.Visible = false;
        }

		private void Atleta_Text_Box_TextChanged(object sender, EventArgs e)
		{

		}

		private void Atleta_Group_Box_Enter(object sender, EventArgs e)
		{

		}

		private void Form1_Load(object sender, EventArgs e)
		{
            // TODO: This line of code loads data into the 'tritonDataSet.Entrenamiento_Individual' table. You can move, or remove it, as needed.
            this.entrenamiento_IndividualTableAdapter.Fill(this.tritonDataSet.Entrenamiento_Individual);
            // TODO: This line of code loads data into the 'tritonDataSet.Cobro_Individual' table. You can move, or remove it, as needed.
            this.cobro_IndividualTableAdapter.Fill(this.tritonDataSet.Cobro_Individual);
            // TODO: This line of code loads data into the 'tritonDataSet.Atleta_Info_Basica' table. You can move, or remove it, as needed.
            this.atleta_Info_BasicaTableAdapter.Fill(this.tritonDataSet.Atleta_Info_Basica);
			// TODO: This line of code loads data into the 'tritonDataSet.Atleta' table. You can move, or remove it, as needed.
			this.atletaTableAdapter.Fill(this.tritonDataSet.Atleta);

		}

		private void Atleta_Tab_Page_Click(object sender, EventArgs e)
		{

		}

		private void Buscar_Atleta_Button_Click(object sender, EventArgs e)
		{
			String texto_busqueda_atleta = Atleta_Text_Box.Text;					// Obtengo el texto que se digitó.
			String filtro = "Nombre like '%" + texto_busqueda_atleta + "%' or Apellido1 like '%" +
							texto_busqueda_atleta + "%' or Apellido2 like '%" + texto_busqueda_atleta + "%'";

			DataRow[] resultados_busqueda = tritonDataSet.Atleta_Info_Basica.Select(filtro);	// Hago un SELECT acorde a la busqueda.  Ojo, es un VIEW.		
			Atleta_Data_Grid_View.DataSource = resultados_busqueda;
		}

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Ganancias_Button_Click(object sender, EventArgs e)
        {
            Error_Label.Visible = false;
            Ganancias_Label.Visible = false;
            int mes = parseo_mes(Mes_Combo_Box.Text);   // Obtengo el mes en int.
            String anno = Anno_Combo_Box.Text;          // Obtengo el año.

            // Verifico los datos
            if( int.TryParse(anno, out int n) == true && mes != -1 ) // Si se digitó un número, todo bien.
            {

                // Conexión Triton.
                String connectionStr = Interfaz_Triton.Properties.Settings.Default.TritonConnectionString;
                SqlConnection connection = new SqlConnection(connectionStr);
                SqlDataAdapter databaseAdapter = new SqlDataAdapter();

                // Ocupo la conexión abierta.
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                // Hago el SELECT de la función para obtener ganancias.
                SqlCommand cmd = new SqlCommand("SELECT dbo.Obtener_Ganancias_Mes(" + mes.ToString() + "," + anno + ")", connection);

                // Obtengo las ganancias, es obligatorio hacer el cast.
                int ganancias = (int)(cmd.ExecuteScalar());

                Ganancias_Label.Text = "$" + ganancias;
                Ganancias_Label.Visible = true;

                // Cierro conexión.
                connection.Close();
            }
            else
            {
                Error_Label.Visible = true;
            }
        }

        private int parseo_mes(String mes)
        {
            switch (mes)
            {
                case "Enero":       return 1;
                case "Febrero":     return 2;
                case "Marzo":       return 3;
                case "Abril":       return 4;
                case "Mayo":        return 5;
                case "Junio":       return 6;
                case "Julio":       return 7;
                case "Agosto":      return 8;
                case "Setiembre":   return 9;
                case "Octubre":     return 10;
                case "Noviembre":   return 11;
                case "Diciembre":   return 12;
                default:            return -1;  //ERROR
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataTable data = tritonDataSet.sp_cantidad_pruebas_fisicas;

        }

        private void Atleta_Data_Grid_View_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            String connectionStr = Interfaz_Triton.Properties.Settings.Default.TritonConnectionString;
            SqlConnection connection = new SqlConnection(connectionStr);
            SqlDataAdapter databaseAdapter = new SqlDataAdapter();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_cantidad_pruebas_fisicas";
            databaseAdapter.SelectCommand = cmd;
            DataSet dataset = new DataSet();
            //connection.Open();
            databaseAdapter.Fill(dataset);
            dataGridView1.DataSource = dataset.Tables[0];
            //dataGridView1.Update();
            //dataGridView1.Refresh();

        }

        private void Atleta_Data_Grid_View_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if( Tipo_Cobro_CB_Conta.Text == "Mensual" )
            {
                Cantidad_Semanas_TB_Conta.Visible = false;
                Label_Cantidad_Semanas_Conta.Visible = false;
                Semanal_TB_Conta.Visible = false;
                Label_Monto_Semanal_Conta.Visible = false;

                Label_Monto_Mensual_Conta.Visible = true;
                Monto_Mensual_TB_Conta.Visible = true;
            }
            else
            {
                Cantidad_Semanas_TB_Conta.Visible = true;
                Label_Cantidad_Semanas_Conta.Visible = true;
                Semanal_TB_Conta.Visible = true;
                Label_Monto_Semanal_Conta.Visible = true;

                Label_Monto_Mensual_Conta.Visible = false;
                Monto_Mensual_TB_Conta.Visible = false;
            }
        }

        private void Anno_Combo_Box_TextChanged(object sender, EventArgs e)
        {

        }

        private void Descuento_Ganancias_Conta_CheckedChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("You are in the CheckBox.CheckedChanged event.");
            if( descuento == false )   // Si se marca, hay descuento.
            {
                descuento = !descuento;

                Label_Codigo_Descuento.Visible = true;
                Label_Fecha_Descuento.Visible = true;
                Label_Porcentaje_Descuento.Visible = true;

                Porcentaje_TB_Descuento.Visible = true;
                Fecha_TB_Descuento.Visible = true;
                Codigo_TB_Descuento.Visible = true;
            }
            else // No hay descuento.
            {
                descuento = !descuento;

                Label_Codigo_Descuento.Visible = false;
                Label_Fecha_Descuento.Visible = false;
                Label_Porcentaje_Descuento.Visible = false;

                Porcentaje_TB_Descuento.Visible = false;
                Fecha_TB_Descuento.Visible = false;
                Codigo_TB_Descuento.Visible = false;
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Button_Insertar_Factura_Click(object sender, EventArgs e)
        {
            String connectionStr = Interfaz_Triton.Properties.Settings.Default.TritonConnectionString;
            SqlConnection connection = new SqlConnection(connectionStr);

            SqlDataAdapter databaseAdapter = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand("Insert into Cobro (ID_Factura_PK,Codigo_Atleta_FK,Fecha_Pago,Fecha_Finalización,Número_Tarjeta,Fecha_Vencimiento,CVC,Codigo_Descuento, Duracion_Descuento,Porcentaje_Descuento ) values (@ID_Factura_PK,@Codigo_Atleta_FK,@Fecha_Pago,@Fecha_Finalización,@Número_Tarjeta,@Fecha_Vencimiento,@CVC,@Codigo_Descuento, @Duracion_Descuento,@Porcentaje_Descuento)", connection);
            int id_factura = Convert.ToInt32(Codigo_Factura_TB_Conta.Text);

            //Creo los Parametros
            cmd.Parameters.Add("@ID_Factura_PK", System.Data.SqlDbType.Int);
            cmd.Parameters.Add("@Codigo_Atleta_FK", System.Data.SqlDbType.Int);
            cmd.Parameters.Add("@Fecha_Pago", System.Data.SqlDbType.Date);
            cmd.Parameters.Add("@Fecha_Finalización", System.Data.SqlDbType.Date);
            cmd.Parameters.Add("@Número_Tarjeta", System.Data.SqlDbType.VarChar);
            cmd.Parameters.Add("@Fecha_Vencimiento", System.Data.SqlDbType.Date);
            cmd.Parameters.Add("@CVC", System.Data.SqlDbType.Int);
            cmd.Parameters.Add("@Codigo_Descuento", System.Data.SqlDbType.Int);
            cmd.Parameters.Add("@Duracion_Descuento", System.Data.SqlDbType.Date);
            cmd.Parameters.Add("@Porcentaje_Descuento", System.Data.SqlDbType.Int);

            //Asigno los valores
            cmd.Parameters["@Codigo_Atleta_FK"].Value = 21;
            cmd.Parameters["@ID_Factura_PK"].Value = id_factura;
            cmd.Parameters["@Fecha_Pago"].Value = DateTime.Parse(Fecha_Pago_TB_Conta.Text);
            cmd.Parameters["@Fecha_Finalización"].Value = DateTime.Parse(Fecha_Vencimiento_TB_Conta.Text);
            cmd.Parameters["@Número_Tarjeta"].Value = Numero_Tarjeta_TB_Conta.Text;
            cmd.Parameters["@Fecha_Vencimiento"].Value = DateTime.Parse(Vencimiento_Tarjeta_TB_Conta.Text);
            cmd.Parameters["@CVC"].Value = Convert.ToInt32(CVC_TB_Conta.Text);

            if (descuento == true)
            {
                MessageBox.Show("descuento true");
                cmd.Parameters["@Codigo_Descuento"].Value = Convert.ToInt32(Codigo_TB_Descuento.Text);
                cmd.Parameters["@Duracion_Descuento"].Value = DateTime.Parse(Fecha_TB_Descuento.Text);
                cmd.Parameters["@Porcentaje_Descuento"].Value = Convert.ToInt32(Porcentaje_TB_Descuento.Text);
            }
            else
            {
                MessageBox.Show("descuento false");
                cmd.Parameters["@Codigo_Descuento"].Value = DBNull.Value;
                cmd.Parameters["@Duracion_Descuento"].Value = DBNull.Value;
                cmd.Parameters["@Porcentaje_Descuento"].Value = DBNull.Value;
            }

            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();

            // Inserto el tipo de Cobro.
            if (Tipo_Cobro_CB_Conta.Text == "Mensual")
            {
                insertar_cobro_mensual(id_factura, Convert.ToDecimal(Monto_Mensual_TB_Conta.Text) );
            }
            else if (Tipo_Cobro_CB_Conta.Text == "Individual")
            {
                insertar_cobro_individual(id_factura, Convert.ToDecimal(Semanal_TB_Conta.Text), Convert.ToInt32(Cantidad_Semanas_TB_Conta.Text));
            }
            else
            {
                // Error, estoy insertando un cobro sin tipo.
                MessageBox.Show("Debe de escoger un tipo de pago", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Fecha_TB_Descuento_TextChanged(object sender, EventArgs e)
        {

        }

        private void insertar_cobro_mensual( int factura, decimal monto_mensual )
        {
            String connectionStr = Interfaz_Triton.Properties.Settings.Default.TritonConnectionString;
            SqlConnection connection = new SqlConnection(connectionStr);

            SqlDataAdapter databaseAdapter = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand("Insert into Cobro_Mensual (ID_Factura_FK,Monto_Mensual) values (@ID_Factura_FK,@Monto_Mensual)", connection);

            cmd.Parameters.Add("@ID_Factura_FK", System.Data.SqlDbType.Int);
            cmd.Parameters.Add("@Monto_Mensual", System.Data.SqlDbType.Money);

            cmd.Parameters["@ID_Factura_FK"].Value = factura;
            cmd.Parameters["@Monto_Mensual"].Value = monto_mensual;

            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }

        private void insertar_cobro_individual(int factura, decimal monto_semanal, int cantidad_semanas)
        {
            String connectionStr = Interfaz_Triton.Properties.Settings.Default.TritonConnectionString;
            SqlConnection connection = new SqlConnection(connectionStr);

            SqlDataAdapter databaseAdapter = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand("Insert into Cobro_Mensual (ID_Factura_FK,Monto_Semanal,Monto_Total,Cantidad_Semanas) values (@ID_Factura_FK,@Monto_Semanal,@Monto_Total,@Cantidad_Semanas)", connection);

            cmd.Parameters.Add("@ID_Factura_FK", System.Data.SqlDbType.Int);
            cmd.Parameters.Add("@Monto_Semanal", System.Data.SqlDbType.Money);
            cmd.Parameters.Add("@Monto_Total", System.Data.SqlDbType.Money);
            cmd.Parameters.Add("@Cantidad_Semanas", System.Data.SqlDbType.Money);

            cmd.Parameters["@ID_Factura_FK"].Value = factura;
            cmd.Parameters["@Monto_Mensual"].Value = monto_semanal;
            cmd.Parameters["@Monto_Total"].Value = monto_semanal*cantidad_semanas;
            cmd.Parameters["@Cantidad_Semanas"].Value = cantidad_semanas;

            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }

        private void dataGridView2_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Button_Insertar_Click(object sender, EventArgs e)
        {
            String connectionStr = Interfaz_Triton.Properties.Settings.Default.TritonConnectionString;
            SqlConnection connection = new SqlConnection(connectionStr);


            SqlDataAdapter databaseAdapter = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand("Insert into Entrenamiento_Individual (Codigo_Entrenamiento_PK,Deporte,Rutina,Nivel) values (@Codigo,@Deporte,@Rutina,@Nivel)", connection);

            //Creo los Parametros
            cmd.Parameters.Add("@Codigo", System.Data.SqlDbType.Int);
            cmd.Parameters.Add("@Deporte", System.Data.SqlDbType.VarChar);
            cmd.Parameters.Add("@Rutina", System.Data.SqlDbType.VarChar);
            cmd.Parameters.Add("@Nivel", System.Data.SqlDbType.VarChar);

            //Asigno los valores

            cmd.Parameters["@Codigo"].Value = Convert.ToInt32(Codigo_TB.Text);
            cmd.Parameters["@Deporte"].Value = Deporte_TB.Text;
            cmd.Parameters["@Rutina"].Value = Rutina_TB.Text;
            cmd.Parameters["@Nivel"].Value = Nivel_TB.Text;


            connection.Open();

            cmd.ExecuteNonQuery();

            //////////Actualiza Datos en Tabla///////////////////////////////
            DataTable dt = new DataTable();
            String querySelect = "SELECT * FROM Entrenamiento_Individual";
            SqlCommand cmd2 = new SqlCommand(querySelect, connection);
            SqlDataAdapter adap = new SqlDataAdapter(cmd2);
            adap.Fill(dt);
            EntrenamientoDG.DataSource = dt;
            EntrenamientoDG.AutoResizeColumns();
            /////////////////////////////////////////////////////////////////

            connection.Close();
        }

        private void Button_Modificar_Click(object sender, EventArgs e)
        {
            String connectionStr = Interfaz_Triton.Properties.Settings.Default.TritonConnectionString;
            SqlConnection connection = new SqlConnection(connectionStr);


            SqlDataAdapter databaseAdapter = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand("Update Entrenamiento_Individual set Deporte=@Deporte, Rutina=@Rutina, Nivel=@Nivel where Codigo_Entrenamiento_PK =@Codigo", connection);


            //Creo los Parametros
            cmd.Parameters.Add("@Codigo", System.Data.SqlDbType.Int);
            cmd.Parameters.Add("@Deporte", System.Data.SqlDbType.VarChar);
            cmd.Parameters.Add("@Rutina", System.Data.SqlDbType.VarChar);
            cmd.Parameters.Add("@Nivel", System.Data.SqlDbType.VarChar);

            //Asigno los valores

            cmd.Parameters["@Codigo"].Value = Convert.ToInt32(Codigo_TB.Text);
            cmd.Parameters["@Deporte"].Value = Deporte_TB.Text;
            cmd.Parameters["@Rutina"].Value = Rutina_TB.Text;
            cmd.Parameters["@Nivel"].Value = Nivel_TB.Text;
            //MessageBox.Show(ModNiv.Text);

            connection.Open();

            cmd.ExecuteNonQuery();

            //////////Actualiza Datos en Tabla///////////////////////////////
            DataTable dt = new DataTable();
            String querySelect = "SELECT * FROM Entrenamiento_Individual";
            SqlCommand cmd2 = new SqlCommand(querySelect, connection);
            SqlDataAdapter adap = new SqlDataAdapter(cmd2);
            adap.Fill(dt);
            EntrenamientoDG.DataSource = dt;
            /////////////////////////////////////////////////////////////////


            connection.Close();
        }

        private void Codigo_TB_TextChanged(object sender, EventArgs e)
        {

        }

        private void Button_Borrar_Click(object sender, EventArgs e)
        {
            String connectionStr = Interfaz_Triton.Properties.Settings.Default.TritonConnectionString;
            SqlConnection connection = new SqlConnection(connectionStr);


            SqlDataAdapter databaseAdapter = new SqlDataAdapter();

            //
            String comando = "delete from Entrenamiento_Individual where ";
            bool hayValor = false;
            if (!String.IsNullOrEmpty(Codigo_TB.Text)) //------------------------------->Codigo
            {
                if (hayValor) //Calcula si hay que poner un OR
                {
                    comando = comando + " AND ";
                }
                comando = comando + "Codigo_Entrenamiento_PK =@Codigo";
                hayValor = true;
            }
            if (!String.IsNullOrEmpty(Deporte_TB.Text))//------------------------------->Deporte
            {
                if (hayValor) //Calcula si hay que poner un OR
                {
                    comando = comando + " AND ";
                }
                comando = comando + "Deporte=@Deporte";
                hayValor = true;
            }
            if (!String.IsNullOrEmpty(Rutina_TB.Text))//------------------------------->Rutina
            {
                if (hayValor) //Calcula si hay que poner un OR
                {
                    comando = comando + " AND ";
                }
                comando = comando + "Rutina=@Rutina";
                hayValor = true;
            }
            if (!String.IsNullOrEmpty(Nivel_TB.Text))//------------------------------->Nivel
            {
                if (hayValor) //Calcula si hay que poner un OR
                {
                    comando = comando + " AND ";
                }
                comando = comando + "Nivel=@Nivel";
                hayValor = true;
            }

            SqlCommand cmd = new SqlCommand(comando, connection);

            //Creo los Parametros
            if (!String.IsNullOrEmpty(Codigo_TB.Text)) //------------------------------->Codigo
            {
                cmd.Parameters.Add("@Codigo", System.Data.SqlDbType.Int);
                cmd.Parameters["@Codigo"].Value = Convert.ToInt32(Codigo_TB.Text);
            }

            if (!String.IsNullOrEmpty(Deporte_TB.Text))//------------------------------->Deporte
            {
                cmd.Parameters.Add("@Deporte", System.Data.SqlDbType.VarChar);
                cmd.Parameters["@Deporte"].Value = Deporte_TB.Text;

            }

            if (!String.IsNullOrEmpty(Rutina_TB.Text))//------------------------------->Rutina
            {
                cmd.Parameters.Add("@Rutina", System.Data.SqlDbType.VarChar);
                cmd.Parameters["@Rutina"].Value = Rutina_TB.Text;
            }

            if (!String.IsNullOrEmpty(Nivel_TB.Text))//------------------------------->Nivel
            {
                cmd.Parameters.Add("@Nivel", System.Data.SqlDbType.VarChar);
                cmd.Parameters["@Nivel"].Value = Nivel_TB.Text;
            }



            connection.Open();
            if (hayValor)
            {
                cmd.ExecuteNonQuery();
                //////////Actualiza Datos en Tabla///////////////////////////////
                DataTable dt = new DataTable();
                String querySelect = "SELECT * FROM Entrenamiento_Individual";
                SqlCommand cmd2 = new SqlCommand(querySelect, connection);
                SqlDataAdapter adap = new SqlDataAdapter(cmd2);
                adap.Fill(dt);
                EntrenamientoDG.DataSource = dt;
                /////////////////////////////////////////////////////////////////
            }

            connection.Close();
        }
    }
}
