using System;
using System.Collections.Generic;
using System.Linq;

namespace Supermarket
{
    class
        Program
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

    class RandomContener
    {
        protected static readonly Random Random = new Random();
    }

    class Buyer : RandomContener
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
            if(_money >= price)
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
            if(_products.Count > 0)
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
            int randomIndex = Random.Next(ProductsCount);
            _products.RemoveAt(randomIndex);
        }

        public void TakeProduct(List<Product> products)
        {
            foreach(Product product in products)
            {
                _products.Add(product);
            }
        }

        public void ShowInfo()
        {
            string namesProducts = "";

            foreach(Product product in _products)
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

    class Supermarket : RandomContener
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
            while(_buyersQueue.Count > 0)
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

            while(isTransactionCompleted == false)
            {
                int price = CalculateTheCost();

                if(_buyer.TryGetBuy(price))
                {
                    _buyer.TakeProduct(_buyerProducts);
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

            for(int i = 0; i < productsCount; i++)
            {
                bill += _buyer.GetProductPrice(i);
            }
            return bill;
        }
    }

    class BuyerCreator : RandomContener
    {
        private const int minCountProducts = 1;
        private const int maxCountProducts = 10;
        private const int minBalanse = 1;
        private const int maxBalanse = 10000;

        private string[] _personName = new[]
        {
            "Саша", "Миша", "Дима", "Леша", "Паша", "Лена", "Вика", "Наташа", "Артем", "Вася"
        };

        public Buyer Create()
        {
            int randomMoney = Random.Next(minBalanse, maxBalanse);
            int randomNameIndex = Random.Next(_personName.Length);
            int randomCount = Random.Next(minCountProducts, maxCountProducts);
            List<Product> products = new List<Product>();
            ProductCreator creator = new ProductCreator();

            for(int j = randomCount; j > 0; j--)
            {
                products.Add(creator.Create());
            }

            return new Buyer(_personName[randomNameIndex], randomMoney, products);
        }
    }

    class ProductCreator : RandomContener, ICreater<Product>
    {
        private List<Product> _products = new List<Product>()
        {
            new Product("Яблоко",100),
            new Product("Перец",60),
            new Product("Молоко",70),
            new Product("Масло",150),
            new Product("Варенье",200),
            new Product("Мясо",500),
            new Product("Сок",120),

        };

        public Product Create()
        {
            int indexProduct = Random.Next(_products.Count);
            Product product = _products[indexProduct];

            return product;
        }
    }


    class SupermarketCreator : RandomContener, ICreater<Queue<Buyer>>
    {
        private BuyerCreator _creator = new BuyerCreator();
        private const int _maxQueueCount = 40;
        private const int _minQueueCount = 10;
        private Queue<Buyer> _queueBauyers = new Queue<Buyer>();

        public Queue<Buyer> Create()
        {
            int QueueCount = Random.Next(_minQueueCount, _maxQueueCount);

            for(int i = 0; i < QueueCount; i++)
            {
                Buyer buyer = _creator.Create();
                _queueBauyers.Enqueue(buyer);
            }

            return _queueBauyers;
        }
    }
}
