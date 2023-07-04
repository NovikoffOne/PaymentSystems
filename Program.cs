using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystems
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Order order = new Order(32343243, 10999);
            LinkGenerator linkGenerator = new LinkGenerator(order);

            Console.WriteLine(linkGenerator.GetLinkVisa);
            Console.WriteLine(linkGenerator.GetLinkMir);
            Console.WriteLine(linkGenerator.GetLinkMasterCard);
        }
    }

    public class Order
    {
        public readonly int Id;
        public readonly int Amount;

        public Order(int id, int amount) => (Id, Amount) = (id, amount);
    }

    public interface IPaymentSystem
    {
        string GetPayingLink(Order order);
    }

    public class PaymentSystem1 : IPaymentSystem
    {
        public string GetPayingLink(Order order)
        {
            return $"pay.system1.ru / order ? amount = 12000RUB & hash ={order.Id}";
        }
    }

    public class PaymentSystem2 : IPaymentSystem
    {
        public string GetPayingLink(Order order)
        {
            return $"order.system2.ru/pay?hash={order.Id} {order.Amount}";
        }
    }

    public class PaymentSystem3 : IPaymentSystem
    {
        Random key = new Random();
        
        public string GetPayingLink(Order order)
        {
            return $"system3.com/pay?amount=12000&curency=RUB&hash={order.Amount}{order.Id}{key.Next(99999)}";
        }
    }

    public class LinkGenerator
    {
        private IPaymentSystem _masterCard;
        private IPaymentSystem _visa;
        private IPaymentSystem _mir;

        private Order _order;

        public string GetLinkMasterCard => _masterCard.GetPayingLink(_order);
        public string GetLinkVisa => _visa.GetPayingLink(_order);
        public string GetLinkMir => _mir.GetPayingLink(_order);

        public LinkGenerator(Order order)
        {
            _order = order;
            _masterCard = new PaymentSystem1();
            _visa = new PaymentSystem2();
            _mir = new PaymentSystem3();
        }
    }
}
