using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TenantsApp.Entities
{
  public  class Rent
    {
        public Guid RentID { get; set; }
        public Guid TenantID { get; set; }
        public decimal  Price { get; set; }
        public DateTime ExpiryDate { get; set; }
        public Tenant Tenant { get; set; }
        public Color ItemBackgroundColor
        {
            get
            {
                DateTime dateNow = DateTime.Parse(DateTime.Now.ToShortDateString());
                this.ExpiryDate = DateTime.Parse(this.ExpiryDate.ToShortDateString());

                if (this.ExpiryDate < dateNow)
                {
                    return Color.LightPink;
                }
                else
                {
                    if(this.ExpiryDate>= dateNow && this.ExpiryDate< dateNow.AddDays(3))
                    {
                        return Color.LightYellow;
                    }
                    else
                    {
                        return Color.White;
                    }
                }
            }
        }
    }
}
