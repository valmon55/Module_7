namespace CS_Basic.Module_7
{
    public static class EmployeeExt
    {
        public static string Nick(this Employee employee)
        {
            return employee.Name.Substring(0, 2) + employee.LastName.Substring(0, 2);
        }
    }
    public class Company
    {
        public static string CompanyName;
        public Company(string companyName)
        {
            CompanyName = companyName;
        }
        protected List<Employee> Employees = new List<Employee>();
        protected internal void EnrollEmployee(Employee employee)
        {
            if (employee != null)
                Employees.Add(employee);
            else
                Console.WriteLine("Сотрудника нет, некого брать на работу!");
        }
    }
    public class Shop : Company
    {
        private List<string> ShopsList = new List<string>();
        Shop(string companyName) : base(companyName) { }
    }
    public class DelivCompany : Company
    {
        DelivCompany(string companyName) : base(companyName) { }
    }
    abstract class Delivery
    {
        public string Address
        {
            get { return Address == null ? "Address is empty" : Address; }
            set
            {
                if (Address == null)
                {
                    Console.WriteLine("Адрес не задан!");
                    Address = "Нет адреса";
                    Console.WriteLine("Установлено: {0}", Address);
                }
                else Address = value;
            }
        }
        /// <summary>
        /// Отправить посылку
        /// </summary>
        /// <param name="address"></param>
        internal abstract void Send(string address);
    }
    public class Employee
    {
        public string Name;
        public string LastName;
        public Employee(string name) { Name = name; }
        public Employee(string name, string lastname) : this(name)
        {
            LastName = lastname;
        }
    }
    public class Courier : Employee
    {
        public Courier(string name) : base(name) { }
        public Courier(string name, string lastname) : base(name, lastname) { }
        public static Courier operator +(Courier courier, string lastname)
        {
            if (courier != null && lastname != null)
            {
                courier.LastName = lastname;
                Console.WriteLine("LastName was changed!");
            }
            return courier;
        }
    }
    public class Staff : Employee
    {
        public Staff(string name) : base(name) { }
        public Staff(string name, string lastname) : base(name, lastname) { }
    }
    class Buyer
    {
        private string Name;
        private string Lastname;
        public string Home_Address;
        public enum DelivType
        {
            Home = 0,
            Shop,
            PickPoint
        }
        //private HomeDelivery homeDelivery;
        //private PickPointDelivery pickPointDelivery;
        //private ShopDelivery shopDelivery;
        public Buyer(string name)
        { Name = name; }
        public Buyer(string name, string lastname) : this(name)
        { Lastname = lastname; }
        //public (string type, string number)[]         phones;
        string email;
        Product[] product_list = new Product[10];
        public void AddToCart(Product product)
        {
            for (int i = 0; i < product_list.Length; i++)
            {
                if (product_list[i] == null)
                {
                    product_list[i] = product;
                    return;
                }
            }
        }
        public void RemoveFromCart(Product product)
        {
            for (int i = product_list.Length - 1; i >= 0; i--)
            {
                if (product_list[i] == product)
                {
                    product_list[i] = null;
                    return;
                }
            }
        }
        private void PrintOrder()
        {
            Console.WriteLine(Name + " " + Lastname + " заказал:");
            for (int i = 0; i < product_list.Length; i++)
                if (product_list[i] != null)
                    Console.WriteLine(product_list[i].Product_name);
        }
        /// <summary>
        /// Заказать доставку
        /// </summary>
        public void CreateOrder<TDelivery>(string addrees, DelivType delivType) where TDelivery : Delivery
        {
            Order<TDelivery> order;
            switch (delivType)
            {
                case DelivType.Shop:
                    order = new Order<TDelivery>(new ShopDelivery() as TDelivery);
                    break;
                case DelivType.PickPoint:
                    order = new Order<TDelivery>(new PickPointDelivery() as TDelivery);
                    break;
                case DelivType.Home:
                    order = new Order<TDelivery>(new HomeDelivery() as TDelivery);
                    break;
                default:
                    Console.WriteLine("Не выбрана корректная доставка");
                    return;
                    break;
            }

            order.Products = this.product_list;
            order.Address = addrees;
            PrintOrder();
            Console.WriteLine("Создан заказ с доставкой на адрес " + addrees);
            order.SendToDelivery();
        }
    }
    class HomeDelivery : Delivery
    {
        internal override void Send(string address)
        {
            Console.WriteLine("Выполняю доставку на дом по адресу: \r\n" + address);
        }
    }
    class PickPointDelivery : Delivery
    {
        internal override void Send(string address)
        {
            Console.WriteLine("Выполняю доставку PickPoint по адресу: \r\n" + address);
        }
    }
    class ShopDelivery : Delivery
    {
        Courier courier { get; set; }
        internal override void Send(string address)
        {
            Console.WriteLine("Выполняю доставку в магазин по адресу: \r\n" + address);
        }
    }
    class Product
    {
        public string Product_name;
        string Product_description;
        /// <summary>
        /// Идентификатор товара
        /// </summary>
        string Product_ID;
        public Product(string product_name)
        { Product_name = product_name; }
    }
    class Order<TDelivery/*,TStruct*/> where TDelivery : Delivery
    {
        private TDelivery _delivery;
        // Агрегация
        public Order(TDelivery delivery)
        {
            _delivery = delivery;
        }
        // композиция
        //private HomeDelivery _homeDelivery;
        //public Order()
        //{
        //    _homeDelivery = new HomeDelivery();
        //}
        public int Number;
        public string Address;
        public string Description;
        public Product[] Products;
        public void SendToDelivery()
        {
            _delivery.Send(Address);
        }
        public void DisplayAddress()
        {
            Console.WriteLine(_delivery.Address);
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Staff Freelancer = new Staff("Bob", "Marley");
            Console.WriteLine(Freelancer.Name + " " + Freelancer.LastName + " nick is " + Freelancer.Nick() + Environment.NewLine);
            Courier JustMerried = new Courier("Jane", "Douglas");
            Console.WriteLine("Modemoiselle {0} {1}", JustMerried.Name, JustMerried.LastName);
            JustMerried = JustMerried + "Kirkland";
            Console.WriteLine("Madam {0} {1} \r\n", JustMerried.Name, JustMerried.LastName);
            Company Fedex = new Company("Fedex");
            Fedex.EnrollEmployee(new Staff("Jimmy", "Brown"));
            Fedex.EnrollEmployee(new Courier("Sami", "Naseri"));
            Fedex.EnrollEmployee(new Courier("Neal", "Oliver"));
            Company Shop = new Company("FoodCourt");
            Shop.EnrollEmployee(new Staff("Frank", "Brook"));
            Shop.EnrollEmployee(new Staff("James", "Bond"));
            Shop.EnrollEmployee(new Courier("O.J.", "Grant"));

            Buyer Jane = new Buyer("Patric", "Jane");
            //Jane.phones[] = Jane.phones[2];
            Jane.Home_Address = "CBI Sacramento";
            Jane.AddToCart(new Product("Сыр"));
            Jane.AddToCart(new Product("Чай"));
            Jane.AddToCart(new Product("Сэндвич"));
            Jane.CreateOrder<HomeDelivery>(Jane.Home_Address, Buyer.DelivType.Home);

            Buyer Teresa = new Buyer("Teresa", "Lisbon");
            Teresa.Home_Address = "CBI Sacramento";
            Teresa.AddToCart(new Product("Салат"));
            Teresa.AddToCart(new Product("Кофе"));
            Teresa.AddToCart(new Product("Чизкейк"));
            Teresa.CreateOrder<ShopDelivery>("Chic av. 2", Buyer.DelivType.Shop);

            Buyer Cho = new Buyer("Cho");
            Cho.Home_Address = "CBI Sacramento";
            Cho.AddToCart(new Product("Кола"));
            Cho.AddToCart(new Product("Гамбургер"));
            Cho.CreateOrder<PickPointDelivery>("5th av. 55", Buyer.DelivType.PickPoint);

            Company FakeCompany = new Company("");
            Console.ReadKey();
        }
    }
}