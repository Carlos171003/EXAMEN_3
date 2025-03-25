using System;
using System.Collections.Generic;
using System.IO;

public class Producto
{
    public int Codigo { get; private set; }
    public string Nombre { get; private set; }
    public decimal Precio { get; private set; }
    public int Inventario { get; private set; }

    public Producto(int codigo, string nombre, decimal precio, int inventario)
    {
        Codigo = codigo;
        Nombre = nombre;
        Precio = precio;
        Inventario = inventario;
    }

    public void ReducirInventario(int cantidad)
    {
        if (Inventario >= cantidad)
            Inventario -= cantidad;
    }
}

public class Inventario
{
    private List<Producto> productos;
    private const string ArchivoInventario = "inventario.txt";

    public Inventario()
    {
        productos = new List<Producto>();
        CargarInventario();
    }

    public void AgregarProducto(string nombre, decimal precio, int cantidad)
    {
        int codigo = productos.Count + 1;
        productos.Add(new Producto(codigo, nombre, precio, cantidad));
        GuardarInventario();
    }

    public Producto BuscarProducto(string criterio)
    {
        return productos.Find(p => p.Nombre.Equals(criterio, StringComparison.OrdinalIgnoreCase) || p.Codigo.ToString() == criterio);
    }

    public void VenderProducto(string criterio, int cantidad)
    {
        Producto producto = BuscarProducto(criterio);
        if (producto != null && producto.Inventario >= cantidad)
        {
            producto.ReducirInventario(cantidad);
            GuardarInventario();
            Console.WriteLine($"Venta realizada. Quedan {producto.Inventario} unidades de {producto.Nombre}.");
        }
        else
        {
            Console.WriteLine("Producto no disponible o cantidad insuficiente.");
        }
    }

    private void GuardarInventario()
    {
        using (StreamWriter sw = new StreamWriter(ArchivoInventario))
        {
            foreach (var producto in productos)
            {
                sw.WriteLine($"{producto.Codigo},{producto.Nombre},{producto.Precio},{producto.Inventario}");
            }
        }
    }

    private void CargarInventario()
    {
        if (File.Exists(ArchivoInventario))
        {
            string[] lineas = File.ReadAllLines(ArchivoInventario);
            foreach (string linea in lineas)
            {
                string[] datos = linea.Split(',');
                productos.Add(new Producto(int.Parse(datos[0]), datos[1], decimal.Parse(datos[2]), int.Parse(datos[3])));
            }
        }
    }
}

public class Tienda
{
    private Inventario inventario;

    public Tienda()
    {
        inventario = new Inventario();
    }

    public void Iniciar()
    {
        while (true)
        {
            Console.WriteLine("1. Agregar producto");
            Console.WriteLine("2. Buscar producto");
            Console.WriteLine("3. Vender producto");
            Console.WriteLine("4. Salir");
            Console.Write("Seleccione una opción: ");
            string opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    Console.Write("Nombre del producto: ");
                    string nombre = Console.ReadLine();
                    Console.Write("Precio: ");
                    decimal precio = decimal.Parse(Console.ReadLine());
                    Console.Write("Cantidad en inventario: ");
                    int cantidad = int.Parse(Console.ReadLine());
                    inventario.AgregarProducto(nombre, precio, cantidad);
                    break;
                case "2":
                    Console.Write("Ingrese nombre o código del producto: ");
                    string criterio = Console.ReadLine();
                    Producto producto = inventario.BuscarProducto(criterio);
                    if (producto != null)
                    {
                        Console.WriteLine($"Código: {producto.Codigo}, Nombre: {producto.Nombre}, Precio: {producto.Precio}, Inventario: {producto.Inventario}");
                    }
                    else
                    {
                        Console.WriteLine("Producto no encontrado.");
                    }
                    break;
                case "3":
                    Console.Write("Ingrese nombre o código del producto a vender: ");
                    string prodVender = Console.ReadLine();
                    Console.Write("Cantidad: ");
                    int cantVender = int.Parse(Console.ReadLine());
                    inventario.VenderProducto(prodVender, cantVender);
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Opción inválida. Intente nuevamente.");
                    break;
            }
        }
    }
}

class Program
{
    static void Main()
    {
        Tienda tienda = new Tienda();
        tienda.Iniciar();
    }
}
