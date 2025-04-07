using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Data.SqlClient;

namespace Lab03
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string connectionString = "Data Source=LAPTOP-DELL;Initial Catalog=Lab03DB;Integrated Security=True;TrustServerCertificate=True";

        public MainWindow()
        {
            InitializeComponent();
        }

        // Listar clientes usando DataTable
        private void btnListarDataTable_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(connectionString);
                using SqlCommand cmd = new SqlCommand("sp_ListarClientes_DataTable", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable tabla = new DataTable();
                adapter.Fill(tabla);
                dgClientesDataTable.ItemsSource = tabla.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al listar con DataTable: " + ex.Message);
            }
        }

        // Listar clientes usando DataReader
        private void btnListarDataReader_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(connectionString);
                using SqlCommand cmd = new SqlCommand("sp_ListarClientes_DataReader", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                List<Cliente> listaClientes = new List<Cliente>();

                while (reader.Read())
                {
                    listaClientes.Add(new Cliente
                    {
                        ClienteID = reader.GetInt32(reader.GetOrdinal("ClienteID")),
                        Nombres = reader.GetString(reader.GetOrdinal("Nombres")),
                        Apellidos = reader.GetString(reader.GetOrdinal("Apellidos")),
                        DNI = reader.GetString(reader.GetOrdinal("DNI"))
                    });
                }

                dgClientesDataReader.ItemsSource = listaClientes;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al listar con DataReader: " + ex.Message);
            }
        }

        // Buscar clientes por nombre usando DataReader
        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(connectionString);
                using SqlCommand cmd = new SqlCommand("sp_BuscarClientesPorNombre", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param = new SqlParameter
                {
                    ParameterName = "@Nombres",
                    SqlDbType = SqlDbType.NVarChar,
                    Size = 50,
                    Value = txtBuscar.Text.Trim()
                };
                cmd.Parameters.Add(param);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                List<Cliente> listaClientes = new List<Cliente>();

                while (reader.Read())
                {
                    listaClientes.Add(new Cliente
                    {
                        ClienteID = reader.GetInt32(reader.GetOrdinal("ClienteID")),
                        Nombres = reader.GetString(reader.GetOrdinal("Nombres")),
                        Apellidos = reader.GetString(reader.GetOrdinal("Apellidos")),
                        DNI = reader.GetString(reader.GetOrdinal("DNI"))
                    });
                }

                dgBuscar.ItemsSource = listaClientes;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar cliente: " + ex.Message);
            }
        }
    }
}