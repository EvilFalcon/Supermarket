using System;
using System.Collections.Generic;
using System.Linq;

namespace Supermarket
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Supermarket";
            Supermarket shop = new Supermarket();
            shop.Work();
        }
    }

    interface ICreater<T>
    {
        T Create();
    }

    static class UserUtils
    {
        private static Random _random = new Random();

        public static int GetRandomNumber(int maxValue, int minValue = 0)
        {
            int number = _random.Next(minValue, maxValue);
            return number;
        }

        public static int GetRandomNumber(int value)
        {
            int number = _random.Next(value);
            return number;
        }
    }

    class Buyer
    {
        private string _name;
        private int _purchaseAmount;
        private List<Product> _products;
        private int _money;

        public int ProductsCount => _products.Count;

        public Buyer(string name, int money, List<Product> cartProducts)
        {
            _name = name;
            _money = money;
            _products = cartProducts;
        }

        public bool TryGetBuy(int price)
        {
            if (_money >= price)
            {
                _purchaseAmount = price;
                _money -= price;
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetProductPrice(int index)
        {
            if (_products.Count > 0)
            {
                return _products[index].Price;
            }
            else
            {
                return default;
            }
        }

        public void DeleteOneProduct()
        {
            int randomIndex = UserUtils.GetRandomNumber(ProductsCount);

            _products.RemoveAt(randomIndex);
        }

        public void ShowInfo()
        {
            string namesProducts = "";

            foreach (Product product in _products)
            {
                namesProducts += $"{product.Name}, ";
            }

            Console.WriteLine($"Я {_name} купил/а : {namesProducts} на сумму : {_purchaseAmount}р. | ");
        }
    }

    class Product
    {
        public Product(string name, int price)
        {
            Name = name;
            Price = price;
        }

        public string Name { get; private set; }
        public int Price { get; private set; }

        public void ShowInfo()
        {
            Console.WriteLine($"продукт : {Name}| цена : {Price}");
        }
    }

    class Supermarket
    {
        SupermarketCreator _supermarketCreator = new SupermarketCreator();
        private Queue<Buyer> _buyersQueue = new Queue<Buyer>();
        private Buyer _buyer;
        private List<Product> _buyerProducts = new List<Product>();
        private int _money;

        internal Supermarket()
        {
            _buyersQueue = _supermarketCreator.Create();
        }

        public void Work()
        {
            while (_buyersQueue.Count > 0)
            {
                _buyer = _buyersQueue.Dequeue();
                SellProducts();
                ShowInfo();
                _buyer.ShowInfo();
                Console.ReadKey();
                Console.Clear();
            }
        }

        private void ShowInfo()
        {
            Console.WriteLine($"|Баланс магазина {_money}р. |");
        }

        private void SellProducts()

        {
            bool isTransactionCompleted = false;

            while (isTransactionCompleted == false)
            {
                int price = CalculateTheCost();

                if (_buyer.TryGetBuy(price))
                {
                    isTransactionCompleted = true;
                    _money += price;
                }
                else
                {
                    _buyer.DeleteOneProduct();
                }
            }
        }

        private int CalculateTheCost()
        {
            int bill = 0;

            int productsCount = _buyer.ProductsCount;

            for (int i = 0; i < productsCount; i++)
            {
                bill += _buyer.GetProductPrice(i);
            }

            return bill;
        }
    }

    class BuyerCreator
    {
        public Buyer Create()
        {
            int minCountProducts = 1;
            int maxCountProducts = 10;
            int minBalanse = 1;
            int maxBalanse = 10000;
            string[] personName = new[]
            {
                "Саша", "Миша", "Дима", "Леша", "Паша", "Лена", "Вика", "Наташа", "Артем", "Вася"
            };

            int randomMoney = UserUtils.GetRandomNumber(maxBalanse, minBalanse);
            int randomNameIndex = UserUtils.GetRandomNumber(personName.Length);
            int randomCount = UserUtils.GetRandomNumber(maxCountProducts, minCountProducts);
            List<Product> products = new List<Product>();
            ProductCreator creator = new ProductCreator();

            for (int j = randomCount; j > 0; j--)
            {
                products.Add(creator.Create());
            }

            return new Buyer(personName[randomNameIndex], randomMoney, products);
        }
    }

    class ProductCreator : ICreater<Product>
    {
        public Product Create()
        {
            List<Product> products = new List<Product>()
            {
                new Product("Яблоко", 100),
                new Product("Перец", 60),
                new Product("Молоко", 70),
                new Product("Масло", 150),
                new Product("Варенье", 200),
                new Product("Мясо", 500),
                new Product("Сок", 120),
            };

            int indexProduct = UserUtils.GetRandomNumber(products.Count);
            Product product = products[indexProduct];

            return product;
        }
    }

    class SupermarketCreator : ICreater<Queue<Buyer>>
    {
        public Queue<Buyer> Create()
        {
            int maxQueueCount = 40;
            int minQueueCount = 10;

            BuyerCreator creator = new BuyerCreator();
            Queue<Buyer> queueBauyers = new Queue<Buyer>();

            int queueCount = UserUtils.GetRandomNumber(maxQueueCount, minQueueCount);

            for (int i = 0; i < queueCount; i++)
            {
                Buyer buyer = creator.Create();
                queueBauyers.Enqueue(buyer);
            }

            return queueBauyers;
        }
    }
}