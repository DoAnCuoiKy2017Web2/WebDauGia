using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebDauGia.Helper
{
    public class UserX
    {
        public List<LUser> Items { get; set; }
        public UserX()
        {
            this.Items = new List<LUser>();
        }

        //public int SumQ()
        //{
        //    return this.Items.Sum(i => i.Quantity);
        //}

        //public void AddItem(CartItem item)
        //{
        //    var eItem = this.Items
        //        .Where(i => i.Product.ProID == item.Product.ProID)
        //        .FirstOrDefault();

        //    if (eItem != null)
        //    {
        //        eItem.Quantity += item.Quantity;
        //    }
        //    else
        //    {
        //        this.Items.Add(item);
        //    }

        //}

        //public void RemoveItem(int proId)
        //{
        //    var toDeleteItem = this.Items
        //       .Where(i => i.Product.ProID == proId)
        //       .FirstOrDefault();

        //    if (toDeleteItem != null)
        //    {
        //        this.Items.Remove(toDeleteItem);
        //    }
        //}
        //public void UpdateItem(int proId, int quantity)
        //{
        //    var toUpdateItem = this.Items
        //       .Where(i => i.Product.ProID == proId)
        //       .FirstOrDefault();

        //    if (toUpdateItem != null)
        //    {
        //        toUpdateItem.Quantity = quantity;
        //    }
        //}
    }
    public class LUser
    {
        public UserX UserX { get; set; }
    }
}