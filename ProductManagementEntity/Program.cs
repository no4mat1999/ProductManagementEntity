using System.Collections.Generic;
using System.Linq;
using System;
using Entity;
using Repository;
using ReportViewer;

namespace ProductManagementEntity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MainMenu();
        }

        private static void MainMenu()
        {
            int option = -1;
            while (option == -1)
            {
                Console.Clear();
                Console.Write("Product Management\n\n");
                Console.WriteLine("1 - Ver todos los productos\n2 - Agregar producto\n3 - Editar producto\n4 - Eliminar producto\n5 - Visor Crystal Reports\n6 - Salir\n");
                option = ReadOption(new int[] { 1, 2, 3, 4, 5, 6 });
            }

            switch (option)
            {
                case 1:
                    Console.Clear();
                    ShowAllProducts();
                    Console.WriteLine("\n\nPrecione ENTER para volver al menu");
                    Console.ReadLine();
                    MainMenu();
                    break;
                case 2:
                    Console.Clear();
                    AddProduct();
                    MainMenu();
                    break;
                case 3:
                    Console.Clear();
                    EditProduct();
                    MainMenu();
                    break;
                case 4:
                    Console.Clear();
                    DeleteProduct();
                    MainMenu();
                    break;
                case 5:
                    Console.Clear();
                    OpenViewer();
                    MainMenu();
                    break;
                case 6:
                    return;
                    break;
            }
        }

        private static void OpenViewer()
        {
            try
            {
                ShowAllProducts();
                List<Product> products = new List<Product>();
                using (ProductRepository repo = new ProductRepository())
                {
                    products = repo.RetriveAll();
                }

                if(products.Count == 0)
                {
                    Console.WriteLine("No hay productos");
                    Console.ReadLine();
                    return;
                }

                Console.WriteLine("\nSeleccione el ID del producto para mostrar en el visor");
                int option = -1;
                while (option == -1)
                {
                    option = ReadOption(products.Select(x => x.Id).ToArray());
                }

                Product selected = products.Where(x => x.Id == option).FirstOrDefault();

                Viewer viewer = new Viewer();
                viewer.SetProduct(selected);
                viewer.ShowDialog();
                viewer.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Ocurrio un error " + e.Message);
                Console.ReadLine();
                return;
            }


        }

        private static void EditProduct()
        {
            try
            {
                ShowAllProducts();
                List<Product> products = new List<Product>();
                using (ProductRepository repo = new ProductRepository())
                {
                    products = repo.RetriveAll();
                }

                if (products.Count == 0)
                {
                    Console.WriteLine("No hay productos");
                    Console.ReadLine();
                    return;
                }

                Console.WriteLine("\nSeleccione el ID del producto que desea editar");
                int option = -1;
                while (option == -1)
                    option = ReadOption(products.Select(x => x.Id).ToArray());

                Product productToEdit = null;
                using (ProductRepository repo = new ProductRepository())
                {
                    productToEdit = repo.Retrive(option);
                }

                Console.WriteLine();
                ShowProduct(productToEdit);

                string newName = null;
                while (newName == null)
                {
                    Console.Write("       Nuevo nombre (ENTER para omitir cambio): ");
                    newName = Console.ReadLine().Trim();

                    if (string.IsNullOrEmpty(newName))
                        newName = productToEdit.Name;
                }

                string newDescription = null;
                while (newDescription == null)
                {
                    Console.Write("  Nueva descripcion (ENTER para omitir cambio): ");
                    newDescription = Console.ReadLine().Trim();

                    if (string.IsNullOrEmpty(newDescription))
                        newDescription = productToEdit.Description;
                }

                decimal newPrice = -1;
                while (newPrice == -1)
                {
                    try
                    {
                        Console.Write("       Nuevo precio (ENTER para omitir cambio): ");
                        string stringNewPrice = Console.ReadLine().Trim();

                        if (string.IsNullOrEmpty(stringNewPrice))
                            newPrice = productToEdit.Price;
                        else
                        {
                            newPrice = decimal.Parse(stringNewPrice);
                            if (newPrice <= 0)
                                throw new Exception();
                        }
                    }
                    catch (Exception)
                    {
                        newPrice = -1;
                    }
                }

                int newStock = -1;
                while (newStock == -1)
                {
                    try
                    {
                        Console.Write("        Nuevo stock (ENTER para omitir cambio): ");
                        string stringNewStock = Console.ReadLine().Trim();

                        if (string.IsNullOrEmpty(stringNewStock))
                            newStock = productToEdit.Stock;
                        else
                        {
                            newStock = int.Parse(stringNewStock);
                            if (newStock <= 0)
                                throw new Exception();
                        }
                    }
                    catch (Exception)
                    {
                        newStock = -1;
                    }
                }

                Product updatedProduct = new Product()
                {
                    Id = productToEdit.Id,
                    Name = newName,
                    Description = newDescription,
                    Price = newPrice,
                    Stock = newStock,
                };

                Console.WriteLine("\nVista previa:");
                ShowProduct(updatedProduct);

                Console.WriteLine("Desea guardar los cambios?\n1 - Si\n2 - No\n");
                option = -1;
                while (option == -1)
                    option = ReadOption(new int[] { 1, 2 });

                if (option == 1)
                {
                    using (ProductRepository repo = new ProductRepository())
                    {
                        repo.Update(updatedProduct);
                    }
                    Console.WriteLine("Producto editado con exito. Precione ENTER para regresar al menu\n");
                    Console.ReadLine();
                }

                return;

            }
            catch (Exception e)
            {
                Console.WriteLine("Ocurrio un error " + e.Message);
                Console.Read();
                return;
            }
        }

        private static void DeleteProduct()
        {
            try
            {
                ShowAllProducts();
                List<Product> products = new List<Product>();
                using (ProductRepository repo = new ProductRepository())
                {
                    products = repo.RetriveAll();
                }

                if (products.Count == 0)
                {
                    Console.WriteLine("No hay productos");
                    Console.ReadLine();
                    return;
                }

                Console.WriteLine("\nSeleccione el ID del producto que desea eliminar");
                int option = -1;
                while (option == -1)
                    option = ReadOption(products.Select(x => x.Id).ToArray());

                Product productToDelete = null;
                using (ProductRepository repo = new ProductRepository())
                {
                    productToDelete = repo.Retrive(option);
                }

                Console.WriteLine();
                ShowProduct(productToDelete);

                Console.WriteLine("\nEliminar?\n1 - Si\n2 - No, regresar al menu\n");
                option = -1;
                while (option == -1)
                    option = ReadOption(new int[] { 1, 2 });

                if (option == 1)
                {
                    using (ProductRepository repo = new ProductRepository())
                    {
                        repo.Delete(productToDelete.Id);
                    }
                    Console.WriteLine("Producto eliminado exitosamente. Precione ENTER para volver al menu\n");
                    Console.ReadLine();
                }
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine("Ocurrio un error " + e.Message);
                Console.Read();
                return;
            }
        }

        private static void AddProduct()
        {
            try
            {
                string productName = null;
                Console.WriteLine("Agregar Producto\n");
                while (string.IsNullOrEmpty(productName))
                {
                    Console.Write("      Nombre del producto (requerido) : ");
                    productName = Console.ReadLine().Trim();
                }

                string productDescription = null;
                while (string.IsNullOrEmpty(productDescription))
                {
                    Console.Write(" Descripcion del producto (requerido) : ");
                    productDescription = Console.ReadLine().Trim();
                }

                decimal productPrice = -1;
                while (productPrice == -1)
                {
                    Console.Write("      Precio del producto (requerido) : ");
                    try
                    {
                        productPrice = decimal.Parse(Console.ReadLine().Trim());
                        if (productPrice <= 0)
                            throw new Exception();
                    }
                    catch (Exception)
                    {
                        productPrice = -1;
                    }
                }

                int productStock = -1;
                while (productStock == -1)
                {
                    Console.Write("       Stock del producto (requerido) : ");
                    try
                    {
                        productStock = int.Parse(Console.ReadLine().Trim());
                        if (productStock < 0)
                            throw new Exception();
                    }
                    catch (Exception)
                    {
                        productStock = -1;
                    }
                }

                Product product = new Product()
                {
                    Name = productName,
                    Description = productDescription,
                    Price = productPrice,
                    Stock = productStock
                };

                Console.WriteLine();
                ShowProduct(product);
                Console.WriteLine("\nDesea guardar el producto?\n1 - Si\n2 - No, volver al menu\n");

                int option = -1;
                while (option == -1)
                    option = ReadOption(new int[] { 1, 2 });

                if (option == 1)
                {
                    using (ProductRepository repo = new ProductRepository())
                    {
                        repo.Create(product);
                    }
                    Console.WriteLine("Producto agregado correctamente. Precione ENTER para volver al menu\n");
                    Console.ReadLine();
                }
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine("Ocurrio un error" + e.Message);
                Console.ReadLine();
                return;
            }
        }


        private static void ShowProduct(Product product)
        {
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine($"Nombre -------=[ {product.Name}");
            Console.WriteLine($"Descripcion --=[ {product.Description}");
            Console.WriteLine($"Precio -------=[ ${product.Price.ToString()}");
            Console.WriteLine($"Stock --------=[ {product.Stock.ToString()} pza");
            Console.WriteLine("------------------------------------------------------------\n");
        }

        private static void ShowAllProducts()
        {
            try
            {
                Console.WriteLine(" Id        Nombre              Descripcion                   Precio              Stock          \n");


                List<Product> products = new List<Product>();
                using (ProductRepository repo = new ProductRepository())
                {
                    products = repo.RetriveAll();
                }


                int idLimit = 5;
                int limit = 16;
                int descExtra = 10;

                foreach (Product product in products)
                {
                    string id = product.Id.ToString();
                    string name = product.Name;
                    string description = (string.IsNullOrEmpty(product.Description)) ? "" : product.Description;
                    string price = "$ " + product.Price.ToString();
                    string stock = product.Stock.ToString() + " pza.";

                    id = (id.Length < idLimit) ? id.PadRight(idLimit) : id.Substring(0, idLimit);
                    name = (name.Length < limit) ? name.PadRight(limit) : name.Substring(0, limit - 3) + "...";
                    description = (description.Length < limit + descExtra) ? description.PadRight(limit + descExtra) : description.Substring(0, limit + descExtra - 3) + "...";
                    price = (price.Length < limit) ? price.PadRight(limit) : price.Substring(0, limit);
                    stock = (stock.Length < limit) ? stock.PadRight(limit) : stock.Substring(0, limit);

                    Console.WriteLine($" {id}    {name}    {description}    {price}    {stock}");

                }
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine("Ocurrio un error " + e.Message);
                Console.ReadLine();
                return;
            }
        }


        private static int ReadOption(int[] validOptions)
        {
            Console.Write("opcion>");
            string option = Console.ReadLine();
            try
            {
                int selected = int.Parse(option);
                if (validOptions.Contains(selected))
                    return selected;

                throw new Exception();
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}
